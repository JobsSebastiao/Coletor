﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Model;

namespace TitaniumColector.Classes
{
    public class Produto
    {
        private Int32 codigoProduto;
        private String ean13;
        private String partnumber;
        private String descricao;
        private Int64 codigoLoteProduto;
        private String identificacaoLoteProduto;
        private Int32 codigoLocalProduto;
        private String nomeLocalProduto;
        private Double pesoBruto;
        private List<EmbalagemProduto> embalagens;
       
    #region"CONTRUTORES"

        public Produto()
        {

        }

        /// <summary>
        /// Inclui ao contrutor valores para os atributos CodigoLoteProduto e IdentificacaoLoteProduto
        /// </summary>
        /// <param name="Codigo">Código do produto</param>
        /// <param name="ean13">Ean13 do produto</param>
        /// <param name="partnumber">`Partnumber do Produto</param>
        /// <param name="Descricao">DEscrição (NOME) do produto</param>
        /// <param name="codigoLocalLote">Código do local oonde está armazenado este produto.</param>
        /// <param name="nomeLocalLote">Nome(identificação) do local de armazenagem do produto</param>
        /// <param name="codLoteProd">Código do lote do produto</param>
        /// <param name="identificacaoLoteProd">Identificação do Lote do produto</param>
        public Produto(int codigo, string ean13, string partnumber, string descricao, string nomeLocalLote, int codLoteProd, string identificacaoLoteProd)
        {
            CodigoProduto = codigo;
            Ean13 = ean13;
            Partnumber = partnumber;
            Descricao = descricao;
            NomeLocalLote = nomeLocalLote;
            CodigoLoteProduto = codLoteProd;
            IdentificacaoLoteProduto = identificacaoLoteProd;
        }

        public Produto(int codigo, string partnumber, string descricao, string nomeLocalLote, int codLoteProd)
        {
            CodigoProduto = codigo;
            Partnumber = partnumber;
            Descricao = descricao;
            NomeLocalLote = nomeLocalLote;
            CodigoLoteProduto = codLoteProd;
        }

        public Produto(Int32 codigo, string ean13, string partnumber, string descricao,double peso)
        {
            this.CodigoProduto = codigo;
            this.Ean13 = ean13;
            this.Partnumber = partnumber;
            this.Descricao = descricao;
            this.Peso = peso;
        }

        public Produto(int codigo, string ean13, string partnumber, string descricao, string nomeLocalLote, Int64 codLoteProd, string identificacaoLoteProd,double peso)
        {
            this.CodigoProduto = codigo;
            this.Ean13 = ean13;
            this.Partnumber = partnumber;
            this.Descricao = descricao;
            this.NomeLocalLote = nomeLocalLote;
            this.CodigoLoteProduto = codLoteProd;
            this.IdentificacaoLoteProduto = identificacaoLoteProd;
            this.Peso = peso;
        }

        /// <summary>
        /// Recebe outro objeto do tipo Produto e seta seus valores para a nova instância do objeto.
        /// </summary>
        /// <param name="obj">Objeto do tipo Produto</param>
        public Produto(Object obj)
        {
            if (obj.GetType() != typeof(Produto))
            {
                this.CodigoProduto = ((Produto)obj).CodigoProduto;
                this.Ean13 = ((Produto)obj).Ean13;
                this.Partnumber = ((Produto)obj).Partnumber;
                this.Descricao = ((Produto)obj).Descricao;
                this.CodigoLocalLote = ((Produto)obj).CodigoLocalLote;
                this.NomeLocalLote = ((Produto)obj).NomeLocalLote;
                this.CodigoLoteProduto = ((Produto)obj).CodigoLoteProduto;
                this.IdentificacaoLoteProduto = ((Produto)obj).IdentificacaoLoteProduto;
                this.Peso = ((Produto)obj).Peso;
                this.Embalagens = ((Produto)obj).embalagens.ToList<EmbalagemProduto>();
            }
        }

    #endregion


    #region "GETS E SETS" 

        public Int32 CodigoProduto
        {
            get { return codigoProduto; }
            set { codigoProduto = value; }
        }

        public String Ean13
        {
            get { return ean13; }
            set { ean13 = value; }
        }

        public String Partnumber
        {
            get { return partnumber; }
            set { partnumber = value; }
        }
        
        public String Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        public Int64 CodigoLoteProduto
        {
            get { return codigoLoteProduto; }
            set { codigoLoteProduto = value; }
        }

        public String IdentificacaoLoteProduto
        {
            get { return identificacaoLoteProduto; }
            set { identificacaoLoteProduto = value; }
        }

        public Int32 CodigoLocalLote
        {
          get { return codigoLocalProduto; }
          set { codigoLocalProduto = value; }
        }
        
        public String NomeLocalLote
        {
          get { return nomeLocalProduto; }
          set { nomeLocalProduto = value; }
        }

        internal List<EmbalagemProduto> Embalagens
        {
            get { return embalagens; }
            set { embalagens = value; }
        }

        public Double Peso
        {
            get { return pesoBruto; }
            set { pesoBruto = value; }
        }

    #endregion

        public override string ToString()
        {
            return String.Format(" Código : {0} \n Ean13 : {1} \n PartNumber : {2} \n Descricão : {3} \n Código Local : {4} \n NomeLocalLote : {5} \n Código Lote : {6} \n Identificação Lote : {7} ",
                                  this.CodigoProduto,this.Ean13,this.Partnumber,this.Descricao,this.CodigoLocalLote, this.NomeLocalLote,this.CodigoLoteProduto,this.IdentificacaoLoteProduto);
        }
  
    }
}
