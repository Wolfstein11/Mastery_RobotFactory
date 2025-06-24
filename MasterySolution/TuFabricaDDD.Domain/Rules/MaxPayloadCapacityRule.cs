// Ruta: TuFabricaDDD.Domain/Rules/MaxPayloadCapacityRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common; // Asegúrate de tener IBusinessRule aquí

namespace TuFabricaDDD.Domain.Rules;

public class MaxPayloadCapacityRule : IBusinessRule
{
    private readonly double _maxCapacity;
    private readonly double _attemptedWeight;

    public MaxPayloadCapacityRule(double maxCapacity, double attemptedWeight)
    {
        _maxCapacity = maxCapacity;
        _attemptedWeight = attemptedWeight;
    }

    public bool IsBroken() => _attemptedWeight > _maxCapacity;

    public string Message => $"La carga de {_attemptedWeight:F1} kg excede la capacidad máxima del brazo robótico ({_maxCapacity:F1} kg).";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}