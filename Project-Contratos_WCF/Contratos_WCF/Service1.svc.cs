using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Odbc;

namespace Contratos_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string Status_Conexao()
        {
            Ender_Banco Conexao = new Ender_Banco();
            string Valid = null;
            string Query="select * from dispesa_realizada;";
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                command.ExecuteNonQuery();
            }
            catch (OdbcException Erro)
            {
                Valid = Erro.Message;
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
        public string Login_Auten(Body_WCF Autenticacao)
        {
            Ender_Banco Conexao = new Ender_Banco();
            string Query = "select tip_senha from autentic where user=? and senha=?;";
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.Parameters.AddWithValue("@user", Autenticacao.User);
                command.Parameters.AddWithValue("@senha", Autenticacao.Senha);
                command.ExecuteNonQuery();
                OdbcDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == null)
                    {
                        Query = "Usuário ou Senha Invalido..";
                    }
                    else
                    {
                        Query = reader.GetString(0);
                    }
                }
            }
            catch (OdbcException ex)
            {
                return ex.Message;
            }
            finally
            {
                Data_Conection.Close();
            }
            return Query;
        }

        //Inserção de dados

        public string RNFN_Nota(Body_WCF RNFN_Body)
        {
            Ender_Banco Conexao = new Ender_Banco();
            string Query = "insert into emissao_nota (id_empresa,num_nota,tip_nota,qtd_itens,totalg,data)"+
                            "values(?,?,?,?,?,curdate());";
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            string Valid = null;
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                command.Parameters.AddWithValue("@id_empresa", RNFN_Body.Id_empresa);
                command.Parameters.AddWithValue("@num_nota", RNFN_Body.Num_nota);
                command.Parameters.AddWithValue("@tip_nota", RNFN_Body.Tip_nota);
                command.Parameters.AddWithValue("@qtd_itens",RNFN_Body.Qtd_itens);
                command.Parameters.AddWithValue("@totalg", RNFN_Body.Totalg);
                command.ExecuteNonQuery();
            }
            catch (OdbcException Erro)
            {
                Valid = Erro.Message;
                if (Valid.Contains("trigger because it is already"))
                {
                    Valid = "Dado Existente";
                }
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
        public string RNFN_Dispesa(Body_WCF RNFN_Body_Dispesa)
        {
            Ender_Banco Conexao = new Ender_Banco();
            string Query = "insert into dispesa_realizada (id_empresa, id_instituicao,id_programa,parcela,num_nota,data)"+
                           "values (?,?,?,?,?,curdate());";
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            string Valid = null;
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                command.Parameters.AddWithValue("@id_empresa", RNFN_Body_Dispesa.Id_empresa);
                command.Parameters.AddWithValue("@id_instituicao", RNFN_Body_Dispesa.Id_instituicao);
                command.Parameters.AddWithValue("@id_programa", RNFN_Body_Dispesa.Id_programa);
                command.Parameters.AddWithValue("@parcela", RNFN_Body_Dispesa.Parcela);
                command.Parameters.AddWithValue("@num_nota", RNFN_Body_Dispesa.Num_nota);
                command.ExecuteNonQuery();
            }
            catch (OdbcException Erro)
            {
                Valid = Erro.Message;
                if (Valid.Contains("trigger because it is already"))
                {
                    Valid = "Dado Existente";
                }
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }

        //Consulta
        public List<string> RNFN_Pesquisa(string CNPJ)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            string Query = "select programa_instituicao.nome_programa,programa_instituicao.id_instituicao from "+
                           "programa_instituicao,"+
                           "cadastro_instituicao "+
                           "where cadastro_instituicao.cnpj=? "+ 
                           "and programa_instituicao.id_programa = cadastro_instituicao.id_instituicao;";
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                command.Parameters.AddWithValue("@cadastro_instituicao.cnpj", CNPJ);
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetString(0));
                    Valid.Add(Reader.GetInt64(1).ToString());
                }
            }
            catch (OdbcException Erro)
            {
                Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }

        //Consulta de Entidade
        public List<string> CE_cadastro_empresa(string Query, string[] Coluna, string[] Filtros)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                for(int cont=0;cont<Coluna.Count();cont++)
                {
                command.Parameters.AddWithValue("@"+Coluna[cont], Filtros[cont]);
                }
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetInt16(0).ToString());
                    Valid.Add(Reader.GetString(1).ToString());
                    Valid.Add(Reader.GetString(2).ToString());
                    Valid.Add(Reader.GetString(3).ToString());
                    Valid.Add(Reader.GetString(4).ToString());
                    Valid.Add(Reader.GetDate(5).ToString("yyyy-MM-dd"));
                }
            }
            catch (OdbcException Erro)
            {
                Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
        public List<string> CE_cadastro_instituicao(string Query, string[] Coluna, string[] Filtros)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                for (int cont = 0; cont < Coluna.Count(); cont++)
                {
                    command.Parameters.AddWithValue("@" + Coluna[cont], Filtros[cont]);
                }
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetInt64(0).ToString());
                    Valid.Add(Reader.GetString(1).ToString());
                    Valid.Add(Reader.GetString(2).ToString());
                    Valid.Add(Reader.GetString(3).ToString());
                    Valid.Add(Reader.GetInt16(4).ToString());
                    Valid.Add(Reader.GetDate(5).ToString("yyyy-MM-dd"));
                }
            }
            catch (OdbcException Erro)
            {
                 Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;

        }
        public List<string> CE_dispesa_realizada(string Query, string[] Coluna, string[] Filtros)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                for (int cont = 0; cont < Coluna.Count(); cont++)
                {
                    command.Parameters.AddWithValue("@" + Coluna[cont], Filtros[cont]);
                }
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetInt64(0).ToString());
                    Valid.Add(Reader.GetString(1).ToString());
                    Valid.Add(Reader.GetString(2).ToString());
                    Valid.Add(Reader.GetString(3).ToString());
                    Valid.Add(Reader.GetInt16(4).ToString());
                    Valid.Add(Reader.GetInt64(5).ToString());
                    Valid.Add(Reader.GetDate(6).ToString("yyyy-MM-dd"));
                }
            }
            catch (OdbcException Erro)
            {
                Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
        public List<string> CE_emissao_nota(string Query, string[] Coluna, string[] Filtros)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                for (int cont = 0; cont < Coluna.Count(); cont++)
                {
                    command.Parameters.AddWithValue("@" + Coluna[cont], Filtros[cont]);
                }
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetString(0).ToString());
                    Valid.Add(Reader.GetInt64(1).ToString());
                    Valid.Add(Reader.GetInt16(2).ToString());
                    Valid.Add(Reader.GetInt16(3).ToString());
                    Valid.Add(Reader.GetDouble(4).ToString());
                    Valid.Add(Reader.GetDate(5).ToString("yyyy-MM-dd"));
                }
            }
            catch (OdbcException Erro)
            {
                Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
        public List<string> CE_pessoa_fisica(string Query, string[] Coluna, string[] Filtros)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                for (int cont = 0; cont < Coluna.Count(); cont++)
                {
                    command.Parameters.AddWithValue("@" + Coluna[cont], Filtros[cont]);
                }
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetInt16(0).ToString());
                    Valid.Add(Reader.GetString(1).ToString());
                    Valid.Add(Reader.GetString(2).ToString());
                    Valid.Add(Reader.GetString(3).ToString());
                    Valid.Add(Reader.GetString(4).ToString());
                    Valid.Add(Reader.GetDate(5).ToString("yyyy-MM-dd"));
                }
            }
            catch (OdbcException Erro)
            {
                Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
        public List<string> CE_programa_instituicao(string Query, string[] Coluna, string[] Filtros)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                for (int cont = 0; cont < Coluna.Count(); cont++)
                {
                    command.Parameters.AddWithValue("@" + Coluna[cont], Filtros[cont]);
                }
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetInt16(0).ToString());
                    Valid.Add(Reader.GetInt16(1).ToString());
                    Valid.Add(Reader.GetString(2).ToString());
                }
            }
            catch (OdbcException Erro)
            {
                Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
        public List<string> CE_rpa(string Query, string[] Coluna, string[] Filtros)
        {
            Ender_Banco Conexao = new Ender_Banco();
            List<string> Valid = new List<string>();
            OdbcConnection Data_Conection = new OdbcConnection(Conexao.Conexao_Banco);
            OdbcCommand command = new OdbcCommand(Query, Data_Conection);
            try
            {
                Data_Conection.Open();
                command.CommandText = Query;
                for (int cont = 0; cont < Coluna.Count(); cont++)
                {
                    command.Parameters.AddWithValue("@" + Coluna[cont], Filtros[cont]);
                }
                command.ExecuteNonQuery();
                OdbcDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Valid.Add(Reader.GetString(0).ToString());
                    Valid.Add(Reader.GetInt64(1).ToString());
                    Valid.Add(Reader.GetDouble(2).ToString());
                    Valid.Add(Reader.GetDouble(3).ToString());
                    Valid.Add(Reader.GetDate(4).ToString("yyyy-MM-dd"));
                }
            }
            catch (OdbcException Erro)
            {
                Valid.Add(Erro.Message);
            }
            finally
            {
                Data_Conection.Close();
            }
            return Valid;
        }
    }
}
