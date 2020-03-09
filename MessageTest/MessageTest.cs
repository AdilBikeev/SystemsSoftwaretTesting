using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientChat.Controllers;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Tests.Controllers.MessageTest
{
    [TestClass]
    public class MessageTest
    {
        /// <summary>
        /// Объект тестируемого контроллера
        /// </summary>
        private static MessageController messageController;

        [ClassInitialize()]
        public static void Init(TestContext testContext)
        {
            messageController = new MessageController();
        }

        [TestMethod]
        [Description("Тестирование отправки сообщения - проверяем, что не выпадают исключения")]
        [DataRow("", "")]
        [DataRow("", "testName")]
        [DataRow("testMessage", "")]
        [DataRow("testMessage", "testName")]
        [DataRow("", "1")]
        [DataRow("1", "")]
        [DataRow("1", "1")]
        public void SendMessage(string msg, string from)
        {
            Assert.IsInstanceOfType(messageController.SendMessage(msg, from), typeof(Boolean));
        }

        [TestMethod]
        [Description("Тестирование получения сообщений со стороны сервера - проверяем, что не выпадают исключения")]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(30)]
        [DataRow(60)]
        public async Task UpdateMessageAsync(int timeSleep)
        {
            try
            {
                Thread.Sleep(timeSleep * 1000);
                _ = await messageController.UpdateMessages();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Description("Проверка наличия файла конфигурации для MessageController")]
        [DataRow(1)]
        [DataRow(60)]
        public void InitConfig(int timeSleep)
        {
            try
            {
                Thread.Sleep(timeSleep * 1000);
                messageController.InitConfig();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
