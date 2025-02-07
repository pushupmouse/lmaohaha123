using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _healthBar;
    [SerializeField] private Transform _fill;
    
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        Vector3 newScale = _fill.localScale;
        newScale.x = healthPercentage;
        _fill.localScale = newScale;
    }
}
