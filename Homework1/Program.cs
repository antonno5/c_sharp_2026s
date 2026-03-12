using System;
using System.Globalization;

Console.WriteLine("=== Калькулятор ===");
Console.WriteLine("Введите 'q' для выхода.");
Console.WriteLine();

while (true)
{
    try
    {
        Console.Write("Введите первое число: ");
        string? firstInput = Console.ReadLine();
        if (IsExitCommand(firstInput))
        {
            Console.WriteLine("Выход из программы.");
            break;
        }

        if (!double.TryParse(firstInput?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double firstNumber))
        {
            Console.WriteLine("Ошибка: введите корректное число.\n");
            continue;
        }

        Console.Write("Введите второе число: ");
        string? secondInput = Console.ReadLine();
        if (IsExitCommand(secondInput))
        {
            Console.WriteLine("Выход из программы.");
            break;
        }

        if (!double.TryParse(secondInput?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double secondNumber))
        {
            Console.WriteLine("Ошибка: введите корректное число.\n");
            continue;
        }

        Console.WriteLine("Выберите операцию: + (сложение), - (вычитание), * (умножение), / (деление)");
        Console.Write("Операция: ");
        string? operation = Console.ReadLine();
        if (IsExitCommand(operation))
        {
            Console.WriteLine("Выход из программы.");
            break;
        }

        double result = operation switch
        {
            "+" => firstNumber + secondNumber,
            "-" => firstNumber - secondNumber,
            "*" => firstNumber * secondNumber,
            "/" => secondNumber != 0 ? firstNumber / secondNumber : throw new DivideByZeroException(),
            _ => throw new ArgumentException("Неизвестная операция. Используйте +, -, *, /")
        };

        Console.WriteLine($"Результат: {firstNumber} {operation} {secondNumber} = {result}\n");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Ошибка: деление на ноль.\n");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}\n");
    }
}

static bool IsExitCommand(string? input)
{
    return string.Equals(input?.Trim(), "q", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(input?.Trim(), "quit", StringComparison.OrdinalIgnoreCase);
}
