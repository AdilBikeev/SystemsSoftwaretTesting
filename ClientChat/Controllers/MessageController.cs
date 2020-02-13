using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientChat.Hellpers;
using ClientChat.Hellpers.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClientChat.Controllers
{
    class MessageController
    {
        /// <summary>
        /// Url адрес севрера
        /// </summary>
        private string url;

        public MessageController ()
        {
            this.url = "localhost:8080";
        }

        public async Task RunPeriodicallyAsync(
            Func<Task> func,
            TimeSpan interval,
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(interval, cancellationToken);
                await func();
            }
        }

        /// <summary>
        /// Отправляет сообщение в общий чат
        /// </summary>
        /// <param name="msg">Сообщение для отправки</param>
        /// <param name="from">Никнейм отправвителя</param>
        /// <returns>Возвращает true в случаи успешной отправки сообщения</returns>
        public bool SendMessage(string msg, string from)
        {
            JObject data = new JObject
            {
                ["username"] = from,
                ["message"] = msg
            };

            string res = HttpHellper.HttpSend($"{ this.url}/api/messages", data, Method.POST, ContentType.JSON);

            if (res != string.Empty)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Обновляет сообщения чата
        /// </summary>
        /// <param name="messages">Сообщения чата</param>
        /// <returns>true - если удалось достучаться до сервера</returns>
        public async Task<string> UpdateMessages()
        {
            string messages = await HttpHellper.HttpSendAsync($"{ this.url}/api/messages", null, Method.GET, ContentType.JSON);

            if (messages != string.Empty)
                return messages;
            else
                return string.Empty;
        }
    }
}
