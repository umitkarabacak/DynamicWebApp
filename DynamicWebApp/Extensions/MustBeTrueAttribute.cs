namespace DynamicWebApp.Extensions;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class MustBeTrueAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is bool boolValue && boolValue)
        {
            return ValidationResult.Success;
        }

        if (!string.IsNullOrWhiteSpace(ErrorMessage)
            && ErrorMessage.Contains("{0}"))
        {
            var displayName = validationContext.DisplayName ?? string.Empty;

            ErrorMessage = ErrorMessage.Replace("{0}", displayName);
        }

        return new ValidationResult(ErrorMessage ?? " The field must be true!");
    }
}
