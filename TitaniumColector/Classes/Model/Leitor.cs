using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    public static class Leitor
    {
        /// <summary>
        /// Valida se a etiqueta é de um tipo válido esperado.
        /// </summary>
        /// <param name="inputValue">string lida pelo Leitor de codigo de barras</param>
        /// <returns> Tipo de etiqueta lido.</returns>
        public static Etiqueta.Tipo validaInputValueEtiqueta(String inputValue)
        {
            Etiqueta.Tipo tipoEtiqueta = Etiqueta.Tipo.INVALID;
            int inputLength = inputValue.Length;

            if (inputLength == 13)
            {
                tipoEtiqueta = Etiqueta.Tipo.BARRAS;
            }

            if (inputLength > 13 && (tipoEtiqueta == Etiqueta.Tipo.INVALID))
            {
                if (inputValue.Contains("PNUMBER:"))
                {
                    if (inputValue.Contains("DESCRICAO:"))
                    {
                        if (inputValue.Contains("EAN13:"))
                        {
                            if (inputValue.Contains("LOTE:"))
                            {
                                if (inputValue.Contains("SEQ:"))
                                {
                                    if (inputValue.Contains("QTD:"))
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

            if (inputLength > 13 && (tipoEtiqueta != Etiqueta.Tipo.INVALID))
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

            return tipoEtiqueta;
        }
    }
}
