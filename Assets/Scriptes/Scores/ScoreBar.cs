using TMPro;
using UnityEngine;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private Score playerScore;
    [SerializeField] private TextMeshProUGUI score;
    private void Start()
    {
        //playerCoin = GetComponent<Coin>();
        //coinValue = GetComponent<CoinCollectible>();
        score.SetText("0");
    }

    private void Update()
    {
        score.SetText(playerScore.currentScore.ToString());
    }
}
