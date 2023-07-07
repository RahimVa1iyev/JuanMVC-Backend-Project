using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Attributes
{
    public class AllowContentTypeAttribute : ValidationAttribute
    {
        private readonly string[] _types;

        public AllowContentTypeAttribute(params string[] types)
        {
            _types = types;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile)
            {
                var file = (IFormFile)value;

                if (!_types.Contains(file.ContentType))
                {
                    return new ValidationResult($"File type must be {string.Join(',', _types)}");
                }
            }


            return ValidationResult.Success;
        }
    }
}
