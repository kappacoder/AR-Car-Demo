using Adic;
using UniRx;
using UnityEngine;
using RaceGame.Scripts.Interfaces.Services;
using RaceGame.Scripts.Interfaces.Controllers;
using UnityEngine.Scripting;

namespace RaceGame.Scripts.Services
{
    /// <summary>
    /// Service that takes care of the generation of game entities
    /// ( eg: user controlled vehicles, obstacles, AI vehicles )
    /// </summary>
    [Preserve]
    public class EntityGeneratorService : IEntityGeneratorService
    {
        // For now let's assume that there will be only one user controlled vehicle
        public IReactiveProperty<GameObject> UserVehicleRX { get; } = new ReactiveProperty<GameObject>();

        [Inject]
        private IInjectionService injectionService;
        
        private GameObject userVehiclePrefab;
        
        public void Init(GameObject userVehiclePrefab)
        {
            this.userVehiclePrefab = userVehiclePrefab;
        }

        public void GenerateUserVehicle(Vector3 position, Quaternion rotation)
        {
            GameObject vehicle = Object.Instantiate(userVehiclePrefab, position, rotation);
            vehicle.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            
            // Attach a vehicle controller
            injectionService.Container.Resolve<IVehicleController>().Init(vehicle.transform);

            UserVehicleRX.Value = vehicle;
        }

        public void DestroyUserVehicle()
        {
            GameObject.Destroy(UserVehicleRX.Value);

            UserVehicleRX.Value = null;
        }
    }
}
