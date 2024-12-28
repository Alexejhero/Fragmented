using System;

namespace Helpers;

public enum Comparison
{
    LessThan,
    GreaterThan,
}

[Serializable]
public struct ValueRequirement
{
    public Comparison type;
    public int threshold;

    public bool IsMetBy(int value)
    {
        return type switch
        {
            Comparison.LessThan => value < threshold,
            Comparison.GreaterThan => value > threshold,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
