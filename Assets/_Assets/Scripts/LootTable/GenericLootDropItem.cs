using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GenericLootDropItem<T>
{
    public T Item;
    public float ProbabilityWeight;

    public float ProbabilityPercent;

    [HideInInspector]
    public float ProbabilityRangeFrom;
    [HideInInspector]
    public float ProbabilityRangeTo;
}