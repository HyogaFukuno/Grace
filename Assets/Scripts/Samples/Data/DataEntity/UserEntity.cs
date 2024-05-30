using System;
using MemoryPack;
using VYaml.Annotations;

namespace Samples.Data.DataEntity;

[MemoryPackable]
[YamlObject]
public readonly partial record struct UserEntity : IUserEntity
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public DateTime CreatedTimestamp { get; init; }
    public DateTime UpdatedTimestamp { get; init; }
}