using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain.Test.CDTs
{
    class CDTTest
    {
        /*
        HU 7.
        Como Usuario quiero realizar consignar mi dinero a mi CDT para ahorrar el dinero sin tener acceso 
        al de acuerdo al término definido.
        Criterios de Aceptación
        7.1 El valor de consignación inicial debe ser de mínimo 1 millón de pesos.
        7.2 Sólo se podrá realizar una sola consignación.
        */
        /*
        Escenario: Valor de consignación 999999
        H7: Como Usuario quiero realizar consignar mi dinero a mi CDT para ahorrar el dinero sin tener acceso 
        al de acuerdo al término definido.
        Criterio de Aceptación:
        7.1 El valor de consignación inicial debe ser de mínimo 1 millón de pesos.
        //El ejemplo o escenario
        Dado El cliente tiene un CDT
        Número 10001, Nombre “Cuenta ejemplo”, Saldo 0
        Cuando Va a consignar un valor 999999
        Entonces El sistema presentará el mensaje. “No puede consignar un valor menor o igual que 1000000 de pesos”
         */
        [Test]
        public void NoPuedeConsignarValorDeMenosUnMillon()
        {
            var cdt = new CDT(numero: "10001", nombre: "Cuenta Ejemplo", termino: "Semestre", tasa: 0.06, fechaCreacion: new(2020, 2, 1));
            decimal valorConsignacion = 999999;
            string respuesta = cdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("No puede consignar un valor menor o igual que 1000000 de pesos", respuesta);
        }

        /*
        Escenario: Consignación correcta
        H7: Como Usuario quiero realizar consignar mi dinero a mi CDT para ahorrar el dinero sin tener acceso 
        al de acuerdo al término definido.
        Criterio de Aceptación:
        7.1 El valor de consignación inicial debe ser de mínimo 1 millón de pesos.
        //El ejemplo o escenario
        Dado El cliente tiene un CDT
        Número 10001, Nombre “Cuenta ejemplo”, Saldo 0
        Cuando Va a consignar un valor 1000000
        Entonces El sistema registra la consignación 
        and presentará el mensaje. “Su nuevo saldo es de 1000000 de pesos”
         */
        [Test]
        public void PuedeHacerConsignaciónCorrecta()
        {
            var cdt = new CDT(numero: "10001", nombre: "Cuenta Ejemplo", termino: "Semestre", tasa: 0.06, fechaCreacion: new(2020, 2, 1));
            decimal valorConsignacion = 1000000;
            string respuesta = cdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("Su nuevo saldo es de 1000000 de pesos", respuesta);
        }

        /*
        Escenario: Segunda Consignación incorrecta
        H7: Como Usuario quiero realizar consignar mi dinero a mi CDT para ahorrar el dinero sin tener acceso 
        al de acuerdo al término definido.
        Criterio de Aceptación:
        7.2 Sólo se podrá realizar una sola consignación.
        //El ejemplo o escenario
        Dado El cliente tiene un CDT
        Número 10001, Nombre “Cuenta ejemplo”, Saldo 0
        Cuando Va a consignar un valor 1000000 por segunda vez
        Entonces El sistema presentará el mensaje. “Solo puede realizar una consignación”
         */
        [Test]
        public void NoPuedeHacerSegundaConsignaciónCorrecta()
        {
            var cdt = new CDT(numero: "10001", nombre: "Cuenta Ejemplo", termino: "Semestre", tasa: 0.06, fechaCreacion: new (2020, 2, 1));
            decimal valorConsignacion = 1000000;
            cdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));
            string respuesta = cdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("Solo puede realizar una consignación", respuesta);
        }

        //----------------------------------------------------------------------------------
        //Retirar
        /*
        HU 8.
        Como Usuario quiero el retiro de mi dinero de mi CDT al finalizar el Término establecido para 
        recuperar el dinero depositado.
        Criterios de Aceptación
        8.1 Los retiros sólo se podrán realizar una vez haya finalizado el término del CDT.
        8.2 Al realizar el retiro se le liquidará un interés de acuerdo a la tasa definida y plazo de 
        termino.
        8.3 El valor a retirar se reduce del saldo del CDT.
        */
        /*
        Escenario: Retiro correcto
        HU 8.
        Como Usuario quiero el retiro de mi dinero de mi CDT al finalizar el Término establecido para 
        recuperar el dinero depositado.
        Criterio de Aceptación:
        8.1 Los retiros sólo se podrán realizar una vez haya finalizado el término del CDT.
        //El ejemplo o escenario
        Dado El cliente tiene un CDT
        Número 10001, Nombre “Cuenta ejemplo”, Saldo 1000000 al 6%EA
        Cuando Va a retirar su dinero ha pasado el término definido
        Entonces El sistema presentará el mensaje. “Su saldo actual para retirar es ”
         */
        [Test]
        public void PuedeRetirarValorCDTCorrecto()
        {
            var cdt = new CDT(numero: "10001", nombre: "Cuenta Ejemplo", termino: "Semestre", tasa: 0.06, fechaCreacion: new(2020, 2, 1));
            decimal valorConsignacion = 1000000;
            cdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));
            string respuesta = cdt.Retirar(fecha: new DateTime(2020, 8, 1));
            Assert.AreEqual($"El interés ganado luego del término definido es {Convert.ToDecimal((Math.Pow((1 + 0.06), 0.5)) - 1)* valorConsignacion}", respuesta);
        }

    }

    internal class CDT
    {
        
        public string Numero { get; private set; }
        public string Nombre { get; private set; }
        public string Termino { get; private set; }
        public double Tasa { get; private set; }
        public decimal Saldo { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        protected List<Movimiento> _movimientos;


        public CDT(string numero, string nombre, string termino, double tasa, DateTime fechaCreacion)
        {
            Numero = numero;
            Nombre = nombre;
            Termino = termino;
            Tasa = tasa;
            FechaCreacion = fechaCreacion;
            _movimientos = new List<Movimiento>();
        }
        public IReadOnlyCollection<Movimiento> Movimientos => _movimientos.AsReadOnly();

        internal string Consignar(decimal valorConsignacion, DateTime fecha)
        {
            if (valorConsignacion < 1000000)
            {
                return "No puede consignar un valor menor o igual que 1000000 de pesos";
            }

            if (!_movimientos.Any())
            {
                _movimientos.Add(new Movimiento(cdt: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;
                return $"Su nuevo saldo es de {Saldo} de pesos";
            }

            if (_movimientos.Any())
            {
                return "Solo puede realizar una consignación";
            }

            throw new NotImplementedException();
        }

        internal string Retirar( DateTime fecha)
        {
            double meses = ((fecha - FechaCreacion).TotalDays / 30);
            if (Math.Round(meses) == CalcularTermino())
            {
                decimal interes = Convert.ToDecimal(CalcularTasa(this.Termino, this.Tasa)) * Saldo;
                //Saldo 
                return $"El interés ganado luego del término definido es { interes }";
            }
            
            throw new NotImplementedException();
        }

        internal int CalcularTermino()
        {
            if (this.Termino.Equals("Trimestre"))
            {
                return 3;
            }else
            {
                if (this.Termino.Equals("Mes"))
                {
                    return 1;
                }else
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
                te = (Math.Pow((1 + tasa), 0.0833333333333333)) - 1;
            }
            if (termino.Equals("Trimestre"))
            {
                te = (Math.Pow((1 + tasa), 0.25)) - 1;
            }
            if (termino.Equals("Semestre"))
            {
                te = (Math.Pow((1 + tasa), 0.5)) - 1;
            }
            if (termino.Equals("Anio"))
            {
                te = (Math.Pow((1 + tasa), 1)) - 1;
            }
            return te;
        }


    }

    internal class Movimiento
    {
        public Movimiento(CDT cdt, DateTime fecha, string tipo, decimal valor)
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
