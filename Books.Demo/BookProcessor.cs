using Books.Demo.Entities;
using Books.Demo.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Books.Demo
{
    public class BookProcessor
    {
        private readonly BookRepository _repository;

        public BookProcessor()
        {
            _repository = BookRepository.GetInstance();
        }

        public async Task ProcessAsync(string inputFile, string outputFile)
        {
            var isbnRows = await File.ReadAllLinesAsync(inputFile);
            var rowNumber = 1;
        
            using var writer = new StreamWriter(outputFile);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        
            csv.WriteHeader<BookRecord>();
            await csv.NextRecordAsync();
        
            foreach (var row in isbnRows)
            {
                var isbns = row.Split(',');
                var rowBooks = await _repository.GetBooksAsync(isbns.ToList());
        
                foreach (var book in rowBooks)
                {
                    var record = new BookRecord
                    {
                        RowNumber = rowNumber,
                        DataRetrievalType = book.IsCached ? "Cache" : "Server",
                        Isbn = book.Isbn ?? "N/A",
                        Title = book.Title ?? "N/A",
                        Subtitle = book.Subtitle ?? "N/A",
                        AuthorNames = string.Join(";", book.Authors?.Select(a => a.Name) ?? []),
                        NumberOfPages = book.NumberOfPages?.ToString() ?? "N/A",
                        PublishDate = book.PublishDate ?? "N/A"
                    };
        
                    csv.WriteRecord(record);
                    await csv.NextRecordAsync();
                }
        
                rowNumber++;
            }
        }
    }
}
