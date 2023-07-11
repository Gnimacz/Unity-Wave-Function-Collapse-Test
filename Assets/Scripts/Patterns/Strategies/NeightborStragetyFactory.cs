using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class NeightborStragetyFactory
{
    Dictionary<string, Type> strategies;
    public NeightborStragetyFactory()
    {
        LoadStrategies();
    }

    private void LoadStrategies()
    {
        strategies = new Dictionary<string, Type>();
        Type[] strategyTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in strategyTypes)
        {
            if (type.GetInterface(typeof(IFindNeighborStrategy).ToString()) != null)
            {
                strategies.Add(type.Name.ToLower(), type);
            }
        }
    }

    internal IFindNeighborStrategy CreateInstance(string strategyName)
    {
        Type t = GetTypeToCreate(strategyName);
        if(t == null)
        {
            t = GetTypeToCreate("more");
        }
        return Activator.CreateInstance(t) as IFindNeighborStrategy;
    }

    private Type GetTypeToCreate(string strategyName)
    {
        foreach (var possibleStrategy in strategies)
        {
            if(possibleStrategy.Key.Contains(strategyName.ToLower()))
            {
                return possibleStrategy.Value;
            }
        }
        return null;
    }
}
