using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Interface;
using TitaniumColector.Forms;
using TitaniumColector.Classes.Utility;
using TitaniumColector.Classes.Dao;
using System.Xml;

namespace TitaniumColector.Classes.Model
{
    public class EtiquetaVenda : Etiqueta
    {
        DaoProduto daoProduto;

        //private Parametros paramValidarSequencia;
        public Int64 Ean13Etiqueta { get; set; }
        //public Int32 SequenciaEtiqueta { get; set; }
        //public Double QuantidadeEtiqueta { get; set; }
        //public DateTime DataHoraValidacao { get; set; }
        //public Int32 volumeEtiqueta;

        //public Int32 VolumeEtiqueta
        //{
        //    get { return volumeEtiqueta; }
        //    set { 
        //            if(value > 0)
        //                volumeEtiqueta = value; 
        //        }
        //}

        public EtiquetaVenda() { }

        public EtiquetaVenda(
                  int ean13Etiqueta
                , string partnumber
                , string descricao
                , string identificacaoLote
                , int sequencia
                , double qtdEmbalagem
                , Etiqueta.Tipo tipoEtiqueta
            )
            : base(partnumber , descricao , identificacaoLote,sequencia, qtdEmbalagem,tipoEtiqueta)
        {
            Ean13Etiqueta = ean13Etiqueta;
        }

        public EtiquetaVenda(Array arrayEtiqueta,Tipo tipoEtiqueta) 
        {
            try
            {
                switch (tipoEtiqueta)
                {
                    case Tipo.QRCODE:

                        foreach (string item in arrayEtiqueta)
                        {
                            string strItem = item.Substring(0, item.IndexOf(":", 0));

                            if (strItem == "PNUMBER")
                            {
                                base.PartnumberEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "DESCRICAO")
                            {
                                DescricaoProdutoEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "EAN13")
                            {
                                Ean13Etiqueta = Convert.ToInt64(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "LOTE")
                            {
                                LoteEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "SEQ")
                            {
                                SequenciaEtiqueta = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "QTD")
                            {
                                QuantidadeEtiqueta = Convert.ToDouble(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                        }

                        break;

                    case Tipo.BARRAS:

                        foreach (string item in arrayEtiqueta)
                        {
                            daoProduto = new DaoProduto();

                            this.TipoEtiqueta = Tipo.BARRAS;
                            Ean13Etiqueta = Convert.ToInt64(item);
                            EtiquetaVenda aux = (EtiquetaVenda)daoProduto.recuperarInformacoesPorEan13Etiqueta(this);

                            if(aux !=null)
                            {
                                DescricaoProdutoEtiqueta = aux.DescricaoProdutoEtiqueta;
                                PartnumberEtiqueta = aux.PartnumberEtiqueta;
                                Ean13Etiqueta = aux.Ean13Etiqueta;
                                LoteEtiqueta = aux.LoteEtiqueta;
                                SequenciaEtiqueta = aux.SequenciaEtiqueta;
                                QuantidadeEtiqueta = aux.QuantidadeEtiqueta;
                            }
                        }

                        break;

                    default:
                        MainConfig.errorMessage("Tipo de Etiqueta indefinido!!", "Leitura Etiquetas");
                        break;
                }

                DataHoraValidacao = DateTime.Now;
                this.TipoEtiqueta = tipoEtiqueta;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        public override bool Equals(object obj)
        {
            if(base.Equals(obj))
            {
                switch (((EtiquetaVenda)obj).TipoEtiqueta)
                {
                    case Tipo.QRCODE:

                        return (Ean13Etiqueta == ((EtiquetaVenda)obj).Ean13Etiqueta && SequenciaEtiqueta == ((Etiqueta)obj).SequenciaEtiqueta);

                    case Tipo.BARRAS:

                        return (Ean13Etiqueta == ((EtiquetaVenda)obj).Ean13Etiqueta);

                    default:
                        return false;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Valida as informações passadas na string capturada pelo coletor de dados.
        /// </summary>
        /// <param name="inputValue">string montada durante a leitura do coletor</param>
        /// <returns>Tipo de etiqueta</returns>
        public override Etiqueta.Tipo validaInputValueEtiqueta(string inputValue)
        {
            Etiqueta.Tipo tipoEtiqueta = Etiqueta.Tipo.INVALID;

            int inputLength = inputValue.Length;

            if (inputLength == 13)
            {
                return  Etiqueta.Tipo.BARRAS;
            }

            if (inputLength > 13 && (tipoEtiqueta == Etiqueta.Tipo.INVALID))
            {
                if (inputValue.Contains("PNUMBER:"))
                {
                    if (inputValue.Contains("DESCRICAO:"))
                    {
                        if (inputValue.Contains("EAN13:"))
                        {
                            if (inputValue.Contains("LOTE:"))
                            {
                                if (inputValue.Contains("SEQ:"))
                                {
                                    if (inputValue.Contains("QTD:"))
                                    {
                                        tipoEtiqueta = Etiqueta.Tipo.QRCODE;
                                    }
                                    else
                                    {
                                        tipoEtiqueta = Etiqueta.Tipo.INVALID;
                                    }
                                }
                                else
                                {
                                    tipoEtiqueta = Etiqueta.Tipo.INVALID;
                                }
                            }
                            else
                            {
                                tipoEtiqueta = Etiqueta.Tipo.INVALID;
                            }
                        }
                        else
                        {
                            tipoEtiqueta = Etiqueta.Tipo.INVALID;
                        }
                    }
                    else
                    {
                        tipoEtiqueta = Etiqueta.Tipo.INVALID;
                    }
                }
                else
                {
                    tipoEtiqueta = Etiqueta.Tipo.INVALID;
                }
            }

            return tipoEtiqueta;
        }

        #region "ainda sem uso "

        //public override void realizaAcao(string inputText,Etiqueta.Tipo tipoEtiqueta)
        //{
        //    switch (tipoEtiqueta)
        //    {
        //        case Etiqueta.Tipo.INVALID:

        //            inputText = string.Empty;
        //             FrmProposta.mostrarMensagem(TitaniumColector.Forms.FrmProposta.enumCor.RED, " Tipo de Etiqueta inválida!!!", TitaniumColector.Forms.FrmProposta.enumCursor.DEFAULT);
        //            break;

        //        case Etiqueta.Tipo.QRCODE:

        //            liberarItem(inputText, tipoEtiqueta);
        //            inputText = string.Empty;
        //            break;

        //        case Etiqueta.Tipo.BARRAS:

        //            paramValidarSequencia = MainConfig.Permissoes_TB1210.retornarParametro("ValidarSequencia");

        //            if (paramValidarSequencia.Valor == "1")
        //            {
        //                this.liberarItem(inputText, tipoEtiqueta);
        //                inputText = string.Empty;
        //                break;
        //            }
        //            else
        //            {
        //                inputText = string.Empty;
        //                FrmProposta.mostrarMensagem(TitaniumColector.Forms.FrmProposta.enumCor.RED, "As configurações atuais não permitem validar etiquetas do tipo Ean13!", TitaniumColector.Forms.FrmProposta.enumCursor.DEFAULT);
        //                break;
        //            }

        //        default:

        //            inputText = string.Empty;
        //            FrmProposta.mostrarMensagem(TitaniumColector.Forms.FrmProposta.enumCor.RED, " Tipo de Etiqueta inválida!!!", TitaniumColector.Forms.FrmProposta.enumCursor.DEFAULT);
        //            break;
        //    }
        //}

        /////<summary>
        ///// Recebe um array de strings referentes aos atributos do obj Etiqueta.
        ///// retorna Um objeto do tipo Etiqueta
        ///// </summary>
        ///// <param name="array">Array de String referentes aos atributos de uma etiqueta</param>
        //public static Etiqueta arrayToEtiqueta(Array array)
        //{
        //    Etiqueta objEtiqueta = new Etiqueta();

        //    foreach (string item in array)
        //    {
        //        string strItem = item.Substring(0, item.IndexOf(":", 0));

        //        if (strItem == "PNUMBER")
        //        {
        //            objEtiqueta.PartnumberEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
        //        }
        //        else if (strItem == "DESCRICAO")
        //        {
        //            objEtiqueta.DescricaoProdutoEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
        //        }
        //        else if (strItem == "EAN13")
        //        {
        //            objEtiqueta.Ean13Etiqueta = Convert.ToInt64(item.Substring(item.IndexOf(":", 0) + 1));
        //        }
        //        else if (strItem == "LOTE")
        //        {
        //            objEtiqueta.LoteEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
        //        }
        //        else if (strItem == "SEQ")
        //        {
        //            objEtiqueta.SequenciaEtiqueta = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
        //        }
        //        else if (strItem == "QTD")
        //        {
        //            objEtiqueta.QuantidadeEtiqueta = Convert.ToDouble(item.Substring(item.IndexOf(":", 0) + 1));
        //        }
        //    }
        //    return objEtiqueta;
        //}

        ///// <summary>
        ///// Verifica se a Etiqueta já foi lida.
        ///// </summary>
        ///// <returns>
        /////          FALSE --> se a etiqueta for encontrada na list
        /////          TRUE --> se a etiqueta ainda não foii lida.
        ///// </returns>
        //public static bool validaEtiquetaNaoLida(Etiqueta objEtiqueta, List<Etiqueta> listEtiquetas)
        //{
        //    //Verifica se o List foi iniciado
        //    if (listEtiquetas != null)
        //    {
        //        if (listEtiquetas.Count == 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            //Verifica se a etiqueta está na lista de etiquetas lidas.
        //            if (validarEtiqueta(objEtiqueta, listEtiquetas))
        //            {
        //                //Caso esteja na lista
        //                return false;
        //            }
        //            else
        //            {
        //                //caso não esteja na lista.
        //                return true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //public static bool validarEtiqueta(Etiqueta etiqueta, List<Etiqueta> listEtiquetas)
        //{
        //    switch (etiqueta.TipoEtiqueta)
        //    {
        //        case Tipo.QRCODE:

        //            foreach (Etiqueta itemList in listEtiquetas.ToList<Etiqueta>())
        //            {

        //                if (itemList.Equals(etiqueta))
        //                {
        //                    return true;
        //                }
        //            }
        //            return false;

        //        case Tipo.BARRAS:

        //            foreach (Etiqueta itemList in listEtiquetas.ToList<Etiqueta>())
        //            {
        //                if (etiqueta.Equals(itemList))
        //                {
        //                    return true;
        //                }
        //            }
        //            return false;

        //        default:
        //            return false;

        //    }

        //}

        //public static String gerarXmlItensEtiquetas(List<Etiqueta> listaEtiquetas)
        //{
        //    String result = "";
        //    try
        //    {
        //        System.IO.StringWriter str = new System.IO.StringWriter();

        //        Variável que irá receber o Xml na forma de String.
        //        System.IO.XmlTextWriter writer = new XmlTextWriter(str);

        //        inicia o documento xml
        //        writer.WriteStartDocument();

        //        define a indentação do arquivo
        //        writer.Formatting = Formatting.Indented;

        //        escreve o elemento raiz
        //        writer.WriteStartElement("Item");
        //        escrever o atributo para o Elemento Raiz Item
        //        writer.WriteAttributeString("Ean", listaEtiquetas[0].Ean13Etiqueta.ToString());

        //        foreach (var item in listaEtiquetas)
        //        {
        //            Elemento Raiz Seq
        //            writer.WriteStartElement("Seq");
        //            Escreve atributos IdEtiqueta
        //            writer.WriteAttributeString("ID", item.SequenciaEtiqueta.ToString());
        //            Escreve atributos TIPO Etiqueta
        //            writer.WriteAttributeString("TIPO", item.TipoEtiqueta.ToString());
        //            Escreve elemento entre a tag Seq
        //            writer.WriteElementString("Qtd", item.QuantidadeEtiqueta.ToString());
        //            writer.WriteElementString("Vol", item.VolumeEtiqueta.ToString());
        //            writer.WriteElementString("Time", item.DataHoraValidacao.ToString());
        //            writer.WriteElementString("Usuario", MainConfig.CodigoUsuarioLogado.ToString());
        //            Encerra o elemento Seq
        //            writer.WriteEndElement();
        //        }

        //        Encerra o elemento Item
        //        writer.WriteEndDocument();

        //         O resultado é uma string.
        //        return result = str.ToString();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion 

      
    }  
} 
