using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Interface;
using TitaniumColector.Forms;
using TitaniumColector.Classes.Utility;
using TitaniumColector.Classes.Dao;
using System.Xml;
using System.IO;
using TitaniumColector.Classes.Exceptions;

namespace TitaniumColector.Classes.Model
{
    public class EtiquetaVenda : Etiqueta
    {
        DaoProduto daoProduto;

     #region "proxima alteração- trazer parametros para esta classe"
        //public Int32 SequenciaEtiqueta { get; set; }
        //public Double QuantidadeEtiqueta { get; set; }
        //public Int32 volumeEtiqueta;

        //public Int32 VolumeEtiqueta
        //{
        //    get { return volumeEtiqueta; }
        //    set { 
        //            if(value > 0)
        //                volumeEtiqueta = value; 
        //        }
        //}

    #endregion

        public Int64 Ean13Etiqueta { get; set; }

        public EtiquetaVenda() { }

        public EtiquetaVenda(int ean13Etiqueta, string partnumber, string descricao, string identificacaoLote, int sequencia, double qtdEmbalagem, Etiqueta.TipoCode tipoEtiqueta)
            : base(partnumber , descricao , identificacaoLote,sequencia, qtdEmbalagem,tipoEtiqueta)
        {
            Ean13Etiqueta = ean13Etiqueta;
        }

        public EtiquetaVenda(Array arrayEtiqueta,TipoCode tipoEtiqueta) 
        {
            try
            {
                switch (tipoEtiqueta)
                {
                    case TipoCode.QRCODE:

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
                            else if (strItem == "CODIGOLOTE")
                            {
                                CodigoLote = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
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

                    case TipoCode.BARRAS:

                        foreach (string item in arrayEtiqueta)
                        {
                            daoProduto = new DaoProduto();

                            this.TipoEtiqueta = TipoCode.BARRAS;
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
                throw new Exception("Problemas durante a validação da etiqueta!\n" + ex);
            }

        }

        /// <summary>
        /// Valida as informações passadas na string capturada pelo coletor de dados.
        /// </summary>
        /// <param name="inputValue">string montada durante a leitura do coletor</param>
        /// <returns>Tipo de etiqueta</returns>
        public override Etiqueta.TipoCode validaInputValueEtiqueta(string inputValue)
        {
            Etiqueta.TipoCode tipoEtiqueta = Etiqueta.TipoCode.INVALID;

            int inputLength = inputValue.Length;

            if (inputLength == 13)
            {
                return  Etiqueta.TipoCode.BARRAS;
            }

            if (inputLength > 13 && (tipoEtiqueta == Etiqueta.TipoCode.INVALID))
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
                                        tipoEtiqueta = Etiqueta.TipoCode.QRCODE;
                                    }
                                    else
                                    {
                                        tipoEtiqueta = Etiqueta.TipoCode.INVALID;
                                    }
                                }
                                else
                                {
                                    tipoEtiqueta = Etiqueta.TipoCode.INVALID;
                                }
                            }
                            else
                            {
                                tipoEtiqueta = Etiqueta.TipoCode.INVALID;
                            }
                        }
                        else
                        {
                            tipoEtiqueta = Etiqueta.TipoCode.INVALID;
                        }
                    }
                    else
                    {
                        tipoEtiqueta = Etiqueta.TipoCode.INVALID;
                    }
                }
                else
                {
                    tipoEtiqueta = Etiqueta.TipoCode.INVALID;
                }
            }
            return tipoEtiqueta;
        }

        public override Etiqueta criarEtiqueta(Array arrayEtiqueta, Etiqueta.TipoCode tipoEtiqueta)
        {
            return new EtiquetaVenda(arrayEtiqueta, tipoEtiqueta);
        }

        public override string montarXml()
        {
            string result = "";

            try
            {
                System.IO.StringWriter str = new System.IO.StringWriter();

                //Variável que irá receber o Xml na forma de String.
                XmlTextWriter writer = new XmlTextWriter(str);

                //inicia o documento xml
                writer.WriteStartDocument();

                //define a indentação do arquivo
                writer.Formatting = Formatting.Indented;

                //escreve o elemento raiz
                writer.WriteStartElement("Item");
                //escrever o atributo para o Elemento Raiz Item

                //writer.WriteAttributeString("Ean", listaEtiquetas[0].Ean13Etiqueta.ToString());

                //Elemento Raiz Seq
                writer.WriteStartElement("Seq");
                //Escreve atributos IdEtiqueta
                writer.WriteAttributeString("ID", this.SequenciaEtiqueta.ToString());
                //Escreve atributos TIPO Etiqueta
                writer.WriteAttributeString("TIPO", this.TipoEtiqueta.ToString());
                //Escreve elemento entre a tag Seq
                writer.WriteElementString("Qtd", this.QuantidadeEtiqueta.ToString());
                writer.WriteElementString("Vol", this.VolumeEtiqueta.ToString());
                writer.WriteElementString("Time", this.DataHoraValidacao.ToString());
                writer.WriteElementString("Usuario", MainConfig.UserOn.Codigo.ToString());
                //Encerra o elemento Seq
                writer.WriteEndElement();

                //Encerra o elemento ItemS
                writer.WriteEndDocument();

                // O resultado é uma string.
                return result = str.ToString();
            }
            catch(Exception)
            {
                throw new CreateXmlException("Não foi possivel criar o xml para a etiqueta " + this.SequenciaEtiqueta );
            }
        }

        public override bool buscarEtiqueta(List<Etiqueta> listEtiquetas)
        {
            switch (this.TipoEtiqueta)
            {
                case TipoCode.QRCODE:

                    foreach (EtiquetaVenda itemList in listEtiquetas)
                    {
                        if (itemList.Equals(this))
                        {
                            return true;
                        }
                    }

                    return false;

                case TipoCode.BARRAS:

                    foreach (EtiquetaVenda itemList in listEtiquetas)
                    {
                        if (this.Equals(itemList))
                        {
                            return true;
                        }
                    }
                    return false;

                default:

                    return false;

            }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                switch (((EtiquetaVenda)obj).TipoEtiqueta)
                {
                    case TipoCode.QRCODE:

                        return (Ean13Etiqueta == ((EtiquetaVenda)obj).Ean13Etiqueta && SequenciaEtiqueta == ((Etiqueta)obj).SequenciaEtiqueta && CodigoLote == ((Etiqueta)obj).CodigoLote);

                    case TipoCode.BARRAS:

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

        public override string ToString()
        {
            return base.ToString() + "EAN13:" + Ean13Etiqueta;
        }
       
        #region "Idisposable"

        private Stream _resource;
        private bool _disposed;

        public override void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_resource != null)
                        _resource.Dispose();
                }

                // Indicate that the instance has been disposed.
                _resource = null;
                _disposed = true;
            }
        }

        #endregion

    }  
} 
