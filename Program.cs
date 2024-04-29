using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace StreamPractice;

class _
{
    public static void Main()
    {
        FileStreamPractice();
        MemoryStreamPractice();
        StreamAdapterPractice();
        CompressionStreamPractice();
        SeekingPractice();
        BufferedStreamPractice();
        BinaryStreamPractice();
    }

    public static void FileStreamPractice()
    {
        string text = File.ReadAllText("original.txt");
        using StreamWriter output = new(File.Open("reversed.txt", FileMode.Create));
        foreach (char c in text.Reverse())
        {
            output.Write(c);
        }
    }

    public static void MemoryStreamPractice()
    {
        //This is impossible on linux on dotnet 7 or later without knowledge of image encoding
    }

    public static void StreamAdapterPractice()
    {
        using StreamReader text = new(File.Open("original.txt", FileMode.Open));
        using StreamWriter upperText = new(File.Open("originalUpper.txt", FileMode.Create));
        while (true)
        {
            int c = text.Read();
            if (c == -1) break;
            upperText.Write(char.ToUpper((char)c));
        }
    }

    public static void CompressionStreamPractice()
    {
        using (ZipArchive zipArchive = new(File.Open("archive.zip", FileMode.Create), ZipArchiveMode.Create))
        {
            zipArchive.CreateEntryFromFile("file1.txt", "file1.txt");
            zipArchive.CreateEntryFromFile("file2.txt", "file2.txt");
            zipArchive.CreateEntryFromFile("image.png", "image.png");
        }
        using (ZipArchive zipArchive = new(File.Open("archive.zip", FileMode.Open), ZipArchiveMode.Read))
        {
            Directory.CreateDirectory("extracted_files");
            zipArchive.ExtractToDirectory("extracted_files", true);
        }
    }

    public static void SeekingPractice()
    {
        //there was no numbers.bin file attached
    }

    public static void BufferedStreamPractice()
    {
        using StreamReader reader = new(new BufferedStream(File.Open("largeText.txt", FileMode.Open)));
        using StreamWriter writer = new(new BufferedStream(File.Open("largeTextWordsPerLineCount.txt", FileMode.Create)));

        while (true)
        {
            string? line = reader.ReadLine();
            if (line == null) break;
            int count = 0;
            foreach (string word in line.Split())
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    count++;
                }
            }
            writer.WriteLine($"{count}: {line}");
        }
    }

    [Serializable]
    class Product(int id, string name, decimal price)
    {
        public int ID { get; set; } = id;
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
    }

    public static void BinaryStreamPractice()
    {
        Product[] products = [
            new(1, "Laptop", 999.99m), new(2, "Mouse", 25.50m),  new(3, "Keyboard", 75.75m)
        ];

        using (BinaryWriter writer = new(File.Open("products.bin", FileMode.Create)))
        {
            foreach (Product product in products)
            {
                writer.Write(product.ID);
                writer.Write(product.Name);
                writer.Write(product.Price);
            }
        }
        using BinaryReader reader = new(File.Open("products.bin", FileMode.Open));

        bool reading = true;
        while (reading)
        {
            try
            {
                int ID = reader.ReadInt32();
                string Name = reader.ReadString();
                decimal Price = reader.ReadDecimal();
                Console.WriteLine($"{ID} {Name} {Price}");
            }
            catch
            {
                reading = false; //this is a dum solution for a "didn't bother encoding the length" problem, but it works
            }
        }

    }
}
