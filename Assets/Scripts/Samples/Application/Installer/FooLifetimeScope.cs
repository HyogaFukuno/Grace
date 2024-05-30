using Grace.Runtime.Presentation;
using Samples.Presentation;
using VContainer;
using VContainer.Unity;

namespace Samples.Application.Installer
{
    public class FooLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IServiceLocator, RootServiceLocator>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<FooPresenter>();
        }
    }
}