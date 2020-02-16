using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AngleNotification: MonoBehaviour
{
    private RawImage screen;
    private Transform cameraTransform;
    public bool fadeEnabled;
    // Start is called before the first frame update
    void Start()
    {
        fadeEnabled = false;
        screen = GameObject.FindObjectsOfType<RawImage>()[0];
        cameraTransform = GameObject.FindObjectsOfType<OVRCameraRig>()[0].centerEyeAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeEnabled)
        {

            // start fading to black if angle more than +/- 30 degrees
            // set the screen black if angle more than +/- 40 degrees
            float y;
            // TODO Implement x & z fade?
            //x = convert(cameraTransform.localRotation.eulerAngles.x);
            //z = convert(cameraTransform.localRotation.eulerAngles.z);
            y = convert(cameraTransform.localRotation.eulerAngles.y);
            if (Mathf.Abs(y) > 40)
            {
                screen.color = new Color32(0, 0, 0, 255);
            }
            else if (Mathf.Abs(y) > 30 )
            {
                screen.color = new Color32(0, 0, 0, (byte)((Mathf.Abs(y) - 30) * (255 / 10)));
            }
            else
            {
                screen.color = new Color32(0, 0, 0, 0);
            }
        }

    }

    private float convert(float angle)
    {
        // helpper
        //cameraTransform.localRotation.eulerAngles.x returs angles in 0 - 360, while unity displays them in range -180 - 180
        if (angle > 180)
        {
            angle = angle - 360;
        }
        return angle;
    }
}
