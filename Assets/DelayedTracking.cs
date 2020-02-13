using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DelayedTracking : MonoBehaviour
{  
    OVRCameraRig cam;

    private int DELAY = 10; // in frames
    private bool delayEnabled;

    private Queue<Quaternion> rotationQueue;
    private Queue<Vector3> positionQueue;

    private Quaternion prevRotation, currentRotation, startRotation;
    private Vector3 prevPosition, currentPosition, startPosition;
    // Start is called before the first frame update
     void Start()
    {
        delayEnabled = false;
        rotationQueue = new Queue<Quaternion>();
        positionQueue = new Queue<Vector3>();
        cam = GameObject.FindObjectsOfType<OVRCameraRig>()[0];
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!delayEnabled)
            {
                enableDelayed(DELAY);
            }
            else
            {
                disableDelayed();
            }
        }
        if (delayEnabled)
        {
			rotationQueue.Enqueue(cam.centerEyeAnchor.localRotation);
            positionQueue.Enqueue(cam.centerEyeAnchor.localPosition);
            if (rotationQueue.Count < DELAY)
            {
                currentRotation = Quaternion.Inverse(cam.centerEyeAnchor.localRotation) * startRotation;
                currentPosition = startPosition;
            }
            else
            {
                currentRotation = Quaternion.Inverse(cam.centerEyeAnchor.localRotation) * rotationQueue.Dequeue();
                currentPosition = positionQueue.Dequeue();
            }
        }
        else
        {
            currentRotation = Quaternion.Inverse(prevRotation) * cam.centerEyeAnchor.localRotation;
            currentPosition = cam.centerEyeAnchor.localPosition;
        }

        cam.trackingSpace.localRotation = currentRotation;
        cam.trackingSpace.localPosition = currentPosition;
        prevRotation = cam.centerEyeAnchor.localRotation;
        prevPosition = cam.centerEyeAnchor.localPosition;
    }

    public void enableDelayed(int delay)
    {
        DELAY = delay;
        delayEnabled = true;
        startRotation = cam.centerEyeAnchor.localRotation;
        startPosition = cam.centerEyeAnchor.localPosition;
        rotationQueue = new Queue<Quaternion>();
        positionQueue = new Queue<Vector3>();
        return;
    }

    public void disableDelayed()
    {
        delayEnabled = false;
        rotationQueue = new Queue<Quaternion>();
        positionQueue = new Queue<Vector3>();
        return;
    }
}
