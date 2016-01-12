using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlClient;
using TitaniumColector.Classes.Model;
using System.ComponentModel;

namespace TitaniumColector.Classes.Dao
{
    class DaoEtiqueta : IDisposable
    {
        private StringBuilder sql01; 

        public DaoEtiqueta() 
        {
           
        }

        public void insertSequencia(List<EtiquetaVenda> listEtiquetas)
        {
            try
            {
                //Limpa a tabela..
                CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0004_Etiquetas");

                foreach (var item in listEtiquetas)
                {
                    //INSERT BASE MOBILE
                    sql01 = new StringBuilder();
                    sql01.Append(" INSERT INTO tb0004_Etiquetas ");
                    sql01.Append("(eanitempropostaETIQUETA, volumeETIQUETA, quantidadeETIQUETA, sequenciaETIQUETA)");
                    sql01.Append("VALUES (");
                    sql01.AppendFormat("{0},", item.Ean13Etiqueta);
                    sql01.AppendFormat("\'{0}\',", item.VolumeEtiqueta);
                    sql01.AppendFormat("\'{0}\',", item.QuantidadeEtiqueta);
                    sql01.AppendFormat("\'{0}\')", item.SequenciaEtiqueta);
          
                    CeSqlServerConn.execCommandSqlCe(sql01.ToString());
                }

            }
            catch (SqlException sqlEx)
            {
                System.Windows.Forms.MessageBox.Show("Erro durante a carga de dados na base Mobile tb0004_Etiquetas.\n Erro : " + sqlEx.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ações para finalizar o processo de itens já alocados.
        /// </summary>
        /// <param name="listEtiquetas">LIst com Etiquetas jáAlocadas.</param>
        public void finalizarAlocacao(List<EtiquetaAlocacao> listEtiquetas)
        {
            try
            {
                foreach (var item in listEtiquetas)
                {
                    if (item.JaAlocado) 
                    {
                        //Insert Engine
                        sql01 = new StringBuilder();
                        sql01.Append(" INSERT INTO tb1212_Lotes_Locais ");
                        sql01.Append("VALUES (");
                        sql01.AppendFormat("{0},", item.CodigoLote);
                        sql01.AppendFormat("{0})", item.CodigoLocalAlocacao);
                        SqlServerConn.execCommandSql(sql01.ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show("Problemas durante carga na base de dados.\n MÉTODO : finalizarAlocacao\nErro : " + Ex.Message);
            }
        }

        public void gravarXmlItemPedido(int codItemPedido,string strXml)
        {
            try
            {
                //Insert Engine
                sql01 = new StringBuilder();
                sql01.Append(" UPDATE tb1403_Itens_Pedidos");
                sql01.AppendFormat(" SET xmlITEMPC = '{0}'", strXml);
                sql01.Append(" FROM tb1403_Itens_Pedidos");
                sql01.AppendFormat(" WHERE codigoITEMPC  = {0}", codItemPedido);
                SqlServerConn.execCommandSql(sql01.ToString());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Problemas durante carga na base de dados.\n MÉTODO : gravarXmlItemPedido\nErro : " + ex.Message);
            }
        }

        #region IDisposable Members

        // booleano para controlar se
        // o método Dispose já foi chamado
        bool disposed = false;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // método privado para controle
        // da liberação dos recursos
        private void Dispose(bool disposing)
        {
            // Verifique se Dispose já foi chamado.
            if (!this.disposed)
            {
                if (disposing)
                {

                }

                // Seta a variável booleana para true,
                // indicando que os recursos já foram liberados
                disposed = true;
            }
        }

        // C#
        ~DaoEtiqueta()
        {
            Dispose(false);
        }
        #endregion
    }
}
