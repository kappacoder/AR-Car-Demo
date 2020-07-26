using Adic.Container;
using UnityEngine.Scripting;
using RaceGame.Scripts.Interfaces.Services;

namespace RaceGame.Scripts.Services
{
    [Preserve]
    public class InjectionService : IInjectionService
    {
        public InjectionService(IInjectionContainer container)
        {
            Container = container;
        }

        public IInjectionContainer Container { get; private set; }
    }
}
