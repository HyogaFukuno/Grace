using System;
using MemoryPack;

namespace Grace.Runtime.Data.DataEntity;

[MemoryPackable(GenerateType.NoGenerate)]
public partial interface IDataEntity
{
    Guid Id { get; }
}