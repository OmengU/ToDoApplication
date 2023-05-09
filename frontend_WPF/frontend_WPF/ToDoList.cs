using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace frontend_WPF
{
    class ToDoList
    {
        public List<ToDo> Todos {get; set;}

        public async void add(string title, string content) 
        {
            ToDo toDo = await createToDo(new ToDoDto { Title = title, Content = content });
            Todos.Add(toDo);
        }
        static async Task<ToDo> createToDo(ToDoDto dto)
        {
            using var client = new HttpClient();
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
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
    }
}
