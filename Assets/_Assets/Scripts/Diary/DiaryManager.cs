using System;
using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance { get; private set; }

    private readonly HashSet<string> _discoveredCreatures = new HashSet<string>();

    public event Action<FishData> OnFishDiscovered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        
        //Load
    }

    public void RegisterFish(FishData fish)
    {
        if (!_discoveredCreatures.Add(fish.UniqueID)) 
            return;
        
        //Save
        OnFishDiscovered?.Invoke(fish);
    }

    public bool IsDiscovered(FishData fish) => _discoveredCreatures.Contains(fish.UniqueID);
}

// [System.Serializable]
// public class DiarySaveData
// {
//     public List<string> CreatureNames;
//     public DiarySaveData(HashSet<string> set) => CreatureNames = new List<string>(set);
// }

public struct DiaryEntry
{
        
}

