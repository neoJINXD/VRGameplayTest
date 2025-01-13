public class SnapGrabFeature : GrabFeature
{
    protected override void HandleUpdate()
    {
        if (IsGrabbed)
        {
            transform.position = Grabbable.CurrentGrabber.transform.position;
            transform.rotation = Grabbable.CurrentGrabber.transform.rotation;
        }
    }
}
