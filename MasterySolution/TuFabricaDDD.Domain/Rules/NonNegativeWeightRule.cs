// Ruta: TuFabricaDDD.Domain/Rules/NonNegativeWeightRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class NonNegativeWeightRule : IBusinessRule
{
    private readonly double _weight;

    public NonNegativeWeightRule(double weight)
    {
        _weight = weight;
    }

    public bool IsBroken() => _weight < 0;

    public string Message => "El peso no puede ser negativo.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}