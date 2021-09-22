using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class CDT
    {

        public string Numero { get; private set; }
        public string Nombre { get; private set; }
        public string Termino { get; private set; }
        public double Tasa { get; private set; }
        public decimal Saldo { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        protected List<MovimientoCDT> _movimientos;


        public CDT(string numero, string nombre, string termino, double tasa, DateTime fechaCreacion)
        {
            Numero = numero;
            Nombre = nombre;
            Termino = termino;
            Tasa = tasa;
            FechaCreacion = fechaCreacion;
            _movimientos = new List<MovimientoCDT>();
        }
        public IReadOnlyCollection<MovimientoCDT> Movimientos => _movimientos.AsReadOnly();

        public string Consignar(decimal valorConsignacion, DateTime fecha)
        {
            if (valorConsignacion < 1000000)
            {
                return "No puede consignar un valor menor o igual que 1000000 de pesos";
            }

            if (!_movimientos.Any())
            {
                _movimientos.Add(new MovimientoCDT(cdt: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;
                return $"Su nuevo saldo es de {Saldo} de pesos";
            }

            if (_movimientos.Any())
            {
                return "Solo puede realizar una consignación";
            }

            throw new NotImplementedException();
        }

        public string Retirar(DateTime fecha)
        {
            double meses = ((fecha - FechaCreacion).TotalDays / 30);
            if (Math.Round(meses) == CalcularTermino())
            {
                decimal interes = Convert.ToDecimal(CalcularTasa(this.Termino, this.Tasa)) * Saldo;
                decimal retiro = Saldo += interes;
                Saldo = 0;
                return $"El valor a retirar es { retiro }";
            }

            if (Math.Round(meses) < CalcularTermino())
            {
                return "No se ha cumplido el término definido para efectuar el retiro";
            }

                throw new NotImplementedException();
        }

        internal int CalcularTermino()
        {
            if (this.Termino.Equals("Trimestre"))
            {
                return 3;
            }
            else
            {
                if (this.Termino.Equals("Mes"))
                {
                    return 1;
                }
                else
                {
                    if (this.Termino.Equals("Semestre"))
                    {
                        return 6;
                    }
                    else
                    {
                        return 12;
                    }
                }
            }
        }

        internal double CalcularTasa(string termino, double tasa)
        {
            double te = 0.0;
            if (termino.Equals("Mes"))
            {
                te = Math.Pow(1 + tasa, 0.08333333333333333333333333333333) - 1;
            }
            if (termino.Equals("Trimestre"))
            {
                te = Math.Pow(1 + tasa, 0.25) - 1;
            }
            if (termino.Equals("Semestre"))
            {
                te = Math.Pow(1 + tasa, 0.5) - 1;
            }
            if (termino.Equals("Anio"))
            {
                te = Math.Pow(1 + tasa, 1) - 1;
            }
            return te;
        }


    }

    public class MovimientoCDT
    {
        public MovimientoCDT(CDT cdt, DateTime fecha, string tipo, decimal valor)
        {
            CDT = cdt;
            Fecha = fecha;
            Tipo = tipo;
            Valor = valor;
        }
        public CDT CDT { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tipo { get; private set; }
        public decimal Valor { get; private set; }
    }
}
