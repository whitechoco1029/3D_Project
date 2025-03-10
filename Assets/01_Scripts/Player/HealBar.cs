using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Scrollbar healthBar; // Scrollbar UI ����

    public void SetHealth(float currentHealth, float maxHealth)
    {
        float healthRatio = currentHealth / maxHealth; // ü�� ���� ��� (0.0 ~ 1.0)
        healthRatio = Mathf.Clamp(healthRatio, 0f, 1f); // ���� 0~1 ������ ���� �ʵ��� ����

        healthBar.size = healthRatio; // Scrollbar�� Size �� ����
    }
}
