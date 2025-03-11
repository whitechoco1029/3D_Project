using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;  // ScriptableObject 데이터 연결
    private bool isPlayerNearby = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isPlayerNearby)
        {
            Debug.Log($"아이템 획득: {itemData.itemName}");
            InventoryManager.Instance.AddItem(itemData); // 인벤토리에 추가
            Destroy(gameObject); // 필드에서 아이템 제거
        }
    }
}
