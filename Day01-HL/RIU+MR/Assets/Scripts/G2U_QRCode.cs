using RhinoInside.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;
using System;

public class G2U_QRCode : MonoBehaviour
{
    public static G2U_QRCode instance;
    public static List<RhinoMarker> MarkerList = new List<RhinoMarker> ();
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!Startup.isLoaded)
        {
            Startup.Init();
        }

        Rhino.Runtime.HostUtils.RegisterNamedCallback("FromGHMarkerList", FromGHMarkerList);

    }
    void FromGHMarkerList(object sender, Rhino.Runtime.NamedParametersEventArgs args)
    {
        if (Application.isPlaying)
        {
            Debug.Log("Get message from GH!");
            if(args.TryGetString("position", out string position))
            {
                args.TryGetString("rotation", out string rotation);
                args.TryGetString("content", out string content);
                
                
                // Split the string by comma
                string[] position_values = position.Split(',');
                // Parse each substring to float
                float p_x = float.Parse(position_values[0]);
                float p_y = float.Parse(position_values[1]);
                float p_z = float.Parse(position_values[2]);
                // Create a new Vector3
                Rhino.Geometry.Point3d p = new Rhino.Geometry.Point3d(p_x, p_y, p_z);
                Vector3 p_ok = p.ToHost();

                // Remove braces and split the string by space
                string[] components = rotation.Replace("{", "").Replace("}", "").Split(' ');
                // Dictionary to store the parsed values
                List<float> q_string = new List<float>();
                // Loop through each component and extract the number after the colon
                foreach (string component in components)
                {
                    // Split the component by colon
                    string[] parts = component.Split(':');
                    // Ensure there are two parts (label and value)
                    if (parts.Length == 2)
                    {
                        float value = float.Parse(parts[1]); // Parse the value as float
                        q_string.Add(value);
                    }
                }

                // Create a new Quaternion
                Quaternion q = new Quaternion(q_string[0], q_string[1], q_string[2], q_string[3]);
                Quaternion q_ok = q.ToHost();

                Debug.Log($"position: {p_ok}, rotation: {q_ok}, content: {content}.");
                //Only works with single marker for now.
                RhinoMarker marker = new RhinoMarker(p_ok, q_ok, content);
                MarkerList.Clear();
                MarkerList.Add(marker);
            }
        }
    }
}
public class RhinoMarker
{
    public Vector3 position;
    public Quaternion rotation;
    public string Content;
    public RhinoMarker(Vector3 position, Quaternion rotation, string content)
    {
        this.position = position;
        this.rotation = rotation;
        Content = content;
    }
}
