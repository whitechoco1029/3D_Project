using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;      // ������ �̸�
    public string description;   // ������ ����
    public Sprite icon;          // ������ ������
    public GameObject prefab;    // ������ ������ (�ʵ忡 ��ġ�� ��� ���)
}
