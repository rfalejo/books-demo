using Books.Demo;

var inputFile = "ISBN_Input_File.txt";
var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
var outputFile = $"books-{timestamp}.csv";

var processor = new BookProcessor();
await processor.ProcessAsync(inputFile, outputFile);

Console.WriteLine($"Output file: {outputFile}");