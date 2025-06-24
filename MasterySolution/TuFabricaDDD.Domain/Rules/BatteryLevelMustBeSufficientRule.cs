// Ruta: TuFabricaDDD.Domain/Rules/BatteryLevelMustBeSufficientRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class BatteryLevelMustBeSufficientRule : IBusinessRule
{
    private readonly double _currentBattery;
    private readonly double _requiredBattery;

    public BatteryLevelMustBeSufficientRule(double currentBattery, double requiredBattery)
    {
        _currentBattery = currentBattery;
        _requiredBattery = requiredBattery;
    }

    public bool IsBroken() => _currentBattery < _requiredBattery;

    public string Message => $"Nivel de batería insuficiente ({_currentBattery:F1}%). Se requiere al menos {_requiredBattery:F1}%.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}
