using System;
using System.Buffers;
using Grace.Runtime.Data.DataEntity;
using MemoryPack;

namespace Samples.Data.DataEntity;

public interface IUserEntity : IDataEntity
{
    string? Name { get; }
}

[MemoryPackable]
public readonly partial record struct UserEntity : IUserEntity
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
}