using System;
using System.IO;
using Grace.Runtime.Data.DataStore;
using Samples.Data.DataEntity;
using Unity.Logging;
using Unity.Logging.Sinks;
using UnityEngine;
using UnityEngine.Profiling;
using Logger = Unity.Logging.Logger;

namespace Samples.Presentation
{
   public class DataStoreProfiling : MonoBehaviour
   {
      static readonly string path = Path.Combine(Application.streamingAssetsPath, "user.bin"); 
      readonly IDataStore<UserEntity> dataStore = new BinaryDataStore<UserEntity>(path);

      void Start()
      {
         Log.Logger = new Logger(new LoggerConfig()
            .WriteTo.UnityEditorConsole(minLevel: LogLevel.Verbose));
      }
   
      void Update()
      {
         // Profiler.BeginSample("Start DataStore.Store");
         //
         // dataStore.Store(new UserEntity { Id = Guid.NewGuid(), Name = "Hyouga" }, true);
         //
         // Profiler.EndSample();
      
         Profiler.BeginSample("Start DataStore.Load");

         //dataStore.Load();
         //dataStore.Store(new UserEntity { Id = Guid.NewGuid(), Name = string.Empty }, false);
         new UserEntity { Id = Guid.NewGuid(), Name = string.Empty };
         //var name = string.Empty;
         //var id = Guid.NewGuid();
         //dataStore.Entities?.ForEach(static x => Log.Info(x));
      
         Profiler.EndSample();
      }
   }
}