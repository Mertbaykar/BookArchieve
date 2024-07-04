using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Book.Archieve.API.Filter
{
    public class ExceptionLoggerFilter : ExceptionFilterAttribute
    {

        private readonly ILogger<ExceptionLoggerFilter> _logger;

        public ExceptionLoggerFilter(ILogger<ExceptionLoggerFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            // serilog ile loglayalım
            _logger.LogError($"Tarih: {DateTime.Now} \nOrijinal/özel hata mesajı: {context.Exception.Message}\nHata Detayı: {context.Exception}");
        }
    }
}
