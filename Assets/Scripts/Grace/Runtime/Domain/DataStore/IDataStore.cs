using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Grace.Runtime.Data.DataEntity;

namespace Grace.Runtime.Data.DataStore;

public interface IDataStore<TDataEntity> where TDataEntity : IDataEntity
{
    bool HasLoaded { get; }
    List<TDataEntity>? Entities { get; }

    void Load();
    void Store(TDataEntity? entity, bool autoStore);
    UniTask LoadAsync(CancellationToken cancellation);
    UniTask StoreAsync(TDataEntity? entity, bool autoStore, CancellationToken cancellation);
}