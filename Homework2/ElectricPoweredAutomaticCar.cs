using Homework2.Cars;
using Homework2.Transmission;

namespace Homework2;

public abstract class ElectricPoweredAutomaticCar : ACar, IElectric, IAutomatical
{
    protected override string PowertrainPart => "electrical car";
    protected override string GearboxPart => "автоматической коробкой передач";
}
