namespace WebStore.Infrastructure.Middleware
{
 /// <summary>
 /// Класс тестового промежуточного ПО
 /// </summary>
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;

        public TestMiddleware(RequestDelegate Next)
        {
            _Next = Next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Обработка информации из context.Request

            var processing_task = _Next(context);//далее работает остальная часть конвейера

            //Выполнить какие-то действия параллельно асинхронно с остальной частью конвейера

            await processing_task; 

            //дообработка информации в context.Response
        }
    }
}
