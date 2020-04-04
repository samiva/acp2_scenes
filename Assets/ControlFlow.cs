using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ControlFlow : MonoBehaviour
{

    private List<int> delays = new List<int>();
    private string fpath = (System.IO.Directory.GetCurrentDirectory()) + "/delays.txt";
    private bool delayEnabled;
    public bool fading;
    public int fadeDirection; // 0 fade to black, -1 fading out
    public float startTime;
    public int beeps;
    private DelayedTracking delayedTracking;
    private AngleNotification angleFadeController;
    private RawImage screen;
    private AudioSource beep;
    private int secondsPerBeep = 1;
    private int fadeDuration = 1;
    // Start is called before the first frame update
    void Start()
    {
        screen = GameObject.FindObjectsOfType<RawImage>()[0];
        delayedTracking = GameObject.FindObjectsOfType<OVRCameraRig>()[0].GetComponent<DelayedTracking>();
        angleFadeController = GameObject.FindObjectsOfType<OVRCameraRig>()[0].GetComponent<AngleNotification>();
        beep = GameObject.FindObjectsOfType<OVRCameraRig>()[0].centerEyeAnchor.GetComponent<AudioSource>();
        delayEnabled = false;
        fading = true;
        fadeDirection = -1;
        startTime = 3.0f;

        if (!File.Exists(fpath))
        {
            StreamWriter sw = new StreamWriter(fpath);
            sw.WriteLine("0,1,2");
            sw.Close();
        }
        StreamReader sr = new StreamReader(fpath);
        string[] contents = sr.ReadLine().Split(',');
        sr.Close();
        foreach(string delay in contents)
        {
            int val = 0;
            if (int.TryParse(delay, out val))
            {
                delays.Add(val);
            }
        }
        startFade();
    }

    // Update is called once per frame
    void Update()
    {
        if (delays.Count == 0 && !delayEnabled)
        {
            if (screen.color.a < 1)
            {
                fadeToBlack();
            }
            else
            {
                Application.Quit();
            }
            return;
        }
        if (fading)
        {
            fadeToBlack();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!delayEnabled)
            {
                enableDelay();
                if (screen.color.a >= 1)
                {
                    startFade();
                }
            }
            else
            {
                disableDelay();
            }
        }
    }

    void fadeToBlack()
    {
        float scale = (Time.time - startTime) - (fadeDuration - 1);
        if (scale >= 1)
        {
            fading = false;
            scale = 1.0f;
        }
        byte a = (byte)(Mathf.Abs(scale + fadeDirection) * 255);
        screen.color = new Color32(0, 0, 0, a);

    }

    void playBeep()
    {
        beeps += 1;
        beep.Play();
        Debug.Log("beep");
    }


    void startFade()
    {
        angleFadeController.fadeEnabled = false;
        if (screen.color.a >= 1)
        {
            fadeDirection = -1;
        }
        else
        {
            fadeDirection = 0;
        }
        fading = true;
        startTime = Time.time;
    }

    void enableDelay()
    {
        startTime = Time.time;
        delayEnabled = true;
        if (delays.Count > 0)
        {
            delayedTracking.enableDelayed(delays[0]);
            delays.RemoveAt(0);
        }
      
    }

    void disableDelay()
    {
        beeps = 0;
        delayEnabled = false;
        delayedTracking.disableDelayed();
        startFade();
    }
}
