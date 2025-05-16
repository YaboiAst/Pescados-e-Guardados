using UnityEngine;

[CreateAssetMenu(menuName = "Fish/New Fish")]
public class FishSO : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private FishRarity _rarity;
    [SerializeField] private Vector2 _gridSize;
    [SerializeField] private float _value;
    [SerializeField] private Sprite _icon;
    
    public string DisplayName => _displayName;
    public FishRarity Rarity => _rarity;
    public Vector2 GridSize => _gridSize;
    public float Value => _value;
    public Sprite Icon => _icon;
}