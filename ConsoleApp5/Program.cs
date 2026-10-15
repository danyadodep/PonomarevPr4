using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;

namespace LibraryApp
{
    public enum Genre
    {
        Fantasy,
        Romance,
        Detective,
        Classic,
        Philosophy,
        Drama
    }
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Book> library = new List<Book>();

            // 1. Тестовые данные
            library.Add(new Book { Id = 1, Title = "Если все кошки в мире исчезнут", Author = "Гэнки Кавамура", Genre = Genre.Drama, Year = 2017, Price = 450.00m, Quantity = 15 });
            library.Add(new Book { Id = 2, Title = "Кафе на краю земли", Author = "Джон Стрелеки", Genre = Genre.Philosophy, Year = 2019, Price = 390.50m, Quantity = 8 });
            library.Add(new Book { Id = 3, Title = "1984", Author = "Джордж Оруэлл", Genre = Genre.Classic, Year = 1949, Price = 350.00m, Quantity = 20 });
            library.Add(new Book { Id = 4, Title = "Грозовой перевал", Author = "Эмили Бронте", Genre = Genre.Classic, Year = 1847, Price = 420.00m, Quantity = 12 });
            library.Add(new Book { Id = 5, Title = "Бог всегда путешествует инкогнито", Author = "Луис Сепульведа", Genre = Genre.Drama, Year = 2020, Price = 480.00m, Quantity = 5 });

            int nextId = 6;
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== БИБЛИОТЕЧНЫЙ УЧЕТ ===");
                Console.ResetColor();
                Console.WriteLine("1. Показать все книги");
                Console.WriteLine("2. Добавить книгу");
                Console.WriteLine("3. Удалить книгу по ID");
                Console.WriteLine("4. Найти книги");
                Console.WriteLine("5. Сортировка (LINQ)");
                Console.WriteLine("6. Самая дорогая / дешевая (LINQ)");
                Console.WriteLine("7. Группировка по авторам (LINQ)");
                Console.WriteLine("0. Выход");
                Console.WriteLine("-------------------------");

                int choice = ReadValidInt("Выберите действие (0-7): ", 0);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\n--- СПИСОК КНИГ ---");
                        if (library.Count == 0) Console.WriteLine("Библиотека пуста.");
                        else
                        {
                            foreach (var book in library)
                                Console.WriteLine($"ID: {book.Id} | {book.Title} | {book.Author} | {book.Genre} | {book.Year} г. | {book.Price} руб. | Кол-во: {book.Quantity}");
                        }
                        Console.WriteLine("\nНажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.WriteLine("\n--- ДОБАВЛЕНИЕ НОВОЙ КНИГИ ---");
                        string title = ReadNonEmptyString("Введите название книги: ");
                        string author = ReadNonEmptyString("Введите автора книги: ");

                        Console.WriteLine("\nДоступные жанры: 1.Fantasy 2.Romance 3.Detective 4.Classic 5.Philosophy 6.Drama");
                        int genreChoice = ReadValidInt("Введите номер жанра (1-6): ", 1);

                        Genre newGenre = Genre.Fantasy;
                        // ЭТОТ SWITCH ТОЛЬКО ДЛЯ ЖАНРОВ!
                        switch (genreChoice)
                        {
                            case 1: newGenre = Genre.Fantasy; break;
                            case 2: newGenre = Genre.Romance; break;
                            case 3: newGenre = Genre.Detective; break;
                            case 4: newGenre = Genre.Classic; break;
                            case 5: newGenre = Genre.Philosophy; break;
                            case 6: newGenre = Genre.Drama; break;
                            default: newGenre = Genre.Fantasy; break;
                        } // <-- КОНЕЦ ВЫБОРА ЖАНРА

                        int year = ReadValidInt("Введите год издания: ", 1000);
                        decimal price = ReadValidDecimal("Введите цену книги: ", 0);
                        int quantity = ReadValidInt("Введите количество экземпляров: ", 1);

                        library.Add(new Book { Id = nextId, Title = title, Author = author, Genre = newGenre, Year = year, Price = price, Quantity = quantity });
                        nextId++;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nКнига '{title}' успешно добавлена с ID: {nextId - 1}");
                        Console.ResetColor();
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        break; // <-- КОНЕЦ CASE 2 (ДОБАВЛЕНИЕ)

                    case 3:
                        Console.WriteLine("\n--- УДАЛЕНИЕ КНИГИ ПО ID ---");
                        int deleteId = ReadValidInt("Введите ID книги для удаления: ", 1);
                        Book bookToRemove = library.FirstOrDefault(b => b.Id == deleteId);

                        if (bookToRemove != null)
                        {
                            library.Remove(bookToRemove);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Книга '{bookToRemove.Title}' успешно удалена.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Ошибка: книги с ID {deleteId} не существует.");
                            Console.ResetColor();
                        }
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case 4:
                        Console.WriteLine("\n--- ПОИСК КНИГ ---");
                        Console.WriteLine("1. По названию  2. По автору  3. По жанру");
                        int searchChoice = ReadValidInt("Выберите тип поиска (1-3): ", 1);
                        List<Book> foundBooks = new List<Book>();

                        if (searchChoice == 1)
                        {
                            string term = ReadNonEmptyString("Введите часть названия: ");
                            foundBooks = library.Where(b => b.Title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                        }
                        else if (searchChoice == 2)
                        {
                            string term = ReadNonEmptyString("Введите часть имени автора: ");
                            foundBooks = library.Where(b => b.Author.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                        }
                        else if (searchChoice == 3)
                        {
                            Console.WriteLine("Доступные жанры: Fantasy, Romance, Detective, Classic, Philosophy, Drama");
                            string genreInput = ReadNonEmptyString("Введите название жанра: ");
                            if (Enum.TryParse<Genre>(genreInput, true, out Genre selectedGenre))
                                foundBooks = library.Where(b => b.Genre == selectedGenre).ToList();
                            else
                                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Ошибка: неверное название жанра."); Console.ResetColor();
                        }

                        Console.WriteLine("\n--- РЕЗУЛЬТАТЫ ---");
                        if (foundBooks.Count > 0)
                            foreach (var b in foundBooks) Console.WriteLine($"ID: {b.Id} | {b.Title} | {b.Author} | {b.Genre} | {b.Year} г. | {b.Price} руб.");
                        else
                            Console.WriteLine("Ничего не найдено.");

                        Console.WriteLine("\nНажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case 5: // СОРТИРОВКА LINQ
                        Console.WriteLine("\n--- СОРТИРОВКА ---");
                        Console.WriteLine("1. По названию (А-Я)");
                        Console.WriteLine("2. По году издания (от старых к новым)");
                        int sortChoice = ReadValidInt("Выберите (1-2): ", 1);

                        var sortedBooks = sortChoice == 1 ? library.OrderBy(b => b.Title) : library.OrderBy(b => b.Year);

                        foreach (var b in sortedBooks)
                            Console.WriteLine($"ID: {b.Id} | {b.Title} | {b.Year} г. | {b.Price} руб.");

                        Console.WriteLine("\nНажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case 6: // МИН/МАКС LINQ
                        Console.WriteLine("\n--- ЦЕНА ---");
                        if (library.Count == 0) Console.WriteLine("Библиотека пуста.");
                        else
                        {
                            var maxPrice = library.OrderByDescending(b => b.Price).First();
                            var minPrice = library.OrderBy(b => b.Price).First();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"САМАЯ ДОРОГАЯ: '{maxPrice.Title}' ({maxPrice.Price} руб.)");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"САМАЯ ДЕШЕВАЯ: '{minPrice.Title}' ({minPrice.Price} руб.)");
                            Console.ResetColor();
                        }
                        Console.WriteLine("\nНажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case 7: // ГРУППИРОВКА LINQ
                        Console.WriteLine("\n--- КНИГИ ПО АВТОРАМ ---");
                        var grouped = library.GroupBy(b => b.Author)
                                             .Select(g => new { Author = g.Key, Count = g.Count() });

                        foreach (var g in grouped)
                            Console.WriteLine($"Автор: {g.Author} | Количество книг: {g.Count}");

                        Console.WriteLine("\nНажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case 0:
                        Console.WriteLine("До свидания!");
                        isRunning = false;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
        static string ReadNonEmptyString(string prompt) //метод чтобы убрать пустые строки для безопасного ввода
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();

                if(string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка, поле не может быть пустым.");
                    Console.ResetColor ();
                }
            }
            while (string.IsNullOrEmpty(input));
            return input;
        }
        static int ReadValidInt(string prompt, int minValue = 0) // метод для безопасного ввода целого числа года или кол-ва
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if(int.TryParse(input, out result) && result >= minValue)
                {
                    return result;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка, надо вводить корректное целое число (не меньше {minValue})");
                Console.ResetColor ();
            }
        }
        static decimal ReadValidDecimal(string prompt, decimal minValue = 0) // Метод для безопасного ввода цены(decimal)
        {
            decimal result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out result) && result >= minValue)
                {
                    return result;
                }
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine($"Ошибка, введите корректную цену, число не меньше {minValue}).");
                Console.ResetColor ();
            }
        }

    }
}