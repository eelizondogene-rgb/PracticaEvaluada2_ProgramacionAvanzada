using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Domain;


namespace WebAvanzada3Cuatrimestre.Application.Mapping
{
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