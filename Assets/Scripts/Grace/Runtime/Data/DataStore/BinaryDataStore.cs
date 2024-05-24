using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Grace.Runtime.Data.DataEntity;
using MemoryPack;
using Unio;
using Unity.Collections;
using Unity.Logging;

namespace Grace.Runtime.Data.DataStore;

public sealed class BinaryDataStore<TDataEntity> : IDataStore<TDataEntity> where TDataEntity : IDataEntity
{
    readonly string path;
    List<TDataEntity>? entities;

    public List<TDataEntity>? Entities => entities;

    public BinaryDataStore(string path)
    {
        this.path = path.Contains(".bin") ? path : throw new InvalidDataException();
        entities = new List<TDataEntity>(capacity: 100);
    }

    void IDataStore<TDataEntity>.Load()
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }
        
        Log.Verbose($"ファイルからEntity情報を読み込みます。");

        using var nativeArray = NativeFile.ReadAllBytes(path);
        MemoryPackSerializer.Deserialize(nativeArray.AsReadOnlySpan(), ref entities);
        
        Log.Verbose($"ファイルから正常にEntity情報を読み込めました。");
    }

    void IDataStore<TDataEntity>.Store(TDataEntity? entity, bool autoStore)
    {
        if (!File.Exists(path))
        {
            using var _ = File.Create(path);
            Log.Warning($"指定のパスのファイルが存在しなかったため、ファイルを作成しました。");
        }

        if (entity is not null)
        {
            entities?.Add(entity);
        }

        if (autoStore)
        {
            Log.Verbose($"ファイルにEntity情報を保存します。");
            
            var bufferWriter = new NativeArrayBufferWriter<byte>(256, Allocator.Temp);
            using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
            var writer = new MemoryPackWriter<NativeArrayBufferWriter<byte>>(ref bufferWriter, state);
        
            MemoryPackSerializer.Serialize(ref writer, in entities);
            NativeFile.WriteAllBytes(path, bufferWriter.WrittenBuffer);
            
            Log.Verbose($"ファイルにEntity情報を正常に保存できました。");
            
            bufferWriter.Dispose();
        }
    }

    async UniTask IDataStore<TDataEntity>.LoadAsync(CancellationToken cancellation)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }
        
        Log.Verbose($"ファイルからEntity情報を読み込みます。");

#if UNITY_WEBGL
        using var nativeArray = await NativeFile.ReadAllBytesAsync(path, SynchronizationStrategy.PlayerLoop, cancellation: cancellation);
#else
        using var nativeArray = await NativeFile.ReadAllBytesAsync(path, SynchronizationStrategy.BlockOnThreadPool, cancellation: cancellation);
#endif
        
        MemoryPackSerializer.Deserialize(nativeArray.AsReadOnlySpan(), ref entities);
        
        Log.Verbose($"ファイルから正常にEntity情報を読み込めました。");
    }

    async UniTask IDataStore<TDataEntity>.StoreAsync(TDataEntity? entity, bool autoStore, CancellationToken cancellation)
    {
        if (!File.Exists(path))
        {
            await using var _ = File.Create(path);
            Log.Warning($"指定のパスのファイルが存在しなかったため、ファイルを作成しました。");
        }

        if (entity is not null)
        {
            entities?.Add(entity);
        }

        if (autoStore)
        {
            Log.Verbose($"ファイルにEntity情報を保存します。");
            
            var nativeArray = new NativeArray<byte>(MemoryPackSerializer.Serialize(entities), Allocator.Temp);
            await NativeFile.WriteAllBytesAsync(path, nativeArray).WithCancellation(cancellation);
         
            Log.Verbose($"ファイルにEntity情報を正常に保存できました。");
        }
    }
}