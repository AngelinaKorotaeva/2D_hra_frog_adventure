using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private TextMeshProUGUI coins;
    
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI health;

    [SerializeField] private GameObject imageObject1; 
    [SerializeField] private GameObject imageObject2;
    [SerializeField] private GameObject imageObject3;
    [SerializeField] private GameObject imageObject4;
    [SerializeField] private GameObject imageObject5;
    [SerializeField] private GameObject imageObject6;

    int gameObj = 1;
    int gameObj2 = 4;
    private bool update = false;

    int AttackMoney = 30;
    int HealthMoney = 30;
    public void MainMenu()
    {
        update = false;
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        attack.SetText("" + AttackMoney);
        health.SetText("" + HealthMoney);

        AttackMoney = PlayerPrefs.GetInt("AttackCost", 30);
        HealthMoney = PlayerPrefs.GetInt("HealthCost", 30);
        attack.SetText("" + (gameObj <= 3 ? AttackMoney.ToString() : ""));
        health.SetText("" + (gameObj2 <= 6 ? HealthMoney.ToString() : ""));

        gameObj = PlayerPrefs.GetInt("AttackProgress", 1);
        gameObj2 = PlayerPrefs.GetInt("HealthProgress", 4);

        if (gameObj > 1) imageObject1.SetActive(true);
        if (gameObj > 2) imageObject2.SetActive(true);
        if (gameObj > 3) imageObject3.SetActive(true);

        if (gameObj2 > 4) imageObject4.SetActive(true);
        if (gameObj2 > 5) imageObject5.SetActive(true);
        if (gameObj2 > 6) imageObject6.SetActive(true);
    }
    private void Update()
    {
        if (!update && InfoCharacter.Instance != null)
        {
            coins.SetText("" + InfoCharacter.Instance.CoinsCompleted);
            update = true;
        }
    }
    public void AttackUpdate()
    {
        if (InfoCharacter.Instance.CoinsCompleted >= AttackMoney)
        {
            if (gameObj <= 3) {
                InfoCharacter.Instance.CoinsCompleted -= AttackMoney;
                InfoCharacter.Instance.AttackValue++;
                coins.SetText("" + InfoCharacter.Instance.CoinsCompleted);
                AttackMoney = AttackMoney * 2;
                attack.SetText("" + AttackMoney);
                if (gameObj == 1)
                {
                    imageObject1.SetActive(true);
                    gameObj++;
                }
                else if (gameObj == 2)
                {
                    imageObject2.SetActive(true);
                    gameObj++;
                }
                else if (gameObj == 3)
                {
                    imageObject3.SetActive(true);
                    gameObj++;
                    attack.SetText("");
                }
                PlayerPrefs.SetInt("AttackCost", AttackMoney);
                PlayerPrefs.SetInt("AttackProgress", gameObj);
                PlayerPrefs.SetInt("HealthProgress", gameObj2);
                PlayerPrefs.Save();
            }
        }
    }
    public void HealthUpdate()
    {
        if (InfoCharacter.Instance.CoinsCompleted >= HealthMoney)
        {
            if (gameObj2 <= 6)
            {
                InfoCharacter.Instance.CoinsCompleted -= HealthMoney;
                InfoCharacter.Instance.Lives++;
                coins.SetText("" + InfoCharacter.Instance.CoinsCompleted);
                HealthMoney = HealthMoney * 2;
                health.SetText("" + HealthMoney);
                if (gameObj2 == 4)
                {
                    imageObject4.SetActive(true);
                    gameObj2++;
                    InfoCharacter.Instance.TotalLives = InfoCharacter.Instance.TotalLives + 0.17f;
                }
                else if (gameObj2 == 5)
                {
                    imageObject5.SetActive(true);
                    gameObj2++;
                    InfoCharacter.Instance.TotalLives = InfoCharacter.Instance.TotalLives + 0.06f;
                }
                else if (gameObj2 == 6)
                {
                    imageObject6.SetActive(true);
                    gameObj2++;
                    health.SetText("");
                    InfoCharacter.Instance.TotalLives = InfoCharacter.Instance.TotalLives + 0.17f;
                }
                
                PlayerPrefs.SetInt("HealthCost", HealthMoney);
                PlayerPrefs.SetInt("AttackProgress", gameObj);
                PlayerPrefs.SetInt("HealthProgress", gameObj2);
                PlayerPrefs.Save();
            }
        }
    }
}
