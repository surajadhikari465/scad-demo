using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TextFileAttachmentAttribute : ValidationAttribute
    {
        private string[] allowedExtensions = new string[] { ".txt" };
        private string[] allowedContentTypes = new string[] { "text/plain" };
        private int maxFileSize = 10 * 1024 * 1024;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;

            // The file is required.
            if (file == null)
            {
                return new ValidationResult("Please choose a file to upload.");
            }

            // The meximum allowed file size is 10MB.
            if (file.ContentLength > maxFileSize)
            {
                return new ValidationResult("Please select a file smaller than 10MB.");
            }

            // Only a plain text document can be uploaded.
            string uploadedFileExtension = System.IO.Path.GetExtension(file.FileName);
            string contentType = file.ContentType;

            if (String.IsNullOrEmpty(uploadedFileExtension) || !allowedExtensions.Contains(uploadedFileExtension) || !allowedContentTypes.Contains(contentType))
            {
                return new ValidationResult("Please select a text file.");
            }

            return ValidationResult.Success;
        }
    }
}