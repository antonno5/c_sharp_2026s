namespace Homework2;

public abstract class CombustionManualCar : ACar, Homework2.Cars.IMechanical, Homework2.Transmission.IMechanical
{
    protected override string PowertrainPart => "автомобиль с ДВС";
    protected override string GearboxPart => "механической коробкой передач";
}
