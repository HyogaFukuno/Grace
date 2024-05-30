using Grace.Runtime.Data.DataEntity;
using Grace.Runtime.Domain.Validator;

namespace Samples.Domain.Validator;

public struct UserValidator<TUserEntity> : IValidator<TUserEntity> where TUserEntity : IDataEntity
{
    public bool IsValid { get; private set; }
    public void Validate(TUserEntity item)
    {
        
    }
}