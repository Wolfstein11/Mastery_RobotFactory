// Ruta: TuFabricaDDD.Domain/Rules/WaterLevelMustBeSufficientRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class WaterLevelMustBeSufficientRule : IBusinessRule
{
    private readonly double _currentLevel;
    private readonly double _requiredLevel;

    public WaterLevelMustBeSufficientRule(double currentLevel, double requiredLevel)
    {
        _currentLevel = currentLevel;
        _requiredLevel = requiredLevel;
    }

    public bool IsBroken() => _currentLevel < _requiredLevel;

    public string Message => $"Nivel de agua insuficiente ({_currentLevel:F1}%). Se requiere al menos {_requiredLevel:F1}%.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}