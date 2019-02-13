using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contratos_WCF
{
    public class Body_WCF
    {
        private string erro_conexao;
        public string Erro_conexao
        {
            get { return erro_conexao; }
            set { erro_conexao = value; }
        }

        private string user;
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        private string senha;
        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        //Inserção RNFN

        //Emissao Nota
        private string id_empresa;

        public string Id_empresa
        {
            get { return id_empresa; }
            set { id_empresa = value; }
        }
        private string num_nota;

        public string Num_nota
        {
            get { return num_nota; }
            set { num_nota = value; }
        }
        private byte tip_nota;

        public byte Tip_nota
        {
            get { return tip_nota; }
            set { tip_nota = value; }
        }
        private string qtd_itens;

        public string Qtd_itens
        {
            get { return qtd_itens; }
            set { qtd_itens = value; }
        }
        
        //Dispesa Realizada

        private string id_instituicao;

        public string Id_instituicao
        {
            get { return id_instituicao; }
            set { id_instituicao = value; }
        }
        private string totalg;

        public string Totalg
        {
            get { return totalg; }
            set { totalg = value; }
        }
        private byte parcela;

        public byte Parcela
        {
            get { return parcela; }
            set { parcela = value; }
        }
        private string id_programa;

        public string Id_programa
        {
            get { return id_programa; }
            set { id_programa = value; }
        }


    }
}