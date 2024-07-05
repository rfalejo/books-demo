using Books.Demo.Entities;
using Newtonsoft.Json.Linq;

namespace Books.Demo.Repositories
{
    public class BookRepository
    {
        private static BookRepository _instance;
        private readonly Dictionary<string, Book> _cache;
        private readonly HttpClient _httpClient;

        private BookRepository()
        {
            _cache = [];
            _httpClient = new HttpClient();
        }

        public static BookRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BookRepository();
            }
            return _instance;
        }

        public async Task<List<Book>> GetBooksAsync(List<string> isbns)
        {
            var cachedBooks = isbns.Where(isbn => _cache.ContainsKey(isbn))
                           .Select(isbn =>
                           {
                               var book = _cache[isbn];
                               book.IsCached = true;
                               return book;
                           })
                           .ToList();

            var isbnsToBeFetched = isbns.Except(_cache.Keys).ToList();

            if (isbnsToBeFetched.Count != 0)
            {
                var fetchedBooks = await FetchBooksFromApiAsync(isbnsToBeFetched);
                fetchedBooks.ForEach(book => _cache[book.Isbn] = book);
                cachedBooks.AddRange(fetchedBooks);
            }

            return cachedBooks;
        }

        private async Task<List<Book>> FetchBooksFromApiAsync(List<string> isbns)
        {
            var isbnString = string.Join(",", isbns.Select(isbn => $"ISBN:{isbn}"));
            var url = $"http://openlibrary.org/api/books?bibkeys={isbnString}&jscmd=details&format=json";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            var books = new List<Book>();
            var jsonObject = JObject.Parse(json);

            foreach (var isbn in isbns)
            {
                var key = $"ISBN:{isbn}";
                if (jsonObject.ContainsKey(key))
                {
                    var bookObject = jsonObject[key];
                    var book = new Book
                    {
                        Isbn = isbn,
                        Title = bookObject["details"]?["title"]?.ToString(),
                        Subtitle = bookObject["details"]?["subtitle"]?.ToString(),
                        PublishDate = bookObject["details"]?["publish_date"]?.ToString(),
                        NumberOfPages = bookObject["details"]?["number_of_pages"]?.ToObject<int>(),
                        Authors = bookObject["details"]?["authors"]?.ToObject<List<Author>>()
                    };
                    books.Add(book);
                }
            }

            return books;
        }
    }
}