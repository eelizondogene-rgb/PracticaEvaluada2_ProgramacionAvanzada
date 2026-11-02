using AutoMapper;
namespace WebAvanzada3Cuatrimestre.Application.Mapping;

// Los integrantes de los CRUD agregaran aqui los mapeos Categoria/Producto y sus DTO.
public class MapeoClases : Profile
{
<<<<<<< Updated upstream
    public MapeoClases() { }
}
=======
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            // Persona 2: Categoria
            CreateMap<Categoria, Dtos.CategoriaDto>().ReverseMap();

            CreateMap<Producto, ProductoDto>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Categoria, opt => opt.Ignore());

        }
    }
}
>>>>>>> Stashed changes
