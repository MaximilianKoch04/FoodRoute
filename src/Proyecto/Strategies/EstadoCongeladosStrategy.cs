using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Proyecto.Models.Emuns;

namespace Proyecto.Strategies
{
    public class EstadoCongeladosStrategy : IEvaluadorEstadoStrategy
    {
        public EstadoLote EvaluarEstado(DateTime fechaVencimiento)
        {
            var diasRestantes = (fechaVencimiento - DateTime.Now).TotalDays;

            if (diasRestantes < 0)
                return EstadoLote.Vencido;
            else if (diasRestantes <= 15)
                return EstadoLote.ProximoAVencer;
            else
                return EstadoLote.Disponible;
        }
    }
}