
namespace EquipmentShop.Core.Constants
{
    public static class ErrorMessages
    {
        // Общие ошибки
        public const string RequiredField = "Поле обязательно для заполнения";
        public const string InvalidEmail = "Некорректный адрес электронной почты";
        public const string InvalidPhone = "Некорректный номер телефона";
        public const string PasswordsDoNotMatch = "Пароли не совпадают";
        public const string InvalidLength = "Длина должна быть от {0} до {1} символов";

        // Товары
        public const string ProductNotFound = "Товар не найден";
        public const string ProductOutOfStock = "Товар отсутствует на складе";
        public const string ProductLowStock = "Товар заканчивается";
        public const string ProductNotAvailable = "Товар временно недоступен";
        public const string InvalidProductQuantity = "Количество должно быть от 1 до {0}";

        // Корзина
        public const string CartEmpty = "Корзина пуста";
        public const string CartNotFound = "Корзина не найдена";
        public const string CartItemNotFound = "Товар не найден в корзине";
        public const string MaxCartItems = "Превышено максимальное количество товаров в корзине";

        // Заказы
        public const string OrderNotFound = "Заказ не найден";
        public const string OrderCannotBeCancelled = "Заказ не может быть отменен";
        public const string OrderAlreadyPaid = "Заказ уже оплачен";
        public const string OrderAlreadyCancelled = "Заказ уже отменен";

        // Пользователь
        public const string UserNotFound = "Пользователь не найден";
        public const string InvalidCredentials = "Неверный email или пароль";
        public const string AccountLocked = "Аккаунт заблокирован. Попробуйте позже";
        public const string EmailAlreadyExists = "Пользователь с таким email уже существует";

        // Оплата
        public const string PaymentFailed = "Ошибка при проведении оплаты";
        public const string PaymentMethodNotAvailable = "Выбранный способ оплаты недоступен";

        // Доставка
        public const string ShippingNotAvailable = "Доставка по указанному адресу недоступна";
        public const string ShippingAddressRequired = "Адрес доставки обязателен";

        // Файлы
        public const string FileTooLarge = "Файл слишком большой. Максимальный размер: {0} MB";
        public const string InvalidFileType = "Недопустимый тип файла. Разрешены: {0}";
        public const string FileUploadFailed = "Ошибка при загрузке файла";

        // Валидация
        public const string InvalidInput = "Некорректные данные";
        public const string TermsNotAccepted = "Необходимо согласие с условиями использования";
        public const string InvalidRating = "Рейтинг должен быть от 1 до 5";

        // Системные
        public const string InternalServerError = "Внутренняя ошибка сервера";
        public const string ServiceUnavailable = "Сервис временно недоступен";
        public const string DatabaseError = "Ошибка базы данных";
    }
}
