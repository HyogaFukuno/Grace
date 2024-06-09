using Grace.Runtime.Presentation;
using VContainer;
using VContainer.Unity;

namespace Samples.Presentation;

public readonly struct RootServiceLocator : IServiceLocator
{
    static LifetimeScope Root => VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance();

    public RootServiceLocator()
    {
    }

    public T Resolve<T>() => Root.Container.Resolve<T>();
}