using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Exceptions;
using System.Xml;
using System.Xml.Linq;
using TitaniumColector.Classes.Model;
using TitaniumColector.Forms;
using TitaniumColector.Classes.Dao;
using System.Data.SqlServerCe;

namespace TitaniumColector.Classes.Procedimentos
{
     static class ProcedimentosLiberacao
     {
        public static Double TotalItens { get; set; }
        public static Double TotalPecas { get; set; }
        public static Double QtdPecasItem { get; set; }
        public static Double PesoTotalProdutos { get; set; }
        public static Double PesoTotalEmbalagens { get; set; }
        public static Double PesoTotalPedido { get; set; }
        public static Int32 TotalVolumes { get; set; }
        public static String Mensagem { get; set; }
        public static Array ArrayStringToEtiqueta { get; set; }
        public static List<Etiqueta> ListEtiquetasLidas { get; set; }
        public static List<Etiqueta> ListEtiquetas { get; set; }
        public static List<EmbalagemSeparacao> ListEmbalagensSeparacao { get; set; }
        
        public static void inicializarProcedimentos(Double tItens, Double tPecas, Double pecasItens,Int32 qtdVolumes)
        {
            TotalItens = tItens;
            TotalPecas = tPecas;
            QtdPecasItem = pecasItens;
            ProcedimentosLiberacao.inicializaQtdVolumes();
            ProcedimentosLiberacao.calcularPesoTotalEmbalagens();
            //ProcedimentosLiberacao.PesoTotalProdutos = 0;

            ListEtiquetasLidas = new List<Etiqueta>();

            //ProximaEtiqueta = 0;
            if (ArrayStringToEtiqueta != null)
            {
                ArrayStringToEtiqueta = null;
            }
        }

        /// <summary>
         /// Não altera o total de peças e o total de itens atualmente setados.
         /// </summary>
         /// <param name="pecasItens">Quantidade de peças do item a ser trabalhado.</param>
        public static void inicializarProcedimentosProximoItem(Double pecasItens)
        {
            TotalItens = TotalItens;
            TotalPecas = TotalPecas;
            QtdPecasItem = pecasItens;

            if (ListEtiquetasLidas != null)
            {
                ListEtiquetasLidas.Clear();
            }
            else
            {
                ListEtiquetasLidas = new List<Etiqueta>();
            }

            //ProximaEtiqueta = 0;

            if (ListEtiquetas != null)
            {
                ListEtiquetas.Clear();
            }

            if (ArrayStringToEtiqueta != null)
            {
                ArrayStringToEtiqueta = null;
            }
        }

        public static Double subtrairQtdPecasItem(Double value)
        {
            if (QtdPecasItem - value >= 0)
            {
                return QtdPecasItem -= value;
            }
            else
            {
                throw new QuantidadeInvalidaException(String.Format("O Valor informado é maior que a Quantidade de peças existentes."));
            }
        }

        #region "Limpando "

        //public static void somarQtdPecasItem(Double value)
        //{
        //    TotalPecas = value;
        //}

        ///// <summary>
        ///// Verifica se a Etiqueta já foi lida.
        ///// </summary>
        ///// <returns>FALSE --> se a etiqueta for encontrada na list
        /////          TRUE --> se a etiqueta ainda não foii lida.
        ///// </returns>
        //public static bool podeTrabalharEtiqueta(Etiqueta objEtiqueta, List<Etiqueta> listEtiquetas)
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
        //            if (Etiqueta.validarEtiqueta(objEtiqueta, listEtiquetas))
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="inputValue"></param>
        ///// <param name="produto"></param>
        ///// <param name="tbProduto"></param>
        ///// <param name="tblote"></param>
        ///// <param name="tbSequencia"></param>
        ///// <param name="tbQuantidade"></param>
        ///// <param name="tbMensagem"></param>
        //public static void lerEtiqueta(String inputValue, ProdutoProposta produto, TextBox tbProduto, TextBox tblote, TextBox tbSequencia, TextBox tbQuantidade, TextBox tbMensagem)
        //{
        //    tbMensagem.Text = "";

        //    ArrayStringToEtiqueta = FileUtility.arrayOfTextFile(inputValue, FileUtility.splitType.PIPE);
        //    Etiqueta objEtiqueta = new EtiquetaVenda();
        //    objEtiqueta = new EtiquetaVenda(ArrayStringToEtiqueta, Etiqueta.TipoCode.QRCODE);
        //    efetuaLeituraEtiqueta(produto, tbProduto, tblote, tbSequencia, tbQuantidade, tbMensagem, (EtiquetaVenda)objEtiqueta);
        //}

        ///// <summary>
        ///// Limpa a list de Etiquetas Lidas.
        ///// </summary>
        //public static void clearListEtiquetasLidas()
        //{
        //    ListEtiquetasLidas.Clear();
        //    ListEtiquetasLidas = null;
        //    ListEtiquetasLidas = new List<Etiqueta>();
        //}

        ///// <summary>
        ///// Valida o tipo de etiqueta lido
        ///// </summary>
        ///// <param name="inputValue">informação capturada pelo Leitor</param>
        ///// <returns>Etiqueta.tipo (EAN13,QRCODE,INVALID)</returns>
        //public static Etiqueta.TipoCode validaInputValueEtiqueta(String inputValue)
        //{
        //    Etiqueta.TipoCode tipoEtiqueta;

        //    int inputLength = inputValue.Length;

        //    if (inputLength == 13)
        //    {
        //        tipoEtiqueta = Etiqueta.TipoCode.BARRAS;
        //    }
        //    else if (inputLength > 13)
        //    {
        //        if (inputValue.Contains("PNUMBER:"))
        //        {
        //            if (inputValue.Contains("DESCRICAO:"))
        //            {
        //                if (inputValue.Contains("EAN13:"))
        //                {
        //                    if (inputValue.Contains("LOTE:"))
        //                    {
        //                        if (inputValue.Contains("SEQ:"))
        //                        {
        //                            if (inputValue.Contains("QTD:"))
        //                            {
        //                                tipoEtiqueta = Etiqueta.TipoCode.QRCODE;
        //                            }
        //                            else
        //                            {
        //                                tipoEtiqueta = Etiqueta.TipoCode.INVALID;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            tipoEtiqueta = Etiqueta.TipoCode.INVALID;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        tipoEtiqueta = Etiqueta.TipoCode.INVALID;
        //                    }
        //                }
        //                else
        //                {
        //                    tipoEtiqueta = Etiqueta.TipoCode.INVALID;
        //                }
        //            }
        //            else
        //            {
        //                tipoEtiqueta = Etiqueta.TipoCode.INVALID;
        //            }
        //        }
        //        else
        //        {
        //            tipoEtiqueta = Etiqueta.TipoCode.INVALID;
        //        }

        //    }
        //    else
        //    {
        //        tipoEtiqueta = Etiqueta.TipoCode.INVALID;
        //    }

        //    return tipoEtiqueta;
        //}


        //public static void continuarLiberacao(Proposta proposta)
        //{
        //    if (proposta.IsInterrompido)
        //    {
        //        proposta.IsInterrompido = false;
        //    }
        //}

        //public static EmbalagemSeparacao retornaEmbalagem(int codigo)
        //{
        //    foreach (var item in ListEmbalagensSeparacao)
        //    {
        //        if (item.Codigo == codigo)
        //        {
        //            return item;
        //        }
        //    }

        //    return null;
        //}

        //public static void addPesoProdutos(Double peso)
        //{
        //    if (peso > 0.0)
        //    {
        //        ProcedimentosLiberacao.PesoTotalProdutos = peso;
        //    }
        //}

        //public static void removePesoProdutos(Double peso)
        //{
        //    if (peso > 0.0 && (ProcedimentosLiberacao.PesoTotalProdutos - peso > 0))
        //    {
        //        ProcedimentosLiberacao.PesoTotalProdutos = peso;
        //    }
        //}

        #endregion

        /// <summary>
        /// Verifica se a Etiqueta já foi lida.
        /// </summary>
        /// <returns>
        ///     FALSE --> se a etiqueta for encontrada na list
        ///     TRUE --> se a etiqueta ainda não foii lida.
        /// </returns>
        public static bool podeTrabalharEtiqueta(Etiqueta objEtiqueta)
        {
            try
            {
                //Verifica se o List foi iniciado
                switch (objEtiqueta.TipoEtiqueta)
                {
                    case Etiqueta.TipoCode.INVALID:
                        return false;
                    case Etiqueta.TipoCode.QRCODE:
                        if (ListEtiquetasLidas != null)
                        {
                            if (ListEtiquetasLidas.Count == 0)
                            {
                                return true;
                            }
                            else
                            {
                                //Verifica se a etiqueta já esta na lista de etiquetas Lidas
                                if (objEtiqueta.buscarEtiqueta(ListEtiquetasLidas))
                                {
                                    //Caso esteja na lista
                                    return false;
                                }
                                else
                                {
                                    //caso não esteja na lista.
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            throw new invalidArgumentException("Não foi possível validar a etiqueta!." + objEtiqueta.SequenciaEtiqueta);
                        }
                    case Etiqueta.TipoCode.BARRAS:
                        return true;
                    default:
                        return false;
                }
            }
            catch (invalidArgumentException ex)
            {
                throw ex;
            }
        }
  
        /// <summary>
        /// Valida as informações lidas pelo coletor e as transformam em um objeto do tipo Etiqueta e continua o procedimento de leitura da etiqueta.
        /// </summary>
        /// <param name="inputValue">Valor fornecido pelo coletor de dados</param>
        /// <param name="tipoEtiqueta">Tipo de etiqueta lida  (EAN13) OU (QRCOODE)</param>
        /// <param name="produto">produto a ser validado durante processo de liberação do item</param>
        /// <param name="tbProduto">Campo para informações ao usuário</param>
        /// <param name="tblote">Campo para informações ao usuário</param>
        /// <param name="tbSequencia">Campo para informações ao usuário</param>
        /// <param name="tbQuantidade">Campo para informações ao usuário</param>
        /// <param name="tbMensagem">Campo para informações ao usuário</param>
        public static void lerEtiqueta(String inputValue,Etiqueta.TipoCode tipoEtiqueta, ProdutoProposta produto, TextBox tbProduto, TextBox tblote, TextBox tbSequencia, TextBox tbQuantidade, TextBox tbMensagem)
        {
            tbMensagem.Text = "";
            //monta um array de string com os dados lidos na etiqueta
            //utiliza as informações para montar um objeto do tipo etiquetaVenda
            var objEtiqueta = new EtiquetaVenda(FileUtility.arrayOfTextFile(inputValue, FileUtility.splitType.PIPE), tipoEtiqueta);
            efetuaLeituraEtiqueta(produto, tbProduto, tblote, tbSequencia, tbQuantidade, tbMensagem, objEtiqueta);
        }
        
        /// <summary>
        /// realiza todo o procedimento para validação de tipo de etiqueta,etiqueta, produto,
        /// validação de sequência,subtração de quantidade de itens registrados na etiqueta.
        /// </summary>
        /// <param name="produto">Produto a ser validado</param>
        /// <param name="tbProduto">Text Box Nome do Produto</param>
        /// <param name="tbLote">Text Box Lote</param>
        /// <param name="tbSequencia">Text Box  Sequencia</param>
        /// <param name="tbQuantidade">Text Box Quantidade</param>
        /// <param name="tbMensagem">Text Box Mensagem</param>
        /// <param name="objEtiqueta">Objeto Etiqueta.</param>
        public static void efetuaLeituraEtiqueta(ProdutoProposta produto,TextBox tbProduto,TextBox tbLote,TextBox tbSequencia,TextBox tbQuantidade,
                                                 TextBox tbMensagem,EtiquetaVenda objEtiqueta)
        {
            try
            {
                if (comparaProdutoEtiquetaProdutoTrabalhado(produto, objEtiqueta))
                {
                    if (objEtiqueta.QuantidadeEtiqueta == 0.0)
                    {
                        throw new QuantidadeInvalidaException("A quantidade de produtos informado na embalagem é inválida!");
                    }
                    if (podeTrabalharEtiqueta(objEtiqueta))
                    {
                        if (QtdPecasItem > 0)
                        {
                            tbProduto.Text = objEtiqueta.PartnumberEtiqueta.ToString() + " - " + objEtiqueta.DescricaoProdutoEtiqueta.ToString();
                            tbLote.Text = objEtiqueta.LoteEtiqueta;
                            tbSequencia.Text = objEtiqueta.SequenciaEtiqueta.ToString();
                            tbQuantidade.Text = MainConfig.intOrDecimal(Convert.ToDouble(subtrairQtdPecasItem(objEtiqueta.QuantidadeEtiqueta)));
                            objEtiqueta.VolumeEtiqueta = ProcedimentosLiberacao.TotalVolumes;
                            addToListEtiquetasLidas(objEtiqueta);
                        }
                    }
                    else
                    {
                        FrmProposta.mostrarMensagem(TitaniumColector.Forms.FrmProposta.enumCor.RED,String.Format("A etiqueta {0} já foi validada.", objEtiqueta.SequenciaEtiqueta),TitaniumColector.Forms.FrmProposta.enumCursor.DEFAULT);
                    }
                }
                else
                {
                    if (ProcedimentosLiberacao.Mensagem != "" || ProcedimentosLiberacao.Mensagem != null)
                    {
                        tbMensagem.Text = ProcedimentosLiberacao.Mensagem;
                    }
                    else 
                    {
                        tbMensagem.Text = String.Format("Produto da etiqueta lida não confere com o item a ser liberado.");
                    }
                }
            }
            catch ( QuantidadeInvalidaException qtdEx)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("error : {0} ", qtdEx.Message);
                MainConfig.errorMessage(sb.ToString(), "Erro de validação!!");
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        /// <summary>
         /// Verifica se  Produto trabalhado e Produto Etiqueta são os mesmos.
         /// </summary>
         /// <param name="propostaProduto">Obj Produto que será verificado </param>
         /// <param name="etiquetaLida"> Obj Etiqueta que será verificado </param>
         /// <returns>True --> Caso sejam iguais.</returns>
         /// 
        public static bool comparaProdutoEtiquetaProdutoTrabalhado(ProdutoProposta produtoProposta, EtiquetaVenda etiquetaLida)
        {
            try
            {
                //Verifica se os produtos são iguais
                switch (etiquetaLida.TipoEtiqueta)
                {
                    case Etiqueta.TipoCode.QRCODE:

                        if (produtoProposta.Partnumber.Equals(etiquetaLida.PartnumberEtiqueta))
                        {
                            if (produtoProposta.Embalagens.Count == 0)
                            {
                                throw new QuantidadeInvalidaException("O produto não possui embalagens cadastradas!!");
                            }
                            foreach (var item in produtoProposta.Embalagens)
                            {
                                if ((etiquetaLida.Ean13Etiqueta.ToString() == item.Ean13Embalagem))
                                {
                                    if (etiquetaLida.QuantidadeEtiqueta == item.Quantidade)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        Mensagem = "Quantidade da etiqueta não confere com a Quantidade de itens da embalagem do produto.";
                                    }
                                }
                                else
                                {
                                    Mensagem = "Ean informado na Etiqueta não confere com o Ean utilizado no produto";
                                }
                            }
                        }
                        else
                        {
                            Mensagem = "Partnumber produto etiqueta não confere com partnumber do item da proposta";
                        }
                        break;

                    case Etiqueta.TipoCode.BARRAS:

                        if (produtoProposta.Partnumber.Equals(etiquetaLida.PartnumberEtiqueta))
                        {
                            foreach (var item in produtoProposta.Embalagens)
                            {
                                if ((etiquetaLida.Ean13Etiqueta.ToString() == item.Ean13Embalagem))
                                {
                                    return true;
                                }
                                else
                                {
                                    Mensagem = "Ean informado na Etiqueta não confere com o Ean do produto";
                                }
                            }
                        }
                        else
                        {
                            Mensagem = "Partnumber produto etiqueta não confere com partnumber do item da proposta";
                        }

                        break;

                    case Etiqueta.TipoCode.INVALID:

                        break;

                    default:

                        break;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
         /// Adiciona uma atiqueta a List Etiquetas Lidas.
         /// </summary>
         /// <param name="etiquetaLida"></param>
        public static void addToListEtiquetasLidas(Etiqueta etiquetaLida)
        {
            ListEtiquetasLidas.Add(etiquetaLida);
        }

        ///<summary>
         ///Altera o Valor do atributo Auxiliar que armazena informações sobre a Quantidade de Pecas
         ///</summary>
         ///<param name="qtd">Quantidade a ser diminuida</param>
         ///<returns>Retorna true caso não ocorra erros
         ///         false se o calculo não ocorrer com esperado.</returns>
        public static Boolean decrementaQtdTotalPecas(double qtd)
        {
            try
            {
                if (ProcedimentosLiberacao.TotalPecas > 0 && (ProcedimentosLiberacao.TotalPecas - qtd >= 0))
                {
                    ProcedimentosLiberacao.TotalPecas -= qtd;
                    return true;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new QuantidadeInvalidaException();
            }
        }

        /// <summary>
        /// Altera o Valor do atributo Auxiliar que armazena informações sobre a Quantidade de Itens
        /// </summary>
        /// <param name="qtd">Quantidade a ser diminuida</param>
        /// <returns>Retorna true caso não ocorra erros
        ///          false se o calculo não ocorrer com esperado.</returns>
        public static Boolean decrementaQtdTotalItens(double qtd )
        {
            try
            {
                if (ProcedimentosLiberacao.TotalItens > 0 && (ProcedimentosLiberacao.TotalItens - qtd >= 0))
                {
                    ProcedimentosLiberacao.TotalItens -= qtd;
                    return true;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new QuantidadeInvalidaException();
            }
        }

        /// <summary>
        /// Altera o status do produto para separado.
        /// </summary>
        /// <param name="item">item que tera o seu status alterado.</param>
        public static void setStatusProdutoParaSeparado(ProdutoProposta item)
        {

            if (item.StatusSeparado == ProdutoProposta.statusSeparado.SEPARADO)
            {
                return;
            }
            else
            {
                item.alteraStatusSeparado();
            }    

        }

        public static void interromperLiberacao(Proposta proposta)
        {
            if (!proposta.IsInterrompido) 
            {
                proposta.IsInterrompido = true;
            }
        }

        /// <summary>
         /// Retorna a Embalagem Setada Como padrão.
         /// </summary>
         /// <returns>Objeto Embalagem Separação setado como padrão.</returns>
        public static EmbalagemSeparacao retornaEmbalagemPadrao()
        {
            try
            {
                foreach (var item in ListEmbalagensSeparacao)
                {
                    if (item.isPadrao())
                    {
                        return item;
                    }
                }

                throw new InvalidOperationException("Não foi encontrada uma embalagem padrão!\n");
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Problemas durante inicialização de volumes!\n" + ex.Message + "\n", ex);
            }
            catch (Exception)
            {
                throw new Exception("Problemas durante Procedimentos de Liberação!!\n");
            }         
        }

        /// <summary>
        /// Define a quantidade de Volumes da Proposta 
        /// </summary>
        /// <remarks>
        /// Quando a quantidade informada for igual a 0 será setando a quantidade de embalagens 
        /// com 1 para a embalagem padrão contida na lista desta forma a quantidade de volumes inicial será 1
        /// </remarks>
        private static void inicializaQtdVolumes()
        {
            if (ProcedimentosLiberacao.TotalVolumes  == 0)
            {
                ProcedimentosLiberacao.setarVolumeInicial();
            }
        }

        /// <summary>
        /// Define a quantidade de Volumes da Proposta 
        /// </summary>
        /// <remarks>
        /// Quando a quantidade informada for igual a 0 será setando a quantidade de embalagens 
        /// com 1 para a embalagem padrão contida na lista desta forma a quantidade de volumes inicial será 1
        /// </remarks>
        /// <param name="qtdVolumes">quantidade de volumes ao iniciar o procedimento</param>
        private static void inicializaQtdVolumes(int qtdVolumes)
        {
            if (qtdVolumes < 0) 
            {
                throw new ArithmeticException("O valor informado não pode ser negativo!");
            }
            else if (qtdVolumes == 0)
            {
                ProcedimentosLiberacao.setarVolumeInicial();
                return;
            }
            else
            {
                TotalVolumes = qtdVolumes;
            } 
        }

        /// <summary>
        /// Inicializa a quantidade de embalagem como 1 para a embalagem que está setada como padrão
        /// </summary>
        /// <exception cref="InvalidOperationException"> Caso não exista uma embalagem padrão setada um erro será mostrado ao usuário.</exception>
        public static void setarVolumeInicial() 
        {
            try 
            {
	            foreach (var item in ListEmbalagensSeparacao)
                {
                    if (item.isPadrao())
                    {
                         item.inicializaQtdEmbalagem();
                         calcularTotalVolumes();
                         return;
                    }
                }

                throw new InvalidOperationException("Não foi encontrada uma embalagem padrão!\n");
            }
            catch(InvalidOperationException ex)
            {
                throw new Exception("Problemas durante inicialização de volumes!\n" + ex.Message + "\n", ex);
            }
            catch (Exception)
            {
	            throw new Exception("Problemas durante Procedimentos de Liberação!!\n");
            }         
        }

        public static void decrementaQtdEmbalagem(int codigoEmbalagem)
        {
            try
            {
                if (!podeDecremetar())
                {
                    throw new invalidArgumentException("A quantidade de volumes não pode ser menoor que 1!");
                }

                foreach (var item in ListEmbalagensSeparacao)
                {
                    if (item.Codigo == codigoEmbalagem)
                    {
                        if (item.remover())
                        {
                            ProcedimentosLiberacao.TotalVolumes--;
                            ProcedimentosLiberacao.PesoTotalEmbalagens -= item.Peso;
                        }
                        return;
                    }
                }

            }
            catch (invalidArgumentException ex)
            {
                MainConfig.errorMessage(ex.Message, "Gerenciar Volumes");
            } 
            catch (InvalidOperationException ex)
            {
                MainConfig.errorMessage(ex.Message,"Gerenciar Volumes");
            }   
        }

        public static void incrementaQtdEmbalagem(int codigoEmbalagem)
        {
            foreach (var item in ListEmbalagensSeparacao)
            {
                if (item.Codigo == codigoEmbalagem)
                {
                     item.adicionar();
                     ProcedimentosLiberacao.TotalVolumes++;
                     ProcedimentosLiberacao.PesoTotalEmbalagens += item.Peso;
                     return;
                }
            }

        }

        public static void tratarParaProximoItem(Proposta objProposta) 
        {
            //processa Quantidade de itens
            ProcedimentosLiberacao.decrementaQtdTotalItens(1);
            //processa Quantidade de peças
            ProcedimentosLiberacao.decrementaQtdTotalPecas(objProposta.ListObjItemProposta[0].Quantidade);
            //incrementa o peso total dos pordutos.
            ProcedimentosLiberacao.PesoTotalProdutos += objProposta.ListObjItemProposta[0].PesoProdutos;
            //atualiza peso total pedido
            ProcedimentosLiberacao.atualizarPesoTotalPedido();
            //seta status para separado
            ProcedimentosLiberacao.setStatusProdutoParaSeparado(objProposta.ListObjItemProposta[0]);
        }
        /// <summary>
         /// Calcula a quantidade de Volumes registrados para a proposta.
         /// </summary>
         /// <returns> quantidade total de cvolumes.</returns>
       
        private static bool podeDecremetar() {
           return ProcedimentosLiberacao.TotalVolumes > 1;
        }

        public static int calcularTotalVolumes()
        {
            ProcedimentosLiberacao.TotalVolumes = 0;

            foreach (var item in ProcedimentosLiberacao.ListEmbalagensSeparacao)
            {
                ProcedimentosLiberacao.TotalVolumes += item.Quantidade;
            }

            return TotalVolumes;
        }

        private static void calcularPesoTotalEmbalagens()
        {
            ProcedimentosLiberacao.PesoTotalEmbalagens = 0;

            foreach (var item in ProcedimentosLiberacao.ListEmbalagensSeparacao)
            {
                ProcedimentosLiberacao.PesoTotalEmbalagens += item.PesoTotal;
            }

        }

        public static void atualizarPesoTotalPedido()
        {
            ProcedimentosLiberacao.PesoTotalPedido = ProcedimentosLiberacao.PesoTotalEmbalagens + ProcedimentosLiberacao.PesoTotalProdutos;
        }

        public static void limpar() 
        {
            TotalItens = 0;
            TotalPecas = 0;
            TotalVolumes = 0;
            PesoTotalProdutos = 0.0;
            QtdPecasItem = 0.0;
            PesoTotalEmbalagens = 0.0;
            PesoTotalPedido = 0.0;
            ListEtiquetasLidas = null;
            ListEtiquetas = null;
            ListEmbalagensSeparacao = null;
            ArrayStringToEtiqueta = null;
            Mensagem="";

        }
        
         /// <summary>
         /// monta o xml para cada codigoitem existente no processo de liberação
         /// </summary>
         /// <returns></returns>
        public static string montarXmlProcedimento()
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

                foreach (var item in ListEtiquetasLidas)
                {
                    //Elemento Raiz Seq
                    writer.WriteStartElement("Seq");
                    //Escreve atributos IdEtiqueta
                    writer.WriteAttributeString("ID", item.SequenciaEtiqueta.ToString());
                    //Escreve atributos TIPO Etiqueta
                    writer.WriteAttributeString("TIPO", item.TipoEtiqueta.ToString());
                    //Escreve elemento entre a tag Seq
                    writer.WriteElementString("Qtd", item.QuantidadeEtiqueta.ToString());
                    writer.WriteElementString("Vol", item.VolumeEtiqueta.ToString());
                    writer.WriteElementString("Time", item.DataHoraValidacao.ToString());
                    writer.WriteElementString("Usuario", MainConfig.UserOn.Codigo.ToString());
                    //Encerra o elemento Seq
                    writer.WriteEndElement();
                }

                //Encerra o elemento Item
                writer.WriteEndDocument();

                // O resultado é uma string.
                return result = str.ToString();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void finalizarProposta(Proposta objProposta,FrmProposta frmProposta)
        {
            try
            {    
                FrmProposta.mostrarMensagem(TitaniumColector.Forms.FrmProposta.enumCor.BLUE, "Gravando informações na base de dados!", TitaniumColector.Forms.FrmProposta.enumCursor.WAIT);
                var daoItemProposta = new DaoProdutoProposta();
                var daoProposta = new DaoProposta();
                var daoEmbalagem = new DaoEmbalagem();

                daoEmbalagem.salvarEmbalagensSeparacao(objProposta);
                daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.FINALIZADO, true, true);
                daoItemProposta.updateItemPropostaRetorno();
                daoProposta.updateVolumeProposta(objProposta.Codigo);
                daoProposta.retiraPropostaListaPrioridade(objProposta.Codigo, MainConfig.UserOn.Codigo);

            }
            catch (Exception ex)
            {
                throw new Exception("finalizarProposta()\n " + ex.Message);
            }
            finally
            {
                FrmAcao frm = new FrmAcao();
                FrmProposta.mostrarMensagem(TitaniumColector.Forms.FrmProposta.enumCor.RED, "", TitaniumColector.Forms.FrmProposta.enumCursor.DEFAULT);
                frmProposta.Dispose();
                frmProposta.Close();
                frm.Show();
            }
        }

        /// <summary>
        /// Realiza todos os procedimentos nescessários para carregar o próximo item a ser separado.
        /// </summary>
        /// 
        /// <returns>
        ///          TRUE --> caso exista um próximo item a ser trabalhado
        ///          FALSE --> caso não exista mais items para serem trabalhados.
        /// </returns>
        public static bool nextItemProposta(Proposta objProposta,FrmProposta frm)
        {
            bool hasItem = false;
            var daoItemProposta = new DaoProdutoProposta();
            var daoEtiqueta = new DaoEtiqueta();
            var objTransacoes = new BaseMobile();

            try
            {
                frm.clearParaProximoItem();

                tratarParaProximoItem(objProposta);
                //grava informações do item na base de dados mobile
                daoItemProposta.updateStatusItemProposta(objProposta.ListObjItemProposta[0]);
                //inseri informações das etiquetas referente ao produto liberado em formato Xml
                daoItemProposta.updateXmlItemProposta(montarXmlProcedimento(), objProposta.ListObjItemProposta[0].CodigoItemProposta);

                //carrega próximo item
                if (frm.temItensConferir())
                {
                    var prod = daoItemProposta.itemATrabalhar();
                    var daoEmbalagem = new DaoEmbalagem();

                    if (prod != null)
                    {
                        //Carrega informações de Embalagem para o produto que será trabalhado.
                        prod.Embalagens = daoEmbalagem.carregarEmbalagensProduto(prod);

                        hasItem = true;

                        objProposta.setNextItemProposta(prod);
                    }
                    else
                    {
                        hasItem = false;
                    }
                }
                else // CASO não tenha um próximo item 
                {
                    hasItem = false;
                }

                //Se existir um próximo item
                if (hasItem)
                {
                    //seta Parametros para iniciar leitura do próximo item
                    inicializarProcedimentosProximoItem(objProposta.ListObjItemProposta[0].Quantidade);

                    //recarrega o form com as informações do próximo item.
                    //frm.fillCamposForm(objProposta, TotalPecas, TotalItens);
                    frm.fillCamposForm(objProposta);
                }
                else
                {
                    frm.clearFormulario(true, true);
                }
            }
            catch (SqlCeException Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar próximo item!", ex);
            }
            finally
            {
                daoEtiqueta = null;
                daoItemProposta = null;
                objTransacoes = null;
            }

            return hasItem;
        }

        /// <summary>
        /// Reliza todos os processos nescessários para efetuar a carga de dados na base Mobile.
        /// </summary>
        public static void carregaBaseMobile(FrmProposta frm)
        {

            var objTransacoes = new BaseMobile();
            var objProposta = new Proposta();
            var daoItemProposta = new DaoProdutoProposta();
            var daoProposta = new DaoProposta();
            var daoProduto = new DaoProduto();
            var daoEmbalagem = new DaoEmbalagem();

            //LIMPA INFORMAÇÕES RESULTANTE DE OUTROS PRODUTOS JÁ CONFERIDOS
            ProcedimentosLiberacao.limpar();

            try
            {
                //Limpa a Base.
                objTransacoes.clearBaseMobile();

                //Carrega um objeto Proposta e inicia todo o procedimento.
                //Caso não exista propostas a serem liberadas gera um exception 
                //onde será feito os tratamentos para evitar o travamento do sistema.
                if ((objProposta = daoProposta.fillTop1PropostaServidor()) != null)
                {
                    daoProposta.InsertOrUpdatePickingMobile(objProposta, MainConfig.UserOn.Codigo, Proposta.StatusLiberacao.TRABALHANDO, DateTime.Now);

                    //recupera o codigoPickingMobile da proposta trabalhada.
                    objProposta.CodigoPikingMobile = daoProposta.selectMaxCodigoPickingMobile(objProposta.Codigo);

                    //Realiza o Insert na Base Mobile
                    daoProposta.insertProposta(objProposta, MainConfig.UserOn.Codigo);

                    //Recupera List com itens da proposta
                    //Insert na Base Mobile tabela tb0002_ItensProposta
                    daoItemProposta.carregarBaseMobileItens(daoItemProposta.fillListItensProposta((int)objProposta.Codigo).ToList<ProdutoProposta>());

                    //Insert na base Mobile tabela tb0003_Produtos
                    //Recupera informações sobre os produtos existentes na proposta
                    daoProduto.insertProdutoBaseMobile(daoProduto.fillListProduto((int)objProposta.Codigo).ToList<Produto>());

                    //Armazena informações de embalagens do produto na base mobile.
                    daoEmbalagem.insertEmbalagemBaseMobile(daoEmbalagem.cargaEmbalagensProduto((int)objProposta.Codigo));

                    //Carrega Informações das Embalagens de Separação.
                    //Carrega Quantidade das Embalagens utilizadas nos volumes da separação
                    ProcedimentosLiberacao.ListEmbalagensSeparacao = daoEmbalagem.carregarInformacoesEmbalagensUtilizadas((Int32)objProposta.CodigoPikingMobile, daoEmbalagem.carregarEmbalagensSeparacao());

                }
                else
                {
                    throw new NoNewPropostaException("Não existem novas propostas a serem liberadas!!");
                }
            }
            catch (SqlQueryExceptions ex)
            {
                frm.exitOnError(ex.Message, "Próxima Proposta", false);
            }
            catch (NoNewPropostaException ex)
            {
                frm.exitOnError(ex.Message, "Próxima Proposta", false);
            }
            catch (SqlCeException sqlEx)
            {
                ProcedimentosLiberacao.interromperLiberacao(objProposta);
                daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, false);
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("O procedimento não pode ser concluído.\n");
                strBuilder.AppendFormat("Erro : {0}", sqlEx.Errors);
                strBuilder.AppendFormat("Description : {0}", sqlEx.Message);
                frm.exitOnError(strBuilder.ToString(), "SqlException!!", false);
            }
            catch (Exception ex)
            {
                ProcedimentosLiberacao.interromperLiberacao(objProposta);
                daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, false);
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("O procedimento não pode ser concluído.\n");
                strBuilder.AppendFormat(" Descrição: {0}", ex.Message);
                strBuilder.Append("\nContate o Administrador do sistema.");
                frm.exitOnError(strBuilder.ToString(), "SqlException!!", false);
            }
            finally
            {
                objTransacoes = null;
                objProposta = null;
                daoProposta = null;
                daoProduto = null;
                daoItemProposta = null;
                daoEmbalagem = null;
            }

        }

        /// <summary>
        /// Realiza todo o processo de liberação para o produto lido
        /// </summary>
        /// <param name="inputText">Valor captado pelo Leitor</param>
        /// <param name="tipoEtiqueta">Tipo de Etiqueta a ser validada</param>
        public static void liberarItem(String inputText, Etiqueta.TipoCode tipoEtiqueta,Proposta objProposta,FrmProposta frm)
        {
            try
            {
                ProcedimentosLiberacao.lerEtiqueta(inputText, tipoEtiqueta, objProposta.ListObjItemProposta[0], frm.tbProduto, frm.tbLote, frm.tbSequencia, frm.tbQuantidade, FrmProposta.tbMensagem);

                if (ProcedimentosLiberacao.QtdPecasItem == 0)
                {
                    if (!ProcedimentosLiberacao.nextItemProposta(objProposta, frm))
                    {
                        ProcedimentosLiberacao.finalizarProposta(objProposta, frm);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Proposta carregarProposta(FrmProposta frm) 
        {
            try
            {
                var proposta = new Proposta();
                var ListInformacoesProposta = new List<string>();
                var daoProposta = new DaoProposta();
                var daoEmbalagem = new DaoEmbalagem();

                //Carrega um list com informações gerais sobre a proposta atual na base Mobile.
                ListInformacoesProposta = daoProposta.fillInformacoesProposta();

                //carrega um obj Proposta com a atual proposta na base mobile 
                //e com o item top 1 da proposta.
                proposta = daoProposta.fillPropostaWithTop1Item();

                //Set o total de peças e o total de Itens para o objeto proposta
                proposta.setTotalValoresProposta(Convert.ToDouble(ListInformacoesProposta[4]), Convert.ToDouble(ListInformacoesProposta[3]));

                //Carrega informações de Embalagem para o produto que será trabalhado.
                proposta.ListObjItemProposta[0].Embalagens = daoEmbalagem.carregarEmbalagensProduto(proposta);

                //Set os valores para os atributos auxiliares.
                ProcedimentosLiberacao.inicializarProcedimentos(Convert.ToDouble(ListInformacoesProposta[4]), Convert.ToDouble(ListInformacoesProposta[3]), proposta.ListObjItemProposta[0].Quantidade, proposta.Volumes);

                //Carrega o formulário com as informações que serão manusueadas para a proposta e o item da proposta
                //this.fillCamposForm(proposta.Numero, (string)proposta.RazaoCliente, proposta.Totalpecas, proposta.TotalItens, (string)proposta.ListObjItemProposta[0].Partnumber, (string)proposta.ListObjItemProposta[0].Descricao, (string)proposta.ListObjItemProposta[0].NomeLocalLote, proposta.ListObjItemProposta[0].Quantidade.ToString());
                frm.fillCamposForm(proposta);

                //Retorna o objeto proposta o qual terá suas informações trabalhadas do processo de conferencia do item.
                return proposta;
            }
            catch (ArithmeticException ex) 
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
