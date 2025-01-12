using UnityEngine;

public class BasketBehaviour : MonoBehaviour
{
    [SerializeField] private int scoreAmount = 1;
    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.CompareTag("Ball"))
        {
            GameManager.Instance.IncreaseScore(scoreAmount);
        }
    }
}
