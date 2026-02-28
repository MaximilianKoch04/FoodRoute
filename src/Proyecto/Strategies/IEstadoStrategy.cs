using System;
using Proyecto.Models.Emuns;

namespace Proyecto.Strategies
{
    public interface IEvaluadorEstadoStrategy
    {
        // Este método recibe la fecha y devuelve si está Disponible, Próximo a Vencer o Vencido
        EstadoLote EvaluarEstado(DateTime fechaVencimiento); 
    }
}