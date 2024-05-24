using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using Cysharp.Threading.Tasks;
using Grace.Runtime.Data.DataEntity;
using Unio;
using Unity.Collections;
using Unity.Logging;

namespace Grace.Runtime.Data.DataStore;

public sealed class JsonDataStore<TDataEntity> : IDataStore<TDataEntity> where TDataEntity : IDataEntity
{
    readonly string path;
    List<TDataEntity>? entities;

    public List<TDataEntity>? Entities => entities;

    public JsonDataStore(string path)
    {
        this.path = path.Contains(".json") ? path : throw new InvalidDataException();
        entities = new List<TDataEntity>(capacity: 1);
    }

    void IDataStore<TDataEntity>.Load()
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }
        
        Log.Verbose($"ファイルからEntity情報を読み込みます。");

        using var nativeArray = NativeFile.ReadAllBytes(path);
        entities = JsonSerializer.Deserialize<List<TDataEntity>>(nativeArray.AsSpan());
        
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
            var jsonWriter = new Utf8JsonWriter(bufferWriter);
            
            JsonSerializer.Serialize(jsonWriter, entities);
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
        
        entities = JsonSerializer.Deserialize<List<TDataEntity>>(nativeArray.AsSpan());
        
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
            
            var bufferWriter = new NativeArrayBufferWriter<byte>(256, Allocator.Temp);
            var jsonWriter = new Utf8JsonWriter(bufferWriter);
            
            JsonSerializer.Serialize(jsonWriter, entities);
            await NativeFile.WriteAllBytesAsync(path, bufferWriter.WrittenBuffer).WithCancellation(cancellation);
         
            Log.Verbose($"ファイルにEntity情報を正常に保存できました。");
        }
    }
}