using System;
using System.Collections.Generic;

try
{
    Console.WriteLine("Testing Repository");
    RunRepositoryTests();
    Console.WriteLine("Testing CollectionUtils");
    RunCollectionUtilsTests();
    Console.WriteLine("Testing completed successfully!!!");
}
catch
{
    Console.WriteLine("Testing failed");
}

static void RunRepositoryTests()
{
    var productRepository = new Repository<Product>();
    var userRepository = new Repository<User>();

    productRepository.Add(new Product { Id = 1, Name = "Ноутбук", Price = 1200m });
    productRepository.Add(new Product { Id = 2, Name = "Мышь", Price = 80m });
    productRepository.Add(new Product { Id = 3, Name = "Смартфон", Price = 1500m });

    userRepository.Add(new User { Id = 1, Name = "Антон", Age = 20 });
    userRepository.Add(new User { Id = 2, Name = "Мария", Age = 22 });

    if (productRepository.Count != 3 || userRepository.Count != 2)
    {
        throw new Exception("Count test failed.");
    }

    Product? byId = productRepository.GetById(2);
    if (byId is null || byId.Name != "Мышь")
    {
        throw new Exception("GetById test failed.");
    }

    IReadOnlyList<Product> expensiveProducts = productRepository.Find(p => p.Price > 1000m);
    if (expensiveProducts.Count != 2)
    {
        throw new Exception("Find test failed.");
    }

    bool duplicateThrown = false;
    try
    {
        productRepository.Add(new Product { Id = 1, Name = "Дубликат", Price = 999m });
    }
    catch (InvalidOperationException)
    {
        duplicateThrown = true;
    }

    if (!duplicateThrown)
    {
        throw new Exception("Duplicate add test failed.");
    }
}

static void RunCollectionUtilsTests()
{
    List<int> numbers = new() { 1, 2, 2, 3, 1, 4, 4 };
    List<int> distinctNumbers = CollectionUtils.Distinct(numbers);
    if (distinctNumbers.Count != 4 || distinctNumbers[0] != 1 || distinctNumbers[3] != 4)
    {
        throw new Exception("Distinct<int> test failed.");
    }

    List<string> wordsForDistinct = new() { "cat", "dog", "cat", "bird", "dog" };
    List<string> distinctWords = CollectionUtils.Distinct(wordsForDistinct);
    if (distinctWords.Count != 3 || distinctWords[2] != "bird")
    {
        throw new Exception("Distinct<string> test failed.");
    }

    List<string> words = new() { "sun", "moon", "sky", "star", "cloud", "sea" };
    Dictionary<int, List<string>> groupedByLength = CollectionUtils.GroupBy(words, w => w.Length);
    if (!groupedByLength.ContainsKey(3) || groupedByLength[3].Count != 3)
    {
        throw new Exception("GroupBy test failed.");
    }

    var first = new Dictionary<string, int>
    {
        ["apple"] = 2,
        ["banana"] = 3
    };
    var second = new Dictionary<string, int>
    {
        ["banana"] = 5,
        ["orange"] = 4
    };
    Dictionary<string, int> merged = CollectionUtils.Merge(first, second, (a, b) => a + b);
    if (merged["banana"] != 8 || merged["apple"] != 2 || merged["orange"] != 4)
    {
        throw new Exception("Merge test failed.");
    }

    List<Product> products = new()
    {
        new Product { Id = 10, Name = "Клавиатура", Price = 300m },
        new Product { Id = 11, Name = "Монитор", Price = 1100m },
        new Product { Id = 12, Name = "Планшет", Price = 900m }
    };
    Product mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
    if (mostExpensive.Id != 11)
    {
        throw new Exception("MaxBy test failed.");
    }
}
