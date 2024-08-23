public class TypewiseAlert
{
    public enum BreachType
    {
        NORMAL,
        TOO_LOW,
        TOO_HIGH
    };

    public enum CoolingType
    {
        PASSIVE_COOLING,
        HI_ACTIVE_COOLING,
        MED_ACTIVE_COOLING
    };

    public enum AlertTarget
    {
        TO_CONTROLLER,
        TO_EMAIL
    };

    public struct BatteryCharacter
    {
        public CoolingType coolingType;
        public string brand;
    }

    private static readonly Dictionary<CoolingType, (int lowerLimit, int upperLimit)> CoolingLimits = new()
    {
        { CoolingType.PASSIVE_COOLING, (0, 35) },
        { CoolingType.HI_ACTIVE_COOLING, (0, 45) },
        { CoolingType.MED_ACTIVE_COOLING, (0, 40) }
    };

    private static readonly Dictionary<BreachType, Action<string>> EmailActions = new()
    {
        { BreachType.TOO_LOW, recepient => Console.WriteLine($"To: {recepient}\nHi, the temperature is too low\n") },
        { BreachType.TOO_HIGH, recepient => Console.WriteLine($"To: {recepient}\nHi, the temperature is too high\n") }
    };

    public static BreachType InferBreach(double value, double lowerLimit, double upperLimit)
    {
        if (value < lowerLimit)
        {
            return BreachType.TOO_LOW;
        }
        if (value > upperLimit)
        {
            return BreachType.TOO_HIGH;
        }
        return BreachType.NORMAL;
    }

    public static BreachType ClassifyTemperatureBreach(CoolingType coolingType, double temperatureInC)
    {
        var limits = CoolingLimits[coolingType];
        return InferBreach(temperatureInC, limits.lowerLimit, limits.upperLimit);
    }

    public static void CheckAndAlert(AlertTarget alertTarget, BatteryCharacter batteryChar, double temperatureInC)
    {
        BreachType breachType = ClassifyTemperatureBreach(batteryChar.coolingType, temperatureInC);

        var alertActions = new Dictionary<AlertTarget, Action<BreachType>>
        {
            { AlertTarget.TO_CONTROLLER, SendToController },
            { AlertTarget.TO_EMAIL, breach => SendToEmail(breach, "a.b@c.com") }
        };

        alertActions[alertTarget](breachType);
    }

    public static void SendToController(BreachType breachType)
    {
        const ushort header = 0xfeed;
        Console.WriteLine($"{header} : {breachType}\n");
    }

    public static void SendToEmail(BreachType breachType, string recepient)
    {
        if (breachType != BreachType.NORMAL && EmailActions.ContainsKey(breachType))
        {
            EmailActions[breachType](recepient);
        }
    }
}
