using System;
using Grace.Runtime.Data.DataEntity;

namespace Samples.Data.DataEntity;

public interface IUserEntity : IDataEntity
{
    string? Name { get; }
    DateTime CreatedTimestamp { get; }
    DateTime UpdatedTimestamp { get; }
}