using System;
using System.Drawing;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using TitaniumColector.Classes;
using TitaniumColector.Classes.Utility;
using TitaniumColector.Classes.Dao;

namespace TitaniumColector
{   
    public static class MainConfig
    {

    #region "PROPRIEDADES"

        private static string strVersaoSO;
        public static string HostName { get; set; }
        public static string DeviceIp { get; set; }
        public static Size ScreenSize { get; set; }
        public static Size ClienteSize { get; set; }
        public static Usuario UserOn { get; set; }
        public static GerenciadorPermissoesParametros Permissoes_TB1210 { get; set; }

        //Fontes Utiizadas no sistema.
        public static Font FontPequenaRegular { get; private set; }
        public static Font FontPequenaBold { get; private set; }
        public static Font FontPequenaItalic { get; private set; }
        public static Font FontPequenaItalicBold { get; private set; }

        public static Font FontPadraoRegular { get; private set; }
        public static Font FontPadraoBold { get; private set; }
        public static Font FontPadraoItalic { get; private set; }
        public static Font FontPadraoItalicBold { get; private set; }

        public static Font FontMediaBold { get; private set; }
        public static Font FontMuitoGrandeBold { get; private set; }

        public static Font FontGrandeRegular { get; private set; }
        public static Font FontGrandeBold { get; private set; }
        public static Font FontGrandeItalic { get; private set; }
        public static Single Tamanho { get; private set; }
        public static FontStyle FontStyle { get; private set; }
        
        //Contantes
        public const int intPositionX = 0;
        public const int intPositionY = 0;
        public const Char PasswordChar = '*';

        //Permissões do sistema.
        public static Boolean ValidarSequenciaEtiqueta{get; set;}

    #endregion

    #region "GET & SETS"

        public static string VersaoSO
        {
            get {  return strVersaoSO != null ? strVersaoSO : "ND"; }

            set
            {
                if (value != null)
                {
                    strVersaoSO = value.Trim();
                }
            }
        }

    #endregion

    #region "MÉTODOS DE CONFIGURAÇÃO"

        public static void setMainConfigurations()
        {
            defineScreenSize();
            capturaVersãoSo();
            capturaHostName();
            capturaIp();
            defineFontPequena();
            defineFontMedia();
            defineFontPadrao();
            defineFontGrande();
            defineFontMuitoGrande();
        }

        private static void defineScreenSize() 
        {
            Size size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            ScreenSize = (size);
        }

        public static void defineClienteSize(Size size)
        {
            ClienteSize = (size);
        }

        private static void capturaVersãoSo()
        {
            VersaoSO  = Environment.OSVersion.ToString();
        }

        private static void capturaHostName()
        {
           string hostName = System.Net.Dns.GetHostName();
           HostName = hostName;
        }

        private static void capturaIp()
        {
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(HostName);
            IPAddress addr = ipEntry.AddressList[ipEntry.AddressList.Length-1];
            DeviceIp = addr.ToString() ;
        }

        public static void recuperaPermissoes() 
        {
            DaoPermissoes daoPermissoes = new DaoPermissoes();
            Permissoes_TB1210 = new GerenciadorPermissoesParametros();
            Permissoes_TB1210 = daoPermissoes.recuperarParametros(Permissoes_TB1210.ListCodigoParametro);
        }

        private static void defineFontPadrao()
        {
            //REGULAR
            Tamanho = new Single();
            Tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Regular;
            FontPadraoRegular = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //BOLD
            Tamanho = new Single();
            Tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontPadraoBold = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //ITALIC
            Tamanho = new Single();
            Tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Italic;
            FontPadraoItalic = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //ITALIC & BOLD
            Tamanho = new Single();
            Tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold | FontStyle.Italic;
            FontPadraoItalicBold = new System.Drawing.Font("Arial", Tamanho, FontStyle);

        }

        private static void defineFontMedia()
        {
            //BOLD
            Tamanho = new Single();
            Tamanho = 12F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontMediaBold = new System.Drawing.Font("Arial", Tamanho, FontStyle);

        }

        private static void defineFontMuitoGrande()
        {
            //BOLD
            Tamanho = new Single();
            Tamanho = 31F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontMuitoGrandeBold = new System.Drawing.Font("Arial", Tamanho, FontStyle);

        }

        private static void defineFontPequena()
        {
            //REGULAR
            Tamanho= new Single();
            Tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Regular;
            FontPequenaRegular = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //ITALIC
            Tamanho= new Single();
            Tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Italic;
            FontPequenaItalic = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //BOLD
            Tamanho= new Single();
            Tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontPequenaBold = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //ITALIC & BOLD
            Tamanho = new Single();
            Tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold | FontStyle.Italic ;
            FontPequenaItalicBold = new System.Drawing.Font("Arial", Tamanho, FontStyle);

        }

        private static void defineFontGrande()
        {
            //REGULAR 
            Tamanho= new Single();
            Tamanho = 20F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Regular;
            FontGrandeRegular = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //BOLD
            Tamanho= new Single();
            Tamanho = 20F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontGrandeBold = new System.Drawing.Font("Arial", Tamanho, FontStyle);

            //Italic
            Tamanho= new Single();
            Tamanho = 20F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Italic;
            FontGrandeItalic = new System.Drawing.Font("Arial", Tamanho, FontStyle);
        }

    #endregion

    #region "METODOS GERAIS"
        /// <summary>
        /// Recupera a largura e altura de uma string.
        /// </summary>
        /// <param name="text">string a ser analizada</param>
        /// <param name="font">Tipo de font utilizada na string</param>
        /// <returns></returns>
        public static SizeF sizeStringEmPixel(string text,Font font)
        {
            SizeF size = new SizeF();
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
            {
               size = graphics.MeasureString(text ,font);
            }
            return size;
        }

        /// <summary>
        /// Carregar Combox a partir de um List carregado com um tipo de objeto.
        /// </summary>
        /// <param name="cb">ComboBox a ser preenchida</param>
        /// <param name="objectList">List preenchida com um tipo de Objeto</param>
        /// <param name="displayName">parâmetro do objeto a ser mostrado na ComboBox</param>
        /// <param name="columnName">parâmetro que terá o seu Valor utilizado na ComboBox. </param>
        public static void carregarComboBox(System.Windows.Forms.ComboBox cb, List<object> objectList, string displayName, string columnName)
        {
            cb.Items.Clear();
            cb.DataSource = objectList;
            cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cb.DisplayMember = displayName;
            cb.ValueMember = columnName;
            cb.SelectedItem = null;
        }

        /// <summary>
        /// Recupera o controle que está com o focus ativo.
        /// </summary>
        /// <param name="parent">Formulário ao qual pertence o controle</param>
        /// <returns>Controle no qual o foco está ativo.</returns>
        public static Control GetFocusedControl(Control parent)
        {
            if (parent.Focused)
            {
                return parent;
            }
            foreach (Control ctrl in parent.Controls)
            {
                Control temp = GetFocusedControl(ctrl);
                if (temp != null)
                {
                    return temp;
                }
            }
            return null;

        }

        /// <summary>
        /// Exibe uma mensagem padronizada ao usuário.
        /// </summary>
        /// <param name="msg">Informação a ser passada</param>
        /// <param name="head">Cabeçalho da caixa de texto.</param>
        public static void errorMessage(String msg,String head) 
        {
            System.Windows.Forms.MessageBox.Show(msg, head, System.Windows.Forms.MessageBoxButtons.OK,
                               System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1);
         }

        // Create a bitmap object and fill it with the specified color.   
        // To make it look like a custom image, draw an ellipse in it.
        public static Bitmap MakeBitmap(Color color, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(color), 0, 0, bmp.Width, bmp.Height);
            g.DrawEllipse(new Pen(Color.DarkGray), 3, 3, width - 6, height - 6);
            g.Dispose();

            return bmp;
        }

        /// <summary>
        /// Valida o numero a ser exibido no formulário tratando a Quantidade de campos após a virgula.
        /// </summary>
        /// <param name="value">Valoe a ser tratado</param>
        /// <returns>String no formato decimal ou inteiro</returns>
        public static String  intOrDecimal(int value)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
            String retorno = "";
            if (Convert.ToDouble(value) % 1 == 0)
            {
                retorno = String.Format(culture, "{0:0} Pçs", value);
            }
            else
            {
                retorno = String.Format(culture, "{0:0.00} Pçs", value);
            }

            return retorno;
        }

        /// <summary>
        /// Valida o numero a ser exibido no formulário tratando a Quantidade de campos após a virgula.
        /// </summary>
        /// <param name="value">Valoe a ser tratado</param>
        /// <returns>String no formato decimal ou inteiro</returns>
        public static String  intOrDecimal(double value)
        {

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
            String retorno = "";
            if (Convert.ToDouble(value) % 1 == 0)
            {
                retorno = String.Format(culture, "{0:0} Pçs", value);
            }
            else
            {
                retorno = String.Format(culture, "{0:0.00} Pçs", value);
            }

            return retorno;
        }

        /// <summary>
        /// Valida o numero a ser exibido no formulário tratando a Quantidade de campos após a virgula.
        /// </summary>
        /// <param name="value">Valoe a ser tratado</param>
        /// <returns>String no formato decimal ou inteiro</returns>
        public static String  intOrDecimal(String value)
        {

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");

            if (Convert.ToDouble(value) % 1 == 0)
            {
                value = String.Format(culture, "{0:0}", value);
            }
            else
            {
                value = String.Format(culture, "{0:0.000}", Convert.ToDouble(value));
            }

            return value;
        }
    #endregion

    }
}
