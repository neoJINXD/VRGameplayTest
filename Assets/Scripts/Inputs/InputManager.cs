using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }
    private VRControls controls;

    // Setting up Observers for each relevant input from the controllers
    private List<Action<Vector3>>[] devicePositionNotifies = new List<Action<Vector3>>[2];
    private List<Action<Quaternion>>[] deviceRotationNotifies = new List<Action<Quaternion>>[2];
    private List<Action<Vector3>>[] deviceVelocityNotifies = new List<Action<Vector3>>[2];
    private List<Action<Vector3>>[] deviceAngularVelocityNotifies = new List<Action<Vector3>>[2];
    private List<Action>[] gripPressedNotifies = new List<Action>[2];
    private List<Action>[] gripReleasedNotifies = new List<Action>[2];
    private List<Action>[] triggerPressedNotifies = new List<Action>[2];
    private List<Action>[] triggerReleasedNotifies = new List<Action>[2];
    private List<Action<Vector2>>[] thumbstickDirectionNotifies = new List<Action<Vector2>>[2];

    private Dictionary<NotifyType, object> notifyMap = new Dictionary<NotifyType, object>();


    public void RegisterNotify<T>(NotifyType type, InputHand hand, T notify)
    {
        (notifyMap[type] as List<T>[])[(int)hand].Add(notify);
    }
    public void UnregisterNotify<T>(NotifyType type, InputHand hand, T notify)
    {
        (notifyMap[type] as List<T>[])[(int)hand].Remove(notify);
    }

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            controls = new VRControls();

            PopulateDefaultNotify();
        } 
        else if (Instance != this) 
        {
            Destroy(this);
        }
    }

    private void PopulateDefaultNotify() 
    {
        for (int i = 0; i < 2; i++)
        {
            devicePositionNotifies[i] = new List<Action<Vector3>>();
            deviceRotationNotifies[i] = new List<Action<Quaternion>>();
            deviceVelocityNotifies[i] = new List<Action<Vector3>>();
            deviceAngularVelocityNotifies[i] = new List<Action<Vector3>>();

            gripPressedNotifies[i] = new List<Action>();
            gripReleasedNotifies[i] = new List<Action>();

            triggerPressedNotifies[i] = new List<Action>();
            triggerReleasedNotifies[i] = new List<Action>();

            thumbstickDirectionNotifies[i] = new List<Action<Vector2>>();
        }

        notifyMap.Add(NotifyType.DevicePosition, devicePositionNotifies);
        notifyMap.Add(NotifyType.DeviceRotation, deviceRotationNotifies);
        notifyMap.Add(NotifyType.DeviceVelocity, deviceVelocityNotifies);
        notifyMap.Add(NotifyType.DeviceAngularVelocity, deviceAngularVelocityNotifies);
        notifyMap.Add(NotifyType.GripPressed, gripPressedNotifies);
        notifyMap.Add(NotifyType.GripReleased, gripReleasedNotifies);
        notifyMap.Add(NotifyType.TiggerPressed, triggerPressedNotifies);
        notifyMap.Add(NotifyType.TriggerReleased, gripPressedNotifies);
        notifyMap.Add(NotifyType.ThumbstickDiction, thumbstickDirectionNotifies);
    }

    private void Start() 
    {
        // input binding for controls
        // Right
        controls.RightHand.Position.performed += ctx => TriggerNotifies(devicePositionNotifies[(int)InputHand.Right], ctx.ReadValue<Vector3>());
        controls.RightHand.Rotation.performed += ctx => TriggerNotifies(deviceRotationNotifies[(int)InputHand.Right], ctx.ReadValue<Quaternion>());
        controls.RightHand.Velocity.performed += ctx => TriggerNotifies(deviceVelocityNotifies[(int)InputHand.Right], ctx.ReadValue<Vector3>());
        controls.RightHand.AngularVelocity.performed += ctx => TriggerNotifies(deviceAngularVelocityNotifies[(int)InputHand.Right], ctx.ReadValue<Vector3>());

        controls.RightHand.Grip.performed += _ => TriggerActionNotifies(gripPressedNotifies[(int)InputHand.Right]);
        controls.RightHand.Grip.canceled += _ => TriggerActionNotifies(gripReleasedNotifies[(int)InputHand.Right]);
        
        controls.RightHand.Trigger.performed += _ => TriggerActionNotifies(triggerPressedNotifies[(int)InputHand.Right]);
        controls.RightHand.Trigger.canceled += _ => TriggerActionNotifies(triggerReleasedNotifies[(int)InputHand.Right]);
        
        controls.RightHand.ThumbstickDirection.performed += ctx => TriggerNotifies(thumbstickDirectionNotifies[(int)InputHand.Right], ctx.ReadValue<Vector2>());
        
        
        // Left
        controls.LeftHand.Position.performed += ctx => TriggerNotifies(devicePositionNotifies[(int)InputHand.Left], ctx.ReadValue<Vector3>());
        controls.LeftHand.Rotation.performed += ctx => TriggerNotifies(deviceRotationNotifies[(int)InputHand.Left], ctx.ReadValue<Quaternion>());
        controls.LeftHand.Velocity.performed += ctx => TriggerNotifies(deviceVelocityNotifies[(int)InputHand.Left], ctx.ReadValue<Vector3>());
        controls.LeftHand.AngularVelocity.performed += ctx => TriggerNotifies(deviceAngularVelocityNotifies[(int)InputHand.Left], ctx.ReadValue<Vector3>());

        controls.LeftHand.Grip.performed += _ => TriggerActionNotifies(gripPressedNotifies[(int)InputHand.Left]);
        controls.LeftHand.Grip.canceled += _ => TriggerActionNotifies(gripReleasedNotifies[(int)InputHand.Left]);
        
        controls.LeftHand.Trigger.performed += _ => TriggerActionNotifies(triggerPressedNotifies[(int)InputHand.Left]);
        controls.LeftHand.Trigger.canceled += _ => TriggerActionNotifies(triggerReleasedNotifies[(int)InputHand.Left]);
        
        controls.LeftHand.ThumbstickDirection.performed += ctx => TriggerNotifies(thumbstickDirectionNotifies[(int)InputHand.Left], ctx.ReadValue<Vector2>());


        controls.Enable();
        controls.RightHand.Enable();
        controls.LeftHand.Enable();
    }

    private void TriggerNotifies<T>(List<Action<T>> notifies, T value)
    {
        foreach (var notify in notifies)
        {
            notify?.Invoke(value);
        }
    }
    private void TriggerActionNotifies(List<Action> notifies)
    {
        foreach (var notify in notifies)
        {
            notify?.Invoke();
        }
    }
}
