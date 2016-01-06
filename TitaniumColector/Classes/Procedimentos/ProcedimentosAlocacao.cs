using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Model;
using TitaniumColector.Forms;
using System.Drawing;

namespace TitaniumColector.Classes.Procedimentos
{
    class ProcedimentosAlocacao
    {
        public List<Etiqueta> listEtiquetas { get; set; }
        public  List<Etiqueta> listEtiquetasAlocadas { get; set; }
        private Etiqueta etiquetaProduto;
        private Array inputStringToEtiqueta;
        //nao uso ainda
        public Form FormPrincipal { get; set; }

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

                    //MONTA UM ARRAY DE STRING COM AS INFORMACOES PASSADAS NO INPUTTEXT
                    inputStringToEtiqueta = FileUtility.arrayOfTextFile(inputText, FileUtility.splitType.PIPE);
                    //GERA UM OBJETO ETIQUETA DO TIPO QUE FOI PASSADO NO PRIMEIRO PÂRAMETRO 
                    etiquetaProduto = Leitor.gerarEtiqueta(new EtiquetaAlocacao(),inputStringToEtiqueta,tipoEtiqueta);
                    //VALIDA A INCLUSÃO OU ALOCAÇÃO DA ETIQUETA;
                    this.trabalhaEtiqueta(etiquetaProduto);

                    inputText = string.Empty;
                    break;

                default:

                    inputText = string.Empty;
                    mostrarMensagem("Não é possível validar a etiqueta lida!!!", "Guardar Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void trabalhaEtiqueta(Etiqueta etiqueta) 
        {
            if (!etiquetaJaValidada(etiqueta))
            {
                this.listEtiquetas.Add(etiqueta);
            }
            else 
            {
                FrmInputAlocacao frmInput = new FrmInputAlocacao();
                frmInput.Show();
            }
        }

        /// <summary>
        /// Verifica se a etiqueta já existe na lista de Etiquetas
        /// </summary>
        /// <param name="etiqueta">Etiqueta a ser validada</param>
        /// <returns>true se ela existe.</returns>
        private bool etiquetaJaValidada(Etiqueta etiqueta) 
        {
            foreach (var item in listEtiquetas )
            {
                if (item.Equals(etiqueta))
                {
                    return true;
                }
            }
            return false;
        }

        private void ordenarList(List<EtiquetaAlocacao> list) 
        {
            list.Sort();
        }

        public static void mostrarMensagem(string mensagem, string caption, MessageBoxButtons msgButton, MessageBoxIcon msgIcon)
        {
            MessageBox.Show(mensagem, caption, msgButton, msgIcon, MessageBoxDefaultButton.Button2);
        }
    }
}
