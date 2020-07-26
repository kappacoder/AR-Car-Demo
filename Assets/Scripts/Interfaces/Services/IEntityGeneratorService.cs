using UniRx;
using UnityEngine;

namespace RaceGame.Scripts.Interfaces.Services
{
    public interface IEntityGeneratorService
    {
        IReactiveProperty<GameObject> UserVehicleRX { get; }

        void Init(GameObject userVehiclePrefab);

        void GenerateUserVehicle(Vector3 position, Quaternion rotation);

        void DestroyUserVehicle();
    }
}
