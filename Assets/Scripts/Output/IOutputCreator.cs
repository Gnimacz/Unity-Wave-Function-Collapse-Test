using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOutputCreator<T>
{
    T OutputImage { get; }
    void CreateOutputImage(PatternManager patternManager, int[][] outputValues, int width, int height);
}
