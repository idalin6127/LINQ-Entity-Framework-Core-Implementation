using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Linq;



public class Author
{
    public int AuthorID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Title
{
    public string ISBN { get; set; }
    public string TitleName { get; set; }
}

public class AuthorISBN
{
    public int AuthorID { get; set; }
    public string ISBN { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Question 02 - Lab 04");

        // Invokes methods
        Question2_1();
        Question2_2();
        Question2_3();
    }

    // Read Authors data from Excel
    public static List<Author> GetAuthorsFromExcel(string filePath)
    {
        var authors = new List<Author>();

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            //var worksheetsCount = package.Workbook.Worksheets.Count;
            //Console.WriteLine($"Total Author Worksheets Count: {worksheetsCount}");

            var worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var author = new Author
                {
                    AuthorID = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                    FirstName = worksheet.Cells[row, 2].Value.ToString(),
                    LastName = worksheet.Cells[row, 3].Value.ToString()
                };

                authors.Add(author);
            }
        }

        return authors;
    }

    // Read Titles data from Excel
    public static List<Title> GetTitlesFromExcel(string filePath)
    {
        var titles = new List<Title>();

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            //var worksheetsCount = package.Workbook.Worksheets.Count;
            //Console.WriteLine($"Total Title Worksheets Count: {worksheetsCount}");

            var worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var title = new Title
                {
                    ISBN = worksheet.Cells[row, 1].Value.ToString(),
                    TitleName = worksheet.Cells[row, 2].Value.ToString()
                };

                titles.Add(title);
            }
        }

        return titles;
    }

    public static List<AuthorISBN> GetAuthorISBNFromExcel(string filePath)
    {
        var authorISBNList = new List<AuthorISBN>();

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            //var worksheetsCount = package.Workbook.Worksheets.Count;
            //Console.WriteLine($"Total AuthorISBN Worksheets Count: {worksheetsCount}");

            var worksheet = package.Workbook.Worksheets[0]; // Update the worksheet index if necessary

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var authorISBN = new AuthorISBN
                {
                    AuthorID = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                    ISBN = worksheet.Cells[row, 2].Value.ToString()
                };

                authorISBNList.Add(authorISBN);
            }
        }

        return authorISBNList;
    }

    // Question 2.1: Get a list of all the titles and the authors who wrote them. Sort the results by title.
    public static void Question2_1()
    {
        var authors = GetAuthorsFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.Authors.xlsx"); // Replace with actual file path for Authors
        var titles = GetTitlesFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.Titles.xlsx"); // Replace with actual file path for Titles
        var authorISBNs = GetAuthorISBNFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.AuthorISBN.xlsx");

        var result = titles.Join(authorISBNs,
                        title => title.ISBN,
                        authorISBN => authorISBN.ISBN,
                        (title, authorISBN) => new
                        {
                            Title = title.TitleName,
                            Authors = authors.Where(author => author.AuthorID == authorISBN.AuthorID).Select(author => $"{author.FirstName} {author.LastName}")
                        })
                        .OrderBy(item => item.Title);

        Console.WriteLine("Question 2.1 Results:");
        foreach (var item in result)
        {
            Console.WriteLine($"Title: {item.Title}");
            foreach (var author in item.Authors)
            {
                Console.WriteLine($"Author: {author}");
            }
        }
    }

    // Question 2.2: Get a list of all the titles and the authors who wrote them. Sort the results by title.
    // Each title sort the authors alphabetically by last name, then first name
    public static void Question2_2()
    {
        var authors = GetAuthorsFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.Authors.xlsx"); // Replace with actual file path for Authors
        var titles = GetTitlesFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.Titles.xlsx"); // Replace with actual file path for Titles
        var authorISBNs = GetAuthorISBNFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.AuthorISBN.xlsx");

        var result = titles
        .Join(authorISBNs,
            title => title.ISBN,
            authorISBN => authorISBN.ISBN,
            (title, authorISBN) => new { Title = title, AuthorID = authorISBN.AuthorID })
        .Join(authors,
            t => t.AuthorID,
            author => author.AuthorID,
            (t, author) => new { Title = t.Title, Author = author })
        .GroupBy(item => item.Title.TitleName)
        .OrderBy(group => group.Key)
        .Select(group => new
        {
            Title = group.Key,
            Authors = group.Select(item => item.Author)
                          .OrderBy(author => author.LastName)
                          .ThenBy(author => author.FirstName)
        });

        Console.WriteLine("Question 2.2 Results:");
        foreach (var item in result)
        {
            Console.WriteLine($"Title: {item.Title}");
            foreach (var author in item.Authors)
            {
                Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
            }
        }
    }

    // Question 2.3: Get a list of all the authors grouped by title, sorted by title;
    // For a given title sort the author names alphabetically by last name then first name.
    public static void Question2_3()
    {
        var authors = GetAuthorsFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.Authors.xlsx"); // Replace with actual file path for Authors
        var titles = GetTitlesFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.Titles.xlsx"); // Replace with actual file path for Titles
        var authorISBNs = GetAuthorISBNFromExcel("C:\\2022F-Centennial College-HD\\2023 Fall\\COMP212 - Programming 3(SEC.004)\\Lab#4\\dbo.AuthorISBN.xlsx");

        var result = titles
                .Join(authorISBNs,
                    title => title.ISBN,
                    authorISBN => authorISBN.ISBN,
                    (title, authorISBN) => new { Title = title, AuthorID = authorISBN.AuthorID })
                .Join(authors,
                    t => t.AuthorID,
                    author => author.AuthorID,
                    (t, author) => new { Title = t.Title, Author = author })
                .GroupBy(item => item.Title.TitleName)
                .OrderBy(group => group.Key)
                .Select(group => new
                {
                    Title = group.Key,
                    Authors = group.Select(item => item.Author)
                                  .OrderBy(author => author.LastName)
                                  .ThenBy(author => author.FirstName)
                });

        Console.WriteLine("Question 2.3 Results:");
        foreach (var item in result)
        {
            Console.WriteLine($"Title: {item.Title}");
            foreach (var author in item.Authors)
            {
                Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
            }
        }
    }
}


