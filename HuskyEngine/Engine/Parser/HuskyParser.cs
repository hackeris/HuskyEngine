﻿using Antlr4.Runtime;
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
        var unaryOperator = context.op.Text switch
        {
            "-" => Operation.Unary.Minus,
            "!" => Operation.Unary.Not,
            _ => throw new ParsingError()
        };

        var operand = Visit(context.powr());

        return new UnaryExpression
        {
            Operand = operand,
            Operator = unaryOperator
        };
    }

    public override IExpression VisitMultiDivOp(HuskyLangParser.MultiDivOpContext context)
    {
        return VisitBinaryExpression(
            context.multOrDiv(),
            context.op.Text,
            context.powr()
        );
    }

    public override IExpression VisitAddSubOp(HuskyLangParser.AddSubOpContext context)
    {
        return VisitBinaryExpression(
            context.plusOrMinus(),
            context.op.Text,
            context.multOrDiv()
        );
    }

    public override IExpression VisitCompareOp(HuskyLangParser.CompareOpContext context)
    {
        return VisitBinaryExpression(
            context.compare(),
            context.op.Text,
            context.plusOrMinus()
        );
    }

    public override IExpression VisitLogicalOp(HuskyLangParser.LogicalOpContext context)
    {
        return VisitBinaryExpression(
            context.logical(),
            context.op.Text,
            context.compare()
        );
    }

    public override IExpression VisitPower(HuskyLangParser.PowerContext context)
    {
        var leftContext = context.powr();
        var rightContext = context.atom();
        var operatorText = context.op.Text;

        return VisitBinaryExpression(leftContext, operatorText, rightContext);
    }

    private IExpression VisitBinaryExpression(
        ParserRuleContext leftContext,
        string operatorText,
        ParserRuleContext rightContext
    )
    {
        var left = Visit(leftContext);
        var right = Visit(rightContext);

        var binaryOperator = operatorText switch
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
            _ => throw new ParsingError()
        };

        var funcType = _predefine.GetFunctionType(
            operatorText,
            new List<IType> { left.Type, right.Type }
        );
        if (funcType == null)
        {
            throw new ParsingError();
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
            throw new ParsingError();
        }

        return new FunctionCall
        {
            Name = functionName,
            Arguments = arguments,
            Type = funcType.ReturnType
        };
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

        return new Indexing
        {
            Indexable = left,
            Index = index
        };
    }

    public override IExpression VisitToIdentifier(HuskyLangParser.ToIdentifierContext context)
    {
        var name = context.identifier().ID().GetText();

        var type = _predefine.GetType(name);
        if (type == null)
        {
            throw new ParsingError();
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
}