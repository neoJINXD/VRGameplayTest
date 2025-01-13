using UnityEngine;

public class RangeGrabTarget : MonoBehaviour
{
    [SerializeField] private Transform targetIndicator;
    [field: SerializeField] public Grabbable Grabbable { get; private set; }


    public void Target()
    {
        targetIndicator.gameObject.SetActive(true);
    }

    public void Untarget()
    {
        targetIndicator.gameObject.SetActive(false);
    }

    private void Start()
    {
        targetIndicator.gameObject.SetActive(false);
    }
}
