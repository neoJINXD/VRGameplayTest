using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GrabManager))]
[RequireComponent(typeof(RangeGrab))]
public class RangeGrabAction : MonoBehaviour
{
    [SerializeField] private float rangeGrabDuration = 0.25f;

    private GrabManager grabManager;
    private RangeGrab rangeGrab;
    private RangeGrabTarget currentRangeGrabTarget;
    private bool excecutingRangeGrab;
    private bool renderAimLine = true;
    private bool gripPressed = false;
    private LineRenderer line;


    private void Start()
    {
        grabManager = GetComponent<GrabManager>();
        rangeGrab = GetComponent<RangeGrab>();
        line = GetComponent<LineRenderer>();

        InputManager.Instance.RegisterNotify<Action>(NotifyType.GripPressed, grabManager.Hand, HandleGripPressed);
        InputManager.Instance.RegisterNotify<Action>(NotifyType.GripReleased, grabManager.Hand, HandleGripReleased);
        InputManager.Instance.RegisterNotify<Action>(NotifyType.TiggerPressed, grabManager.Hand, HandleTriggerPressed);
    }

    private void Update()
    {
        // Line renderer to help aim range grab
        if (renderAimLine)
        {
            var positions = new Vector3[2];
            positions[0] = transform.position;
            positions[1] = transform.position + (transform.TransformDirection(Vector3.forward) * rangeGrab.Range);
            line.SetPositions(positions);
        }

        // not holding onto something and aiming at a grabbable
        if (gripPressed && !grabManager.HasCurrentGrabbable() && rangeGrab.HasActiveRangedGrabTarget())
        {
            currentRangeGrabTarget = rangeGrab.CurrentRangeGrabTarget;
        }
    }

    private void HandleGripPressed()
    {
        gripPressed = true;
    }

    private void HandleGripReleased()
    {
        if (currentRangeGrabTarget != null)
        {
            currentRangeGrabTarget = null;

            if (excecutingRangeGrab)
            {
                StopCoroutine(ExecuteRangeGrab());
            }
        }

        renderAimLine = true;
        line.enabled = true;

        gripPressed = false;
    }

    private void HandleTriggerPressed()
    {
        if (currentRangeGrabTarget != null && !excecutingRangeGrab)
        {
            renderAimLine = false;
            line.enabled = false;
            StartCoroutine(ExecuteRangeGrab());
        }
    }

    private IEnumerator ExecuteRangeGrab()
    {
        var executionTime = 0f;
        var originPosition = currentRangeGrabTarget.transform.position;
        var originRotation = currentRangeGrabTarget.transform.rotation;

        excecutingRangeGrab = true;

        // disable physics
        if (currentRangeGrabTarget.transform.parent.GetComponent<Rigidbody>())
        {
            currentRangeGrabTarget.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            currentRangeGrabTarget.transform.parent.GetComponent<Rigidbody>().useGravity = false;
        }

        // lerp ball into hand
        while (executionTime < rangeGrabDuration)
        {
            if (currentRangeGrabTarget == null)
            {
                break;
            }

            executionTime += Time.deltaTime;

            var percentageTime = executionTime / rangeGrabDuration;

            currentRangeGrabTarget.transform.parent.position = Vector3.Lerp(originPosition, grabManager.transform.position, percentageTime * percentageTime);
            currentRangeGrabTarget.transform.parent.rotation = Quaternion.Lerp(originRotation, grabManager.transform.rotation, percentageTime * percentageTime);

            yield return new WaitForEndOfFrame();
        }

        if (currentRangeGrabTarget != null)
        {
            // reenable physics
            if (currentRangeGrabTarget.transform.parent.GetComponent<Rigidbody>())
            {
                currentRangeGrabTarget.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
                currentRangeGrabTarget.transform.parent.GetComponent<Rigidbody>().useGravity = true;
            }

            grabManager.Grab(currentRangeGrabTarget.Grabbable);
            currentRangeGrabTarget = null;
        }

        excecutingRangeGrab = false;
    }
}
