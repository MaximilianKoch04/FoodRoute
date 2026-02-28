using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Proyecto.Models.Emuns
{
    public enum EstadoLote
    {
        EnInspeccion = 1,
        Disponible = 2,
        ProximoAVencer = 3,
        Agotado = 4,
        Vencido = 5,
        Descartado = 6
    }
}