using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace WebApplication3.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "Full Name is required")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Credit Card Number is required")]
        [DataType(DataType.CreditCard)]
        [Encrypted(ErrorMessage = "Credit Card Number must be encrypted")] // Placeholder logic for encryption
        public string CreditCardNo { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phonenumber { get; set; }

        [Required(ErrorMessage = "Delivery Address is required")]
        [DataType(DataType.MultilineText)]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        [UniqueEmail(ErrorMessage = "Email address must be unique")] // Placeholder logic for uniqueness
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
        [PasswordComplexity(ErrorMessage = "Password must include at least one lowercase letter, one uppercase letter, one number, and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        [AllowedFileExtensions(".jpg", ErrorMessage = "Only .jpg files are allowed")]
        public IFormFile Photo { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowSpecialCharacters(ErrorMessage = "Special characters are allowed")]
        public string AboutMe { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }

    // Custom Validation Attributes

    public class EncryptedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Placeholder logic for encryption validation
            // You need to implement the actual encryption validation here
            return ValidationResult.Success;
        }
    }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Placeholder logic for unique email validation
            // You need to implement the actual uniqueness validation here
            return ValidationResult.Success;
        }
    }

    public class PasswordComplexityAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // Placeholder logic for password complexity check
            // You need to implement the actual password complexity check here
            // For example, check for at least one lowercase letter, one uppercase letter, one number, and one special character
            string password = value as string;
            return !string.IsNullOrEmpty(password) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   password.Any(c => !char.IsLetterOrDigit(c));
        }
    }

    // Custom Validation Attribute for Allowed File Extensions
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedFileExtensionsAttribute(string extensions)
        {
            _extensions = extensions.Split(',');
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Any(e => e.Equals(fileExtension)))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }

    public class AllowSpecialCharactersAttribute : RegularExpressionAttribute
    {
        public AllowSpecialCharactersAttribute() : base("^[a-zA-Z0-9!@#$%^&*(),.?\":{}|<> _-]+$")
        {
            ErrorMessage = "Special characters are not allowed";
        }
    }
}