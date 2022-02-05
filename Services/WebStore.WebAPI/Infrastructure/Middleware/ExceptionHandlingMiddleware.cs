using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ExceptionHandlingMiddleware> _Logger;

        public ExceptionHandlingMiddleware (RequestDelegate Next, ILogger<ExceptionHandlingMiddleware> Logger)
        {
            _Next = Next;
            _Logger = Logger;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception error)
            {
                HandleException(context, error);
                throw;
            }
        }


        private void HandleException(HttpContext context, Exception Error)
        {
            _Logger.LogError(Error, "Ошибка при обработке запроса {0}", context.Request.Path);
        }
    }
}
