using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using Cysharp.Threading.Tasks;
using Grace.Runtime.Data.DataEntity;
using Grace.Runtime.Extensions;
using Microsoft.Extensions.Logging;
using Unio;
using Unity.Collections;

namespace Grace.Runtime.Data.DataStore;

public sealed class JsonDataStore<TDataEntity> : IDataStore<TDataEntity> where TDataEntity : IDataEntity
{
    readonly struct FileStatus
    {
        public bool Exists { get; init; }
    }

    readonly ILogger<JsonDataStore<TDataEntity>> logger;
    readonly string path;
    readonly bool isByteWriter;
    FileStatus? fileStatus;
    List<TDataEntity>? entities;

    public bool HasLoaded { get; private set; }
    public List<TDataEntity>? Entities => entities;

    public JsonDataStore(ILogger<JsonDataStore<TDataEntity>> logger, string path, bool isByteWriter = false)
    {
        this.logger = logger;
        this.path = path.Contains(".json") ? path : throw new InvalidDataException("your specified path's extension is not allowed. allowed only [*.json] extension.");
        this.isByteWriter = isByteWriter;
        entities = new List<TDataEntity>(capacity: 100);
    }

    void IDataStore<TDataEntity>.Load()
    {
        fileStatus ??= new FileStatus { Exists = File.Exists(path) };

        if (fileStatus is { Exists: false })
        {
            throw new FileNotFoundException();
        }
        
        logger.LogTrace($"ファイルからEntity情報を読み込みます。");
        
        using var nativeArray = NativeFile.ReadAllBytes(path);
        entities = JsonSerializer.Deserialize<List<TDataEntity>>(nativeArray.AsReadOnlySpan());

        HasLoaded = true;
        
        logger.LogTrace($"ファイルから正常にEntity情報を読み込めました。");
    }

    void IDataStore<TDataEntity>.Store(TDataEntity? entity, bool autoStore)
    {
        if (fileStatus is { Exists: false })
        {
            using var _ = File.Create(path);
            fileStatus = new FileStatus { Exists = true };
            logger.LogWarning($"指定のパスのファイルが存在しなかったため、ファイルを作成しました。");
        }

        if (entity is not null)
        {
            if (entities is { Count: > 0 })
            {
                var matchedIndex = entities.FindIndex(entity, static (x, y) => x.Id.Equals(y.Id));
                if (matchedIndex != -1)
                {
                    entities.RemoveAt(matchedIndex);
                }
            }
            
            entities?.Add(entity);
        }

        if (autoStore)
        {
            logger.LogTrace($"ファイルにEntity情報を保存します。");

            if (isByteWriter)
            {
                var bufferWriter = new NativeArrayBufferWriter<byte>(256, Allocator.Temp);
                var jsonWriter = new Utf8JsonWriter(bufferWriter);
            
                JsonSerializer.Serialize(jsonWriter, entities, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                });
                NativeFile.WriteAllBytes(path, bufferWriter.WrittenBuffer);
            }
            else
            {
                File.WriteAllText(path, JsonSerializer.Serialize(entities, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                }));
            }

            logger.LogTrace($"ファイルにEntity情報を正常に保存できました。");
        }
    }

    async UniTask IDataStore<TDataEntity>.LoadAsync(CancellationToken cancellation)
    {
        fileStatus ??= new FileStatus { Exists = File.Exists(path) };

        if (fileStatus is { Exists: false })
        {
            throw new FileNotFoundException();
        }
        
        logger.LogTrace($"ファイルからEntity情報を読み込みます。");

#if UNITY_WEBGL
        using var nativeArray = await NativeFile.ReadAllBytesAsync(path, SynchronizationStrategy.PlayerLoop, cancellation: cancellation);
#else
        using var nativeArray = await NativeFile.ReadAllBytesAsync(path, SynchronizationStrategy.BlockOnThreadPool, cancellation: cancellation);
#endif
        
        entities = JsonSerializer.Deserialize<List<TDataEntity>>(nativeArray.AsReadOnlySpan());

        HasLoaded = true;
        
        logger.LogTrace($"ファイルから正常にEntity情報を読み込めました。");
    }

    async UniTask IDataStore<TDataEntity>.StoreAsync(TDataEntity? entity, bool autoStore, CancellationToken cancellation)
    {
        if (fileStatus is { Exists: false })
        {
            await using var _ = File.Create(path);
            fileStatus = new FileStatus { Exists = true };
            logger.LogWarning($"指定のパスのファイルが存在しなかったため、ファイルを作成しました。");
        }

        if (entity is not null)
        {
            if (entities is { Count: > 0 })
            {
                var matchedIndex = entities.FindIndex(entity, static (x, y) => x.Id.Equals(y.Id));
                if (matchedIndex != -1)
                {
                    entities.RemoveAt(matchedIndex);
                }
            }
            
            entities?.Add(entity);
        }

        if (autoStore)
        {
            logger.LogTrace($"ファイルにEntity情報を保存します。");
            
            var bufferWriter = new NativeArrayBufferWriter<byte>(256, Allocator.Temp);
            var jsonWriter = new Utf8JsonWriter(bufferWriter);
            
            JsonSerializer.Serialize(jsonWriter, entities);
            await NativeFile.WriteAllBytesAsync(path, bufferWriter.WrittenBuffer).WithCancellation(cancellation);
         
            logger.LogTrace($"ファイルにEntity情報を正常に保存できました。");
        }
    }
}