using System.Collections.Generic;
using Grace.Runtime.Extensions;
using Microsoft.Extensions.Logging;
using UnityEngine;
using VContainer;
using ZLogger;

namespace Samples.Presentation
{
    public class LinqSample : MonoBehaviour
    {
        [Inject] readonly ILogger<LinqSample>? logger;
        
        void Start()
        {
            var dic = new Dictionary<int, string>
            {
                { 5, "Glacier" },
                { 7, "foo" },
                { 21, "Texture" }
            };

            dic.ForEach((pair, index) => logger?.ZLogTrace($"Key: {pair.Key}, Value: {pair.Value}, Index: {index}"));
        }
    }
}