// Ruta: TuFabricaDDD.Domain/Rules/RobotNotBrokenRule.cs
using FluentResults;
using TuFabricaDDD.Domain.Common;
using TuFabricaDDD.Domain.Types; // Necesario para RobotStatus

namespace TuFabricaDDD.Domain.Rules;

public class RobotNotBrokenRule : IBusinessRule
{
    private readonly RobotStatus _status;

    public RobotNotBrokenRule(RobotStatus status)
    {
        _status = status;
    }

    public bool IsBroken() => _status == RobotStatus.Broken;

    public string Message => "El robot está averiado y no puede realizar esta operación.";

    public Result CheckRule() => IsBroken() ? Result.Fail(Message) : Result.Ok();
}
