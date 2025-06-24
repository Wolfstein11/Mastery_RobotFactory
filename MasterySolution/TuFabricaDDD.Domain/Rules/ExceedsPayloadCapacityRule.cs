// Ruta: TuFabricaDDD.Domain/Rules/ExceedsPayloadCapacityRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class ExceedsPayloadCapacityRule : IBusinessRule
{
    private readonly double _maxCapacity;
    private readonly double _attemptedTotalWeight;

    public ExceedsPayloadCapacityRule(double maxCapacity, double attemptedTotalWeight)
    {
        _maxCapacity = maxCapacity;
        _attemptedTotalWeight = attemptedTotalWeight;
    }

    public bool IsBroken() => _attemptedTotalWeight > _maxCapacity;

    public string Message => $"La carga excede la capacidad máxima del robot ({_maxCapacity:F1} kg). Carga intentada: {_attemptedTotalWeight:F1} kg.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}