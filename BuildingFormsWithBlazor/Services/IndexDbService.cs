using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.JSInterop;
using BuildingFormsWithBlazor.DataStructures;

namespace BuildingFormsWithBlazor.Services
{


    // To support offline use, we use this simple local data repository
    // instead of performing data access directly against the server.
    // This would not be needed if we assumed that network access was always
    // available.
    public class IndexDbService     
    {

        private readonly HttpClient httpClient;
        private readonly IJSRuntime js;

        public IndexDbService(HttpClient httpClient, IJSRuntime js)
        {
            this.httpClient = httpClient;
            this.js = js;
        }


        public ValueTask SaveUser(User user)
         => PutAsync("localdata", null, user);

        public ValueTask<User[]> GetLocalData() => GetAllAsync<User[]>("localdata");

      public  ValueTask<T>  GetAsync<T>(string storeName, object key)   => js.InvokeAsync<T>("indexDbUserStore.get", storeName, key);
       

        public ValueTask PutAsync<T>(string storeName, object key, T value)
       => js.InvokeVoidAsync("indexDbUserStore.put", storeName, key, value);

      public     ValueTask  DeleteAsync(string storeName, object key)
          => js.InvokeVoidAsync("indexDbUserStore.delete", storeName, key);

        public ValueTask ClearAsync(string storeName) => js.InvokeVoidAsync("indexDbUserStore.clear", storeName);

      public  ValueTask<T>  GetAllAsync<T>(string storeName)
      => js.InvokeAsync<T>("indexDbUserStore.getAll", storeName);
    }
}
