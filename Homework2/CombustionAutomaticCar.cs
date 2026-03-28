namespace Homework2;

public abstract class CombustionAutomaticCar : ACar, Homework2.Cars.IMechanical, Homework2.Transmission.IAutomatical
{
    protected override string PowertrainPart => "автомобиль с ДВС";
    protected override string GearboxPart => "автоматической коробкой передач";
}
