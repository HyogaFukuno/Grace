using System;
using System.IO;
using Grace.Runtime.Data.DataStore;
using Microsoft.Extensions.Logging;
using Samples.Data.DataEntity;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZLogger.Unity;

namespace Samples.Application.Installer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [Serializable]
        enum UserDataStoreType
        {
            Binary,
            Yaml,
            Json
        }

        [SerializeField] UserDataStoreType dataStoreType;
        [SerializeField] string filename = "user.json";
        
        protected override void Configure(IContainerBuilder builder)
        {
            var path = Path.Combine(UnityEngine.Application.streamingAssetsPath, filename);

            builder.RegisterInstance(LoggerFactory.Create(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddZLoggerUnityDebug();
            }));
            builder.Register(typeof(Logger<>), Lifetime.Singleton).As(typeof(ILogger<>));
            
            switch (dataStoreType)
            {
                case UserDataStoreType.Binary:
                    builder.Register(typeof(BinaryDataStore<UserEntity>), Lifetime.Singleton)
                        .As<IDataStore<UserEntity>>()
                        .WithParameter(Path.ChangeExtension(path, ".bin"))
                        .WithParameter(false);
                    break;
                case UserDataStoreType.Yaml:
                    builder.Register(typeof(YamlDataStore<UserEntity>), Lifetime.Singleton)
                        .As<IDataStore<UserEntity>>()
                        .WithParameter(Path.ChangeExtension(path, ".yml"))
                        .WithParameter(false);
                    break;
                case UserDataStoreType.Json:
                    builder.Register(typeof(JsonDataStore<UserEntity>), Lifetime.Singleton)
                        .As<IDataStore<UserEntity>>()
                        .WithParameter(Path.ChangeExtension(path, ".json"))
                        .WithParameter(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}