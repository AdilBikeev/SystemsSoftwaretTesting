using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ClientChat.Hellpers.Http
{
    public static class Method
    {
        public const string POST = "POST";
        public const string GET = "GET";
    }

    public static class ContentType
    {
        public const string JSON = "application/json";
    }

    static class HttpHellper
    {

        /// <summary>
        /// Производит Http запрос
        /// </summary>
        /// <param name="url">URL-адрес куда кидать запрос</param>
        /// <param name="data">Тело запроса</param>
        /// <param name="method">Тип запроса</param>
        /// <param name="contentType">Тип передаваемых данных</param>
        /// <returns></returns>
        public static string HttpSend(string url, JObject data, string method, string contentType)
        {
            string res = string.Empty;

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = method; // для отправки используется метод Post
                                         // данные для отправки

                // преобразуем данные в массив байтов
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data.ToString());

                // устанавливаем тип содержимого - параметр ContentType
                request.ContentType = contentType;

                // Устанавливаем заголовок Content-Length запроса - свойство ContentLength
                request.ContentLength = byteArray.Length;

                //записываем данные в поток запроса
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        res = reader.ReadToEnd();
                    }
                }

                response.Close();
            }
            catch (Exception exc)
            {
                //res = exc.Message;
            }

            return res;
        }
    }
}
