using Adic;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;
using RaceGame.Scripts.Interfaces.Services;
using RaceGame.Scripts.Interfaces.Controllers;
using UnityStandardAssets.CrossPlatformInput;

namespace RaceGame.Scripts.Controllers
{
    [Preserve]
    public class VehicleController : IVehicleController
    {
        [Inject]
        private IEntityGeneratorService entityGeneratorService;

        private Transform vehicleTransform;
        
        private Rigidbody rigidbody;
        
        private WheelCollider[] wheelColliders = new WheelCollider[4];
        private GameObject[] wheelMeshes = new GameObject[4];

        const float MaxSteerAngle = 25f;
        const float MaxSpeed = 0.65f;
        const float ReverseTorque = 500f;
        
        private float steerAngle;
        private float currentTorque = 2500f;

        public void Init(Transform vehicleTransform)
        {
            this.vehicleTransform = vehicleTransform;

            LoadReferences();
            
            Subscribe();
        }

        private void LoadReferences()
        {
            rigidbody = vehicleTransform.GetComponent<Rigidbody>();
            
            Transform wheelCollidersWrapper = vehicleTransform.Find("WheelColliders");
            
            for (int i = 0; i < 4; i++)
                wheelColliders[i] = wheelCollidersWrapper.GetChild(i).GetComponent<WheelCollider>();
            
            Transform wheelMeshesWrapper = vehicleTransform.Find("WheelMeshes");
            
            for (int i = 0; i < 4; i++)
                wheelMeshes[i] = wheelMeshesWrapper.GetChild(i).gameObject;
        }

        private void Subscribe()
        {
            Observable.EveryUpdate()
                .TakeUntilDestroy(vehicleTransform.gameObject)
                .Subscribe(x =>
                {
                    float h = CrossPlatformInputManager.GetAxis("Horizontal");
                    float v = CrossPlatformInputManager.GetAxis("Vertical");
                    
                    Move(h, v, v);
                });
        }
        
        private void Move(float steering, float accel, float footbrake)
        {
            // Clamp inputs
            steering = Mathf.Clamp(steering, -1, 1);
            accel = Mathf.Clamp(accel, 0, 1);
            footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);

            Steer(steering);
            
            ApplyDrive(accel, footbrake);
            CapSpeed();
        }
        
        private void Steer(float steering)
        {
            // Update the wheel meshes' position and rotation
            for (int i = 0; i < 4; i++)
            {
                Quaternion quat;
                Vector3 position;
                
                wheelColliders[i].GetWorldPose(out position, out quat);
                
                wheelMeshes[i].transform.position = position;
                wheelMeshes[i].transform.rotation = quat;
            }
            
            // Set the steer on the front wheels ( Assuming that wheels 0 and 1 are the front wheels )
            steerAngle = steering * MaxSteerAngle;
            wheelColliders[0].steerAngle = steerAngle;
            wheelColliders[1].steerAngle = steerAngle;
        }
        
        private void ApplyDrive(float accel, float footbrake)
        {
            float thrustTorque = accel * (currentTorque / 4f);

            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].motorTorque = thrustTorque;

                if (footbrake <= 0)
                    continue;

                // Apply brake
                wheelColliders[i].brakeTorque = 0f;
                wheelColliders[i].motorTorque = -ReverseTorque * footbrake;
            }
        }
        
        private void CapSpeed()
        {
            float speed = rigidbody.velocity.magnitude;
            
            if (speed > MaxSpeed)
                rigidbody.velocity = MaxSpeed * rigidbody.velocity.normalized;
        }
    }
}
