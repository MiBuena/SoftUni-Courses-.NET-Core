using BookShop.Data;
using BookShop.Models;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using Z.EntityFramework.Plus;

namespace BookShop
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using (var context = new BookShopContext())
            {
                //context.Database.Migrate();

                IncreasePrices(context);

                //Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction myEnum = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var titles = context.Books
                .Where(x => x.AgeRestriction == myEnum)
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, titles);

            return result;
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var titles = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, titles);

            return result;
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var bookData = context.Books.Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x =>
                new { x.Title, x.Price })
                .ToList();

            var result = new StringBuilder();

            foreach (var book in bookData)
            {
                result.AppendLine($"{book.Title} - ${book.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}");
            }

            return result.ToString();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.HasValue && x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ").Select(x => x.ToLower());

            var books = context.Books
                .Where(x => x.BookCategories.Any(y => categories.Contains(y.Category.Name.ToLower())))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateToCheck = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var booksBeforeDate = context.Books
                .Where(x => x.ReleaseDate < dateToCheck)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new { x.Title, x.EditionType, x.Price })
                .ToList();

            var result = new StringBuilder();

            foreach (var book in booksBeforeDate)
            {
                result.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}");
            }

            return result.ToString();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.ToLower().EndsWith(input.ToLower()))
                .OrderBy(x => x.FirstName)
                .Select(x => new { x.FirstName, x.LastName })
                .ToList();

            var result = new StringBuilder();

            foreach (var author in authors)
            {
                result.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return result.ToString();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var titles = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, titles);

            return result.Trim();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    BookTitle = x.Title,
                    Author = new
                    {
                        FirstName = x.Author.FirstName,
                        LastName = x.Author.LastName
                    }
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} ({book.Author.FirstName} {book.Author.LastName})");
            }

            return sb.ToString();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var count = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .Count();

            return count;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var data = context
                .Authors
                .Select(x =>
            new
            {
                x.FirstName,
                x.LastName,
                Sum = x.Books.Sum(y => y.Copies)
            })
                .OrderByDescending(x => x.Sum);

            var sb = new StringBuilder();

            foreach (var item in data)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} - {item.Sum}");
            }

            return sb.ToString();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesData = context
                .Categories
                .Select(x =>
                new
                {
                    CategoryName = x.Name,
                    Income = x.CategoryBooks.Sum(y => y.Book.Copies * y.Book.Price)
                })
                .OrderByDescending(x => x.Income)
                .ThenBy(x => x.CategoryName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var categoryData in categoriesData)
            {
                sb.AppendLine($"{categoryData.CategoryName} ${categoryData.Income.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}");
            }

            return sb.ToString();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var data = context.Categories
                .OrderBy(x => x.Name)
                .Select(x =>
            new
            {
                CategoryName = x.Name,
                BookInformation = x.CategoryBooks
                .OrderByDescending(y => y.Book.ReleaseDate)
                .Take(3)
                .Select(p => new
                {
                    Title = p.Book.Title,
                    ReleaseYear = p.Book.ReleaseDate.Value.Year
                }).ToList()



            })
                .ToList();


            var result = new StringBuilder();

            foreach (var dataItem in data)
            {
                result.AppendLine($"--{dataItem.CategoryName}");

                foreach (var bookItem in dataItem.BookInformation)
                {
                    result.AppendLine($"{bookItem.Title} ({bookItem.ReleaseYear})");
                }
            }

            return result.ToString();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .Update(x => new Book()
                {
                    Price = x.Price * 1.05M
                });
        }
    }
}
