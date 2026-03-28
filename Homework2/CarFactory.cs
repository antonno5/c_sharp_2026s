namespace Homework2;

public static class CarFactory
{
    public static ICar Create(CarType type) => type switch
    {
        CarType.Tesla => new Tesla(),
        CarType.MercedesEClass => new MercedesEClass(),
        CarType.LadaVesta => new LadaVesta(),
        CarType.PorscheTaycan => new PorscheTaycan(),
        CarType.FordFocus => new FordFocus(),
        CarType.MazdaMx5 => new MazdaMx5(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}
