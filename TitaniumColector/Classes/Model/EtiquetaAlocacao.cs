﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    class EtiquetaAlocacao : Etiqueta,IComparable
    {
        public int CodigoProduto { get; set; }
        public int CodigoItemAlocacao { get; set; }
        public int CodigoLocalAlocacao { get; set; }
        public int CodigoLote { get; set; }
        public string LocaisLote { get; set; }
        public string VolumeItemAlocacao { get; set; }
        public string LocalAlocacao { get; set; }
        public string DescricaoCompletaProduto { get; set; }
        public bool JaAlocado { get; set; }

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
                                this.CodigoItemAlocacao = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "CODIGOPRODUTO")
                            {
                                this.CodigoProduto = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "CODIGOLOTE")
                            {
                                this.CodigoLote = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
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

                            this.DescricaoCompletaProduto = String.Format("{0}-{1}",base.PartnumberEtiqueta,base.DescricaoProdutoEtiqueta);
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
                switch (((EtiquetaAlocacao)obj).TipoEtiqueta)
                {
                    case Tipo.QRCODE:

                        return ( 
                                    this.CodigoProduto == ((EtiquetaAlocacao)obj).CodigoProduto 
                                 && this.CodigoItemAlocacao == ((EtiquetaAlocacao)obj).CodigoItemAlocacao 
                                 && this.CodigoLote == ((EtiquetaAlocacao)obj).CodigoLote
                                 && this.VolumeItemAlocacao == ((EtiquetaAlocacao)obj).VolumeItemAlocacao
                                );

                    default:
                        return false;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.LocaisLote.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("CodigoItem : {0}, Produto : {1} , Lote : {2}, Volume : {3}, Local Armazenagem : {4}"
                            ,this.CodigoItemAlocacao,this.DescricaoCompletaProduto,base.LoteEtiqueta,this.VolumeItemAlocacao,this.LocalAlocacao);
        }

        #region IComparable Members

        //Define os termos utilizado comparar e ordenar itens da classe.
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            else
                return (this.LocaisLote.CompareTo(((EtiquetaAlocacao)obj).LocaisLote));
        }

        #endregion
    }
}