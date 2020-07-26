using Adic;
using Adic.Container;
using RaceGame.Scripts.Services;
using RaceGame.Scripts.Controllers;
using RaceGame.Scripts.Interfaces.Services;
using RaceGame.Scripts.Interfaces.Controllers;

namespace RaceGame.Scripts
{
    public class GameContainer : ContextRoot
    {
        public override void SetupContainers()
        {
            IInjectionContainer container = new InjectionContainer()
                .RegisterExtension<UnityBindingContainerExtension>()
                .RegisterExtension<EventCallerContainerExtension>();

            IInjectionService injectionService = new InjectionService(container);

            container
                .Bind<IInjectionService>().To(injectionService)
                .Bind<IEntityGeneratorService>().ToSingleton<EntityGeneratorService>();

            BindControllers(container);

            AddContainer(container);
        }

        public override void Init() { }

        private void BindControllers(IInjectionContainer container)
        {
            container
                .Bind<IUserTouchController>().To<UserTouchController>()
                .Bind<IVehicleController>().To<VehicleController>();
        }
    }
}
