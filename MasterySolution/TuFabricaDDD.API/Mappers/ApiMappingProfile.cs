// Ruta: TuFabricaDDD.API/Mappers/ApiMappingProfile.cs
using AutoMapper;
using TuFabricaDDD.Application.Commands; // Para CreateUnitCommand
using TuFabricaDDD.GrpcContracts.Units; // Para CreateUnitRequest
using TuFabricaDDD.GrpcContracts.Common; // Para RobotCategory (gRPC)
using TuFabricaDDD.Domain.Types; // Para RobotCategory (Dominio)

namespace TuFabricaDDD.API.Mappers
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            // Mapeo de gRPC Request a Application Command
            CreateMap<CreateUnitRequest, CreateUnitCommand>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (Domain.Types.RobotCategory)src.Category))
                // Asegúrate de que los nombres de los campos coincidan o usa ForMember explícitamente.
                // Si el nombre de la propiedad en el source (request) es diferente al del dest (command),
                // necesitarás un .ForMember(). Asumimos que la mayoría coinciden.
                // Por ejemplo, si Category en gRPC es GrpcCategory y en el comando es Category, necesitarías:
                // .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.GrpcCategory))

                // Aquí manejamos la conversión de int (gRPC enum) a Domain.Types.RobotCategory
                // La conversión (Domain.Types.RobotCategory)src.Category es implícita si los valores numéricos coinciden,
                // pero si el gRPC enum tiene un 0 (UNKNOWN_CATEGORY) y tu dominio no lo mapea a algo válido,
                // podrías necesitar un ValueConverter o un .ConvertUsing.
                // Si ambos tienen 0 como UnknownCategory/Unknown, esta conversión directa funciona.
                ;

            // También puedes definir mapeos inversos si los necesitaras para respuestas
            // CreateMap<SomeDomainObject, SomeGrpcResponse>();
        }
    }
}
