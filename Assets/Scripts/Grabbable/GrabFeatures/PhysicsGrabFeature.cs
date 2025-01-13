using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsGrabFeature : GrabFeature
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float throwSpeedMultiplier = 1f;


    protected override void HandleGrabbed()
    {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    protected override void HandleLetGo()
    {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;

        // actual "throwing" part
        rigidbody.velocity = Grabbable.CurrentGrabber.Velocity * throwSpeedMultiplier;
        rigidbody.angularVelocity = Grabbable.CurrentGrabber.AngularVelocity;
    }

    protected override void HandleStart()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }
}
