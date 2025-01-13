using UnityEngine;

public class HapticImpulseGrabFeature : GrabFeature
{
    [SerializeField] private HapticsImpulse hapticsImpulse;
    protected override void HandleGrabbed()
    {
        hapticsImpulse.Execute(Grabbable.CurrentGrabber.Hand);
    }
}
