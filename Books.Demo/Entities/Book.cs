namespace Books.Demo.Entities
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public List<Author> Authors { get; set; }
        public int? NumberOfPages { get; set; }
        public string PublishDate { get; set; }
        public bool IsCached { get; set; }
    }
}
