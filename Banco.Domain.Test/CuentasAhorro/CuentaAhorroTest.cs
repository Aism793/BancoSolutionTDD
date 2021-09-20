using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Banco.Domain.Test.CuentasAhorro
{
    public class CuentaAhorroTest
    {

        /*
        Escenario: Valor de consignación -1
        H1: COMO Cajero del Banco QUIERO realizar consignaciones a una cuenta de ahorro PARA salvaguardar el dinero.
        Criterio de Aceptación:
        1.2 El valor de la consignación no puede ser menor o igual a 0.
        //El ejemplo o escenario
        Dado El cliente tiene una cuenta de ahorro 
        Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
        Cuando Va a consignar un valor -1
        Entonces El sistema presentará el mensaje. “El valor a consignar es incorrecto”
         */
        /*
         ENTITY=> CUENTA AHORRO => AGREGADO ROOT
               => MOVIMIENTOS 
         */
        [Test]
        public void NoPuedeConsignarValorDeMenosUno()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = -1;
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020,2,1), ciudad: "Valledupar");
            Assert.AreEqual("El valor a consignar es incorrecto", respuesta);
        }

        /*
          Escenario: Consignación Inicial Correcta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el 
            dinero.
            Criterio de Aceptación:
           
            1.1 La consignación inicial debe ser mayor o igual a 50 mil pesos
            1.3 El valor de la consignación se le adicionará al valor del saldo aumentará

            Dado El cliente tiene una cuenta de ahorro 
            Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
            Cuando Va a consignar el valor inicial de 50 mil pesos 
            Entonces El sistema registrará la consignación
            AND presentará el mensaje. “Su Nuevo Saldo es de $50.000,00 pesos m/c”.
         */
        [Test]
        public void PuedeHacerConsignacionInicialCorrecta()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = 50000;
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            Assert.AreEqual(1, cuentaAhorro.Movimientos.Count);//Criterio general
            Assert.AreEqual("Su Nuevo Saldo es de 50000 pesos m/c", respuesta);
        }

        /*
            Escenario: Consignación Inicial Incorrecta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
            Criterio de Aceptación:
            1.1 La consignación inicial debe ser mayor o igual a 50 mil pesos
            Dado
            El cliente tiene una cuenta de ahorro con
            Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
            Cuando
            Va a consignar el valor inicial de $49.950 pesos
            Entonces
            El sistema no registrará la consignación
            AND presentará el mensaje. “El valor mínimo de la primera consignación debe ser de $50.000 mil pesos. Su nuevo saldo es $0 pesos”.
         */
        [Test]
        public void NoPuedeHacerConsignacionInicialCorrecta()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = 49950;
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            Assert.AreEqual(0, cuentaAhorro.Movimientos.Count);//Criterio general
            Assert.AreEqual("El valor mínimo de la primera consignación debe ser de $50.000 mil pesos. Su nuevo saldo es $0 pesos", respuesta);
        }

        /*
            Escenario: Consignación posterior a la inicial correcta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
            Criterio de Aceptación:
            1.3 El valor de la consignación se le adicionará al valor del saldo aumentará
            Dado
            El cliente tiene una cuenta de ahorro con un saldo de 30.000
            Cuando
            Va a consignar el valor inicial de $49.950 pesos
            Entonces
            El sistema registrará la consignación
            AND presentará el mensaje. “Su Nuevo Saldo es de $79.950,00 pesos m/c”.
         */
        [Test]
        public void PuedeHacerConsignacionPosteriorALaInicialCorrecta()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            cuentaAhorro.Consignar(valorConsignacion: 50000, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            cuentaAhorro.Retirar(valorRetiro: 20000, fecha: new DateTime(2020, 2, 1));

            decimal valorConsignacion = 49950;  
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");

            Assert.AreEqual(3, cuentaAhorro.Movimientos.Count);//Criterio general
            Assert.AreEqual("Su Nuevo Saldo es de $79950 pesos m/c", respuesta);
        }

        /*
            Escenario: Consignación posterior a la inicial correcta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
            Criterio de Aceptación:
            1.4 La consignación nacional (a una cuenta de otra ciudad) tendrá un costo de $10 mil pesos.
            Dado
            El cliente tiene una cuenta de ahorro con un saldo de 30.000 perteneciente a una sucursal de la ciudad de Bogotá y se realizará una consignación desde 
            una sucursal de la Valledupar.
            Cuando
            Va a consignar el valor inicial de $49.950 pesos.
            Entonces
            El sistema registrará la consignación restando el valor a consignar los 10 mil pesos.
            AND presentará el mensaje. “Su Nuevo Saldo es de $69.950,00 pesos m/c”.
         */
        [Test]
        public void PuedeHacerConsignacionPosteriorALaInicialDesdeOtraSucursalCorrecta()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            cuentaAhorro.Consignar(valorConsignacion: 50000, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            cuentaAhorro.Retirar(valorRetiro: 20000, fecha: new DateTime(2020, 2, 1));

            decimal valorConsignacion = 49950;           
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Bogotá");
            
            Assert.AreEqual(3, cuentaAhorro.Movimientos.Count);//Criterio general
            Assert.AreEqual("Su Nuevo Saldo es de $69950 pesos m/c", respuesta);
        }

        //----------------------------------------------------------------------------------------------------------------------------
        //HU: Como Usuario quiero realizar retiros a una cuenta de ahorro para obtener el dinero en efectivo
        /*
        Escenario: Valor de retiro -1
        H2: Como Usuario quiero realizar retiros a una cuenta de ahorro para obtener el dinero en efectivo
        Criterio de Aceptación:
        1.2 El valor de a retirar no puede ser menor o igual a 0.
        //El ejemplo o escenario
        Dado El cliente tiene una cuenta de ahorro 
        Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
        Cuando Va a retirar un valor -1
        Entonces El sistema presentará el mensaje. “El valor a retirar es incorrecto”
        */
        [Test]
        public void NoPuedeRetirarValorDeMenosUno()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = 50000;
            cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            string respuesta = cuentaAhorro.Retirar(valorRetiro: -1, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("El valor a retirar es incorrecto", respuesta);
        }

        /*
        Escenario: Valor de retiro mayor al saldo
        H2: Como Usuario quiero realizar retiros a una cuenta de ahorro para obtener el dinero en efectivo
        Criterio de Aceptación:
        1.2 El valor de a retirar no puede ser mayor al saldo.
        //El ejemplo o escenario
        Dado El cliente tiene una cuenta de ahorro 
        Número 10001, Nombre “Cuenta ejemplo”, Saldo de 50.000
        Cuando Va a retirar un valor 50.001
        Entonces El sistema presentará el mensaje. “El valor a retirar no puede ser mayor al saldo”
        */
        [Test]
        public void NoPuedeRetirarValorMayorQueElSaldo()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = 50000;
            cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            decimal valorRetiro = 50001;
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("El valor a retirar no puede ser mayor al saldo", respuesta);
        }

        /*
          Escenario: Retiro Inicial Correcto
            HU: H2: Como Usuario quiero realizar retiros a una cuenta de ahorro para obtener el dinero en efectivo
            Criterio de Aceptación:          
            1.1 El saldo mínimo de la cuenta debe ser de 20 mil pesos
            1.3 El valor de retiro se descontará del saldo

            Dado El cliente tiene una cuenta de ahorro 
            Número 10001, Nombre “Cuenta ejemplo”, Saldo de 50.000 pesos
            Cuando Va a retirar el valor inicial de 20 mil pesos 
            Entonces El sistema registrará el retiro
            AND presentará el mensaje. “Su Nuevo Saldo es de 30.000 pesos m/c”.
         */
        [Test]
        public void PuedeHacerRetiroInicialCorrecto()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = 50000;
            cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            decimal valorRetiro = 30000;
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("Su Nuevo Saldo es de 20000 pesos m/c", respuesta);
        }

        /*
          Escenario: Retiro Inicial Incorrecto
            HU: H2: Como Usuario quiero realizar retiros a una cuenta de ahorro para obtener el dinero en efectivo
            Criterio de Aceptación:          
            1.1 El saldo mínimo de la cuenta debe ser de 20 mil pesos
            1.3 El valor de retiro se descontará del saldo

            Dado El cliente tiene una cuenta de ahorro 
            Número 10001, Nombre “Cuenta ejemplo”, Saldo de 50.000 pesos
            Cuando Va a retirar el valor inicial de 30.001 mil pesos 
            Entonces El sistema no registrará el retiro
            AND presentará el mensaje. “La cuenta no puede tener saldo menor a 20.000 pesos”.
         */
        [Test]
        public void NoPuedeHacerRetiroInicialCorrecto()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = 50000;
            cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            decimal valorRetiro = 30001;
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("La cuenta no puede tener saldo menor a 20.000 pesos", respuesta);
        }

        /*
          Escenario: Cuarto retiro correcto
            HU: H2: Como Usuario quiero realizar retiros a una cuenta de ahorro para obtener el dinero en efectivo
            Criterio de Aceptación:          
            1.1 Después del cuarto retiro del mes, se cobra un valor de 5 mil pesos
            1.3 El valor de retiro se descontará del saldo

            Dado El cliente tiene una cuenta de ahorro 
            Número 10001, Nombre “Cuenta ejemplo”, Saldo de 50.000 pesos
            Cuando Va a retirar por cuarta vez el valor de 5 mil pesos 
            Entonces El sistema registrará el retiro descontando 5 mil pesos
            AND presentará el mensaje. “El nuevo saldo de la cuenta es de 25.000 pesos”.
         */
        [Test]
        public void PuedeHacerCuartoRetiroCorrecto()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            decimal valorConsignacion = 50000;
            cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            decimal valorRetiro = 5000;
            cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("El nuevo saldo de la cuenta es de 25000 pesos", respuesta);
        }

    }

}
