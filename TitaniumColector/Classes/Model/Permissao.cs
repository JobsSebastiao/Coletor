using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    public class Permissao
    {
        public Int16 ValorUsuarioMetodo { get; private set; }
        public string MetodoMetodo { get; private set; }

        public Permissao(Int16 valorUsuarioMetodo,string metodoMetodo) 
        {
            this.ValorUsuarioMetodo = valorUsuarioMetodo;
            this.MetodoMetodo = metodoMetodo;
        }
    }
}
