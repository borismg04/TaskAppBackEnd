namespace Models
{
    public class ReponseModel
    {
        public string? message { get; set; }
        public Boolean success { get; set; }
        public object? result { get; set; }
        public int statusCode { get; set; }

    }
}
