namespace BackMonoLegal.Domain.Mappers
{
    using AutoMapper;
    using BackMonoLegal.Domain.Models;

    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cliente, FacturaClienteDTO>()
                    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Nombre));
                cfg.CreateMap<Factura, FacturaClienteDTO>();
            });

            return configuration.CreateMapper();
        }
    }


}
