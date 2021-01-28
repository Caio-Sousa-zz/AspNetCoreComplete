using Microsoft.AspNetCore.Mvc.Razor;
using System;
using DevIO.Business.Models;

namespace DevIO.App.Extensions
{
    public static class RazorExtensions
    {
        public static string FormatDocument(this RazorPage page, TipoFornecedor tipoFornecedor, string documento)
        {
            return tipoFornecedor == TipoFornecedor.PessoaFisica? 
                   Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00"):
                   Convert.ToUInt64(documento).ToString(@"00\.000\.000\/000\-00");
        }
    }
}