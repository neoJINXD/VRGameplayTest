using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour
{
    [SerializeField] private UnityEvent onGrab;
    [SerializeField] private UnityEvent onLetGo;
    public GrabManager CurrentGrabber { get; private set; }


    public void Grab(GrabManager grabManager)
    {
        if (CurrentGrabber != null)
        {
            CurrentGrabber.LostControlOfGrabbable(this);
        }

        CurrentGrabber = grabManager;
        onGrab.Invoke();
    }

    public void LetGo()
    {
        onLetGo.Invoke();
        CurrentGrabber = null;
    }

    public void RegisterGrabFeature(GrabFeature grabFeature)
    {
        onGrab.AddListener(grabFeature.OnGrab);
        onLetGo.AddListener(grabFeature.OnLetGo);
    }
}
