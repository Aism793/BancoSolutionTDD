using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain.Test.TarjetasCredito
{
    class TarjetaCreditoTest
    {
        /*
        HU 5.
           Como Usuario quiero realizar consignaciones (abonos) a una Tarjeta Crédito para abonar al saldo
           del servicio.
           Criterios de Aceptación
           5.1 El valor a abono no puede ser menor o igual a 0.
           5.2 El abono podrá ser máximo el valor del saldo de la tarjeta de crédito.
           5.3 Al realizar un abono el cupo disponible aumentará con el mismo valor que el valor del abono
           y reducirá de manera equivalente el saldo.  
        */
        /*
        Escenario: Valor de consignación -1
        H5: Como Usuario quiero realizar consignaciones (abonos) a una Tarjeta Crédito para abonar al saldo
           del servicio.
        Criterio de Aceptación:
        5.1 El valor a abono no puede ser menor o igual a 0.
        //El ejemplo o escenario
        Dado El cliente tiene una tarjeta de crédito 
        Número 10001, Nombre “Cuenta ejemplo”, Cupo de 100000 pesos
        Cuando Va a consignar un valor -1
        Entonces El sistema presentará el mensaje. “No puede consignar un valor menor o igual que 0”
         */
        [Test]
        public void NoPuedeHacerConsignacionMenosUno()
        {
            var tarjetaCredito = new TarjetaCredito(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 100000);

            decimal valorConsignacion = -1;
            string respuesta = tarjetaCredito.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(0, tarjetaCredito.Movimientos.Count);
            Assert.AreEqual("No puede consignar un valor menor o igual que 0", respuesta);
        }

        /*
        Escenario: consignación correcta
        H5: Como Usuario quiero realizar consignaciones (abonos) a una Tarjeta Crédito para abonar al saldo
           del servicio.
        Criterio de Aceptación:
        5.2 El abono podrá ser máximo el valor del saldo de la tarjeta de crédito.
        5.3 Al realizar un abono el cupo disponible aumentará con el mismo valor que el valor del abono
           y reducirá de manera equivalente el saldo.  
        //El ejemplo o escenario
        Dado El cliente tiene una tarjeta de crédito  
        Número 10001, Nombre “Cuenta ejemplo”, Cupo de 100000 pesos
        Cuando Va a consignar un valor 50000 
        Entonces El sistema disminuirá el saldo, aumentará el cupo
        and presentará el mensaje. “Su nuevo saldo es de 50000 pesos y su nuevo cupo de 150000 pesos”
         */
        [Test]
        public void PuedeHacerAbonoCorrecto()
        {
            var tarjetaCredito = new TarjetaCredito(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 100000);

            decimal valorConsignacion = 50000;
            string respuesta = tarjetaCredito.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(1, tarjetaCredito.Movimientos.Count);
            Assert.AreEqual("Su nuevo saldo es de 50000 pesos y su nuevo cupo de 150000 pesos", respuesta);
        }

        /*
        Escenario: consignación incorrecta
        H5: Como Usuario quiero realizar consignaciones (abonos) a una Tarjeta Crédito para abonar al saldo
           del servicio.
        Criterio de Aceptación:
        5.2 El abono podrá ser máximo el valor del saldo de la tarjeta de crédito.
        //El ejemplo o escenario
        Dado El cliente tiene una tarjeta de crédito 
        Número 10001, Nombre “Cuenta ejemplo”, Saldo de 100000 pesos
        Cuando Va a consignar un valor 100001 
        Entonces El sistema presentará el mensaje. “No puede abonar más de 100000 pesos”
         */
        [Test]
        public void NoPuedeHacerAbonoCorrecto()
        {
            var tarjetaCredito = new TarjetaCredito(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 100000);

            decimal valorConsignacion = 100001;
            string respuesta = tarjetaCredito.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(0, tarjetaCredito.Movimientos.Count);
            Assert.AreEqual("No puede abonar más de 100000 pesos", respuesta);
        }

        //--------------------------------------------------------------------------------------------

        //Retirar
        /*
            HU 6.
            Como Usuario quiero realizar retiros (avances) a una cuenta de crédito para retirar dinero en forma de avances del servicio de crédito.
            Criterios de Aceptación
            6.1 El valor del avance debe ser mayor a 0.
            6.2 Al realizar un avance se debe reducir el valor disponible del cupo con el valor del avance.
            6.3 Un avance no podrá ser mayor al valor disponible del cupo
        */
        /*
        Escenario: Valor de retiro -1
        H6: Como Usuario quiero realizar retiros (avances) a una cuenta de crédito para retirar dinero en 
        forma de avances del servicio de crédito.
        Criterio de Aceptación:
        5.1 El valor a retirar no puede ser menor o igual a 0.
        //El ejemplo o escenario
        Dado El cliente tiene una tarjeta de crédito  
        Número 10001, Nombre “Cuenta ejemplo”, Cupo de 100000 pesos
        Cuando Va a retirar un valor -1
        Entonces El sistema presentará el mensaje. “No puede retirar un valor menor o igual que 0”
         */
        [Test]
        public void NoPuedeHacerRetiroMenosUno()
        {
            var tarjetaCredito = new TarjetaCredito(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 100000);

            decimal valorRetiro = -1;
            string respuesta = tarjetaCredito.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(0, tarjetaCredito.Movimientos.Count);
            Assert.AreEqual("No puede retirar un valor menor o igual que 0", respuesta);
        }

        /*
        Escenario: retiro correcto
        H6: Como Usuario quiero realizar retiros (avances) a una cuenta de crédito para retirar dinero en 
        forma de avances del servicio de crédito.
        Criterio de Aceptación:
        6.2 Al realizar un avance se debe reducir el valor disponible del cupo con el valor del avance. 
        //El ejemplo o escenario
        Dado El cliente tiene una tarjeta de crédito  
        Número 10001, Nombre “Cuenta ejemplo”, Cupo de 100000 pesos
        Cuando Va a retirar un valor 50000 
        Entonces El sistema disminuirá el valor del cupo
        and presentará el mensaje. “Su nuevo cupo es de 50000 pesos”
         */
        [Test]
        public void PuedeHacerRetiroCorrecto()
        {
            var tarjetaCredito = new TarjetaCredito(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 100000);

            decimal valorRetiro = 50000;
            string respuesta = tarjetaCredito.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(1, tarjetaCredito.Movimientos.Count);
            Assert.AreEqual("Su nuevo cupo es de 50000 pesos", respuesta);
        }

        /*
        Escenario: retiro incorrecto
        H6: Como Usuario quiero realizar retiros (avances) a una cuenta de crédito para retirar dinero en 
        forma de avances del servicio de crédito
        Criterio de Aceptación:
        6.3 Un avance no podrá ser mayor al valor disponible del cupo
        //El ejemplo o escenario
        Dado El cliente tiene una tarjeta de crédito 
        Número 10001, Nombre “Cuenta ejemplo”, Cupo de 100000 pesos
        Cuando Va a retirar un valor 100001 
        Entonces El sistema presentará el mensaje. “No puede retirar más de 100000 pesos”
         */
        [Test]
        public void NoPuedeHacerRetiroCorrecto()
        {
            var tarjetaCredito = new TarjetaCredito(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 100000);

            decimal valorRetiro = 100001;
            string respuesta = tarjetaCredito.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(0, tarjetaCredito.Movimientos.Count);
            Assert.AreEqual("No puede retirar más de 100000 pesos", respuesta);
        }

    }

    
}
