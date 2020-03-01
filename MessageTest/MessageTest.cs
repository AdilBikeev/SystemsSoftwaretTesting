using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientChat.Controllers;
using System;

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
    }
}
