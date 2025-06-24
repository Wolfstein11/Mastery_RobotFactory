// Ruta: TuFabricaDDD.API/Services/UnitGrpcService.cs
using Grpc.Core; // Para ServerCallContext
using MediatR; // Para IMediator
using AutoMapper; // Para IMapper
using FluentResults; // Para Result
using TuFabricaDDD.Application.Commands; // Para CreateUnitCommand
using TuFabricaDDD.GrpcContracts.Units; // Espacio de nombres generado por Protobuf para UnitService y mensajes
using TuFabricaDDD.GrpcContracts.Common; // Espacio de nombres generado por Protobuf para RobotCategory
using static TuFabricaDDD.GrpcContracts.Units.UnitService; // Importa el servicio gRPC generado

namespace TuFabricaDDD.API.Services
{
    // Hereda de la clase base abstracta generada por Grpc.Tools
    public class UnitGrpcService : UnitServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<UnitGrpcService> _logger; // Para logging

        public UnitGrpcService(IMediator mediator, IMapper mapper, ILogger<UnitGrpcService> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        // Implementación del método RPC CreateUnit
        public override async Task<CreateUnitResponse> CreateUnit(CreateUnitRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received CreateUnit request for SerialNumber: {SerialNumber}", request.SerialNumber);

            // 1. Mapear CreateUnitRequest (DTO gRPC) a CreateUnitCommand (comando de aplicación)
            // Aquí es donde necesitaremos un mapeo de AutoMapper.
            var command = _mapper.Map<CreateUnitCommand>(request);

            // 2. Despachar el comando a través de MediatR
            var result = await _mediator.Send(command);

            // 3. Manejar el resultado y construir la respuesta gRPC
            if (result.IsSuccess)
            {
                _logger.LogInformation("Unit created successfully with ID: {UnitId}", result.Value);
                return new CreateUnitResponse
                {
                    UnitId = result.Value.ToString(), // Convertir Guid a string para el ID de la respuesta
                    IsSuccess = true,
                    Message = "Unit created successfully."
                };
            }
            else
            {
                _logger.LogError("Failed to create unit. Errors: {Errors}", string.Join("; ", result.Errors.Select(e => e.Message)));
                return new CreateUnitResponse
                {
                    UnitId = string.Empty,
                    IsSuccess = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Message)) // Unir todos los mensajes de error
                };
            }
        }
    }
}