using System;
using TitaniumColector.SqlServer;
using System.Text;
using TitaniumColector.Classes.Model;
using System.Collections.Generic;
using TitaniumColector.Classes.Utility;

namespace TitaniumColector.Classes
{
    public class Usuario
    {
        public int Codigo { get; set; }
        public int Pasta { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string NomeCompleto { get; set; }
        public statusUsuario StatusUsuario { get; set; }
        public statusLogin StatusLogin { get; set; }
        public StringBuilder Sql01 { get; set; }
        public IList<Permissao> Permissoes { get; set; }
        public Int64 CodigoAcesso { get; set; }

    #region "ENUMS" 

        public enum usuarioProperty { CODIGO = 1, PASTA = 2, NOME = 3, SENHA = 4, NOMECOMPLETO = 5, STATUSUSUARIO = 6 }
        public enum statusLogin { LOGADO = 0, NAOLOGADO = 1 }
        public enum statusUsuario { DESATIVADO = 0, ATIVO = 1 }

    #endregion

    #region "Contrutores"

        public Usuario()
        {
        }

        public Usuario(int codigoUsuario, int pastaUsuario, string nomeUsuario, string senhaUsuario, string nomeCompletoUsuario, statusUsuario statusUsuario)
        {
            Codigo = codigoUsuario;
            Pasta = pastaUsuario;
            Nome = nomeUsuario;
            Senha = senhaUsuario;
            NomeCompleto = nomeCompletoUsuario;
            StatusUsuario = statusUsuario;
            StatusLogin = statusLogin.NAOLOGADO;
        }

        public Usuario(Usuario user)
        {
            Codigo = user.Codigo;
            Pasta = user.Pasta;
            Nome = user.Nome;
            Senha = user.Senha;
            NomeCompleto = user.NomeCompleto;
            StatusUsuario = user.StatusUsuario;
            StatusLogin = user.StatusLogin;
        }

    #endregion

    #region "Métodos"

        public bool validaSenha(object obj, string strSenha)
        {

            bool retorno = false;

            if (obj == null || (obj.GetType() != typeof(Usuario)))
            {
                retorno = false;
            }
            else
            {
                if (strSenha == ((Usuario)obj).Senha)
                {
                    retorno = true;
                }
            }
            return retorno;
        }

        public bool validaNome(object obj, string strNome)
        {

            bool retorno = false;

            if (obj == null || (obj.GetType() != typeof(Usuario)))
            {
                retorno = false;
            }
            else
            {
                if (strNome == ((Usuario)obj).Nome)
                {
                    retorno = true;
                }
            }
            return retorno;
        }

        /// <summary>
        /// Valida usuário durante Login
        /// </summary>
        /// <param name="obj">Objeto a ser validado como Objeto Usuário</param>
        /// <param name="usuario">Valor do campo Usuario</param>
        /// <param name="senha">Valor do campo senha</param>
        /// <returns>Validação True
        ///          Nao validado False;</returns>
        public bool validaUsuario(object obj, string usuario, string senha)
        {
            bool retorno = false;
            if (Equals(obj))
            {
                if (validaNome(obj, usuario))
                {
                    if (validaSenha(obj, senha))
                    {
                        retorno = true;
                    }
                    else
                    {
                        retorno = false;
                    }
                }
                else
                {
                    retorno = false;
                }
            }
            else
            {
                retorno = false;
            }
            return retorno;
        }

        public override string ToString()
        {

            return " Código : " + Codigo + "\n" + Environment.NewLine +
                   " Pasta : " + Pasta + Environment.NewLine +
                   " Senha : " + Senha + Environment.NewLine +
                   " Nome : " + Nome + Environment.NewLine +
                   " Nome Completo :" + NomeCompleto + Environment.NewLine +
                   " Status Usuario :" + StatusUsuario;
        }

        public override bool Equals(object obj)
        {
            System.Type type = obj.GetType();
            if (obj == null || (type != typeof(Usuario)))
            {
                return false;
            }
            else
            {
                return Codigo == ((Usuario)obj).Codigo;
            }

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Registra o acesso do usuário na Tabela tb0207_Acessos.
        /// E define o status de Login do usuário como Usuario.StatusLogin.LOGADO OU NAOLOGADO
        /// </summary>
        /// <param name="user">Código do usuário.</param>
        /// <param name="tipodeAcao"> ENUM Usuario.StatusLogin da classe usuário</param>
        /// <returns>Retorna o código do acesso atual do usuário.</returns>
        public long registrarAcesso(Usuario user, Usuario.statusLogin tipodeAcao)
        {

            Int64 retorno = 0;
            this.StatusLogin = tipodeAcao;
            MainConfig.UserOn = user;
            Sql01 = new StringBuilder();

            switch (tipodeAcao)
            {
                case statusLogin.LOGADO:
                    Sql01.Length = 0;
                    //Insere o acesso e inicia a transação
                    Sql01.Append("INSERT INTO tb0207_Acessos (usuarioACESSO, maquinaACESSO)");
                    Sql01.Append(" VALUES (" + user.Codigo + ",'" + MainConfig.HostName + "')");
                    SqlServerConn.execCommandSql(Sql01.ToString());
                    break;
                case statusLogin.NAOLOGADO:
                    Sql01.Length = 0;
                    Sql01.Append("UPDATE tb0207_Acessos");
                    Sql01.Append(" SET encerradoACESSO = 1,horaencerramentoACESSO = getdate(),duracaoACESSO = DATEDIFF(MINUTE,horaaberturaACESSO,getDATE())");
                    Sql01.AppendFormat(" WHERE codigoACESSO = {0}", MainConfig.UserOn.CodigoAcesso);
                    SqlServerConn.execCommandSql(Sql01.ToString());
                    return 0;
                default:
                    break;
            }

            //Recupera o código do acesso
            Sql01.Length = 0;
            Sql01.Append("SELECT MAX(codigoACESSO) AS novoACESSO");
            Sql01.Append(" FROM tb0207_Acessos");
            System.Data.SqlClient.SqlDataReader dr = SqlServerConn.fillDataReader(Sql01.ToString());
            if ((dr.FieldCount > 0))
            {
                while ((dr.Read()))
                {
                    retorno = (Int32)dr["novoACESSO"];
                }
            }

            SqlServerConn.closeConn();
            dr.Close();
            return retorno;

        }

        public void registrarAcesso(Usuario.statusLogin tipodeAcao)
        {

            Int64 retorno = 0;
            this.StatusLogin = tipodeAcao;

            Sql01 = new StringBuilder();

            switch (tipodeAcao)
            {
                case statusLogin.LOGADO:
                    Sql01.Length = 0;
                    //Insere o acesso e inicia a transação
                    Sql01.Append("INSERT INTO tb0207_Acessos (usuarioACESSO, maquinaACESSO)");
                    Sql01.Append(" VALUES (" + this.Codigo + ",'" + MainConfig.HostName + "')");
                    SqlServerConn.execCommandSql(Sql01.ToString());
                    break;
                case statusLogin.NAOLOGADO:
                    Sql01.Length = 0;
                    Sql01.Append("UPDATE tb0207_Acessos");
                    Sql01.Append(" SET encerradoACESSO = 1,horaencerramentoACESSO = getdate(),duracaoACESSO = DATEDIFF(MINUTE,horaaberturaACESSO,getDATE())");
                    Sql01.AppendFormat(" WHERE codigoACESSO = {0}", MainConfig.UserOn.CodigoAcesso);
                    SqlServerConn.execCommandSql(Sql01.ToString());
                    //return 0;
                    break;
                default:
                    break;
            }

            //Recupera o código do acesso
            Sql01.Length = 0;
            Sql01.Append("SELECT MAX(codigoACESSO) AS novoACESSO");
            Sql01.Append(" FROM tb0207_Acessos");
            System.Data.SqlClient.SqlDataReader dr = SqlServerConn.fillDataReader(Sql01.ToString());
            if ((dr.FieldCount > 0))
            {
                while ((dr.Read()))
                {
                    retorno = (Int32)dr["novoACESSO"];
                }
            }

            SqlServerConn.closeConn();
            dr.Close();

            this.CodigoAcesso = retorno;
            //return retorno;

        }

        public void definePermissoes() 
        {
            var gerenciador = new GerenciadorPermissoesParametros();
            this.Permissoes = gerenciador.ListPermissoes(this.Codigo);
        }

        public bool temPermissoes() 
        {
            return this.Permissoes.Count > 0;
        }

    #endregion
        
    }
}
