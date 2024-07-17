using Zenject;

namespace Forklift.Installers
{
    public class ForkliftInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Input.ForkliftInputHandler>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Controllers.ForkliftController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Physics.ForkliftMovement>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Physics.Lift>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraControl.CameraController>().FromComponentInHierarchy().AsSingle();
        }
    }
}