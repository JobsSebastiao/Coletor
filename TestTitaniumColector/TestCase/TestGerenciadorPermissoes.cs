using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TitaniumColector;
using TitaniumColector.Classes.Utility;

namespace TestTitaniumColector
{   
    [TestFixture]
    public class TestGerenciadorPermissoes
    {
        [Test]
        public void DeveValidarListaDeParametros() 
        {
            var gerenciador = new GerenciadorPermissoesParametros();

            Assert.AreEqual("ValidarSequencia",gerenciador.ListCodigoParametro[0]);
            Assert.AreEqual(1,gerenciador.ListCodigoParametro.Count);
        }

        [Test]
        public void DeveValidarListaPermissoes()
        {
            var gerenciador = new GerenciadorPermissoesParametros();

            Assert.AreEqual("Guarda Volumes Mobile", gerenciador.ListMetodosPermissoes[0]);
            Assert.AreEqual("Liberacao Vendas Mobile", gerenciador.ListMetodosPermissoes[1]);
            Assert.AreEqual(2, gerenciador.ListMetodosPermissoes.Count);
        }
    }
}
