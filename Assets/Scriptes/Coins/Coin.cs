using TMPro;
using UnityEngine;

public class Coin:MonoBehaviour
{
    [SerializeField] private int objectMoney;
    public int currentMoney { get; private set; }

    private void Awake()
    {
        
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        InfoCharacter.Instance.Coins += amount;
    }
}
