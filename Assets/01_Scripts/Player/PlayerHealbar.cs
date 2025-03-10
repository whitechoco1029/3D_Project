using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 1.00f; // 1.00 = 100%
    private float currentHealth;
    public HealthBar healthBar; // 체력바 연결

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // 체력 감소 테스트
        {
            TakeDamage(0.1f); // 10% 감소
        }

        if (Input.GetKeyDown(KeyCode.H)) // 체력 회복 테스트
        {
            Heal(0.1f); // 10% 회복
        }
    }
}
