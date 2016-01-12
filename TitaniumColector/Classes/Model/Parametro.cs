using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Utility
{
    class Parametro
    {
        public String Codigo {get; set;}
        public String Descricao { get; set; }
        public String Valor { get; set; }
        public Int32 Auxiliar { get; set; }


        public Parametro(String codigo,String descricao,String valor,Int32 auxiliar) 
        {
            this.Codigo = codigo;
            this.Descricao = descricao;
            this.Valor = valor;
            this.Auxiliar = auxiliar;
        }
    }
}
