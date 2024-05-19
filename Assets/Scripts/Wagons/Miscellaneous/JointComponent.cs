using System;
using UnityEngine;
using Wagons.Systems;
using Wagons.Wagon_Types;

namespace Wagons.Miscellaneous
{
    public class JointComponent : MonoBehaviour
    {
        [SerializeField] private Transform frontJointPoint;
        [SerializeField] private Transform jointPoint;
        [SerializeField] private LineRenderer connectionRenderer;
        [SerializeField] private ConfigurableJoint joint;
        [SerializeField] private float minAnchorDrawDistance;

        private bool _isConnected;
        
        private IWagon _connectedWagon;
        private JointComponent _connectedJoint;
        
        private ConfigurableJointMotion _dataMotionX, _dataMotionY, _dataMotionZ;
        private ConfigurableJointMotion _dataMotionXAngular, _dataMotionYAngular, _dataMotionZAngular;

        private JointDrive _driveX, _driveY, _driveZ, _driveSlerp;

        private float _driveSpringX, _driveSpringY, _driveSpringZ, _driveSpringSlerp;
        private float _driveDamperX, _driveDamperY, _driveDamperZ, _driveDamperSlerp;

        private void Awake()
        {
            _dataMotionX = joint.xMotion;
            _dataMotionY = joint.yMotion;
            _dataMotionZ = joint.zMotion;
            _dataMotionXAngular = joint.angularXMotion;
            _dataMotionYAngular = joint.angularYMotion;
            _dataMotionZAngular = joint.angularZMotion;

            _driveX = joint.xDrive;
            _driveY = joint.yDrive;
            _driveZ = joint.zDrive;
            _driveSlerp = joint.slerpDrive;

            _driveSpringX = _driveX.positionSpring;
            _driveDamperX = _driveX.positionDamper;
            
            _driveSpringY = _driveY.positionSpring;
            _driveDamperY = _driveY.positionDamper;

            _driveSpringZ = _driveZ.positionSpring;
            _driveDamperZ = _driveZ.positionDamper;

            _driveSpringSlerp = _driveSlerp.positionSpring;
            _driveDamperSlerp = _driveSlerp.positionDamper;
            
            Disable();
        }

        private void Update()
        {
            if (_isConnected)
            {
                UpdateRenderer();
            }
        }

        public void Connect(IWagon connectedWagon)
        {
            if (joint.connectedBody != null)
            {
                Debug.Log("Tried to connect to a joint that already has a connected object");
                
                return;
            }

            _connectedWagon = connectedWagon;
            _connectedJoint = _connectedWagon.GetWagon().backJoint;
            joint.connectedBody = _connectedWagon.GetWagon().gameObject.GetComponent<Rigidbody>();
            _isConnected = true;

            SetUpRenderer();
            Enable();
        }

        public Vector3 GetPosition()
        {
            return jointPoint.position;
        }

        // Supporting function used to calculate offsets related to the wagon
        public float GetAbsDistanceFromWagonCenter()
        {
            return Math.Abs(Vector3.Distance(gameObject.transform.position, jointPoint.position));
        }
        
        public void Disconnect()
        {
            _connectedWagon = null;
            _connectedJoint = null;
            joint.connectedBody = null;
            _isConnected = false;
            
            Disable();
        }

        private void Enable()
        {
            joint.xMotion = _dataMotionX;
            joint.yMotion = _dataMotionY;
            joint.zMotion = _dataMotionZ;
            joint.angularXMotion = _dataMotionXAngular;
            joint.angularYMotion = _dataMotionYAngular;
            joint.angularZMotion = _dataMotionZAngular;
            
            _driveX.positionSpring = _driveSpringX;
            _driveX.positionDamper = _driveDamperX;

            _driveY.positionSpring = _driveSpringY;
            _driveY.positionDamper = _driveDamperY;

            _driveZ.positionSpring = _driveSpringZ;
            _driveZ.positionDamper = _driveDamperZ;

            _driveSlerp.positionSpring = _driveSpringSlerp;
            _driveSlerp.positionDamper = _driveDamperSlerp;
            
            joint.xDrive = _driveX;
            joint.yDrive = _driveY;
            joint.zDrive = _driveZ;
            joint.slerpDrive = _driveSlerp;
        }

        private void Disable()
        {
            joint.xMotion = ConfigurableJointMotion.Free;
            joint.yMotion = ConfigurableJointMotion.Free;
            joint.zMotion = ConfigurableJointMotion.Free;
            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Free;

            _driveX.positionSpring = 0;
            _driveX.positionDamper = 0;

            _driveY.positionSpring = 0;
            _driveY.positionDamper = 0;

            _driveZ.positionSpring = 0;
            _driveZ.positionDamper = 0;

            _driveSlerp.positionSpring = 0;
            _driveSlerp.positionDamper = 0;

            joint.xDrive = _driveX;
            joint.yDrive = _driveY;
            joint.zDrive = _driveZ;
            joint.slerpDrive = _driveSlerp;
        }

        private void SetUpRenderer()
        {
            connectionRenderer.positionCount = 2;
            
            connectionRenderer.SetPositions
            (
                new []
                {
                    GetPosition(),
                    _connectedJoint.frontJointPoint.position
                }
            );
            
            // var anchorPoint = transform.TransformPoint(joint.anchor);
            // var connectedAnchorPoint = transform.TransformPoint(joint.connectedAnchor);
            //
            // connectionRenderer.positionCount = 4;
            //
            // connectionRenderer.SetPositions
            // (
            //     new []
            //     {
            //         GetPosition(),
            //         anchorPoint,
            //         connectedAnchorPoint,
            //         _connectedJoint.frontJointPoint.position
            //         
            //     }
            // );
        }

        private void UpdateRenderer()
        {
            connectionRenderer.SetPositions
            (
                new []
                {
                    GetPosition(),
                    _connectedJoint.frontJointPoint.position
                }
            );
            
            // var anchorPoint = transform.TransformPoint(joint.anchor);
            // var connectedAnchorPoint = 
            //     _connectedWagon.GetWagon().transform.TransformPoint(joint.connectedAnchor);
            //
            // if (Vector3.Distance(anchorPoint, connectedAnchorPoint) <= minAnchorDrawDistance)
            // {
            //     connectionRenderer.positionCount = 2;
            //     connectionRenderer.SetPositions
            //     (
            //         new []
            //         {
            //             GetPosition(),
            //             _connectedJoint.frontJointPoint.position
            //         }
            //     );
            // }
            // else
            // {
            //     connectionRenderer.positionCount = 4;
            //     connectionRenderer.SetPositions
            //     (
            //         new []
            //         {
            //             GetPosition(),
            //             anchorPoint,
            //             connectedAnchorPoint,
            //             _connectedJoint.frontJointPoint.position
            //         }
            //     );
            // }
        }
        
        public void UpdateAnchors()
        {
            joint.anchor = new Vector3
            {
                x = 0, 
                y = 0, 
                z = -GetAbsDistanceFromWagonCenter() - WagonManager.Instance.wagonSpawnDistance / 2
            };
            
            joint.autoConfigureConnectedAnchor = true;
            // ReSharper disable once Unity.InefficientPropertyAccess
            joint.autoConfigureConnectedAnchor = false;
            
            joint.connectedAnchor = new Vector3
            {
                x = 0, 
                y = 0, 
                z = GetAbsDistanceFromWagonCenter() + WagonManager.Instance.wagonSpawnDistance / 2
            };
        }
    }
}