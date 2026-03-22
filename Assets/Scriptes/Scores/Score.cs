using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private int objectScore;
    public int currentScore { get; private set; }

    private void Awake()
    {

    }

    public void AddScore(int score)
    {
        currentScore += score;
        InfoCharacter.Instance.Score += score;
    }
}
