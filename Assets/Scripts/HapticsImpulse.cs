using System;
using UnityEngine;

[Serializable]
public class HapticsImpulse
{
    [SerializeField] private int channel;
    [SerializeField] private float amplitude;
    [SerializeField] private float duration;

    public void Execute(InputHand hand)
    {
        InputManager.Instance.SentHaptics(hand, channel, amplitude, duration);
    }
}
