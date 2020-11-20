using BuildingFormsWithBlazor.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace BuildingFormsWithBlazor.Services
{
    public class UserService
    {
        private const string mongoApi = "http://localhost:9799/api/";
        private static readonly List<User> Users;
        private readonly IServiceProvider services;
        private HttpClient http;
        public UserService(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            services = serviceProvider;
            http = httpClient;
        }
        static UserService()
        {

            Users = new List<User>
            {
                new User { FirstName = "John", LastName = "Doe", City = "London", Email = "johndoe@testemail.com", IsActive = true, DateOfBirth = new DateTime(1975, 01, 03) },
                new User { FirstName = "Alex", LastName = "Hales", City = "NewYork", Email = "alexhales@googlemail.com", IsActive = true, DateOfBirth = new DateTime(1955, 06, 11) },
                new User { FirstName = "Justin", LastName = "Li", City = "Shanghai", Email = "justinli@yahoo.com", IsActive = true, DateOfBirth = new DateTime(1983, 09, 04) },
                new User { FirstName = "Doug", LastName = "Lee", City = "Sydney", Email = "douglee@outlook.com", IsActive = false, DateOfBirth = new DateTime(2000, 07, 07) },
                new User { FirstName = "Rahul", LastName = "Mittal", City = "NewDelhi", Email = "rmittal@rediff.com", IsActive = true, DateOfBirth = new DateTime(1996, 11, 11) }
            };

        }

        public async Task<User[]> GetUsersAsync()
        {
             User[] todoItems = new User[0];
            bool hasunsavedrecords = false;
            bool somefailure = false;
            hasunsavedrecords = await HasUnSavedRecordsinIndexDB();

            if (hasunsavedrecords)
            {

                var localDataStore = services.GetRequiredService<IndexDbService>();
                todoItems = await localDataStore.GetAllAsync<User[]>("serverdata");
            }
            else
            {
                try
                {
                    todoItems = await http.GetFromJsonAsync<User[]>(mongoApi + "getallusers");
                    if (todoItems.Length > 0)
                    {

                        await ClearAll("serverdata");
                        foreach (var item in todoItems)
                        {
                            var localDataStore = services.GetRequiredService<IndexDbService>();
                            await localDataStore.PutAsync<User>("serverdata", null, item);
                        }
                    }
                }
                catch (Exception)
                {

                    somefailure = true;
                }

                finally {
                    if (somefailure)
                    {
                        var localDataStore = services.GetRequiredService<IndexDbService>();
                        todoItems = await localDataStore.GetAllAsync<User[]>("serverdata");
                    }
                }

            }

           
            return todoItems;
        }

        public async  Task  AddUser(User user)
        {
            bool isSuccessful = false;
            Users.Add(user);
            var localDataStore =  services.GetRequiredService<IndexDbService>();
            await localDataStore.SaveUser(user);
            try
            {
                var response = await http.PostAsJsonAsync(mongoApi+"createuser", user);
                isSuccessful = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't save the record");
              
            }
            finally
            {
                if (isSuccessful)
                {

                    await DeleteAsync("localdata", user.Email);
                }

            }

    
           


        }


        public async Task SynchronizeAsync()
        {
            // If there are local edits, always send them first
            foreach (var user in await GetLocalData())
            {
                try
                {
                    var api = mongoApi + "createuser";
                    Console.WriteLine(api);
                    (await http.PostAsJsonAsync(api, user)).EnsureSuccessStatusCode();
                    await DeleteAsync("localdata", user.Email);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Sync Failure");
                }
            
            }

  
            //await FetchChangesAsync();
        }

   

        private ValueTask ClearAll(string storename)
        {
            var localDataStore = services.GetRequiredService<IndexDbService>();
            return localDataStore.ClearAsync(storename);

        }
        private ValueTask DeleteAsync(string v, string email)
        {

            var localDataStore = services.GetRequiredService<IndexDbService>();
             return localDataStore.DeleteAsync(v, email);
        }

        public async Task<bool> HasUnSavedRecordsinIndexDB()
        {

            Console.WriteLine("Getting the local changes count");
            User[] result = await GetLocalData();
            Console.WriteLine(result.Length);
      

            return result.Length > 0;

        }

        private async Task<User[]> GetLocalData()
        {
            var localDataStore = services.GetRequiredService<IndexDbService>();
            var result = await localDataStore.GetLocalData();
            return result;
        }
    }


 
}
