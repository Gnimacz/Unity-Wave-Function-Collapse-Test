using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBaseValue : IValue<TileBase>
{
    public TileBase Value { get; private set;}

    public TileBaseValue(TileBase value)
    {
        Value = value;
    }

    public bool Equals(IValue<TileBase> x, IValue<TileBase> y)
    {
        return x.Value == y.Value;
    }

    public int GetHashCode(IValue<TileBase> obj)
    {
        return obj.GetHashCode();
    }

    public bool Equals(IValue<TileBase> other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }
}
