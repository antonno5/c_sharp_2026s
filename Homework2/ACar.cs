namespace Homework2;

public abstract class ACar : ICar
{
    protected abstract string BrandName { get; }
    protected abstract string PowertrainPart { get; }
    protected abstract string GearboxPart { get; }
    protected virtual int Seats => 5;
    protected virtual string InfotainmentPart => "стандартная медиасистема";

    public string GetDescription()
    {
        return $"«{BrandName}: {PowertrainPart} with {GearboxPart}, {Seats} местами, {InfotainmentPart}»";
    }
}
