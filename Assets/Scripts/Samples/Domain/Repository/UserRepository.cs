using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Grace.Runtime.Data.DataStore;
using Grace.Runtime.Domain.Repository;
using Grace.Runtime.Extensions;
using Samples.Data.DataEntity;

namespace Samples.Domain.Repository;

public readonly struct UserRepository<TUserEntity> : IRepository<TUserEntity, Guid, string> where TUserEntity : struct, IUserEntity
{
    readonly IDataStore<TUserEntity> dataStore;
    readonly Dictionary<Guid, TUserEntity> entityById;
    readonly Dictionary<int, TUserEntity> entityByName;

    public UserRepository(IDataStore<TUserEntity> dataStore)
    {
        this.dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        entityById = new(capacity: 100);
        entityByName = new(capacity: 100);
    }

    public TUserEntity Find(Guid key)
    {
        if (!dataStore.HasLoaded)
        {
            dataStore.Load();
            OnRefreshDictionaries();
        }
        
        return entityById[key];
    }
    
    public TUserEntity Find(string key)
    {
        if (!dataStore.HasLoaded)
        {
            dataStore.Load();
            OnRefreshDictionaries();
        }

        return entityByName[key.GetHashCode()];
    }
    
    public void Store(TUserEntity entity, bool autoStore) => dataStore.Store(entity, autoStore);

    public async UniTask<TUserEntity> FindAsync(Guid key, CancellationToken cancellation)
    {
        if (!dataStore.HasLoaded)
        {
            await dataStore.LoadAsync(cancellation);
            OnRefreshDictionaries();
        }
        
        return dataStore.Entities?.First(key, static (x, key) => x.Id.Equals(key)) ?? throw new InvalidOperationException();
    }
    
    public async UniTask<TUserEntity> FindAsync(string key, CancellationToken cancellation)
    {
        if (!dataStore.HasLoaded)
        {
            await dataStore.LoadAsync(cancellation);
            OnRefreshDictionaries();
        }
        
        return dataStore.Entities?.FirstOrDefault(key, static (x, key) => x.Name == key) ?? throw new InvalidOperationException();
    }

    public UniTask StoreAsync(TUserEntity entity, bool autoStore, CancellationToken cancellation)
        => dataStore.StoreAsync(entity, autoStore, cancellation);


    void OnRefreshDictionaries()
    {
        entityById.Clear();
        entityById.AddRange(dataStore.Entities?.ToDictionary(x => x.Id));
        
        entityByName.Clear();
        entityByName.AddRange(dataStore.Entities?.ToDictionary(x => x.Name!.GetHashCode()));
    }
}