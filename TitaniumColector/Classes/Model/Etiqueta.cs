using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TitaniumColector.Classes.Dao;
using TitaniumColector.Classes.Model;
using System.IO;

namespace TitaniumColector.Classes 
{
    public class Etiqueta:IDisposable
    {
        
        //PROPRIEDADES EM COMUM ''
        public TipoCode TipoEtiqueta { get;  set; }
        public string DescricaoProdutoEtiqueta { get; set; }
        public string PartnumberEtiqueta { get; set; }
        public int CodigoLote { get; set; }
        public String LoteEtiqueta { get; set; }
        public string Xml { get; protected set; }
        public DateTime DataHoraValidacao { get; set; }
        public int Diferencial { get;  set; }
        
        //PROPRIEDADES DEVEM SER PASSADAS PARA A CLASSE ETIQUETAVENDA
        //public Int64 Ean13Etiqueta { get; set; }
        public Int32 SequenciaEtiqueta { get; set; }
        public Double QuantidadeEtiqueta { get; set; }
        public Int32 volumeEtiqueta;

        //private DaoProduto daoProduto;
        public enum TipoCode {INVALID=0,QRCODE=1,BARRAS=2 }

        public Int32 VolumeEtiqueta
        {
            get { return volumeEtiqueta; }
            set
            {
                if (value > 0)
                    volumeEtiqueta = value;
            }
        }

    #region "CONTRUTORES"

        public Etiqueta() { }

        public Etiqueta(String partnumber, String descricao, String lote, Int32 sequencia, Double quantidade, TipoCode tipoEtiqueta)
        {

            PartnumberEtiqueta = partnumber;
            DescricaoProdutoEtiqueta = descricao;
            LoteEtiqueta = lote;
            QuantidadeEtiqueta = quantidade;
            DataHoraValidacao = DateTime.Now;

            switch (tipoEtiqueta)
            {
                case TipoCode.QRCODE:

                    SequenciaEtiqueta = sequencia;
                    TipoEtiqueta = TipoCode.QRCODE;
                    break;

                case TipoCode.BARRAS:

                    SequenciaEtiqueta = 0;
                    TipoEtiqueta = TipoCode.BARRAS;
                    break;

                default:
                    break;
            }

        }

    #endregion

    #region "METODOS"

        public virtual Etiqueta.TipoCode validaInputValueEtiqueta(String inputValue) { return TipoCode.INVALID;}
        public virtual void realizaAcao(string inputText,Etiqueta.TipoCode tipoEtiqueta){}
        public virtual Etiqueta criarEtiqueta(Array arrayEtiqueta, Etiqueta.TipoCode tipoEtiqueta) { return new Etiqueta(); }
        public virtual string montarXml(){ return null;}
        public virtual bool buscarEtiqueta(List<Etiqueta> listEtiquetas)
        {
            switch (this.TipoEtiqueta)
            {
                case TipoCode.QRCODE:

                    foreach (var itemList in listEtiquetas.ToList<Etiqueta>())
                    {

                        if (itemList.Equals(this))
                        {
                            return true;
                        }
                    }
                    return false;

                case TipoCode.BARRAS:

                    foreach (Etiqueta itemList in listEtiquetas.ToList<Etiqueta>())
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

        /// <summary>
        /// Verifica se já existe um determinado Objeto Etiqueta em um list.
        /// </summary>
        /// <param name="etiqueta">Obj etiqueta que será buscado</param>
        /// <param name="ListEtiquetas">list do tipo Etiqueta onde derá feita a varredura</param>
        /// <returns>TRUE(Se o objeto existe na list)
        ///          FALSE (Se o objeto não existe na list)</returns>
        public static bool validarEtiqueta(Etiqueta etiqueta,List<Etiqueta> listEtiquetas) 
        {
            switch (etiqueta.TipoEtiqueta )
            {
                case TipoCode.QRCODE:

                    foreach (Etiqueta itemList in listEtiquetas.ToList<Etiqueta>())
                    {

                        if (itemList.Equals(etiqueta))
                        {
                            return true;
                        }
                    }
                    return false;

                case TipoCode.BARRAS:

                    foreach (Etiqueta itemList in listEtiquetas.ToList<Etiqueta>())
                    {
                        if (etiqueta.Equals(itemList))
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
             System.Type type = obj.GetType();

             return (obj == null || (type.BaseType != typeof(Etiqueta)))? false : true;
        }

        public override string ToString()
        {
            return String.Format("PNUMBER:{0}|DESCRICAO:{1}|LOTE:{2}|TIPO:{3}|HORA_VALIDACAO:{4}"
                , PartnumberEtiqueta, DescricaoProdutoEtiqueta, LoteEtiqueta,TipoEtiqueta,DataHoraValidacao.ToString());
        }

        public override int GetHashCode()
        {
            return this.SequenciaEtiqueta;
        }

    #region IDisposable Members

        private Stream _resource;
        private bool _disposed;

        public virtual void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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

    #endregion

    #region "Sob Visão"

    ///// <summary>
        ///// Verifica se a Etiqueta já foi lida.
        ///// </summary>
        ///// <returns>FALSE --> se a etiqueta for encontrada na list
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

    #endregion
}
