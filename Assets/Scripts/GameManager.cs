using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int score { get; private set; } = 0;

    public void IncreaseScore()
    {
        score += 1;
    }
}
