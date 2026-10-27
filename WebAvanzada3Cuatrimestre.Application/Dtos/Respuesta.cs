using System;
using System.Collections.Generic;
using System.Text;

namespace WebAvanzada3Cuatrimestre.Application.Dtos
{
    //Patron de diseno Result, tiene objetivo de estandarizar la respuesta de los servicios, para que siempre devuelvan un objeto con la misma estructura, y no solo el dato, sino tambien un mensaje y un codigo de estado.
    public class Respuesta<T> 
    {

        public bool EsCorrecto { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public int Codigo { get; set; }

        public T Dato {get; set; }


        public Respuesta()
        {
            EsCorrecto = true;
            Mensaje = "Operacion realizada correctamente";
            Codigo = 200;
        }
    }
}
