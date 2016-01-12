using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Dao;
using TitaniumColector.Classes.Model;

namespace TitaniumColector.Classes.Utility
{
     class GerenciadorPermissoesParametros
    {
        public List<Parametro> Parametros{ get; set; }
        public List<Permissao> Permissoes { get; set; }
        public List<String> ListCodigoParametro{ get;set; }
        public List<String> ListMetodosPermissoes {get; set; }

        public GerenciadorPermissoesParametros() {
            this.Parametros = new List<Parametro>();
            //incializa a lista com os parametro a serem mantidos no mobile.
            gerarListaCodigoParametro();
            gerarListaPermissoes();
        }

        //Adiciona um parametro a lista.
        public void addParametro(Parametro param) {
            this.Parametros.Add(param);
        }

        //Adiciona uma permissao a lista.
        public void addPermissao(Permissao permissao)
        {
            this.Permissoes.Add(permissao);
        }

        /// <summary>
        /// retorna um parametro solicitado.
        /// caso ele não exista na lista retorna null.
        /// </summary>
        /// <param name="codigoParametro">Codigo do Parametro a ser retornado</param>
        /// <returns>obj Parametros caso exista ou null</returns>
        public Parametro retornarParametro(String codigoParametro) 
        {
            foreach (var item in this.Parametros)
            {
                if (item.Codigo == codigoParametro)
                {
                    return item;
                }
            }

            return null;
        }

        public Permissao retornarPermissao(String metodoPermissao)
        {
            foreach (var item in this.Permissoes)
            {
                if (item.MetodoMetodo.Equals(metodoPermissao))
                {
                    return item;
                }
            }

            return null;
        }

         /// <summary>
         /// retorna um parametro solicitado.
         /// caso ele não exista na lista retorna null
         /// </summary>
        /// <param name="codigoParametro">Codigo do Parametro a ser retornado</param>
         /// <param name="listaParametros">list de parametros onde realizar a busca.</param>
         /// <returns></returns>
        public Parametro retornarParametro(String codigoParametro,List<Parametro> listaParametros)
        {
            foreach (var item in listaParametros)
            {
                if (item.Codigo == codigoParametro)
                {
                    return item;
                }
            }

            return null;
        }

        public void gerarListaCodigoParametro()
        {
            this.ListCodigoParametro = new List<string>();
            this.ListCodigoParametro.Add("ValidarSequencia");
        }

        public void gerarListaPermissoes()
        {
            this.ListMetodosPermissoes = new List<string>();
            this.ListMetodosPermissoes.Add("Guarda Volumes Mobile");
            this.ListMetodosPermissoes.Add("Liberacao Vendas Mobile");
        }

        public string stringPermissoes() 
        {
            var str = "\'";
            var count = this.ListMetodosPermissoes.Count;
            var i=0;

            foreach (var item in this.ListMetodosPermissoes)
            {
                if (i < count-1)
                {
                    str += item.ToString() + "\',\'";
                }
                else 
                {
                    str += item.ToString() + "\'";
                }
                
                i++;
            }

            return str;
        }

        public IList<Permissao> ListPermissoes(int codigoUsuario) 
        {
            var dao = new DaoPermissoes();
            return dao.listaPermissoes(codigoUsuario, this.stringPermissoes());
        }
     }
}
