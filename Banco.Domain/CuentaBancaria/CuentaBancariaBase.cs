using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain.CuentaBancaria
{
    public abstract class CuentaBancariaBase
    {

        protected List<MovimientoCuenta> _movimientos;
        public string Numero { get; private set; }
        public string Nombre { get; private set; }
        public string Ciudad { get; private set; }
        public decimal Saldo { get; protected set; }
        protected decimal ValorMinimoConsignacionInicial;

        public CuentaBancariaBase(string numero, string nombre, string ciudad, decimal valorMinimoConsignacionInicial)
        {
            Numero = numero;
            Nombre = nombre;
            Ciudad = ciudad;
            ValorMinimoConsignacionInicial = valorMinimoConsignacionInicial;
            _movimientos = new List<MovimientoCuenta>();
        }

        public IReadOnlyCollection<MovimientoCuenta> Movimientos => _movimientos.AsReadOnly();

        public virtual string Consignar(decimal valorConsignacion, DateTime fecha, string ciudad)
        {
            if (valorConsignacion <= 0 && this.Ciudad.Equals(ciudad))
            {
                return "El valor a consignar es incorrecto";
            }
            if (!_movimientos.Any() && valorConsignacion >= ValorMinimoConsignacionInicial && this.Ciudad.Equals(ciudad))
            {
                _movimientos.Add(new MovimientoCuenta(cuentaBancariaBase: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;

                return $"Su Nuevo Saldo es de {Saldo} pesos m/c";
            }
            if (!_movimientos.Any() && valorConsignacion < ValorMinimoConsignacionInicial && this.Ciudad.Equals(ciudad))
            {
                return "El valor mínimo de la primera consignación debe ser de $50.000 mil pesos. Su nuevo saldo es $0 pesos";
            }
            if (_movimientos.Any() && this.Ciudad.Equals(ciudad))
            {
                _movimientos.Add(new MovimientoCuenta(cuentaBancariaBase: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;
                return $"Su Nuevo Saldo es de ${Saldo} pesos m/c";
            }
            if (_movimientos.Any() && !this.Ciudad.Equals(ciudad))
            {
                decimal costoConsignacionNacional = 10000;
                _movimientos.Add(new MovimientoCuenta(cuentaBancariaBase: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += (valorConsignacion - costoConsignacionNacional);
                return $"Su Nuevo Saldo es de ${Saldo} pesos m/c";
            }
            throw new NotImplementedException();
        }

        public abstract string Retirar(decimal valorRetiro, DateTime fecha);

    }
}
