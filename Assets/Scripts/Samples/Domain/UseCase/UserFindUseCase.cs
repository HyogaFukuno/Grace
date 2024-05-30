using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Samples.Data.DataEntity;
using Samples.Domain.Repository;

namespace Samples.Domain.UseCase;

public readonly ref struct UserFindUseCase<TUserEntity> where TUserEntity : struct, IUserEntity
{
    readonly UserRepository<TUserEntity> repository;

    public UserFindUseCase(UserRepository<TUserEntity> repository) => this.repository = repository;
    public UserFindUseCase() => throw new ArgumentNullException("repository");

    public TUserEntity Find(Guid id) => repository.Find(id);
    
    public TUserEntity Find(string name) => repository.Find(name);

    public UniTask<TUserEntity> FindAsync(Guid id, CancellationToken cancellation) 
        => repository.FindAsync(id, cancellation);
    
    public UniTask<TUserEntity> FindAsync(string name, CancellationToken cancellation) 
        => repository.FindAsync(name, cancellation);
}