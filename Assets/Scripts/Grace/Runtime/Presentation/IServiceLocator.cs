namespace Grace.Runtime.Presentation;

public interface IServiceLocator
{
    T Resolve<T>();
}