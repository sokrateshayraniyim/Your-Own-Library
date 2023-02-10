using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;

namespace Kitaplık
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup();
        }

        static void Startup()
        {

            Console.WriteLine("1. Kitap Ekle");
            Console.WriteLine("2. Kitap Sorgula");
            Console.WriteLine("3. Çıkış");

            Console.Write("Seçiminiz: ");
            int selection = Convert.ToInt32(Console.ReadLine());

            switch (selection)
            {
                case 1:
                    AddBook();
                    break;
                case 2:
                    SearchBooks();
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                    break;
            }
        }


        static void AddBook()
        {
            Console.Write("Kitap Adı: ");
            string bookName = Console.ReadLine();

            Console.Write("Yazar Adı: ");
            string authorName = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection("Data Source=(local);Initial Catalog=BookDB;Integrated Security=True"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO Books (BookName, AuthorName) VALUES (@BookName, @AuthorName)", connection))
                {
                    command.Parameters.AddWithValue("@BookName", bookName);
                    command.Parameters.AddWithValue("@AuthorName", authorName);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Kitap başarıyla eklendi.");
                    Console.ReadLine();
                    Startup();
                }
            }
        }

        static void SearchBooks()
        {
            Console.Write("Aranacak Kitap Adı: ");
            string bookName = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection("Data Source=(local);Initial Catalog=BookDB;Integrated Security=True"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT BookName, AuthorName FROM Books WHERE BookName LIKE @BookName", connection))
                {
                    command.Parameters.AddWithValue("@BookName", "%" + bookName + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Kitap Adı\tYazar Adı");
                        Console.WriteLine("---------\t----------");

                        while (reader.Read())
                        {
                            Console.WriteLine("{0}\t{1}", reader["BookName"], reader["AuthorName"]);
                        }
                    }
                }



                Console.ReadLine();
                Startup();
            }
        }
    }
}
