using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class TarjetaCredito
    {
        public string Numero { get; private set; }
        public string Nombre { get; private set; }
        public decimal Cupo { get; private set; }
        public decimal Saldo { get; protected set; }
        protected List<MovimientoTarjeta> _movimientos;

        public TarjetaCredito(string numero, string nombre, decimal cupo)
        {
            Numero = numero;
            Nombre = nombre;
            Cupo = cupo;
            Saldo = Cupo;
            _movimientos = new List<MovimientoTarjeta>();
        }
        public IReadOnlyCollection<MovimientoTarjeta> Movimientos => _movimientos.AsReadOnly();

        public string Consignar(decimal valorConsignacion, DateTime fecha)
        {
            if (valorConsignacion <= 0)
            {
                return "No puede consignar un valor menor o igual que 0";
            }

            if (valorConsignacion <= Saldo)
            {
                Saldo -= valorConsignacion;
                Cupo += valorConsignacion;
                _movimientos.Add(new MovimientoTarjeta(tarjetaCredito: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                return $"Su nuevo saldo es de {Saldo} pesos y su nuevo cupo de {Cupo} pesos";
            }

            if (valorConsignacion > Saldo)
            {
                return $"No puede abonar más de {Saldo} pesos";
            }

            throw new NotImplementedException();
        }

        public string Retirar(decimal valorRetiro, DateTime fecha)
        {
            if (valorRetiro <= 0)
            {
                return "No puede retirar un valor menor o igual que 0";
            }

            if (valorRetiro <= Cupo)
            {
                Cupo -= valorRetiro;
                _movimientos.Add(new MovimientoTarjeta(tarjetaCredito: this, fecha: fecha, tipo: "RETIRO", valor: valorRetiro));
                return $"Su nuevo cupo es de {Cupo} pesos";
            }

            if (valorRetiro > Cupo)
            {
                return $"No puede retirar más de {Cupo} pesos";
            }

            throw new NotImplementedException();
        }
    }

    public class MovimientoTarjeta
    {
        public TarjetaCredito TarjetaCredito { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tipo { get; private set; }
        public decimal Valor { get; private set; }
        public MovimientoTarjeta(TarjetaCredito tarjetaCredito, DateTime fecha, string tipo, decimal valor)
        {
            TarjetaCredito = tarjetaCredito;
            Fecha = fecha;
            Tipo = tipo;
            Valor = valor;
        }

    }
}
