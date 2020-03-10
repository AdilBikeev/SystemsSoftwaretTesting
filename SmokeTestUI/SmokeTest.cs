using ClientChat;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Linq;

namespace SmokeTestUI
{
    [TestClass]
    public class SmokeTest
    {
        private bool status;
        private object locker = new object();

        [TestMethod]
        [Description("�������� ����������� ��������� � ��������� ����������")]
        public void OpenAndCloseApp()
        {
            Thread thread = new Thread(() =>
            {
                lock (locker)
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
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (!status)
            {
                Assert.Fail("�� ������� ������� ����");
            }
        }

        [TestMethod]
        [Description("������� �������� �������")]
        [DataRow("admin")]
        [DataRow("1")]
        [DataRow("")]
        [DataRow(" ")]
        public void TryChangeNickname(string nickname)
        {
            Thread thread = new Thread(() =>
            {
                lock (locker)
                {
                    MainWindow main = new MainWindow();
                    try
                    {
                        main.Show();
                        main.NICKNAME = nickname;

                        Assert.AreEqual(nickname, main.NICKNAME);

                        main.Close();
                        status = true;
                    }
                    catch (InvalidOperationException)
                    {
                        status = false;
                    }
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (!status)
            {
                Assert.Fail("�� ������� ������� ����");
            }
        }

        [TestMethod]
        [Description("������� �������� ���� ���������")]
        [DataRow("���������")]
        [DataRow("1")]
        [DataRow("")]
        [DataRow(" ")]
        public void TryChangeMessage(string msg)
        {
            Thread thread = new Thread(() =>
            {
                lock (locker)
                {
                    MainWindow main = new MainWindow();
                    try
                    {
                        main.Show();
                        main.MESSAGE = msg;

                        Assert.AreEqual(msg, main.MESSAGE);

                        main.Close();
                        status = true;
                    }
                    catch (InvalidOperationException)
                    {
                        status = false;
                    }
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (!status)
            {
                Assert.Fail("�� ������� ������� ����");
            }
        }

        [TestMethod]
        [Description("�������� ����������� title ���������� � ��������� ������ �� �������")]
        public void CheckStatus()
        {
            Thread thread = new Thread(() =>
            {
                lock (locker)
                {
                    MainWindow main = new MainWindow();
                    try
                    {
                        main.Show();

                        if (main.STATUS_DESC_PATH.Values.FirstOrDefault(x => x == main.Title) != null) status = true;

                        main.Close();
                    }
                    catch (InvalidOperationException)
                    {
                        status = false;
                    }
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (!status)
            {
                Assert.Fail("Title ����� ������������ ��������");
            }
        }
    }
}
