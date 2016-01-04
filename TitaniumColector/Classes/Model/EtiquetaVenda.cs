using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Interface;

namespace TitaniumColector.Classes.Model
{
    public class EtiquetaVenda : Etiqueta
    {

        ///<summary>
        /// Recebe um array de strings referentes aos atributos do obj Etiqueta.
        /// retorna Um objeto do tipo Etiqueta
        /// </summary>
        /// <param name="array">Array de String referentes aos atributos de uma etiqueta</param>
        public static Etiqueta arrayToEtiqueta(Array array)
        {
            Etiqueta objEtiqueta = new Etiqueta();

            foreach (string item in array)
            {
                string strItem = item.Substring(0, item.IndexOf(":", 0));

                if (strItem == "PNUMBER")
                {
                    objEtiqueta.PartnumberEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                }
                else if (strItem == "DESCRICAO")
                {
                    objEtiqueta.DescricaoProdutoEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                }
                else if (strItem == "EAN13")
                {
                    objEtiqueta.Ean13Etiqueta = Convert.ToInt64(item.Substring(item.IndexOf(":", 0) + 1));
                }
                else if (strItem == "LOTE")
                {
                    objEtiqueta.LoteEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                }
                else if (strItem == "SEQ")
                {
                    objEtiqueta.SequenciaEtiqueta = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                }
                else if (strItem == "QTD")
                {
                    objEtiqueta.QuantidadeEtiqueta = Convert.ToDouble(item.Substring(item.IndexOf(":", 0) + 1));
                }
            }
            return objEtiqueta;
        }


        /// <summary>
        /// Verifica se a Etiqueta já foi lida.
        /// </summary>
        /// <returns>
        ///          FALSE --> se a etiqueta for encontrada na list
        ///          TRUE --> se a etiqueta ainda não foii lida.
        /// </returns>
        public static bool validaEtiquetaNaoLida(Etiqueta objEtiqueta, List<Etiqueta> listEtiquetas)
        {
            //Verifica se o List foi iniciado
            if (listEtiquetas != null)
            {
                if (listEtiquetas.Count == 0)
                {
                    return true;
                }
                else
                {
                    //Verifica se a etiqueta está na lista de etiquetas lidas.
                    if (validarEtiqueta(objEtiqueta, listEtiquetas))
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
                return true;
            }
        }
    }  
}
