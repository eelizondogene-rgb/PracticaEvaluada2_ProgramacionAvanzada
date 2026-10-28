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

            // Persona 3: aquí se agrega el mapeo de Producto

        }
    }
}