using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Model;

namespace TitaniumColector.Classes.Procedimentos
{
    class ProcedimentosAlocacao
    {
        private List<Etiqueta> listEtiquetas;
        private List<Etiqueta> listEtiquetasAlocadas;
        private Etiqueta etiquetaProduto;
        private Array inputStringToEtiqueta;

    #region "SingleTon"

        private static ProcedimentosAlocacao instancia;

        private ProcedimentosAlocacao() 
        {
            listEtiquetas = new List<Etiqueta>();
            listEtiquetasAlocadas = new List<Etiqueta>();
        }

        public static ProcedimentosAlocacao Instanciar
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ProcedimentosAlocacao();
                }
                return instancia;
            }

        }

     #endregion

        public void realizarAcao(string inputText,Etiqueta.Tipo tipoEtiqueta)
        {
            switch (tipoEtiqueta)
            {
                case Etiqueta.Tipo.INVALID:

                    inputText = string.Empty;
                    mostrarMensagem(" Tipo de Etiqueta inválida!!!","Guardar Volumes", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    break;

                case Etiqueta.Tipo.QRCODE:

                    //this.liberarItem(inputText, tipoEtiqueta);
                    //MONTA UM ARRAY DE STRING COM AS INFORMACOES PASSADAS NO INPUTTEXT
                    inputStringToEtiqueta = FileUtility.arrayOfTextFile(inputText, FileUtility.splitType.PIPE);
                    etiquetaProduto = Leitor.gerarEtiqueta(new EtiquetaAlocacao(),inputStringToEtiqueta,tipoEtiqueta);
                    inputText = string.Empty;
                    break;

                case Etiqueta.Tipo.BARRAS:
                    inputText = string.Empty;
                    mostrarMensagem("Não é possível validar etiqueta do tipo Codigo de Barras!!!", "Guardar Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                default:

                    inputText = string.Empty;
                    mostrarMensagem("Não é possível validar etiqueta do tipo Codigo de Barras!!!", "Guardar Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        public static void mostrarMensagem(string mensagem,string caption,MessageBoxButtons msgButton,MessageBoxIcon msgIcon)
        {
            MessageBox.Show(mensagem,caption,msgButton,msgIcon,MessageBoxDefaultButton.Button2);
        }

    }
}
