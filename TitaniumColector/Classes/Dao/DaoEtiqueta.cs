﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlClient;
using TitaniumColector.Classes.Model;

namespace TitaniumColector.Classes.Dao
{
    class DaoEtiqueta
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

        public void finalizarAlocacao(List<EtiquetaAlocacao> listEtiquetas,List<string> xmlItens)
        {
            try
            {
                foreach (var item in listEtiquetas)
                {
                    //Insert Engine
                    sql01 = new StringBuilder();
                    sql01.Append(" INSERT INTO tb1212_ ");
                    sql01.Append("(eanitempropostaETIQUETA, volumeETIQUETA, quantidadeETIQUETA, sequenciaETIQUETA)");
                    sql01.Append("VALUES (");
                    sql01.AppendFormat("{0},", item.Ean13Etiqueta);
                    sql01.AppendFormat("\'{0}\',", item.VolumeEtiqueta);
                    sql01.AppendFormat("\'{0}\',", item.QuantidadeEtiqueta);
                    sql01.AppendFormat("\'{0}\')", item.SequenciaEtiqueta);

                    //CeSqlServerConn.execCommandSqlCe(sql01.ToString());
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

    }
}
