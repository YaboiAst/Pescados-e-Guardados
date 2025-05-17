using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishingSpotInteraction : MonoBehaviour
{
    // [SerializeField] private List<FishSO> _availableFishes;
    [SerializeField] private MinigameType _minigameType;
    [SerializeField] private FishLootDropTable _lootTable;

    private FishSO _currentFish;

    private void OnValidate()
    {
        _lootTable.ValidateTable();
    }

    public void Interact()
    {
        _currentFish = _lootTable.GetLootDropItem().Item;
        Debug.Log(_currentFish.DisplayName);

        // int difficulty;
        
        // switch (_currentFish.Rarity)
        // {
        //     case FishRarity.Common:
        //         difficulty = Random.Range(10, 20);
        //         break;
        //     case FishRarity.Uncommon:
        //         difficulty = Random.Range(20, 30);
        //         break;
        //     case FishRarity.Rare:
        //         difficulty = Random.Range(30, 40);
        //         break;
        //     case FishRarity.Epic:
        //         difficulty = Random.Range(40, 50);
        //         break;
        //     case FishRarity.Legendary:
        //         difficulty = Random.Range(50, 60);
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
        
        MinigameManager.s_Instance.StartMinigame(50, _minigameType, OnMinigameComplete);
    }

    private void OnMinigameComplete(MinigameResult result)
    {
        if (result == MinigameResult.Won)
        {
            Debug.Log($"Voce pescou um {_currentFish.DisplayName} de raridade {_currentFish.Rarity}");
            _currentFish = null;
        }
        else
        {
            Debug.Log("Voce fracassou e nao pegou o peixe");
            _currentFish = null;
        }
    }
}
