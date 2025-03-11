using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;      // 아이템 이름
    public string description;   // 아이템 설명
    public Sprite icon;          // 아이템 아이콘
    public GameObject prefab;    // 아이템 프리팹 (필드에 배치할 경우 사용)
}
