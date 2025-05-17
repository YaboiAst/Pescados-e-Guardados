using UnityEngine;

[CreateAssetMenu(menuName = "Fish/New Fish")]
public class FishSO : ScriptableObject
{
    [SerializeField] private string _displayName;
    [TextArea]
    [SerializeField] private string _description;
    [SerializeField] private FishRarity _rarity;
    [SerializeField] private Vector2 _gridSize;
    [SerializeField] private float _points;
    [SerializeField] private Sprite _icon;
    [SerializeField] private float _minWeight;
    [SerializeField] private float _maxWeight;
    
    public string DisplayName => _displayName;
    public string Description => _description;
    public FishRarity Rarity => _rarity;
    public Vector2 GridSize => _gridSize;
    public float Points => _points;
    public Sprite Icon => _icon;
    public float MinWeight => _minWeight;
    public float MaxWeight => _maxWeight;
}