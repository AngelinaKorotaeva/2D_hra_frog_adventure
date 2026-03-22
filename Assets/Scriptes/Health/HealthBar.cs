using UnityEngine;
using UnityEngine.UI;

public class HealthBar: MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount =  InfoCharacter.Instance.TotalLives;
    }

    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 6; 
    }
}