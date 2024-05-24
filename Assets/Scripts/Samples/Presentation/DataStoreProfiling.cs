using System;
using System.IO;
using Grace.Runtime.Data.DataStore;
using Samples.Data.DataEntity;
using Unity.Logging;
using Unity.Logging.Sinks;
using UnityEngine;
using UnityEngine.Profiling;
using Logger = Unity.Logging.Logger;

public class DataStoreProfiling : MonoBehaviour
{
   static readonly string path = Path.Combine(Application.streamingAssetsPath, "user.bin"); 
   readonly IDataStore<IUserEntity> dataStore = new BinaryDataStore<IUserEntity>(path);

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

      dataStore.Load();
      dataStore.Entities?.ForEach(x => Log.Info(x));
      
      Profiler.EndSample();
   }
   
}
