namespace Models.DTO
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Profile { get; set; }
    }

    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Profile { get; set; }
    }
}
