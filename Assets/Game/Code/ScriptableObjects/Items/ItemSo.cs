using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "ItemsSO", order = 0)]
public class ItemSo : ScriptableObject
{
    private void Awake()
    {
        Id = IdGenerator.GenerateId();
    }

    public int Id { get; private set; }

    public Sprite ItemImage => _itemImage;
    
    public string ItemName => _itemName;

    public float ItemPrice => _itemPrice;

    public GameObject Prefab => _prefab;

    [SerializeField] private Sprite _itemImage;
    
    [SerializeField] private string _itemName;

    [SerializeField] private float _itemPrice;

    [SerializeField] private GameObject _prefab;
}