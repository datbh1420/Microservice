using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utility
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] extensionsAllowed;

        public AllowedExtensionsAttribute(string[] extensionsAllowed)
        {
            this.extensionsAllowed = extensionsAllowed;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!extensionsAllowed.Contains(extension.ToLower()))
                {
                    return new ValidationResult("This photo extension is not allowed!");
                }
            }


            return ValidationResult.Success;
        }
    }
}
