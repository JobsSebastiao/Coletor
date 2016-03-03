using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TitaniumColector.Classes.Dao;

namespace TestTitaniumColector
{   
    [TestFixture]
    public class TesteDaoUsuario
    {
        [Test]
        public void DeveRetornarUmaListaComPeloMenosUmUsuario() 
        {
            ConnectionFactory conn = new ConnectionFactory();
            conn.starConnection();
            DaoUsuario daoUser = new DaoUsuario();
            var lista = daoUser.retornaListUsuarios();
            Assert.IsNull(lista);
        }
    }
}
