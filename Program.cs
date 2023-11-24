using System;
using System.IO.Ports;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Net.Http.Headers;

class Program
{
    static string apiUrl = "http://estacioneja.somee.com/EstacioneJa/Vagas"; // Substitua pela URL correta da API
    static HttpClient httpClient;

    static void Main()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",  "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJhbmlsbWFyQGdtYWlsLmNvbSIsInJvbGUiOiIyIiwibmJmIjoxNjk4NDMyMzU4LCJleHAiOjE2OTg1MTg3NTgsImlhdCI6MTY5ODQzMjM1OH0.8mPq9le4kNN5tdVZpjLRS-C_sjZpYuJzsF0vQnCsfDllHmQDBvRgPkRx2FT0epuGj2l0sjlw7p3DjYSC2j01ow");

        using (SerialPort serialPort = new SerialPort("COM4", 9600)) // Substitua 'COM4' pela porta correta
        {
            serialPort.Open();

            while (true) // Loop externo para repetir o processo continuamente
            {
                for (int i = 0; i < 4; i++)
                {
                    string data = serialPort.ReadLine().Trim();
                    Console.WriteLine($"Dados recebidos do Arduino: {data}");

                    // Envie os dados para a API
                    SendDataToAPI(data);
                }

                serialPort.Close();

                // Aguarde um intervalo antes de abrir a porta serial novamente
                Thread.Sleep(5000); // Intervalo de 5 segundos

                // Abra a porta serial novamente para a próxima iteração
                serialPort.Open();
            }
        }
    }

    static void SendDataToAPI(string data)
    {
        var content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = httpClient.PutAsync(apiUrl, content).Result; // Espera sincronamente

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Dados enviados com sucesso para a API.");
        }
        else
        {
            Console.WriteLine("Falha ao enviar os dados para a API.");
        }
    }
}