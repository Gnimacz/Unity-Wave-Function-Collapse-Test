using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFindNeighborStrategy
{
    // List<TileContainer> FindNeighbors(TileContainer tileContainer, IValue<TileContainer>[][] tileContainers);
    Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResults patterFinderResult);
}
