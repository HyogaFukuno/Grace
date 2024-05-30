using Grace.Runtime.Presentation;
using VContainer;
using VContainer.Unity;

namespace Samples.Presentation;

public sealed class RootServiceLocator : IServiceLocator
{
    LifetimeScope? root;

    LifetimeScope Root => root ??= VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance();

    public T Resolve<T>() => Root.Container.Resolve<T>();
}