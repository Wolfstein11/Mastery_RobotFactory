// Ruta: TuFabricaDDD.Domain/Common/IEntity.cs
using System;

namespace TuFabricaDDD.Domain.Common;

/// <summary>
/// Interfaz de marcador para entidades de dominio.
/// Define una propiedad Id para identificar la entidad.
/// </summary>
public interface IEntity
{
    Guid Id { get; }
}