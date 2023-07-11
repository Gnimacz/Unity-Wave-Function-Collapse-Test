using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IInputReader<T>
{
    IValue<T>[][] ReadInput();
}
