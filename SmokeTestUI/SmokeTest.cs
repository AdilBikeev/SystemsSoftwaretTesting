using ClientChat;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace SmokeTestUI
{
    [TestClass]
    public class SmokeTest
    {
        private bool status;

        [TestMethod]
        [Description("Проверка возмонжости открывать и закрывать приложение")]
        public void OpenAndCloseApp()
        {
            Thread thread = new Thread(() =>
            {
                MainWindow main = new MainWindow();
                try
                {
                    main.Show();
                    main.Close();
                    status = true;
                }
                catch (InvalidOperationException)
                {
                    status = false;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (!status)
            {
                Assert.Fail("Не удалось открыть окно");
            }
        }
    }
}
