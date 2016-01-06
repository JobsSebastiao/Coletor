﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    public class Leitor
    {
        //private Etiqueta objEtiqueta;

        /// <summary>
        /// Valida se a etiqueta é de um tipo válido esperado.
        /// </summary>
        /// <param name="inputValue">string lida pelo Leitor de codigo de barras</param>
        /// <returns> Tipo de etiqueta lido.</returns>
        public static Etiqueta.Tipo validaInputValueEtiqueta(String inputValue,Etiqueta etiqueta)
        {
            return etiqueta.validaInputValueEtiqueta(inputValue);
        }

        public static void realizarAcao(string inputText,Etiqueta etiqueta,Etiqueta.Tipo tipoEtiqueta) 
        {
            etiqueta.realizaAcao(inputText, tipoEtiqueta);
        }

        public static Etiqueta gerarEtiqueta(Etiqueta etiqueta,Array arrayStringToEtiqueta,Etiqueta.Tipo tipoEtiqueta)
        {
            return etiqueta.criarEtiqueta(arrayStringToEtiqueta,tipoEtiqueta);
        }
    }
}
