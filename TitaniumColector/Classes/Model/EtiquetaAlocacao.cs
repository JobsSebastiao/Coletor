using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    class EtiquetaAlocacao : Etiqueta
    {
        private int codigoProduto { get; set; }
        private int codigoItemAlocacao { get; set; }
        private string LocaisLote { get; set; }
        private string VolumeItemAlocacao { get; set; }

        /// <summary>
        /// Valida informações do inputText no acionamento da "pistola"
        /// </summary>
        /// <param name="inputValue">string lida pelo coletor</param>
        /// <returns>Tipo de etiqueta lido</returns>
        public override Etiqueta.Tipo validaInputValueEtiqueta(string inputValue)
        {
            Etiqueta.Tipo tipoEtiqueta = Etiqueta.Tipo.INVALID;

            int inputLength = inputValue.Length;

            if (inputLength == 13)
            {
                tipoEtiqueta = Etiqueta.Tipo.BARRAS;
            }

            if (inputLength > 13 && (tipoEtiqueta == Etiqueta.Tipo.INVALID))
            {
                if (inputValue.Contains("CODIGOITEM:"))
                {
                    if (inputValue.Contains("CODIGOPRODUTO:"))
                    {
                        if (inputValue.Contains("CODIGOLOTE:"))
                        {
                            if (inputValue.Contains("LOTE:"))
                            {
                                if (inputValue.Contains("PARTNUMBER:"))
                                {
                                    if (inputValue.Contains("PRODUTO:"))
                                    {
                                        if (inputValue.Contains("LOCAISLOTE:"))
                                        {
                                            if (inputValue.Contains("VOLUME:"))
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

            return tipoEtiqueta;
        }

        public EtiquetaAlocacao() { }

        public EtiquetaAlocacao(Array arrayEtiqueta, Etiqueta.Tipo tipoEtiqueta)
        {
            try
            {
                switch (tipoEtiqueta)
                {
                    case Tipo.QRCODE:

                        foreach (string item in arrayEtiqueta)
                        {
                            string strItem = item.Substring(0, item.IndexOf(":", 0));

                            if (strItem == "CODIGOITEM")
                            {
                                this.codigoItemAlocacao = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "CODIGOPRODUTO")
                            {
                                this.codigoProduto = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "CODIGOLOTE")
                            {
                                base.LoteEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "NOMELOTE")
                            {
                                base.LoteEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "PARTNUMBER")
                            {
                                base.PartnumberEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "NOMEPRODUTO")
                            {
                                base.DescricaoProdutoEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "LOCAISLOTE")
                            {
                                this.LocaisLote = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "VOLUME")
                            {
                                this.VolumeItemAlocacao = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                        }

                        break;

                    default:
                        MainConfig.errorMessage("Tipo de Etiqueta indefinido!!", "Leitura Etiquetas");
                        break;
                }

                base.DataHoraValidacao = DateTime.Now;
                base.TipoEtiqueta = tipoEtiqueta;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override Etiqueta criarEtiqueta(Array arrayEtiqueta, Etiqueta.Tipo tipoEtiqueta)
        {
            return new EtiquetaAlocacao( arrayEtiqueta,  tipoEtiqueta);
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                switch (((EtiquetaVenda)obj).TipoEtiqueta)
                {
                    case Tipo.QRCODE:

                        return false; //(Ean13Etiqueta == ((EtiquetaVenda)obj).Ean13Etiqueta && SequenciaEtiqueta == ((Etiqueta)obj).SequenciaEtiqueta);

                    case Tipo.BARRAS:

                        return false; //(Ean13Etiqueta == ((EtiquetaVenda)obj).Ean13Etiqueta);

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
    }
}
