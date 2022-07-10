using System.Runtime.InteropServices;

namespace HuskyEngine.Data.Cache;

[StructLayout(LayoutKind.Sequential)]
public struct FactorCacheItem
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
    public string Symbol;

    public float Value;
}