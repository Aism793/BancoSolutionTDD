using Banco.Domain.CuentaBancaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class CuentaCorriente : CuentaBancariaBase
    {
        public decimal Sobregiro { get; private set; }

        public CuentaCorriente(string numero, string nombre, string ciudad, decimal sobregiro) : base(numero, nombre, ciudad, 100000)
        {
            Sobregiro = -sobregiro;
        }

        public override string Retirar(decimal valorRetiro, DateTime fecha)
        {
            var nuevoSaldoTemporal = Saldo - valorRetiro - valorRetiro * 4 / 1000;
            if (nuevoSaldoTemporal > Sobregiro)
            {
                Saldo = nuevoSaldoTemporal;
                return $"Su Nuevo Saldo es de {Saldo} pesos m/c";
            }
            throw new NotImplementedException();
        }
    }
}
