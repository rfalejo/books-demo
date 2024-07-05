# Books.Demo
This console application retrieves book information from the OpenLibrary API based on ISBN numbers provided in an input file, and outputs the results to a CSV file.

## Dependencies

- .NET 8.0 or later
- Newtonsoft.Json
- CsvHelper

## Usage

1. Ensure you have .NET 8.0 or later installed on your machine.
2. Clone this repository or download the source code.
3. Navigate to the project directory in your terminal.
4. Create an input file named `ISBN_Input_File.txt` in the project directory, with each line containing comma-separated ISBNs.
5. Run the following command:

```
dotnet run
```

The program will process the input file and generate an output CSV file named `books-{timestamp}.csv` in the same directory.

## Input File Format

The input file should be named `ISBN_Input_File.txt` and contain ISBNs separated by commas, with each line representing a new row. For example:

```
0201558025
0201558025,0984782869
0984782869,048641714X,0565095021
```

## Output File Format

The output file will be a CSV with the following columns:

1. Row Number
2. Data Retrieval Type (Server or Cache)
3. ISBN
4. Title
5. Subtitle
6. Author Name(s)
7. Number of Pages
8. Publish Date

If any field is not available, it will be marked as "N/A" in the output.
