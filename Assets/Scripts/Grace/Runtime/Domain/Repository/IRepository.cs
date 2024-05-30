using System.Threading;
using Cysharp.Threading.Tasks;
using Grace.Runtime.Data.DataEntity;

namespace Grace.Runtime.Domain.Repository;

public interface IRepository<TDataEntity, in TKey> where TDataEntity : IDataEntity
{
    TDataEntity Find(TKey key);
    void Store(TDataEntity entity, bool autoStore);
    UniTask<TDataEntity> FindAsync(TKey key, CancellationToken cancellation);
    UniTask StoreAsync(TDataEntity entity, bool autoStore, CancellationToken cancellationToken);
}

public interface IRepository<TDataEntity, in TPrimaryKey, in TSecondaryKey> where TDataEntity : IDataEntity
{
    TDataEntity Find(TPrimaryKey key);
    TDataEntity Find(TSecondaryKey key);
    void Store(TDataEntity entity, bool autoStore);
    
    UniTask<TDataEntity> FindAsync(TPrimaryKey key, CancellationToken cancellation);
    UniTask<TDataEntity> FindAsync(TSecondaryKey key, CancellationToken cancellation);
    UniTask StoreAsync(TDataEntity entity, bool autoStore, CancellationToken cancellationToken);
}

public interface IRepository<TDataEntity, in TPrimaryKey, in TSecondaryKey, in TTertiaryKey> where TDataEntity : IDataEntity
{
    TDataEntity Find(TPrimaryKey key);
    TDataEntity Find(TSecondaryKey key);
    TDataEntity Find(TTertiaryKey key);
    void Store(TDataEntity entity, bool autoStore);
    
    UniTask<TDataEntity> FindAsync(TPrimaryKey key, CancellationToken cancellation);
    UniTask<TDataEntity> FindAsync(TSecondaryKey key, CancellationToken cancellation);
    UniTask<TDataEntity> FindAsync(TTertiaryKey key, CancellationToken cancellation);
    UniTask StoreAsync(TDataEntity entity, bool autoStore, CancellationToken cancellationToken);
}