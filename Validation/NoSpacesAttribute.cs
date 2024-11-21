using System.ComponentModel.DataAnnotations;

namespace DynamicGridGeneration.Validation
{
    public class NoSpacesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value.ToString().Contains(" "))
            {
                return new ValidationResult("Spaces are not allowed in the column name.");
            }

            return ValidationResult.Success;
        }
    }
}
