using Client.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("1 - отправка сообщений, 2 - слушать сообщение, 3 - получить последние сообщения за 10 минут");
            Console.WriteLine("Введите цифру: ");

            int number = Convert.ToInt32(Console.ReadLine());

            if (number == 1)
            {
                await Send();
            }
            else if (number == 2)
            {
                await Listen();
            }
            else if (number == 3) 
            {
                await GetLastMessagesForTenMin();
            }

        }

        static async Task Listen()
        {
            HubConnection _hubConnection = new HubConnectionBuilder()
               .WithUrl("https://localhost:8081/messagehub")
               .WithAutomaticReconnect()
               .Build();

            var messageInput = new Message();

            _hubConnection.On<int, string, DateTime>("ReceiveMessage", (serialNumber, message, time) =>
            {
                Console.WriteLine($"{Convert.ToString(serialNumber)}: {message} - {time}");
            });

            await _hubConnection.StartAsync();

            Console.ReadKey();
        }

        static async Task GetLastMessagesForTenMin()
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("https://localhost:8081/Messages");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Ошибка: {response.StatusCode}");
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var messages = JsonSerializer.Deserialize<List<MessageInput>>(content);

            foreach (var message in messages)
            {
                Console.WriteLine($"{message.serialnumber}: {message.message} - {message.sendTime}");
            }

            Console.WriteLine("Все сообщения успешно прочитаны.");
            Console.ReadKey();
        }

        static async Task Send()
        {
            HttpClient client = new HttpClient();
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int serialNumber = 1;

            while (true) 
            {
                char[] buffer = new char[128];
                for (int i = 0; i < 128; i++)
                {
                    buffer[i] = chars[random.Next(chars.Length)];
                }

                Dictionary<string, string> credentials1 = new Dictionary<string, string>
                {
                    {"serialNumber", Convert.ToString(serialNumber)},
                    {"message", new string(buffer)}
                };

                var jsonAddGroup = JsonSerializer.Serialize(credentials1);
                var contentAddGroup = new StringContent(jsonAddGroup, Encoding.UTF8, "application/json");

                await client.PostAsync("https://localhost:8081/Messages", contentAddGroup);

                serialNumber++;

                Console.WriteLine("Сообщение отправлено!");

                Thread.Sleep(1000);
            }
        }

    }
}
