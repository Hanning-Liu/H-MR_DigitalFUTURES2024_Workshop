using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.Input;
using System;

public class U2G_Eye : MonoBehaviour
{
    [SerializeField]
    private GazeInteractor gazeInteractor;

    Vector3 pos;
    Vector3 dir;
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        pos = gazeInteractor.rayOriginTransform.position;
        dir = gazeInteractor.rayOriginTransform.forward;
        SendHLEyeGaze(pos, dir);
    }

    private void SendHLEyeGaze(Vector3 pos, Vector3 dir)
    {
        using (var args = new Rhino.Runtime.NamedParametersEventArgs())
        {
            Rhino.Geometry.Point3d pos_ok = pos.ToRhino();
            Rhino.Geometry.Vector3d vForward = new Rhino.Geometry.Vector3d(dir.x, dir.z, dir.y);

            args.Set("HLEyePos", pos_ok);
            args.Set("HLEyeForward", vForward);
            Rhino.Runtime.HostUtils.ExecuteNamedCallback("ToGH_EyeGaze", args);
        }
    }
}
