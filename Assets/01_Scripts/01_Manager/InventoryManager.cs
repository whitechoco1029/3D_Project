using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> inventoryItems = new List<ItemData>(); // 인벤토리 리스트

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(ItemData item)
    {
        inventoryItems.Add(item);
        Debug.Log($"인벤토리에 {item.itemName} 추가됨!");
    }
}
