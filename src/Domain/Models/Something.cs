namespace Domain.Models
{
    public record Something(string Name)
    {
        public static DateTime CreatedAt => DateTime.Now;
    }
}
