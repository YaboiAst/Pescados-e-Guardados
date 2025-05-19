using UnityEngine;

public class FishInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject _fishInfoUIPrefab;

    public static FishInfoPanel Instance;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void GenerateFishInfoProbabilities(FishLootTable fishes)
    {
        ClearFishInfoProbabilities();

        foreach (FishItem fish in fishes.LootDropItems)
        {
            FishInfoUI fishInfo = Instantiate(_fishInfoUIPrefab, this.transform).GetComponent<FishInfoUI>();

            fishInfo.SetupFishInfo(fish);
        }

        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void ClearFishInfoProbabilities()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}