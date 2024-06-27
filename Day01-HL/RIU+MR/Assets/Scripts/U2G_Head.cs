using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U2G_Head : MonoBehaviour
{
    GameObject cam;
    Vector3 position;

    Vector3 vRight;
    Vector3 vUp;
    private void Start()
    {
         cam = GameObject.Find("Main Camera");
    }
    void Update()
    {
        position = cam.transform.position;
        vRight = cam.transform.right;
        vUp = cam.transform.up;
        SendCamPose(position, vRight, vUp);
    }

    public void SendCamPose(Vector3 pos, Vector3 vRight, Vector3 vUp)
    {
        using (var args = new Rhino.Runtime.NamedParametersEventArgs())
        {
            Rhino.Geometry.Point3d pos_ok = pos.ToRhino();
            Rhino.Geometry.Vector3d vRight_ok = new Rhino.Geometry.Vector3d(vRight.x, vRight.z, vRight.y);
            Rhino.Geometry.Vector3d vUp_ok = new Rhino.Geometry.Vector3d(vUp.x, vUp.z, vUp.y);

            args.Set("CamPos", pos_ok);
            args.Set("CamvRight", vRight_ok);
            args.Set("CamvUp", vUp_ok);
            Rhino.Runtime.HostUtils.ExecuteNamedCallback("ToGH_CamPose", args);
        }
    }
}
