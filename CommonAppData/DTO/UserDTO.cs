namespace MyProApiDiplom.CommonAppData.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; } // Игнорируется при создании пользователя
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ?RoleId { get; set; } // Используется для отправки идентификатора роли
        
    }
}
