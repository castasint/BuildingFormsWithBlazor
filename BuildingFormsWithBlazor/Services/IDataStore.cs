using System.Threading.Tasks;

namespace BuildingFormsWithBlazor.Services
{
    public interface IDataStore
    {
        ValueTask<T> GetAsync<T>(string storeName, object key);
        ValueTask PutAsync<T>(string storeName, object key, T value);
        ValueTask DeleteAsync(string storeName, object key);
        ValueTask<T> GetAllAsync<T>(string storeName);



    }
}
