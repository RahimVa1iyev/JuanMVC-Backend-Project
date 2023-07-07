using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Attributes
{
    public class FileMaxLengthAttribute : ValidationAttribute
    {
        private readonly int _maxLength;

        public FileMaxLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is IFormFile)
            {
                var file = (IFormFile)value;

                if (file.Length > _maxLength)
                {
                    return new ValidationResult($"File size must be {_maxLength / 1024 / 1024}");
                }

            }

            return ValidationResult.Success;
        }
    }
}
