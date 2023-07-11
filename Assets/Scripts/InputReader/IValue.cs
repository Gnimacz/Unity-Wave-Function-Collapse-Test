using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IValue<T> : IEqualityComparer<IValue<T>>, IEquatable<IValue<T>>
{
    T Value { get; }
}
