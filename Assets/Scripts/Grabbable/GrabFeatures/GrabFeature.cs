using UnityEngine;
public abstract class GrabFeature : MonoBehaviour
{
    [field: SerializeField] protected Grabbable Grabbable { get; private set; }
    public bool IsGrabbed { get; private set; }


    public void OnGrab()
    {
        IsGrabbed = true;
        HandleGrabbed();
    }
    public void OnLetGo()
    {
        IsGrabbed = false;
        HandleLetGo();
    }

    protected virtual void HandleGrabbed() { }
    protected virtual void HandleLetGo() { }

    private void Start()
    {
        if (Grabbable == null)
        {
            Grabbable = GetComponent<Grabbable>();
        }

        if (Grabbable != null)
        {
            Grabbable.RegisterGrabFeature(this);
        }

        HandleStart();
    }

    private void Update()
    {
        HandleUpdate();
    }

    protected virtual void HandleStart() { }
    protected virtual void HandleUpdate() { }
}
