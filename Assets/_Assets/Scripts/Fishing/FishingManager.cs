using UnityEngine;

public class FishingManager
{
    public static FishLootTable CurrentLootTable { get; private set; }
    public static FishItem CurrentFish { get; private set; }

    public static void StartFishing(ref FishLootTable lootTable)
    {
        CurrentLootTable = lootTable;
        CurrentFish = CurrentLootTable.GetLootDropItem();
        
        MinigameManager.Instance.StartMinigame(50, MinigameType.Circle, OnMinigameComplete);
        
        FishingSpotProbabilitiesUI.Instance.GenerateUI(CurrentLootTable);
        Debug.Log("NNNNNNNNN");
    }
    
    private static void OnMinigameComplete(MinigameResult result)
    {
        if (result == MinigameResult.Won)
        {
            Debug.Log($"Voce pescou um {CurrentFish.Item.DisplayName} de raridade {CurrentFish.Item.Rarity}");
            DiaryManager.Instance.RegisterFish(CurrentFish.Item);
        }
        else
        {
            Debug.Log("Voce fracassou e nao pegou o peixe");
        }
        
        FishingSpotProbabilitiesUI.Instance.ClearUI();
        CurrentFish = null;
    }
}
