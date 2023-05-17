using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace frontend_WPF
{
    class ToDoList
    {
        public List<ToDo> Todos {get; set;}

        public async Task add(string title, string content) 
        {
            ToDo toDo = await createToDo(new ToDoDto{ Title = title, Content = content});
        }
        public async Task remove(int index)
        {
            if (Todos.Count > 0) await deleteToDo(Todos[index].Id);
        }
        public async Task markComplete(int index)
        {
            if(Todos.Count > 0) await SetComplete(Todos[index].Id);
        }
        public async Task alterData(int index, string title, string content)
        {
            if (Todos.Count > 0) {
                if (title == string.Empty && content != string.Empty) await ChangeData(Todos[index].Id, new ToDoDto { Title = Todos[index].Title, Content = content });
                else if (title != string.Empty && content == string.Empty) await ChangeData(Todos[index].Id, new ToDoDto { Title = title, Content = Todos[index].Content });
                else if (title != string.Empty && content != string.Empty) await ChangeData(Todos[index].Id, new ToDoDto { Title = title, Content = content });
            }
        }
        static async Task<ToDo> createToDo(ToDoDto dto)
        {
            using var client = new HttpClient();
            var jsonBody = JsonConvert.SerializeObject(dto);

            try
            {
                var response = await client.PostAsync("http://localhost:5161/api/todo", new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ToDo>(responseJson);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating todo: " + ex.Message);
            }
        }
        static async Task<bool> deleteToDo(Guid id)
        {
            using var client = new HttpClient();

            try
            {
                var response = await client.DeleteAsync($"http://localhost:5161/api/todo/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Delete request failed. Error: " + error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while sending the delete request: " + ex.Message);
            }
            return false;
        }
        static async Task<ToDo> SetComplete(Guid id)
        {
            using var client = new HttpClient();
            
            try
            {
                var response = await client.PatchAsync($"http://localhost:5161/api/todo/{id}/complete", null);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ToDo>(responseJson);
            }
            catch (Exception ex)
            {
                throw new Exception("Error marking todo complete: " + ex.Message);
            }
        }
        static async Task<ToDo> ChangeData(Guid id, ToDoDto dto)
        {
            using var client = new HttpClient();
            var jsonBody = JsonConvert.SerializeObject(dto);

            try
            {
                var response = await client.PatchAsync($"http://localhost:5161/api/todo/{id}", new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ToDo>(responseJson);
            }
            catch (Exception ex)
            {
                throw new Exception("Error changing todo data: " + ex.Message);
            }
        }
    }
}
