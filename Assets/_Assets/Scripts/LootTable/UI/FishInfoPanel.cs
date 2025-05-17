using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject _fishInfoUIPrefab;

    public static FishInfoPanel s_Instance;

    private void Awake()
    {
        s_Instance = this;
    }

    public void GenerateFishInfoProbabilities(List<FishLootDropItem> fishes)
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }


        foreach(var fish in fishes)
        {
            var fishInfo = Instantiate(_fishInfoUIPrefab, this.transform).GetComponent<FishInfoUI>();

            fishInfo.SetupFishInfo(fish);
        }
    }
}