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

    [Fact]
    public void CheckAndAlert_SendsToController_WhenAlertTargetIsController()
    {
        // Arrange
        var batteryChar = new TypewiseAlert.BatteryCharacter
        {
            coolingType = TypewiseAlert.CoolingType.HI_ACTIVE_COOLING,
            brand = "BrandA"
        };

        // Capture console output
        using var consoleOutput = new ConsoleOutput();
        
        // Act
        TypewiseAlert.CheckAndAlert(TypewiseAlert.AlertTarget.TO_CONTROLLER, batteryChar, 50);

        // Assert
        Assert.Contains("0xfeed : TOO_HIGH", consoleOutput.GetOutput());
    }

    [Fact]
    public void CheckAndAlert_SendsToEmail_WhenAlertTargetIsEmail()
    {
        // Arrange
        var batteryChar = new TypewiseAlert.BatteryCharacter
        {
            coolingType = TypewiseAlert.CoolingType.PASSIVE_COOLING,
            brand = "BrandB"
        };

        // Capture console output
        using var consoleOutput = new ConsoleOutput();

        // Act
        TypewiseAlert.CheckAndAlert(TypewiseAlert.AlertTarget.TO_EMAIL, batteryChar, 5);

        // Assert
        Assert.Contains("To: a.b@c.com", consoleOutput.GetOutput());
        Assert.Contains("Hi, the temperature is too low", consoleOutput.GetOutput());
    }

    [Fact]
    public void SendToController_PrintsCorrectMessage()
    {
        // Capture console output
        using var consoleOutput = new ConsoleOutput();

        // Act
        TypewiseAlert.SendToController(TypewiseAlert.BreachType.TOO_HIGH);

        // Assert
        Assert.Contains("0xfeed : TOO_HIGH", consoleOutput.GetOutput());
    }

    [Fact]
    public void SendToEmail_PrintsCorrectMessageForTooLow()
    {
        // Capture console output
        using var consoleOutput = new ConsoleOutput();

        // Act
        TypewiseAlert.SendToEmail(TypewiseAlert.BreachType.TOO_LOW, "a.b@c.com");

        // Assert
        Assert.Contains("To: a.b@c.com", consoleOutput.GetOutput());
        Assert.Contains("Hi, the temperature is too low", consoleOutput.GetOutput());
    }

    [Fact]
    public void SendToEmail_DoesNotPrintForNormal()
    {
        // Capture console output
        using var consoleOutput = new ConsoleOutput();

        // Act
        TypewiseAlert.SendToEmail(TypewiseAlert.BreachType.NORMAL, "a.b@c.com");

        // Assert
        Assert.Equal(string.Empty, consoleOutput.GetOutput());
    }
}

}
