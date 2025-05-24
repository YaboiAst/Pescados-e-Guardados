using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Fish/New Fish")]
public class FishSO : ScriptableObject
{
    [SerializeField] private string _displayName;
    [TextArea]
    [SerializeField] private string _description;
    [SerializeField] private FishRarity _rarity;
    [SerializeField] private float _basePoints;
    [SerializeField] private GameObject _fishPrefab;
    [SerializeField] private Sprite _icon;
    [MinMaxSlider(0.0f, 100.0f)]
    [SerializeField] private Vector2 _weightRange;
    
    public string DisplayName => _displayName;
    public string Description => _description;
    public FishRarity Rarity => _rarity;
    public float BasePoints => _basePoints;
    public GameObject FishPrefab => _fishPrefab;
    public Sprite Icon => _icon;
    public float MinWeight => _weightRange.x;
    public float MaxWeight => _weightRange.y;
}