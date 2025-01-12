using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public static GameManager Instance { get; set; }

    public int score { get; private set; } = 0;

    public void IncreaseScore(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreText.text = $"Score: {score}";
    }

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else if (Instance != this) 
        {
            Destroy(this);
        }
    }
}
