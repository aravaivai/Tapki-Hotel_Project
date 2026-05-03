using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Zakharov_hotel
{
    public class Database
    {
        private readonly string supabaseUrl = "https://nwtneozbnrhqcntdedpv.supabase.co";
        private readonly string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im53dG5lb3pibnJocWNudGRlZHB2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzAxMTM3ODEsImV4cCI6MjA4NTY4OTc4MX0.OIcnF_SUmSP9h6IEPZqt7nGXrYBWmCJ9Nws7EyuiWRI";

        private HttpClient CreateClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("apikey", supabaseKey);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {supabaseKey}");
            return client;
        }

        public DataTable ExecuteQuery(string query)
        {
            try
            {
                return Task.Run(() => SendRequestAsync(query)).Result;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Ошибка БД: " + msg);
                return new DataTable();
            }
        }

        // 3. Метод для INSERT, UPDATE, DELETE
        public void ExecuteUpdate(string query)
        {
            try
            {
                Task.Run(() => SendRequestAsync(query)).Wait();
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Ошибка БД: " + msg);
            }
        }

        // Внутренний метод отправки запроса
        private async Task<DataTable> SendRequestAsync(string sqlQuery)
        {
            using (var client = CreateClient())
            {
                var payload = new { query = sqlQuery };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                string endpoint = $"{supabaseUrl}/rest/v1/rpc/exec_sql";

                var response = await client.PostAsync(endpoint, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Supabase вернул ошибку: {response.StatusCode}. Детали: {responseBody}");
                }

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(responseBody);
                return dt ?? new DataTable();
            }
        }
    }
}