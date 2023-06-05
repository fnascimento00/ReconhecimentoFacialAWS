using System.Net;

namespace ReconhecimentoFacialAWS.ViewModels;

public class ErrorVM
{
    public HttpStatusCode StatusCode { get; set; }
    public string Acao { get; set; }
    public string Origem { get; set; }
    public string Mensagem { get; set; }
    public string RastreamentoPilha { get; set; }
}
