using System;
using System.Buffers;
using Grace.Runtime.Data.DataEntity;
using MemoryPack;

namespace Samples.Data.DataEntity;

[MemoryPackable]
[MemoryPackUnion(0, typeof(UserEntity))]
public partial interface IUserEntity : IDataEntity
{
    string? Name { get; }
}

[MemoryPackable]
public partial record UserEntity : IUserEntity
{ 
    public Guid Id { get; init; }
    public string? Name { get; init; }
}