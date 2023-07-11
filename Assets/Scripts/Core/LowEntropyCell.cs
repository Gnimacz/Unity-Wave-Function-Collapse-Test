using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LowEntropyCell : IComparable<LowEntropyCell>, IEqualityComparer<LowEntropyCell>
{
    public Vector2Int position { get; set; }
    public float entropy { get; set; }
    private float smallEntropyNoise;
    public LowEntropyCell(Vector2Int position, float entropy)
    {
        smallEntropyNoise = UnityEngine.Random.Range(0.001f, 0.005f);
        this.position = position;
        this.entropy = entropy + smallEntropyNoise;
    }
    public int CompareTo(LowEntropyCell obj)
    {
        if(entropy > obj.entropy)
        {
            return 1;
        }
        else if(entropy < obj.entropy)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public bool Equals(LowEntropyCell x, LowEntropyCell y)
    {
        return x.position == y.position;
    }

    public int GetHashCode(LowEntropyCell obj)
    {
        return obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}
