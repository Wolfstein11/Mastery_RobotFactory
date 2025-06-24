// Ruta: TuFabricaDDD.Domain/Rules/DetergentLevelMustBeSufficientRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class DetergentLevelMustBeSufficientRule : IBusinessRule
{
    private readonly double _currentLevel;
    private readonly double _requiredLevel;

    public DetergentLevelMustBeSufficientRule(double currentLevel, double requiredLevel)
    {
        _currentLevel = currentLevel;
        _requiredLevel = requiredLevel;
    }

    public bool IsBroken() => _currentLevel < _requiredLevel;

    public string Message => $"Nivel de detergente insuficiente ({_currentLevel:F1}%). Se requiere al menos {_requiredLevel:F1}%.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}