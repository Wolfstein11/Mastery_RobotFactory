// Ruta: TuFabricaDDD.Domain/Rules/MustHaveManipulatorsRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;

namespace TuFabricaDDD.Domain.Rules;

public class MustHaveManipulatorsRule : IBusinessRule
{
    private readonly bool _hasManipulators;

    public MustHaveManipulatorsRule(bool hasManipulators)
    {
        _hasManipulators = hasManipulators;
    }

    public bool IsBroken() => !_hasManipulators;

    public string Message => "El robot humanoide no tiene manipuladores para realizar esta tarea.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}