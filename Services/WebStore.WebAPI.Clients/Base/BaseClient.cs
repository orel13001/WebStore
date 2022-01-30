using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected HttpClient Http { get; }

        protected string Address { get; }

        protected BaseClient(HttpClient client, string Address)
        {
            Http = client;
            this.Address = Address;
        }


        protected T? Get<T>(string url) => GetAsync<T>(url).Result;
        protected async Task<T?> GetAsync<T> (string url, CancellationToken Cancel = default)
        {
            var response = await Http.GetAsync(url, Cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: Cancel);
        }


        protected HttpResponseMessage Post<T>(string url, T value) => PostAsync<T>(url, value).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T value, CancellationToken Cancel = default)
        {
            var response = await Http.PostAsJsonAsync(url, value, Cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T value) => PutAsync<T>(url, value).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T value, CancellationToken Cancel = default)
        {
            var response = await Http.PutAsJsonAsync(url, value, Cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }


        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
        {
            var response = await Http.DeleteAsync(url, Cancel).ConfigureAwait(false);
            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);  // Если есть финализатор, то для текущего объекта отключаем финализацию в сборщике мусора
        }

        //~BaseClient() => Dispose(false);


        protected bool _Dispose;
        protected virtual void Dispose(bool disposing)
        {
            if (_Dispose)
                return;
            _Dispose = true;

            if(disposing)
            {
                //освобождаем управляемые ресурсы
            }

            // освобождаем неуправляемые ресурсы
        }
    }
}
