using Adic;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using RaceGame.Scripts.Interfaces.Services;
using RaceGame.Scripts.Interfaces.Controllers;
using UnityEngine.Scripting;

namespace RaceGame.Scripts.Controllers
{
    [Preserve]
    public class UserTouchController : IUserTouchController
    {
        [Inject]
        private IEntityGeneratorService entityGeneratorService;
        
        private ARRaycastManager raycastManager;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();

        public void Init(ARRaycastManager raycastManager)
        {
            this.raycastManager = raycastManager;
            
            Subscribe();
        }

        private void Subscribe()
        {
            // Listen for the user's touch over an AR plane
            // This will call entityGeneratorService to spawn a vehicle
            Observable.EveryUpdate()
                .Where(x => entityGeneratorService.UserVehicleRX.Value == null && Input.touchCount > 0)
                .Subscribe(x =>
                {
                    var touchPosition = Input.GetTouch(0).position;

                    if (!raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
                        return;

                    var hitPose = hits[0].pose;

                    entityGeneratorService.GenerateUserVehicle(hitPose.position, hitPose.rotation);
                });
        }
    }
}
