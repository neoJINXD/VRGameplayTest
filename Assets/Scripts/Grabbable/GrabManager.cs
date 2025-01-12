using System;
using UnityEngine;

public class GrabManager : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }
    public Vector3 AngularVelocity { get; private set; }

    [SerializeField] private InputHand hand;
    [SerializeField] private float grabRadius;
    [SerializeField] private LayerMask grabLayer;

    private Grabbable currentlyGrabbing;

    public void LostControlOfGrabbable(Grabbable grabbable)
    {
        if (currentlyGrabbing == grabbable)
        {
            currentlyGrabbing = null;
        }
    }

    private void Start()
    {
        InputManager.Instance.RegisterNotify<Action>(NotifyType.TiggerPressed, hand, HandleTriggerPressed);
        InputManager.Instance.RegisterNotify<Action>(NotifyType.TriggerReleased, hand, HandleTriggerReleased);
        InputManager.Instance.RegisterNotify<Action>(NotifyType.GripPressed, hand, HandleGripPressed);
        InputManager.Instance.RegisterNotify<Action>(NotifyType.GripReleased, hand, HandleGripReleased);
        InputManager.Instance.RegisterNotify<Action<Vector3>>(NotifyType.DeviceVelocity, hand, HandleVelocity);
        InputManager.Instance.RegisterNotify<Action<Vector3>>(NotifyType.DeviceAngularVelocity, hand, HandleAngularVelocity);
    }

    private void HandleTriggerPressed()
    {
        HandleGrabbingObject();
    }

    private void HandleTriggerReleased()
    {
        HandleLettingGoObject();
    }

    private void HandleGripPressed()
    {
        HandleGrabbingObject();
    }

    private void HandleGripReleased()
    {
        HandleLettingGoObject();
    }

    private void HandleVelocity(Vector3 velocity)
    {
        Velocity = velocity;
    }

    private void HandleAngularVelocity(Vector3 angularVelocity)
    {
        AngularVelocity = angularVelocity;
    }

    private void HandleGrabbingObject()
    {
        var collisions = Physics.OverlapSphere(transform.position, grabRadius, grabLayer);

        // find closest object to grab onto
        Grabbable grabbable = null;
        float closestDistance = float.PositiveInfinity;

        foreach (var collision in collisions)
        {
            var distance = (collision.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                grabbable = collision.GetComponentInParent<Grabbable>();
                closestDistance = distance;
            }
        }

        // found a grabbable object
        if (grabbable != null)
        {
            Grab(grabbable);
        }
    }

    private void Grab(Grabbable grabbable)
    {
        if (currentlyGrabbing == grabbable)
        {
            return;
        }

        if (currentlyGrabbing != null)
        {
            currentlyGrabbing.LetGo();
        }

        currentlyGrabbing = grabbable;
        currentlyGrabbing.Grab(this);
    }

    private void HandleLettingGoObject()
    {
        if (currentlyGrabbing != null)
        {
            currentlyGrabbing.LetGo();
            currentlyGrabbing = null;
        }
    }

}