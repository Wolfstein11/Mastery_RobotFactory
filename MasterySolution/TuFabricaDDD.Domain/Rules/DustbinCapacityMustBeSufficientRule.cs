// Ruta: TuFabricaDDD.Domain/Rules/DustbinCapacityMustBeSufficientRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class DustbinCapacityMustBeSufficientRule : IBusinessRule
{
    private readonly double _currentCapacity;
    private readonly double _maxAllowedCapacity;

    public DustbinCapacityMustBeSufficientRule(double currentCapacity, double maxAllowedCapacity)
    {
        _currentCapacity = currentCapacity;
        _maxAllowedCapacity = maxAllowedCapacity;
    }

    public bool IsBroken() => _currentCapacity >= _maxAllowedCapacity;

    public string Message => $"La papelera está demasiado llena ({_currentCapacity:F1}%). No se puede iniciar la tarea.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}