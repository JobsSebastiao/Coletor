using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using TitaniumColector.Classes.SqlServer;
using System.Data;

namespace TestTitaniumColector
{
    public class ConnectionFactory
    {
        public SqlConnection Conn { get; private set; }
        public DataTable Dt { get; private set; }

        public void starConnection()
        {
            SqlServerConn.configuraStrConnection
            (@"C:\Users\sebastiao.martins\Documents\Visual Studio 2008\Projects\Coletor\z_Auxiliares", "strConn.txt");
            this.Conn = SqlServerConn.openConn();
        }

        public DataTable fillDataTable(string sql01)
        {
            try
            {
                if (this.Conn.Equals(null)) 
                {
                    throw new Exception("Verificar Conexão");
                }

                SqlDataAdapter da = new SqlDataAdapter(sql01,this.Conn);
                da.Fill(this.Dt);

                return Dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorre um problema na conexão com a base de dados." + Environment.NewLine + "Erro : " + ex.Message);
            }
            finally
            {
                CloseConnection ();
            }
        }

        public void CloseConnection() 
        {
            if (this.Conn.State == ConnectionState.Open) 
            {
                this.Conn.Close();
            }
        }
    }
}
