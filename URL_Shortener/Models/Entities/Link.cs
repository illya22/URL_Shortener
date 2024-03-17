namespace URL_Shortener.Models.Entities
{
    public class Link
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserId { get; set; } // Зовнішній ключ для посилання на користувача

        public ApplicationUser User { get; set; }
    }
}
