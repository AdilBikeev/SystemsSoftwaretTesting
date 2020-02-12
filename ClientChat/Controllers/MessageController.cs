using System;
using System.Collections.Generic;
using System.Text;
using ClientChat.Hellpers;
using ClientChat.Hellpers.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClientChat.Controllers
{
    class MessageController
    {
        private string url;

        public MessageController ()
        {
            this.url = "localhost:8080/api/message";
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

            string res = HttpHellper.HttpSend(this.url, data, Method.GET, ContentType.JSON);

            if (res != string.Empty)
                return true;
            else
                return false;
        }
    }
}
