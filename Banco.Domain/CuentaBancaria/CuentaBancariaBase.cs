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

        public CuentaBancariaBase(string numero, string nombre, string ciudad)
        {
            Numero = numero;
            Nombre = nombre;
            Ciudad = ciudad;
            _movimientos = new List<MovimientoCuenta>();
        }

        public IReadOnlyCollection<MovimientoCuenta> Movimientos => _movimientos.AsReadOnly();

        public virtual string Consignar(decimal valorConsignacion, DateTime fecha, string ciudad)
        {
            if (!_movimientos.Any() && valorConsignacion < 100000)
            {
                return "El valor a consignar es incorrecto";
            }
            throw new NotImplementedException();
        }

        public abstract string Retirar(decimal valorRetiro, DateTime fecha);

    }
}
