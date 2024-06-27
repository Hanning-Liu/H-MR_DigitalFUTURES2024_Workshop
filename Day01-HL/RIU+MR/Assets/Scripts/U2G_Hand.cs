using MixedReality.Toolkit.Subsystems;
using MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class U2G_Hand : MonoBehaviour
{
    private List<GameObject> instantiatedHandJoints = new List<GameObject>();
    void Update()
    {

        var aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        bool allJointsAreValid_L = aggregator.TryGetEntireHand(XRNode.LeftHand, out IReadOnlyList<HandJointPose> joints_L);
        bool allJointsAreValid_R = aggregator.TryGetEntireHand(XRNode.RightHand, out IReadOnlyList<HandJointPose> joints_R);
        SendHandJoints(joints_L, joints_R);
        foreach (var joint in instantiatedHandJoints)
        {
            Destroy(joint);
        }
        instantiatedHandJoints.Clear(); 

    }
    public void SendHandJoints(IReadOnlyList<HandJointPose> joints_L, IReadOnlyList<HandJointPose> joints_R)
    {
        using (var args = new Rhino.Runtime.NamedParametersEventArgs())
        {

            List<Rhino.Geometry.Point3d> pos_L = new List<Rhino.Geometry.Point3d>();
            List<string> Str_pos_L = new List<string>();
            foreach (HandJointPose p in joints_L)
            {
                pos_L.Add(p.Position.ToRhino());
            }
            foreach(Rhino.Geometry.Point3d p in pos_L)
            {
                Str_pos_L.Add(p.ToString());
            }

            List<Rhino.Geometry.Vector3d> Up_L = new List<Rhino.Geometry.Vector3d>();
            List<string> Str_Up_L = new List<string>();
            foreach (HandJointPose p in joints_L)
            {
                Up_L.Add(new Rhino.Geometry.Vector3d(p.Up.x, p.Up.z, p.Up.y));
            }
            foreach (Rhino.Geometry.Vector3d p in Up_L)
            {
                Str_Up_L.Add(p.ToString());
            }

            List<Rhino.Geometry.Vector3d> Right_L = new List<Rhino.Geometry.Vector3d>();
            List<string> Str_Right_L = new List<string>();
            foreach (HandJointPose p in joints_L)
            {
                Right_L.Add(new Rhino.Geometry.Vector3d(p.Up.x, p.Up.z, p.Up.y));
            }
            foreach (Rhino.Geometry.Vector3d p in Right_L)
            {
                Str_Right_L.Add(p.ToString());
            }


            List<Rhino.Geometry.Point3d> pos_R = new List<Rhino.Geometry.Point3d>();
            List<string> Str_pos_R = new List<string>();
            foreach (HandJointPose p in joints_R)
            {
                pos_R.Add(p.Position.ToRhino());
            }
            foreach (Rhino.Geometry.Point3d p in pos_R)
            {
                Str_pos_R.Add(p.ToString());
            }

            List<Rhino.Geometry.Vector3d> Up_R = new List<Rhino.Geometry.Vector3d>();
            List<string> Str_Up_R = new List<string>();
            foreach (HandJointPose p in joints_R)
            {
                Up_R.Add(new Rhino.Geometry.Vector3d(p.Up.x, p.Up.z, p.Up.y));
            }
            foreach (Rhino.Geometry.Vector3d p in Up_R)
            {
                Str_Up_R.Add(p.ToString());
            }

            List<Rhino.Geometry.Vector3d> Right_R = new List<Rhino.Geometry.Vector3d>();
            List<string> Str_Right_R = new List<string>();
            foreach (HandJointPose p in joints_R)
            {
                Right_R.Add(new Rhino.Geometry.Vector3d(p.Up.x, p.Up.z, p.Up.y));
            }
            foreach (Rhino.Geometry.Vector3d p in Right_R)
            {
                Str_Right_R.Add(p.ToString());
            }

            args.Set("pos_L", Str_pos_L);
            args.Set("Up_L", Str_Up_L);
            args.Set("Right_L", Str_Right_L);
            args.Set("pos_R", Str_pos_R);
            args.Set("Up_R", Str_Up_R);
            args.Set("Right_R", Str_Right_R);
            Rhino.Runtime.HostUtils.ExecuteNamedCallback("ToGH_HandJoints", args);
        }
    }
}
