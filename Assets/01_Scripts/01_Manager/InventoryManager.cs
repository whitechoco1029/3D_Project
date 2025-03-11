using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> inventoryItems = new List<ItemData>(); // �κ��丮 ����Ʈ

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
        Debug.Log($"�κ��丮�� {item.itemName} �߰���!");
    }
}
