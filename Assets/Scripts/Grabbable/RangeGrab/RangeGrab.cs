using UnityEngine;

[RequireComponent(typeof(GrabManager))]
public class RangeGrab : MonoBehaviour
{
    [SerializeField] private LayerMask rangedGrabLayer;
    [SerializeField] private float range = 5f;
    public float Range { get { return range; } }
    public RangeGrabTarget CurrentRangeGrabTarget { get; private set; }

    private GrabManager grabManager;


    public bool HasActiveRangedGrabTarget()
    {
        return CurrentRangeGrabTarget != null;
    }

    private void Start()
    {
        grabManager = GetComponent<GrabManager>();
    }

    private void Update()
    {
        // already grabbing something, ignore range targets
        if (grabManager.HasCurrentGrabbable())
        {
            return;
        }

        if (Physics.Raycast(new Ray(transform.position, transform.forward), out var hit, range, rangedGrabLayer))
        {
            var rangedGrabTarget = hit.collider.GetComponent<RangeGrabTarget>();
            if (rangedGrabTarget != null && rangedGrabTarget != CurrentRangeGrabTarget)
            {
                if (CurrentRangeGrabTarget != null)
                {
                    CurrentRangeGrabTarget.Untarget();
                }

                CurrentRangeGrabTarget = rangedGrabTarget;
                CurrentRangeGrabTarget.Target();
            }
        }
        else if (CurrentRangeGrabTarget != null)
        {
            CurrentRangeGrabTarget.Untarget();
            CurrentRangeGrabTarget = null;
        }
    }
}
