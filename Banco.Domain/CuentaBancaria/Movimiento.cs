using Banco.Domain.CuentaBancaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class Movimiento
    {
        public Movimiento(CuentaBancariaBase cuentaBancariaBase, DateTime fecha, string tipo, decimal valor)
        {
            CuentaAhorro = cuentaBancariaBase;
            Fecha = fecha;
            Tipo = tipo;
            Valor = valor;
        }

        public CuentaBancariaBase CuentaAhorro { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tipo { get; private set; }
        public decimal Valor { get; private set; }
    }
}
