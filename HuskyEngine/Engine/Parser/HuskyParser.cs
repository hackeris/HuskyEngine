using Antlr4.Runtime;
using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Parser.Errors;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Parser;

public class HuskyParser : HuskyLangBaseVisitor<IExpression>
{
    private readonly IPredefine _predefine;

    private HuskyParser(IPredefine predefine)
    {
        _predefine = predefine;
    }

    public override IExpression VisitUnaryOp(HuskyLangParser.UnaryOpContext context)
    {
        var opText = context.op.Text;
        var unaryOperator = opText switch
        {
            "-" => Operation.Unary.Minus,
            "!" => Operation.Unary.Not,
            _ => throw new ParsingError(context,
                $"Unsupported operator {opText}")
        };

        var operand = Visit(context.powr());

        var funcType = _predefine.GetFunctionType(
            opText,
            new List<IType> { operand.Type }
        );
        if (funcType == null)
        {
            throw new ParsingError(context,
                $"Unsupported operator {opText} on {operand.Type}");
        }

        return new UnaryExpression
        {
            Operand = operand,
            Operator = unaryOperator,
            Type = funcType.ReturnType
        };
    }

    public override IExpression VisitMultiDivOp(HuskyLangParser.MultiDivOpContext context)
    {
        return VisitBinaryExpression(
            context.multOrDiv(),
            context.op,
            context.powr()
        );
    }

    public override IExpression VisitAddSubOp(HuskyLangParser.AddSubOpContext context)
    {
        return VisitBinaryExpression(
            context.plusOrMinus(),
            context.op,
            context.multOrDiv()
        );
    }

    public override IExpression VisitCompareOp(HuskyLangParser.CompareOpContext context)
    {
        return VisitBinaryExpression(
            context.compare(),
            context.op,
            context.plusOrMinus()
        );
    }

    public override IExpression VisitLogicalOp(HuskyLangParser.LogicalOpContext context)
    {
        return VisitBinaryExpression(
            context.logical(),
            context.op,
            context.compare()
        );
    }

    public override IExpression VisitPower(HuskyLangParser.PowerContext context)
    {
        var leftContext = context.powr();
        var rightContext = context.atom();

        return VisitBinaryExpression(leftContext, context.op, rightContext);
    }

    private IExpression VisitBinaryExpression(
        ParserRuleContext leftContext,
        IToken operatorToken,
        ParserRuleContext rightContext
    )
    {
        var left = Visit(leftContext);
        var right = Visit(rightContext);

        var binaryOperator = operatorToken.Text switch
        {
            "^" => Operation.Binary.Power,
            "+" => Operation.Binary.Add,
            "-" => Operation.Binary.Sub,
            "*" => Operation.Binary.Mul,
            "/" => Operation.Binary.Div,
            "=" => Operation.Binary.Equal,
            "!=" => Operation.Binary.NotEqual,
            ">" => Operation.Binary.Greater,
            "<" => Operation.Binary.Lower,
            ">=" => Operation.Binary.GreaterOrEq,
            "<=" => Operation.Binary.LowerOrEq,
            "&" => Operation.Binary.And,
            "|" => Operation.Binary.Or,
            _ => throw new ParsingError(operatorToken,
                $"Unsupported operator {operatorToken.Text}")
        };

        var funcType = _predefine.GetFunctionType(
            operatorToken.Text,
            new List<IType> { left.Type, right.Type }
        );
        if (funcType == null)
        {
            throw new ParsingError(operatorToken,
                $"Unsupported binary operator {operatorToken.Text} on {left.Type} and {right.Type}");
        }

        return new BinaryExpression
        {
            Left = left,
            Operator = binaryOperator,
            Right = right,
            Type = funcType.ReturnType
        };
    }

    public override IExpression VisitFuncCall(HuskyLangParser.FuncCallContext context)
    {
        var functionName = context.functionName.GetText();

        var arguments = UnfoldArgList(context.argList());
        var argTypes = (
            from arg in arguments
            select arg.Type
        ).ToList();
        var funcType = _predefine.GetFunctionType(functionName, argTypes);
        if (funcType == null)
        {
            var typesString = argTypes
                .Select(e => e.ToString())
                .Aggregate((a, b) => a + "," + b);
            throw new ParsingError(context.functionName,
                $"Could not find function {functionName}({typesString})");
        }

        return new FunctionCall(
            functionName,
            arguments,
            funcType.ReturnType
        );
    }

    private List<IExpression> UnfoldArgList(HuskyLangParser.ArgListContext? context)
    {
        if (context == null)
        {
            return new List<IExpression>();
        }
        else
        {
            var tail = Visit(context.expression());
            var head = UnfoldArgList(context.argList());
            head.Add(tail);
            return head;
        }
    }

    public override IExpression VisitArrayIndex(HuskyLangParser.ArrayIndexContext context)
    {
        var left = Visit(context.indexable);
        var index = Visit(context.index);

        var resultType = _predefine.GetIndexingType(left.Type, index.Type);
        if (resultType == null)
        {
            throw new ParsingError(
                context.index, $"Cannot indexing {index.Type} on {left.Type}");
        }

        return new Indexing
        {
            Index = index,
            Indexable = left,
            Type = resultType
        };
    }

    public override IExpression VisitIdentifier(HuskyLangParser.IdentifierContext context)
    {
        var name = context.ID().GetText();

        var type = _predefine.GetType(name);
        if (type == null)
        {
            throw new ParsingError(context,
                $"Identifier {name} is not found");
        }

        return new Identifier(name, type);
    }

    public override IExpression VisitInt(HuskyLangParser.IntContext context)
    {
        int value = Int32.Parse(context.GetText());
        return new Literal(value);
    }

    public override IExpression VisitDouble(HuskyLangParser.DoubleContext context)
    {
        double value = Double.Parse(context.GetText());
        return new Literal((float)value);
    }

    public override IExpression VisitConstantE(HuskyLangParser.ConstantEContext context)
    {
        return new Literal((float)Math.E);
    }

    public override IExpression VisitConstantPI(HuskyLangParser.ConstantPIContext context)
    {
        return new Literal((float)Math.PI);
    }

    public override IExpression VisitToExpression(HuskyLangParser.ToExpressionContext context)
    {
        return Visit(context.expression());
    }

    public override IExpression VisitToLogical(HuskyLangParser.ToLogicalContext context)
    {
        return Visit(context.logical());
    }

    public override IExpression VisitToCompare(HuskyLangParser.ToCompareContext context)
    {
        return Visit(context.compare());
    }

    public override IExpression VisitToPlusOrMinus(HuskyLangParser.ToPlusOrMinusContext context)
    {
        return Visit(context.plusOrMinus());
    }

    public override IExpression VisitToMultOrDiv(HuskyLangParser.ToMultOrDivContext context)
    {
        return Visit(context.multOrDiv());
    }

    public override IExpression VisitToUnary(HuskyLangParser.ToUnaryContext context)
    {
        return Visit(context.unary());
    }

    public override IExpression VisitToPower(HuskyLangParser.ToPowerContext context)
    {
        return Visit(context.powr());
    }

    public override IExpression VisitToAtom(HuskyLangParser.ToAtomContext context)
    {
        return Visit(context.atom());
    }

    public override IExpression VisitToIdentifier(HuskyLangParser.ToIdentifierContext context)
    {
        return Visit(context.identifier());
    }

    public override IExpression VisitBraces(HuskyLangParser.BracesContext context)
    {
        return Visit(context.expression());
    }

    public static IExpression Parse(string code, IPredefine pred)
    {
        var stream = new AntlrInputStream(code);
        var lexer = new HuskyLangLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new HuskyLangParser(tokens);
        var tree = parser.statement();

        var visitor = new HuskyParser(pred);
        return visitor.Visit(tree);
    }

    public static IExpression Parse(string code, IRuntime runtime)
    {
        return Parse(code, runtime.GetPredefine());
    }
}