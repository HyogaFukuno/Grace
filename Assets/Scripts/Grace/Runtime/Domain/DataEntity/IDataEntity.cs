using System;
using MemoryPack;

namespace Grace.Runtime.Data.DataEntity;

public interface IDataEntity
{
    Guid Id { get; }
}