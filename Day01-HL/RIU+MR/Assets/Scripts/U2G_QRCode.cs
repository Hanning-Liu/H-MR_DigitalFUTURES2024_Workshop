using System.Collections.Generic;
using UnityEngine;
using RhinoInside.Unity;
using System;
using System.Linq;

#if MIXED_REALITY_OPENXR
using Microsoft.MixedReality.OpenXR;
#else
using SpatialGraphNode = Microsoft.MixedReality.SampleQRCodes.WindowsXR.SpatialGraphNode;
#endif
namespace Microsoft.MixedReality.SampleQRCodes
{
    public class U2G_QRCode : MonoBehaviour
    {
        static public Vector3 position;
        static public Vector3 vRight;
        static public Vector3 vUp;

        private SortedDictionary<System.Guid, GameObject> qrCodesObjectsList;
        private bool clearExisting = false;
        private SpatialGraphNode node;
        public static Matrix4x4 RWtoUW = Matrix4x4.identity;

        struct ActionData
        {
            public enum Type
            {
                Added,
                Updated,
                Removed
            };
            public Type type;
            public QR.QRCode qrCode;

            public ActionData(Type type, QR.QRCode qRCode) : this()
            {
                this.type = type;
                qrCode = qRCode;
            }
        }

        private Queue<ActionData> pendingActions = new Queue<ActionData>();

        // Use this for initialization
        void Start()
        {
            qrCodesObjectsList = new SortedDictionary<System.Guid, GameObject>();

            QRCodesManager.Instance.QRCodesTrackingStateChanged += Instance_QRCodesTrackingStateChanged;
            QRCodesManager.Instance.QRCodeAdded += Instance_QRCodeAdded;
            QRCodesManager.Instance.QRCodeUpdated += Instance_QRCodeUpdated;
            QRCodesManager.Instance.QRCodeRemoved += Instance_QRCodeRemoved;
        }
        private void Instance_QRCodesTrackingStateChanged(object sender, bool status)
        {
            if (!status)
            {
                clearExisting = true;
            }
        }

        private void Instance_QRCodeAdded(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {

            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Added, e.Data));
            }
        }

        private void Instance_QRCodeUpdated(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {

            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Updated, e.Data));
            }
        }

        private void Instance_QRCodeRemoved(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {

            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Removed, e.Data));
            }
        }

        private void HandleEvents()
        {
            lock (pendingActions)
            {
                while (pendingActions.Count > 0)
                {
                    var action = pendingActions.Dequeue();
                    if (action.type == ActionData.Type.Added)
                    {

                        System.Guid Id = action.qrCode.SpatialGraphNodeId;
                        if (node == null || node.Id != Id)
                        {
                            node = (Id != System.Guid.Empty) ? SpatialGraphNode.FromStaticNodeId(Id) : null;
                            Debug.Log("Initialize SpatialGraphNode Id= " + Id);
                        }
                        if (node != null)
                        {
#if MIXED_REALITY_OPENXR
                            if (node.TryLocate(FrameTime.OnUpdate, out Pose pose))
#else
                                if (node.TryLocate(out Pose pose))
#endif
                            {
                                if (Camera.main.transform.parent != null)
                                {
                                    pose = pose.GetTransformedBy(Camera.main.transform.parent);
                                }
                            }
                            Matrix4x4 markertoUW = Matrix4x4.identity;
                            Vector3 x = pose.rotation * (new Vector3(1, 0, 0));
                            Vector3 y = pose.rotation * (new Vector3(0, 0, 1));
                            Vector3 z = pose.rotation * (new Vector3(0, -1, 0));

                            markertoUW.SetColumn(0, new Vector4(x.x, x.y, x.z, 0));
                            markertoUW.SetColumn(1, new Vector4(y.x, y.y, y.z, 0));
                            markertoUW.SetColumn(2, new Vector4(z.x, z.y, z.z, 0));
                            markertoUW.SetColumn(3, new Vector4(pose.position.x, pose.position.y, pose.position.z, 1));

                            position = pose.position;
                            vRight = new Vector3(markertoUW.GetColumn(0).x, markertoUW.GetColumn(0).y, markertoUW.GetColumn(0).z);
                            vUp = new Vector3(markertoUW.GetColumn(1).x, markertoUW.GetColumn(1).y, markertoUW.GetColumn(1).z);
                        }
                    }
                    else if (action.type == ActionData.Type.Updated)
                    {
                        if (!qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                        {

                            System.Guid Id = action.qrCode.SpatialGraphNodeId;
                            if (node == null || node.Id != Id)
                            {
                                node = (Id != System.Guid.Empty) ? SpatialGraphNode.FromStaticNodeId(Id) : null;
                                Debug.Log("Initialize SpatialGraphNode Id= " + Id);
                            }
                            if (node != null)
                            {
#if MIXED_REALITY_OPENXR
                                if (node.TryLocate(FrameTime.OnUpdate, out Pose pose))
#else
                                if (node.TryLocate(out Pose pose))
#endif
                                {
                                    if (Camera.main.transform.parent != null)
                                    {
                                        pose = pose.GetTransformedBy(Camera.main.transform.parent);
                                    }
                                }
                                Matrix4x4 markertoUW = Matrix4x4.identity;

                                Vector3 x = pose.rotation * (new Vector3(1, 0, 0));
                                Vector3 y = pose.rotation * (new Vector3(0, 0, 1));
                                Vector3 z = pose.rotation * (new Vector3(0, -1, 0));

                                markertoUW.SetColumn(0, new Vector4(x.x, x.y, x.z, 0));
                                markertoUW.SetColumn(1, new Vector4(y.x, y.y, y.z, 0));
                                markertoUW.SetColumn(2, new Vector4(z.x, z.y, z.z, 0));
                                markertoUW.SetColumn(3, new Vector4(pose.position.x, pose.position.y, pose.position.z, 1));

                                position = pose.position;
                                vRight = new Vector3(markertoUW.GetColumn(0).x, markertoUW.GetColumn(0).y, markertoUW.GetColumn(0).z);
                                vUp = new Vector3(markertoUW.GetColumn(1).x, markertoUW.GetColumn(1).y, markertoUW.GetColumn(1).z);
                            }
                        }
                    }
                    else if (action.type == ActionData.Type.Removed)
                    {
                        if (qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                        {
                            Destroy(qrCodesObjectsList[action.qrCode.Id]);
                            qrCodesObjectsList.Remove(action.qrCode.Id);
                        }
                    }
                }
            }
            if (clearExisting)
            {
                clearExisting = false;
                foreach (var obj in qrCodesObjectsList)
                {
                    Destroy(obj.Value);
                }
                qrCodesObjectsList.Clear();

            }
        }

        void Update()
        {
            HandleEvents();
            SendMarkerPose(position, vRight, vUp);
        }

        public void SendMarkerPose(Vector3 pos, Vector3 vRight, Vector3 vUp)
        {
            using (var args = new Rhino.Runtime.NamedParametersEventArgs())
            {
                Rhino.Geometry.Point3d pos_ok = pos.ToRhino();
                Rhino.Geometry.Vector3d vRight_ok = new Rhino.Geometry.Vector3d(vRight.x, vRight.z, vRight.y);
                Rhino.Geometry.Vector3d vUp_ok = new Rhino.Geometry.Vector3d(vUp.x, vUp.z, vUp.y);

                args.Set("MarkerPos", pos_ok);
                args.Set("MarkervRight", vRight_ok);
                args.Set("MarkervUp", vUp_ok);
                Rhino.Runtime.HostUtils.ExecuteNamedCallback("ToGH_MarkerPose", args);
            }
        }
    }

}
