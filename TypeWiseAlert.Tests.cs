using System;
using Xunit;
namespace Conclusive
{

public class TypewiseAlertTests
{
    [Theory]
    [InlineData(20, 10, 30, TypewiseAlert.BreachType.NORMAL)]
    [InlineData(5, 10, 30, TypewiseAlert.BreachType.TOO_LOW)]
    [InlineData(35, 10, 30, TypewiseAlert.BreachType.TOO_HIGH)]
    public void InferBreach_ReturnsCorrectBreachType(double value, double lowerLimit, double upperLimit, TypewiseAlert.BreachType expectedBreachType)
    {
        var breachType = TypewiseAlert.InferBreach(value, lowerLimit, upperLimit);
        Assert.Equal(expectedBreachType, breachType);
    }

    [Theory]
    [InlineData(TypewiseAlert.CoolingType.PASSIVE_COOLING, 30, TypewiseAlert.BreachType.NORMAL)]
    [InlineData(TypewiseAlert.CoolingType.HI_ACTIVE_COOLING, 50, TypewiseAlert.BreachType.TOO_HIGH)]
    [InlineData(TypewiseAlert.CoolingType.MED_ACTIVE_COOLING, 0, TypewiseAlert.BreachType.NORMAL)]
    public void ClassifyTemperatureBreach_ReturnsCorrectBreachType(TypewiseAlert.CoolingType coolingType, double temperatureInC, TypewiseAlert.BreachType expectedBreachType)
    {
        var breachType = TypewiseAlert.ClassifyTemperatureBreach(coolingType, temperatureInC);
        Assert.Equal(expectedBreachType, breachType);
    }

  

   

    
   

   
}

}
