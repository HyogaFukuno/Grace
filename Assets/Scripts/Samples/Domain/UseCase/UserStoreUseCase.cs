using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Samples.Data.DataEntity;
using Samples.Domain.Repository;

namespace Samples.Domain.UseCase;

public readonly ref struct UserStoreUseCase<TUserEntity> where TUserEntity : struct, IUserEntity
{
    readonly UserRepository<TUserEntity> repository;
    
    public UserStoreUseCase(UserRepository<TUserEntity> repository) => this.repository = repository;
    public UserStoreUseCase() => throw new ArgumentNullException("repository");

    public void Store(TUserEntity entity, bool autoStore = false)
    {
        repository.Store(entity, autoStore);
    }

    public UniTask StoreAsync(TUserEntity entity, bool autoStore = false, CancellationToken cancellation = default)
    {
        return repository.StoreAsync(entity, autoStore, cancellation);
    }
}