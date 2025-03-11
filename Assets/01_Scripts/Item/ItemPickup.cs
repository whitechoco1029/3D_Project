using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;  // ScriptableObject ������ ����
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
            Debug.Log($"������ ȹ��: {itemData.itemName}");
            InventoryManager.Instance.AddItem(itemData); // �κ��丮�� �߰�
            Destroy(gameObject); // �ʵ忡�� ������ ����
        }
    }
}
