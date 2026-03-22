using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinBar:MonoBehaviour
{
    [SerializeField] private Coin playerCoin;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private Image coinImage;
    private void Start()
    {
        //playerCoin = GetComponent<Coin>();
        //coinValue = GetComponent<CoinCollectible>();
        coins.SetText("0");
    }

    private void Update()
    {
        coins.SetText(playerCoin.currentMoney.ToString());
    }
}
