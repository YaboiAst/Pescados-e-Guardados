using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishInfoUI : MonoBehaviour
{
    [SerializeField] private Image _fishIcon;
    [SerializeField] private TMP_Text _fishProbabilityText;

    public void SetupFishInfo(FishItem fish)
    {
        _fishIcon.sprite = fish.Item.Icon;
        _fishProbabilityText.SetText(fish.ProbabilityPercent.ToString("F2") + "%");

        Color textColor = Color.white;

        switch(fish.Item.Rarity)
        {
            case FishRarity.Uncommon:
                textColor = Color.green;
                break;
            case FishRarity.Rare:
                textColor = Color.blue;
                break;
            case FishRarity.Epic:
                textColor = Color.magenta;
                break;
            case FishRarity.Legendary:
                textColor = Color.yellow;
                break;        
        }

        _fishProbabilityText.color = textColor;
    }
}