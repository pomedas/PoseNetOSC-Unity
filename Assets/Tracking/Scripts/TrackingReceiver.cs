using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using System.Linq;
using System.Diagnostics;

public class TrackingReceiver : MonoBehaviour
{
    //GameObjects to be controlled with Posenet
    public GameObject nose;
    public GameObject leftShoulder;
    public GameObject rightShoulder;
    public GameObject leftElbow;
    public GameObject rightElbow;
    public GameObject leftWrist;
    public GameObject rightWrist;
    public GameObject leftHip;
    public GameObject rightHip;
    public GameObject leftKnee;
    public GameObject rightKnee;
    public GameObject leftAnkle;
    public GameObject rightAnkle;

    //OSC Variables
    private OSCReceiver _receiver;
    private const string _oscAddress = "/pose/0";

    //Dictionary to store pose data
    public Dictionary<string, Vector3> pose = new Dictionary<string, Vector3>();

    //The number of lines
    int lengthOfLineRenderer = 18;

    //Set active the line renderer
    public bool lineRenderOn = false; 

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Set up OSC receiver
        StartOSCReceiver();

        //Initialize pose
        StartPose();

        //Creates the lines
        if (lineRenderOn)
        {
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 2f;
            lineRenderer.positionCount = lengthOfLineRenderer;
        }

    }

    void StartOSCReceiver() {
        // Creating a receiver.
        _receiver = gameObject.AddComponent<OSCReceiver>();

        // Set local port.
        _receiver.LocalPort = 9876;

        // Bind "MessageReceived" method to special address.
        _receiver.Bind(_oscAddress, MessageReceived);
    }

    void StartPose() {
        pose.Add("nose", Vector3.zero);
        pose.Add("leftShoulder", Vector3.zero);
        pose.Add("rightShoulder", Vector3.zero);
        pose.Add("leftElbow", Vector3.zero);
        pose.Add("rightElbow", Vector3.zero);
        pose.Add("leftWrist", Vector3.zero);
        pose.Add("rightWrist", Vector3.zero);
        pose.Add("leftHip", Vector3.zero);
        pose.Add("rightHip", Vector3.zero);
        pose.Add("leftKnee", Vector3.zero);
        pose.Add("rightKnee", Vector3.zero);
        pose.Add("leftAnkle", Vector3.zero);
        pose.Add("rightAnkle", Vector3.zero);
    }
    

    // Update is called once per frame
    void Update()
    {
        //Update GameObjects positions that represents the joins in PoseNet
        //Note: these mappings are mirror, to obtain a correct side position when you see the character from the back
        nose.transform.position = pose["nose"];
        leftShoulder.transform.position = pose["rightShoulder"];
        rightShoulder.transform.position = pose["leftShoulder"];
        leftElbow.transform.position = pose["rightElbow"];
        rightElbow.transform.position = pose["leftElbow"];
        leftWrist.transform.position = pose["rightWrist"];
        rightWrist.transform.position = pose["leftWrist"];
        leftHip.transform.position = pose["rightHip"];
        rightHip.transform.position = pose["leftHip"];
        leftKnee.transform.position = pose["rightKnee"];
        rightKnee.transform.position = pose["leftKnee"];
        leftAnkle.transform.position = pose["rightAnkle"];
        rightAnkle.transform.position = pose["leftAnkle"];

        //If lineRenderOn = true draw the lines
        if (lineRenderOn)
        {
            //Update lines
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            var points = new Vector3[lengthOfLineRenderer];
            lineRenderer.SetPosition(0, leftWrist.transform.position);
            lineRenderer.SetPosition(1, leftElbow.transform.position);
            lineRenderer.SetPosition(2, leftShoulder.transform.position);
            lineRenderer.SetPosition(3, leftHip.transform.position);
            lineRenderer.SetPosition(4, leftKnee.transform.position);
            lineRenderer.SetPosition(5, leftAnkle.transform.position);
            lineRenderer.SetPosition(6, leftKnee.transform.position);
            lineRenderer.SetPosition(7, leftHip.transform.position);
            lineRenderer.SetPosition(8, rightHip.transform.position);
            lineRenderer.SetPosition(9, rightKnee.transform.position);
            lineRenderer.SetPosition(10, rightAnkle.transform.position);
            lineRenderer.SetPosition(11, rightKnee.transform.position);
            lineRenderer.SetPosition(12, rightHip.transform.position);
            lineRenderer.SetPosition(13, rightShoulder.transform.position);
            lineRenderer.SetPosition(14, leftShoulder.transform.position);
            lineRenderer.SetPosition(15, rightShoulder.transform.position);
            lineRenderer.SetPosition(16, rightElbow.transform.position);
            lineRenderer.SetPosition(17, rightWrist.transform.position);
        }

    }

    protected void MessageReceived(OSCMessage message)
    {
        List<OSCValue> list = message.Values;
        //UnityEngine.Debug.Log(list.Count);

        for(int i=0;i<list.Count; i+=3)
        {
            string key = "";
            Vector2 position = Vector3.zero; 

            OSCValue val0 = list.ElementAt(i);
            if (val0.Type == OSCValueType.String) key = val0.StringValue;
            OSCValue val1 = list.ElementAt(i+1);
            if (val1.Type == OSCValueType.Float) position.x = val1.FloatValue-250;
            OSCValue val2 = list.ElementAt(i+2);
            if (val2.Type == OSCValueType.Float) position.y = -(val2.FloatValue-250);

            if (pose.ContainsKey(key)) {
                pose[key] = position; 
            }
        }

    }

}
