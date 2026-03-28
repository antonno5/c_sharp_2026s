using Homework2;

Console.WriteLine("Введите марку автомобиля или done для остановки ввода :");

while (true)
{
    string? line = Console.ReadLine();
    if (string.Equals(line?.Trim(), "done", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (!CarInputParser.TryParseCarType(line, out CarType carType))
    {
        Console.WriteLine("Неизвестная марка. Доступны: Tesla, Mercedes E-Class, Lada Vesta, Porsche Taycan, Ford Focus, Mazda MX-5 (или имя enum CarType).");
        Console.WriteLine();
        Console.WriteLine("Введите марку автомобиля или done для остановки ввода :");
        continue;
    }

    ICar car = CarFactory.Create(carType);
    Console.WriteLine(car.GetDescription());
    Console.WriteLine();
    Console.WriteLine("Введите марку автомобиля или done для остановки ввода :");
}
