using Banco.Domain.CuentaBancaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class CuentaAhorro : CuentaBancariaBase
    {

        public CuentaAhorro(string numero, string nombre, string ciudad) : base(numero, nombre, ciudad)
        {

        }

        public override string Consignar(decimal valorConsignacion, DateTime fecha, string ciudad)
        {
            if (valorConsignacion < 0 && this.Ciudad.Equals(ciudad))
            {
                return "El valor a consignar es incorrecto";
            }
            if (!_movimientos.Any() && valorConsignacion >= 50000 && this.Ciudad.Equals(ciudad))
            {
                _movimientos.Add(new Movimiento(cuentaBancariaBase: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;

                return $"Su Nuevo Saldo es de {Saldo} pesos m/c";
            }
            if (!_movimientos.Any() && valorConsignacion < 50000 && this.Ciudad.Equals(ciudad))
            {
                return "El valor mínimo de la primera consignación debe ser de $50.000 mil pesos. Su nuevo saldo es $0 pesos";
            }
            if (_movimientos.Any() && this.Ciudad.Equals(ciudad))
            {
                _movimientos.Add(new Movimiento(cuentaBancariaBase: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;
                return $"Su Nuevo Saldo es de ${Saldo} pesos m/c";
            }
            if (_movimientos.Any() && !this.Ciudad.Equals(ciudad))
            {
                decimal costoConsignacionNacional = 10000;
                _movimientos.Add(new Movimiento(cuentaBancariaBase: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += (valorConsignacion - costoConsignacionNacional);
                return $"Su Nuevo Saldo es de ${Saldo} pesos m/c";
            }
            throw new NotImplementedException();
        }

        public override string Retirar(decimal valorRetiro, DateTime fecha)
        {
            if(valorRetiro <= 0)
            {
                return "El valor a retirar es incorrecto";
            }

            if (valorRetiro > Saldo)
            {
                return "El valor a retirar no puede ser mayor al saldo";
            }

            int cantidadRetirosMes = _movimientos.Where(i => i.Tipo.Equals("RETIRO") && i.Fecha.Month == fecha.Month).Count() + 1;
            if ((Saldo-valorRetiro)>= 20000 && cantidadRetirosMes <= 3)
            {
                Saldo -= valorRetiro;
                _movimientos.Add(new Movimiento(cuentaBancariaBase: this, fecha: fecha, tipo: "RETIRO", valor: valorRetiro));
                return $"Su Nuevo Saldo es de {Saldo} pesos m/c";
            }

            if ((Saldo - valorRetiro) < 20000)
            {
                return "La cuenta no puede tener saldo menor a 20.000 pesos";
            }

            if ((Saldo-valorRetiro)>=20000 && cantidadRetirosMes >= 4)
            {
                decimal costoRetiro = 5000;
                Saldo = (Saldo - valorRetiro) - costoRetiro;
                _movimientos.Add(new Movimiento(cuentaBancariaBase: this, fecha: fecha, tipo: "RETIRO", valor: valorRetiro));
                return $"El nuevo saldo de la cuenta es de {Saldo} pesos";
            }

            throw new NotImplementedException();
        }
    }

    
}
