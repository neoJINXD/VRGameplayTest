using System;
using UnityEngine;

public class DebugHand : MonoBehaviour 
{
  [SerializeField] private InputHand hand;

  private void Start() 
  {
    InputManager.Instance.RegisterNotify<Action<Vector3>>(NotifyType.DeviceVelocity ,hand, HandleDeviceVelocity);
    InputManager.Instance.RegisterNotify<Action>(NotifyType.TiggerPressed, hand, HandleTriggerPressed);
    InputManager.Instance.RegisterNotify<Action>(NotifyType.TriggerReleased, hand, HandleTriggerReleased);
    InputManager.Instance.RegisterNotify<Action>(NotifyType.GripPressed, hand, HandleGripPressed);
  }

  private void HandleGripPressed()
  {
    Debug.Log($"{hand}: Grip Pressed");
  }

  private void HandleDeviceVelocity(Vector3 value)
  {
    Debug.Log($"{hand}: Velocity {value}");
  }

  private void HandleTriggerPressed()
  {
    Debug.Log($"{hand}: Trigger Pressed");
  }
  private void HandleTriggerReleased()
  {
    Debug.Log($"{hand}: Trigger Released");
  }
}
