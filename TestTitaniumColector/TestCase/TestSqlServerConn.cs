using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace TestTitaniumColector
{
    [TestFixture]
    public class TestSqlServerConn
    {

        [Test]
        public void VerificaAConexao() 
        {
            SqlServerConn.configuraStrConnection
                (@"C:\Users\sebastiao.martins\Documents\Visual Studio 2008\Projects\Coletor\z_Auxiliares", "strConn.txt");
             
            System.Data.SqlClient.SqlConnection conn =  SqlServerConn.openConn();
            Assert.AreEqual(ConnectionState.Open, conn.State);
            Console.WriteLine("Opened");
            conn.Close();
            Assert.AreEqual(ConnectionState.Closed, conn.State);
            Console.WriteLine("Closed");

        }

        [Test]
        public void DeveEncontrarOArquivoComStrConnection() 
        {
            TitaniumColector.Classes.SqlServer.
                SqlServerConn.configuraStrConnection
                (@"C:\Users\sebastiao.martins\Documents\Visual Studio 2008\Projects\Coletor\z_Auxiliares","strConn.txt");

            Console.WriteLine(TitaniumColector.Classes.SqlServer.SqlServerConn.StringConection.ToString());

            Assert.AreEqual("Password=*******;Persist Security Info=False;User ID=sa;Initial Catalog=*********;Data Source=*******.ddns.com.br"
                , TitaniumColector.Classes.SqlServer.SqlServerConn.StringConection.ToString());

        }
    }
}
