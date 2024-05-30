using System;
using Grace.Runtime.Data.DataStore;
using Grace.Runtime.Presentation;
using Microsoft.Extensions.Logging;
using Samples.Data.DataEntity;
using Samples.Domain.Repository;
using Samples.Domain.UseCase;
using VContainer.Unity;

namespace Samples.Presentation;

public sealed class FooPresenter : IInitializable
{
    readonly ILogger<FooPresenter> logger;
    readonly IServiceLocator locator;

    public FooPresenter(ILogger<FooPresenter> logger, IServiceLocator locator)
    {
        this.logger = logger;
        this.locator = locator;
    }

    void IInitializable.Initialize()
    {
        logger.LogTrace("Initialize");
        
        var repository = new UserRepository<UserEntity>(locator.Resolve<IDataStore<UserEntity>>());
        var findUseCase = new UserFindUseCase<UserEntity>(repository);
        var storeUseCase = new UserStoreUseCase<UserEntity>(repository);

        // storeUseCase.Store(new UserEntity
        // {
        //     Id = Guid.NewGuid(),
        //     Name = "Glacier",
        //     CreatedTimestamp = DateTime.Now,
        //     UpdatedTimestamp = DateTime.Now
        // }, true);

        //var id = Guid.Parse("793cac56-e306-490a-97b8-12424b7e290d".AsSpan());
        var user = findUseCase.Find("Glacier");
        
        // //
        // //
        storeUseCase.Store(user with { UpdatedTimestamp = DateTime.Now }, true);
    }
}