using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DelayedTracking : MonoBehaviour
{  
    OVRCameraRig cam;

    private int DELAY = 100; // in frames
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
        if (delayEnabled)
        {
			rotationQueue.Enqueue(cam.centerEyeAnchor.localRotation);
            if (rotationQueue.Count <= DELAY)
            {
                currentRotation = Quaternion.Inverse(cam.centerEyeAnchor.localRotation) * startRotation;
            }
            else
            {
                currentRotation = Quaternion.Inverse(cam.centerEyeAnchor.localRotation) * rotationQueue.Dequeue();
            }
        }
        else
        {
            currentRotation = Quaternion.identity;//Quaternion.Inverse(prevRotation) * cam.centerEyeAnchor.localRotation;
        }

        cam.trackingSpace.localRotation = currentRotation;
        prevRotation = cam.centerEyeAnchor.localRotation;
    }

    public void enableDelayed(int delay)
    {
        DELAY = delay;
        delayEnabled = true;
        startRotation = cam.centerEyeAnchor.localRotation;
        startPosition = cam.centerEyeAnchor.localPosition;
        rotationQueue.Clear();
        positionQueue.Clear();
        return;
    }

    public void disableDelayed()
    {
        delayEnabled = false;
        rotationQueue.Clear();
        positionQueue.Clear();
        return;
    }
}
