using System;

namespace Memories.Data;

public enum Comparison
{
    LessThan,
    GreaterThan,
}

[Serializable]
public struct MoodRequirement
{
    public Comparison type;
    public int threshold;

    public bool Matches(int mood)
    {
        return type switch
        {
            Comparison.LessThan => mood < threshold,
            Comparison.GreaterThan => mood > threshold,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
