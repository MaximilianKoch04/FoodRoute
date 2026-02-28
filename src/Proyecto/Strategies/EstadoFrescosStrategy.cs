using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models.Emuns;

namespace Proyecto.Strategies
{
    public class EstadoFrescosStrategy : IEvaluadorEstadoStrategy
    {
        public EstadoLote EvaluarEstado(DateTime fechaVencimiento)
        {
            var diasRestantes = (fechaVencimiento - DateTime.Now).TotalDays;

            if (diasRestantes < 0)
                return EstadoLote.Vencido;
            else if (diasRestantes <= 3)
                return EstadoLote.ProximoAVencer;
            else
                return EstadoLote.Disponible;
        }
        
    }
}