
using System.ComponentModel.DataAnnotations;

namespace EquipmentShop.Core.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефон обязателен")]
        [Phone(ErrorMessage = "Некорректный номер телефона")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, ErrorMessage = "Пароль должен быть от {2} до {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо согласие с условиями")]
        [Display(Name = "Я согласен с условиями использования")]
        public bool AcceptTerms { get; set; }

        [Display(Name = "Подписаться на новости")]
        public bool SubscribeToNewsletter { get; set; }
    }

    public class UserProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int OrderCount { get; set; }
        public List<AddressViewModel> Addresses { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
    }

    public class AddressViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Домашний адрес";
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = "Беларусь";
        public bool IsDefault { get; set; }

        public string FullAddress => $"{City}, {AddressLine1}" +
            (!string.IsNullOrEmpty(AddressLine2) ? $", {AddressLine2}" : "") +
            (!string.IsNullOrEmpty(Region) ? $", {Region}" : "");
    }
}
