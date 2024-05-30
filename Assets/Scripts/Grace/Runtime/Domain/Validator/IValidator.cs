namespace Grace.Runtime.Domain.Validator;

public interface IValidator<in T>
{
    bool IsValid { get; }

    void Validate(T item);
}