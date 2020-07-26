using Adic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using RaceGame.Scripts.Interfaces.Services;
using RaceGame.Scripts.Interfaces.Controllers;

namespace RaceGame.Scripts
{
    public class GameCore : MonoBehaviour
    {
        [SerializeField] private GameObject userVehiclePrefab;
        [SerializeField] private ARRaycastManager raycastManager;

        [Inject] 
        private IEntityGeneratorService entityGeneratorService;
        [Inject] 
        private IInjectionService injectionService;

        private void Start()
        {
            this.Inject();
        }

        [Inject]
        private void PostConstruct()
        {
            InitServices();
            
            InitControllers();
        }

        private void InitServices()
        {
            entityGeneratorService.Init(userVehiclePrefab);
        }

        private void InitControllers()
        {
            injectionService.Container.Resolve<IUserTouchController>().Init(raycastManager);
        }
    }
}
