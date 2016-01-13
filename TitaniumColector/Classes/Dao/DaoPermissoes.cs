using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Utility;
using System.Data;
using System.Data.SqlClient;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.Exceptions;
using TitaniumColector.Classes.Model;

namespace TitaniumColector.Classes.Dao
{
    class DaoPermissoes
    {
        private StringBuilder sql01;
        //private DataTable dt;

        /// <summary>
        /// Recupera informações sobre as permissões existente no Titanium
        /// </summary>
        /// <param name="codigoPermissoes"> Lista de permissões a serem salvas na base mobile.</param>
        /// <returns>List com informações sobre os parâmetros das permissões a serem utilizadas no mobile. </returns>
        public GerenciadorPermissoesParametros recuperarParametros(List<String> codigoPermissoes)
        {
            try
            {
                var gerenciadorPermissoes = new GerenciadorPermissoesParametros();
                Parametro param;

                sql01 = new StringBuilder();
                sql01.Append(" SELECT codigoPARAMETRO,descricaoPARAMETRO,valorPARAMETRO,COALESCE(auxiliarPARAMETRO,0) as auxiliarPARAMETRO FROM tb1210_Parametros");
                SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

                while ((dr.Read()))
                {
                    //Valida as permissões contida na lista de permissões a serem utilizadas no mobile.
                    foreach (string item in codigoPermissoes)
                    {
                        if (item == (string)dr["codigoParametro"])
                        {
                            param = new Parametro((string)dr["codigoParametro"], (string)dr["descricaoPARAMETRO"], (string)dr["valorPARAMETRO"], Convert.ToInt32(dr["auxiliarPARAMETRO"]));
                            gerenciadorPermissoes.addParametro(param);
                        }
                    }
                }

                return gerenciadorPermissoes;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar parâmetros na base de dados.\nMotivo:" + ex.Message , ex);
            }
        }

        public IList<Permissao> listaPermissoes(int codigoUsuario,string metodosMetodos) 
        {

            IList<Permissao> list = new List<Permissao>();

            sql01 = new StringBuilder();
            sql01.Append(" SELECT tb0205_Metodos.valorUSUARIOMETODO, metodoMETODO");
            sql01.Append(" FROM tb0205_Metodos, tb0034_Metodos, tb0031_Componentes, tb0201_Usuarios");
            sql01.Append(" WHERE codigoMETODO = metodoUSUARIOMETODO");
            sql01.Append(" AND codigoUSUARIO = usuarioUSUARIOMETODO");
            sql01.Append(" AND codigoCOMPONENTE = 5");
            sql01.AppendFormat(" AND usuarioUSUARIOMETODO = {0}",codigoUsuario);
            sql01.AppendFormat(" AND metodoMETODO IN ({0})", metodosMetodos);
           
            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            while (dr.Read()) 
            {
                var permissao = new Permissao(Convert.ToInt16(dr["valorUSUARIOMETODO"]), (string)dr["metodoMETODO"]);
                list.Add(permissao);
            }

            return list;
        }
    }
}
