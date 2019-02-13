using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Contratos_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string Status_Conexao();

        [OperationContract]
        string Login_Auten(Body_WCF Autenticacao);

        [OperationContract]
        string RNFN_Nota(Body_WCF RNFN_Body);

        [OperationContract]
        string RNFN_Dispesa(Body_WCF RNFN_Body_Dispesa);

        [OperationContract]
        List<string> RNFN_Pesquisa(string CNPJ);

        [OperationContract]
        List<string> CE_cadastro_empresa(string Query, string[] Coluna, string[] Filtros);

        [OperationContract]
        List<string> CE_cadastro_instituicao(string Query, string[] Coluna, string[] Filtros);

        [OperationContract]
        List<string> CE_dispesa_realizada(string Query, string[] Coluna, string[] Filtros);

        [OperationContract]
        List<string> CE_emissao_nota(string Query, string[] Coluna, string[] Filtros);

        [OperationContract]
        List<string> CE_pessoa_fisica(string Query, string[] Coluna, string[] Filtros);

        [OperationContract]
        List<string> CE_programa_instituicao(string Query, string[] Coluna, string[] Filtros);

        [OperationContract]
        List<string> CE_rpa(string Query, string[] Coluna, string[] Filtros);
    }
}
