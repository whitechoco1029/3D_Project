using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Scrollbar healthBar; // Scrollbar UI 연결

    public void SetHealth(float currentHealth, float maxHealth)
    {
        float healthRatio = currentHealth / maxHealth; // 체력 비율 계산 (0.0 ~ 1.0)
        healthRatio = Mathf.Clamp(healthRatio, 0f, 1f); // 값이 0~1 범위를 넘지 않도록 제한

        healthBar.size = healthRatio; // Scrollbar의 Size 값 변경
    }
}
