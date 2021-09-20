using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 HU 3. 
Como Usuario quiero realizar consignaciones a una cuenta corriente para salvaguardar el dinero.
Criterios de Aceptación
3.1 La consignación inicial debe ser de mínimo 100 mil pesos. 
3.2 El valor consignado debe ser adicionado al saldo de la cuenta
 */

namespace Banco.Domain.Test.CuentasCorriente
{
    class CuentaCorrienteTest
    {
        /// <summary>
        /// CA 3.1 La consignación inicial debe ser de mínimo 100 mil pesos. 
        /// </summary>
        [Test]
        public void NoPuedoConsignarNoventaMilPesosIniciales()
        {
            /*
             * DADO 
             * CUANDO
             * ENTONCES
             */
            var cuentaCorriente = new CuentaCorriente(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar", sobregiro: 1000000);
            decimal valorConsignacion = 90000;
            string respuesta = cuentaCorriente.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "Valledupar");
            Assert.AreEqual(0, cuentaCorriente.Movimientos.Count);//Criterio general
            Assert.AreEqual("El valor a consignar es incorrecto", respuesta);
        }

        /// <summary>
        /// HU 4. Como Usuario quiero realizar retiros a una cuenta corriente para salvaguardar el dinero.
        // Criterios de Aceptación
        // 4.1 El valor a retirar se debe descontar del saldo de la cuenta.
        // 4.2 El saldo mínimo deberá ser mayor o igual al cupo de sobregiro
        // 4.3 El retiro tendrá un costo del 4×Mil
        /// </summary>
        [Test]
        public void PuedoRetirarCienMilPesosIniciales()
        {

            #region DADO EL CLIENTE TIENE UNA CUENTA CORRIENTE CON UN SOBREGIRO PERMITIDO DE 1.000.000
            var cuentaCorriente = new CuentaCorriente(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar", sobregiro: 1000000);
            #endregion
            #region CUANDO retire 100.000 pesos
            decimal valorRetiro = 100000;
            string respuesta = cuentaCorriente.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            #endregion
            #region ENTONCES el sistema descontará el 4xmil de la transacción Y el saldo de la cuenta será de 
            Assert.AreEqual(0, cuentaCorriente.Movimientos.Count);//Criterio general
            Assert.AreEqual(-100400, cuentaCorriente.Saldo);
            Assert.AreEqual("Su Nuevo Saldo es de -100400 pesos m/c", respuesta);
            #endregion

        }
    }
}
