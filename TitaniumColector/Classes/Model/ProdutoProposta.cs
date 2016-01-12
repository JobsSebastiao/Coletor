using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes
{
    class ProdutoProposta : Produto
    {
        private int codigoItemProposta;
        private int propostaItemProposta;
        private double quantidade;
        private statusSeparado isSeparado;
        private Int32 lotereservaItemProposta;
        private String strlotesReserva;
        private Double quantidadeEmbalagem;
        private String nomeLocaisItemProposta;
        private Double pesoProdutos;

        public enum statusSeparado { NAOSEPARADO = 0, SEPARADO = 1 };

        #region   "CONTRUTORES"

        public ProdutoProposta()
        {

        }

        /// <summary>
        /// Utilizado na primeira carga de itens da proposta
        /// </summary>
        /// <param name="intCodigoItemProposta"></param>
        /// <param name="intPropostaItemProposta"></param>
        /// <param name="dblQuantidade"></param>
        /// <param name="isSeparado"></param>
        /// <param name="strLotesReserva"></param>
        /// <param name="strNomesLocaisItem"></param>
        /// <param name="intCodigoProduto"></param>
        /// <param name="strEan13"></param>
        /// <param name="strPartnumber"></param>
        /// <param name="strDescricao"></param>
        /// <param name="peso"></param>
        public ProdutoProposta
            (
                  Int32 intCodigoItemProposta
                , Int32 intPropostaItemProposta
                , double dblQuantidade
                , statusSeparado isSeparado
                , string strLotesReserva
                , string strNomesLocaisItem
                , Int32 intCodigoProduto
                , string strEan13
                , string strPartnumber
                , string strDescricao
                , double peso
            )
            : base(intCodigoProduto, strEan13, strPartnumber, strDescricao, peso)
        {
            this.CodigoItemProposta = intCodigoItemProposta;
            this.PropostaItemProposta = intPropostaItemProposta;
            this.Quantidade = dblQuantidade;
            this.StatusSeparado = isSeparado;
            this.NomeLocaisItemProposta = strNomesLocaisItem;
            this.LotesReserva = strLotesReserva;
            this.calcularPesoProdutos();
        }

        public ProdutoProposta(Int32 codigoItemProposta, Int32 propostaItemProposta, Double quantidade, statusSeparado isSeparado, Int32 loteReservaItemProposta,
         Int32 codigoProduto, String ean13, String partnumber, String nomeProduto, String nomeLocalLote, Int32 codigoLoteProduto, String identificacaoLoteProduto)
            : base(codigoProduto, ean13, partnumber, nomeProduto, nomeLocalLote, codigoLoteProduto, identificacaoLoteProduto)
        {
            this.CodigoItemProposta = codigoItemProposta;
            this.PropostaItemProposta = propostaItemProposta;
            this.Quantidade = quantidade;
            this.StatusSeparado = isSeparado;
            this.LotereservaItemProposta = loteReservaItemProposta;
        }

        public ProdutoProposta(Int32 codigoItemProposta, Int32 propostaItemProposta, Double quantidade, statusSeparado isSeparado, String lotesReservaItemProposta,
            Int32 codigoProduto, String ean13, String partnumber, String nomeProduto, String nomeLocalLote, Int32 codigoLoteProduto, String identificacaoLoteProduto,Double pesoProduto)
            : base(codigoProduto, ean13, partnumber, nomeProduto, nomeLocalLote, codigoLoteProduto, identificacaoLoteProduto,pesoProduto)
        {
            this.CodigoItemProposta = codigoItemProposta;
            this.PropostaItemProposta = propostaItemProposta;
            this.Quantidade = quantidade;
            this.StatusSeparado = isSeparado;
            this.LotesReserva = lotesReservaItemProposta;
            this.calcularPesoProdutos();
        }

        #endregion

        #region "GETS E SETS"

        public int CodigoItemProposta
        {
            get { return codigoItemProposta; }
            set { codigoItemProposta = value; }
        }

        public int PropostaItemProposta
        {
            get { return propostaItemProposta; }
            set { propostaItemProposta = value; }
        }

        public double Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        internal statusSeparado StatusSeparado
        {
            get { return isSeparado; }
            set { isSeparado = value; }
        }

        public Int32 LotereservaItemProposta
        {
            get { return lotereservaItemProposta; }
            set { lotereservaItemProposta = value; }
        }

        public String NomeLocaisItemProposta
        {
            get { return nomeLocaisItemProposta; }
            set { nomeLocaisItemProposta = value; }
        }

        public Double QuantidadeEmbalagem
        {
            get { return quantidadeEmbalagem; }
            set { quantidadeEmbalagem = value; }
        }

        public String LotesReserva
        {
            get { return strlotesReserva; }
            set { strlotesReserva = value; }
        }

        #endregion

        /// <summary>
        /// Altera o statusSeparado do Produto entre SEPARADO e NAOSEPARADO
        /// </summary>
        /// <param name="itemProposta">OBJETO </param>
        public void alteraStatusSeparado()
        {
            if (this.StatusSeparado == statusSeparado.NAOSEPARADO)
            {
                this.StatusSeparado = statusSeparado.SEPARADO;
            }
            else
            {
                this.StatusSeparado = statusSeparado.NAOSEPARADO;
            }
        }

        public Double PesoProdutos
        {
            get { return pesoProdutos; }
            set { pesoProdutos = value; }
        }

        private void calcularPesoProdutos()
        {
            PesoProdutos = this.Quantidade * base.Peso;
        }

        /// <summary>
        /// Altera o statusSeparado do Produto entre SEPARADO e NAOSEPARADO
        /// </summary>
        /// <param name="itemProposta">OBJETO DO TIPO PRODUTOPROPOSTA</param>
        public void alteraStatusSeparado(ProdutoProposta itemProposta)
        {
            if (itemProposta.GetType() == typeof(ProdutoProposta))
            {
                if (itemProposta.StatusSeparado == statusSeparado.NAOSEPARADO)
                {
                    itemProposta.StatusSeparado = statusSeparado.SEPARADO;
                }
                else
                {
                    itemProposta.StatusSeparado = statusSeparado.NAOSEPARADO;
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("\n Código Item : {0} \n Proposta Item : {1} \n Quantidade : {2} \n Status Separado : {3} \n Lote da Reserva : {4}",
                                                    this.CodigoItemProposta, this.PropostaItemProposta, this.Quantidade, this.StatusSeparado, this.LotereservaItemProposta);
        }
    }
}
