// Ruta: TuFabricaDDD.API/Services/UnitGrpcService.cs
using AutoMapper; // Para IMapper
using FluentResults; // Para Result
using Google.Protobuf.WellKnownTypes;
using Grpc.Core; // Para ServerCallContext
using MediatR; // Para IMediator
using TuFabricaDDD.Application.Commands; // Para CreateUnitCommand
using TuFabricaDDD.GrpcContracts.Common; // Espacio de nombres generado por Protobuf para RobotCategory
using TuFabricaDDD.GrpcContracts.Units; // Espacio de nombres generado por Protobuf para UnitService y mensajes
using static TuFabricaDDD.GrpcContracts.Units.UnitService; // Importa el servicio gRPC generado
using TuFabricaDDD.Application.Queries; // Para GetAllUnitsQuery
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

        // Implementación del método RPC UpdateUnit
        public override async Task<Empty> UpdateUnit(UpdateUnitRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received UpdateUnit request for ID: {Id}", request.Id);

            var command = _mapper.Map<UpdateUnitCommand>(request);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update unit. Errors: {Errors}", string.Join("; ", result.Errors.Select(e => e.Message)));
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join("; ", result.Errors.Select(e => e.Message))));
            }

            _logger.LogInformation("Unit updated successfully with ID: {Id}", request.Id);
            return new Empty();
        }

        // Implementación del método RPC DeleteUnit
        public override async Task<Empty> DeleteUnit(DeleteRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received DeleteUnit request for ID: {Id}", request.Id);

            var command = new DeleteUnitCommand(Guid.Parse(request.Id));
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete unit. Errors: {Errors}", string.Join("; ", result.Errors.Select(e => e.Message)));
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join("; ", result.Errors.Select(e => e.Message))));
            }

            _logger.LogInformation("Unit deleted successfully with ID: {Id}", request.Id);
            return new Empty();
        }

        // Implementación del método RPC GetAllUnits
        public override async Task<UnitsResponse> GetAllUnits(GetAllUnitsRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received GetAllUnits request");

            var query = _mapper.Map<GetAllUnitsQuery>(request);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get units. Errors: {Errors}", string.Join("; ", result.Errors.Select(e => e.Message)));
                throw new RpcException(new Status(StatusCode.Internal, string.Join("; ", result.Errors.Select(e => e.Message))));
            }

            var response = new UnitsResponse();
            response.Items.AddRange(result.Value.Select(unit => _mapper.Map<UnitResponse>(unit)));
            return response;
        }

        // Implementación del método RPC GetUnitById *Not done*
        /*
        public override async Task<NullableUnitResponse> GetUnitById(GetRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received GetUnitById request for ID: {Id}", request.Id);

            var query = new GetUnitByIdQuery { Id = Guid.Parse(request.Id) };
            var result = await _mediator.Send(query);

            var response = new NullableUnitResponse();
            if (result.IsSuccess && result.Value != null)
            {
                response.Unit = _mapper.Map<UnitResponse>(result.Value);
            }
            return response;
        }
        */
    }
}