using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Model;
using System.Data.SqlClient;
using TitaniumColector.Classes.Exceptions;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlServerCe;
using TitaniumColector.Classes.Procedimentos;
using System.Windows.Forms;

namespace TitaniumColector.Classes.Dao
{
    class DaoEmbalagem
    {
        StringBuilder sql01;
        private EmbalagemProduto embalagem = null;

        public DaoEmbalagem()
        {

        }

        public List<EmbalagemProduto> cargaEmbalagensProduto(int codigoProposta)
        {
            sql01 = new StringBuilder();
            List<EmbalagemProduto> listaEmbalagens = new List<EmbalagemProduto>();

            try
            {
                sql01.Append(" SELECT codigoEMBALAGEMPRODUTO,COALESCE(nomeEMBALAGEMPRODUTO,'ND') AS nomeEMBALAGEMPRODUTO,produtoEMBALAGEMPRODUTO,quantidadeEMBALAGEMPRODUTO,padraoEMBALAGEMPRODUTO,COALESCE(embalagemEMBALAGEMPRODUTO,0) AS embalagemEMBALAGEMPRODUTO,COALESCE(codigobarrasEMBALAGEMPRODUTO,'0000000000000') AS codigobarrasEMBALAGEMPRODUTO ");
                sql01.Append(" FROM tb0504_Embalagens_Produtos");
                sql01.Append(" INNER JOIN tb0501_Produtos ON codigoPRODUTO = produtoEMBALAGEMPRODUTO");
                sql01.Append(" WHERE produtoEMBALAGEMPRODUTO IN(");
                sql01.Append("						                SELECT produtoRESERVA AS codigoPRODUTO");
                sql01.Append("						                FROM tb1206_Reservas (NOLOCK)");
                sql01.Append("						                INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA");
                sql01.Append("						                INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO");
                sql01.AppendFormat("						        WHERE propostaITEMPROPOSTA = {0}", codigoProposta);
                sql01.Append("						                AND tipodocRESERVA = 1602 ");
                sql01.Append("						                AND statusITEMPROPOSTA = 3");
                sql01.Append("						                AND separadoITEMPROPOSTA = 0");
                sql01.Append("						                GROUP BY produtoRESERVA");
                sql01.Append("                                 )");
                sql01.Append(" AND lixeiraPRODUTO = 0");
                sql01.Append(" ORDER BY produtoEMBALAGEMPRODUTO");

                SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

                while ((dr.Read()))
                {
                    {
                        embalagem = new EmbalagemProduto(Convert.ToInt32(dr["codigoEMBALAGEMPRODUTO"]), (string)dr["nomeEMBALAGEMPRODUTO"], (EmbalagemProduto.PadraoEmbalagem)dr["padraoEMBALAGEMPRODUTO"]
                                                         , Convert.ToInt32(dr["produtoEMBALAGEMPRODUTO"]), Convert.ToDouble(dr["quantidadeEMBALAGEMPRODUTO"])
                                                         , Convert.ToInt32(dr["embalagemEMBALAGEMPRODUTO"])
                                                         , (string)dr["codigobarrasEMBALAGEMPRODUTO"]);

                        listaEmbalagens.Add(embalagem);

                    }

                }

                dr.Close();
                SqlServerConn.closeConn();

                if (listaEmbalagens.Count == 0)
                {
                    throw new SqlQueryExceptions("Não foi possível recuperar informações sobre embalagens para esta proposta :  " + codigoProposta);
                }

                embalagem = null;
                return listaEmbalagens;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void insertEmbalagemBaseMobile(List<EmbalagemProduto> listaEmbalagens)
        {
            try
            {
                //Limpa a tabela..
                CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0005_Embalagens");

                foreach (var item in listaEmbalagens)
                {

                    //Query de insert na Base Mobile
                    sql01 = new StringBuilder();
                    sql01.Append("INSERT INTO tb0005_Embalagens");
                    sql01.Append("(codigoEMBALAGEM, nomeEMBALAGEM, produtoEMBALAGEM, quantidadeEMBALAGEM, padraoEMBALAGEM, embalagemEMBALAGEM, ean13EMBALAGEM)");
                    sql01.Append("VALUES (");
                    sql01.AppendFormat("{0},", item.Codigo);
                    sql01.AppendFormat("'{0}',", item.Nome);
                    sql01.AppendFormat("{0},", item.ProdutoEmbalagem);
                    sql01.AppendFormat("{0},", item.Quantidade);
                    sql01.AppendFormat("{0},", (int)item.Padrao);
                    sql01.AppendFormat("{0},", item.TipoEmbalagem);
                    sql01.AppendFormat("'{0}')", item.Ean13Embalagem);

                    CeSqlServerConn.execCommandSqlCe(sql01.ToString());
                }

            }
            catch (SqlCeException sqlEx)
            {
                throw sqlEx;

            }
            catch (Exception Ex)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("Ocorreram problemas durante a carga de dados na tabela tb0002_ItensProposta. \n");
                strBuilder.Append("O procedimento não pode ser concluído \n");
                strBuilder.AppendFormat("Description : {0}", Ex.Message);

                MainConfig.errorMessage(strBuilder.ToString(), "Error in Query!!");
            }

        }

        public List<EmbalagemProduto> carregarEmbalagensProduto(Produto produto)
        {

            EmbalagemProduto objEmbalagem = null;
            List<EmbalagemProduto> listaEmbalagens = new List<EmbalagemProduto>();

            sql01 = new StringBuilder();
            sql01.Append(" SELECT        TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA, COUNT(*) AS TLINHAS");
            sql01.Append(" FROM            tb0002_ItensProposta AS TB_ITEM INNER JOIN");
            sql01.Append(" tb0001_Propostas AS TB_PROP ON TB_ITEM.propostaITEMPROPOSTA = TB_PROP.codigoPROPOSTA INNER JOIN");
            sql01.Append(" tb0005_Embalagens AS TB_EMB ON TB_ITEM.codigoprodutoITEMPROPOSTA = TB_EMB.produtoEMBALAGEM");
            sql01.Append(" GROUP BY TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA");
            sql01.AppendFormat(" HAVING        (TB_EMB.produtoEMBALAGEM = {0})", produto.CodigoProduto);

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            while ((dr.Read()))
            {
                objEmbalagem = new EmbalagemProduto(

                      Convert.ToInt32(dr["codigoEMBALAGEM"])
                      , (string)dr["nomeEMBALAGEM"]
                      , (EmbalagemProduto.PadraoEmbalagem)Convert.ToInt32(dr["padraoEMBALAGEM"])
                      , Convert.ToInt32(dr["produtoEMBALAGEM"])
                      , Convert.ToDouble(dr["quantidadeEMBALAGEM"])
                      , Convert.ToInt32(dr["embalagemEMBALAGEM"])
                      , (string)dr["ean13EMBALAGEM"]);

                listaEmbalagens.Add(objEmbalagem);
            }

            return listaEmbalagens;

        }
        /// <summary>
        /// Retorna informações da base de dados principal sobre as embalagens utilizadas para seeparação de produtos 
        /// </summary>
        /// <returns>Lista de objetos EmbalagemSeparacao</returns>
        public List<EmbalagemSeparacao> carregarEmbalagensSeparacao()
        {

            EmbalagemSeparacao objEmbalagem = null;
            List<EmbalagemSeparacao> listaEmbalagens = new List<EmbalagemSeparacao>();

            sql01 = new StringBuilder();

            sql01.Append(" SELECT codigoEMBALAGEM,nomeEMBALAGEM,produtoEMBALAGEM,pesoEMBALAGEM,padraoEMBALAGEM");
            sql01.Append(" FROM tb0545_Embalagens");
            sql01.Append(" INNER JOIN tb0501_Produtos ON codigoPRODUTO = produtoEMBALAGEM");

            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            while ((dr.Read()))
            {
                objEmbalagem = new EmbalagemSeparacao(
                                                            Convert.ToInt32(dr["codigoEMBALAGEM"])
                                                          , (string)dr["nomeEMBALAGEM"]
                                                          , (Embalagem.PadraoEmbalagem)Convert.ToInt32(dr["padraoEMBALAGEM"])
                                                          , Convert.ToInt32(dr["produtoEMBALAGEM"])
                                                          , Convert.ToDouble(dr["pesoEMBALAGEM"])
                                                      );

                listaEmbalagens.Add(objEmbalagem);
            }

            return listaEmbalagens;

        }

        public List<EmbalagemProduto> carregarEmbalagensProduto(Proposta proposta)
        {
            EmbalagemProduto objEmbalagem = null;
            List<EmbalagemProduto> listaEmbalagens = new List<EmbalagemProduto>();

            sql01 = new StringBuilder();
            sql01.Append(" SELECT        TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA, COUNT(*) AS TLINHAS");
            sql01.Append(" FROM            tb0002_ItensProposta AS TB_ITEM INNER JOIN");
            sql01.Append(" tb0001_Propostas AS TB_PROP ON TB_ITEM.propostaITEMPROPOSTA = TB_PROP.codigoPROPOSTA INNER JOIN");
            sql01.Append(" tb0005_Embalagens AS TB_EMB ON TB_ITEM.codigoprodutoITEMPROPOSTA = TB_EMB.produtoEMBALAGEM");
            sql01.Append(" GROUP BY TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA");
            sql01.AppendFormat(" HAVING        (TB_EMB.produtoEMBALAGEM = {0})", proposta.ListObjItemProposta[0].CodigoProduto);

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            while ((dr.Read()))
            {
                objEmbalagem = new EmbalagemProduto(

                    Convert.ToInt32(dr["codigoEMBALAGEM"])
                    , (string)dr["nomeEMBALAGEM"]
                    , (EmbalagemProduto.PadraoEmbalagem)Convert.ToInt32(dr["padraoEMBALAGEM"])
                    , Convert.ToInt32(dr["produtoEMBALAGEM"])
                    , Convert.ToDouble(dr["quantidadeEMBALAGEM"])
                    , Convert.ToInt32(dr["embalagemEMBALAGEM"])
                    , (string)dr["ean13EMBALAGEM"]);

                listaEmbalagens.Add(objEmbalagem);
            }

            return listaEmbalagens;
        }

        public void salvarEmbalagensSeparacao(Proposta proposta)
        {
            bool isUpdate = false;
            int codigoEmbalagensSeparacao = 0;

            try
            {
                //RECUPERA O CODIGO DOS VOLUMES JÁ SALVOS PARA A TABELA DE EMBALAGENS SEPARARACAO
                int[,] infoEmbalagens = recuperaCodigoEmbalagensSeparacao(proposta.Codigo, proposta.CodigoPikingMobile);

                //VERIFICA TODAS AS EMBALAGENS UTILIZADAS NA SEPARACAO
                foreach (var item in ProcedimentosLiberacao.ListEmbalagensSeparacao.ToList<EmbalagemSeparacao>())
                {
                    if (infoEmbalagens != null)
                    {
                        //LOOP NOS CODIGOS RETORNADOS DA BASE ENGINE
                        for (int i = 0; i < infoEmbalagens.GetLength(0); i++)
                        {
                            isUpdate = false;

                            //SE O CODIGO DA EMBALAGEM SENDO VERIFICADA É IGUAL A UM DOS RETORNADOS NA BASE DE DADOS 
                            //SERÁ FEITO O UPDATE E NÃO O INSERT.
                            if (item.Codigo == infoEmbalagens[i, 1])
                            {
                                //RECUPERA O CODIGO DA LINHA QUE SOFRERÁ UPDATE
                                codigoEmbalagensSeparacao = infoEmbalagens[i, 0];
                                isUpdate = true;
                                break;
                            }

                        }
                    }

                    if (!isUpdate)
                    {
                        insertTb1653EmbalagensSeparacao( proposta.CodigoPikingMobile, item.Codigo, (int)item.Quantidade);
                    }
                    else if (isUpdate)
                    {
                        updateTb1653EmbalagensSeparacao(codigoEmbalagensSeparacao, proposta.CodigoPikingMobile, (int)item.Quantidade);
                    }
                }

            }
            catch (Exception ex)
            {   
                throw new Exception("salvarEmbalagensSeparacao()\n" + ex.Message, ex);
            }
            
        }

        public int[,] recuperaCodigoEmbalagensSeparacao(Int64 codigoProposta,Int64 codigoPikingMobile)
        {
            int tamanho = 0;
            int count = 0;
            int[,] embalagens = null;

            sql01 = new StringBuilder();
            sql01.Append(" SELECT [codigoEMBALAGEMSEPARACOES],[embalagemEMBALAGEMSEPARACOES],COUNT(*) OVER() AS [Total_Rows]");
            sql01.Append(" FROM [dbo].[tb1653_Picking_Embalagem_Separacoes]");
            sql01.AppendFormat(" WHERE pickingmobileEMBALAGEMSEPARACOES = {0}", codigoPikingMobile);

            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            while (dr.Read())
            {
                if (count == 0)
                {
                    tamanho = Convert.ToInt32(dr["Total_Rows"]);
                    embalagens = new int[tamanho, 2];
                }

                embalagens[count, 0] = Convert.ToInt32(dr["codigoEMBALAGEMSEPARACOES"]);
                embalagens[count, 1] = Convert.ToInt32(dr["embalagemEMBALAGEMSEPARACOES"]);

                count++;
            }
            return embalagens;  
        }

        public void insertTb1653EmbalagensSeparacao(int codigoPickingMobile,int codigoEmbalagem,int quantidadeEmbalagem) 
        {
            try
            {
                sql01 = new StringBuilder();

                sql01.Append("INSERT INTO [dbo].[tb1653_Picking_Embalagem_Separacoes]");
                sql01.Append("(pickingmobileEMBALAGEMSEPARACOES");
                sql01.Append(",embalagemEMBALAGEMSEPARACOES");
                sql01.Append(",quantidadeEMBALAGEMSEPARACOES)");
                sql01.AppendFormat("VALUES ( {0},", codigoPickingMobile);
                sql01.AppendFormat("{0},", codigoEmbalagem);
                sql01.AppendFormat("{0})", quantidadeEmbalagem);
                SqlServerConn.execCommandSql(sql01.ToString());
            }
            catch (SqlCeException ex)
            {  
                throw new Exception("insertTb1653EmbalagensSeparacao()\n" + ex.Message,ex);
            }   
        }

        public void updateTb1653EmbalagensSeparacao(int codigoEmbalagensSeparacao,int codigoPickingMobile, int quantidadeEmbalagem)
        {
            try
            {
                sql01 = new StringBuilder();

                sql01.Append(" UPDATE [dbo].[tb1653_Picking_Embalagem_Separacoes]");
                sql01.AppendFormat(" SET [quantidadeEMBALAGEMSEPARACOES] = {0}", quantidadeEmbalagem);
                sql01.AppendFormat(" WHERE [pickingmobileEMBALAGEMSEPARACOES] = {0}", codigoPickingMobile);
                sql01.AppendFormat(" AND [codigoEMBALAGEMSEPARACOES] = {0}", codigoEmbalagensSeparacao);
                SqlServerConn.execCommandSql(sql01.ToString());
     
            }
            catch (Exception ex)
            {
                throw new Exception("updateTb1653EmbalagensSeparacao()\n" + ex.Message, ex);
            }
        }

        public List<EmbalagemSeparacao> carregarInformacoesEmbalagensUtilizadas(int codigoPickingMobile,List<EmbalagemSeparacao> embalagens) 
        {

            sql01 = new StringBuilder();
            int count = 0;
            sql01.Append(" SELECT    pesototalprodutosPICKINGMOBILE");
            sql01.Append(" , pickingmobileEMBALAGEMSEPARACOES");
            sql01.Append(" , embalagemEMBALAGEMSEPARACOES");
            sql01.Append(" , quantidadeEMBALAGEMSEPARACOES");
            sql01.Append(" , SUM (quantidadeEMBALAGEMSEPARACOES) OVER() totalVOLUMES");
            sql01.Append(" FROM tb1651_Picking_Mobile ");
            sql01.Append(" INNER JOIN tb1653_Picking_Embalagem_Separacoes ON codigoPICKINGMOBILE = pickingmobileEMBALAGEMSEPARACOES");
            sql01.Append(" INNER JOIN TB1601_PROPOSTAS ON CODIGOpROPOSTA = propostaPICKINGMOBILE"); 
            sql01.AppendFormat(" WHERE codigoPICKINGMOBILE = {0}",codigoPickingMobile);
            sql01.Append(" AND quantidadeEMBALAGEMSEPARACOES > 0");

            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            while ((dr.Read()))
            {
                if (count == 0)
                {
                    count++;
                    ProcedimentosLiberacao.PesoTotalProdutos = Convert.ToDouble(dr["pesototalprodutosPICKINGMOBILE"]);
                    ProcedimentosLiberacao.TotalVolumes = Convert.ToInt32(dr["totalVOLUMES"]);
                }

                foreach (var item in embalagens)
                {
                    if ( Convert.ToInt16(dr["embalagemEMBALAGEMSEPARACOES"]) == item.Codigo)
                    {
                        item.Quantidade = Convert.ToInt32(dr["quantidadeEMBALAGEMSEPARACOES"]);
                    } 
                }
            }

            return embalagens;

        }
    }
}