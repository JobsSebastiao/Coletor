using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Model;
using TitaniumColector.Forms;
using System.Drawing;
using System.IO;

namespace TitaniumColector.Classes.Procedimentos
{
    class ProcedimentosAlocacao : IDisposable
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
                this.FormPrincipal.Enabled = false;
                FrmInputAlocacao frmInput = new FrmInputAlocacao((EtiquetaAlocacao)etiqueta,this.FormPrincipal);
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

        public void alocarProduto(EtiquetaAlocacao etiquetaAlocar) 
        {
            if (etiquetaAlocar.JaAlocado)
            {
                if(this.etiquetaJaValidada(etiquetaAlocar))
                {
                    this.listEtiquetasAlocadas.Add(etiquetaAlocar);
                    this.listEtiquetas.Remove(etiquetaAlocar);
                }
            }
        }

    #region "Idisposable"

        private Stream _resource;
        private bool _disposed;

        public void Dispose()
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
}
