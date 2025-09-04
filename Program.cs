using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ExtractData
{
    internal class Program
    {
        #region Variáveis de Configuração

        public static string UserName
        {
            get; set;
        }
        public static string Modelo
        {
            get; set;
        }
        public static string ApiUrl
        {
            get; set;
        }
        public static string ApiKey
        {
            get; set;
        }

        #endregion

        static async Task Main(string[] args)
        {
            UserName = Environment.GetEnvironmentVariable("OPENROUTER_USERNAME") ?? "Extension Project";
            Modelo = Environment.GetEnvironmentVariable("OPENROUTER_MODEL") ?? "google/gemini-2.5-flash-image-preview:free";
            ApiUrl = Environment.GetEnvironmentVariable("OPENROUTER_APIURL") ?? "https://openrouter.ai/api/v1/chat/completions";
            ApiKey = Environment.GetEnvironmentVariable("OPENROUTER_APIKEY") ?? throw new Exception("OPENROUTER_APIKEY não definida.");

            Console.WriteLine("Inicializado com:");
            Console.WriteLine($"Modelo: {Modelo}");
            Console.WriteLine($"URL: {ApiUrl}");

            List<string> imagens = new List<string>();
            imagens.Add(@"C:\Users\dhonathan.campos\Downloads\f52aa10a-e01d-458e-8522-8bf976efb164.jpeg");

            //var result = await ExtrairTextoComPromptAsync("Que dia é hoje?");
            //Console.WriteLine(result.ToString());
            var resultimg = await ExtrairDadosComImagensAsync("Extraia o nome, cpf e a data de nascimento dessa imagem", imagens);
            Console.WriteLine(resultimg.ToString());
        }

        public static async Task<JObject> ExtrairTextoComPromptAsync(string prompt)
        {
            var payload = new
            {
                model = Modelo,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            return await EnviarRequisicaoAsync(payload);
        }

        public static async Task<JObject> ExtrairDadosComImagensAsync(string prompt, List<string> imagens)
        {
            var contentList = new List<object>();

            foreach (var item in imagens)
            {
                string base64 = Convert.ToBase64String(File.ReadAllBytes(item));
                contentList.Add(new
                {
                    type = "image_url",
                    image_url = new
                    {
                        url = $"data:image/jpeg;base64,{base64}"
                    }
                });
            }
            contentList.Add(new
            {
                type = "text",
                text = prompt + "\n\nRetorne os dados no seguinte formato JSON:\n" +
                       "{ \"nome\": \"\", \"data_nascimento\": \"\", \"cpf\": \"\" }"
            });

            var payload = new
            {
                model = Modelo,
                messages = new[]
                {
                    new { role = "user", content = contentList }
                }
            };

            return await EnviarRequisicaoAsync(payload);
        }

        public static async Task<JObject> ExtrairDadosComPdfAsync(string prompt, string pdfPath)
        {
            if (!File.Exists(pdfPath))
                throw new FileNotFoundException("Arquivo PDF não encontrado.", pdfPath);

            var pdfBase64 = Convert.ToBase64String(File.ReadAllBytes(pdfPath));

            var contentList = new List<object>
    {
        new
        {
            type = "file_url",
            file_url = new
            {
                url = $"data:application/pdf;base64,{pdfBase64}"
            }
        },
        new
        {
            type = "text",
            text = prompt + "\n\nRetorne os dados no seguinte formato JSON:\n" +
                   "{ \"nome\": \"\", \"data_nascimento\": \"\", \"cpf\": \"\", \"numero_documento\": \"\" }"
        }
    };

            var payload = new
            {
                model = Modelo,
                messages = new[]
                {
            new { role = "user", content = contentList }
        }
            };

            return await EnviarRequisicaoAsync(payload);
        }


        private static async Task<JObject> EnviarRequisicaoAsync(object payload)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
            client.DefaultRequestHeaders.Add("X-Title", "ExtractDataApp");

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync(ApiUrl, content).Result;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Erro: {response.StatusCode} - {responseBody}");
                return null;
            }

            return JObject.Parse(responseBody);
        }
    }
}
