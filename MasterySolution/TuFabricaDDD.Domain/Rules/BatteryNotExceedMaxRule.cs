// Ruta: TuFabricaDDD.Domain/Rules/BatteryNotExceedMaxRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class BatteryNotExceedMaxRule : IBusinessRule
{
    private readonly double _currentBattery;
    private readonly double _chargeAmount;

    public BatteryNotExceedMaxRule(double currentBattery, double chargeAmount)
    {
        _currentBattery = currentBattery;
        _chargeAmount = chargeAmount;
    }

    public bool IsBroken() => (_currentBattery + _chargeAmount) > 100.0;

    public string Message => $"La carga de la batería excedería el 100% ({_currentBattery + _chargeAmount:F1}%).";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}