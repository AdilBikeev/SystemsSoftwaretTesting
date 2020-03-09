using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientChat.Controllers;
using Path = System.IO.Path;

namespace ClientChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum StatusChat
        {
            [Description("Соединение успешно установлено")]
            Online = 0,

            [Description("Попытка восстановить соединение")]
            Bad,

            [Description("Соединение разорвано")]
            Failed
        }

        private MessageController messageController;

        private readonly string currentPath = $"{Directory.GetCurrentDirectory()}/Content/StatusChat";

        private Dictionary<StatusChat, string> statusImagesPath = null;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                 messageController = new MessageController();
                _ = messageController.RunPeriodicallyAsync(this.UpdateMsg, new TimeSpan(0, 0, 0, 0, 300), CancellationToken.None);

                statusImagesPath = new Dictionary<StatusChat, string>()
                {
                    [StatusChat.Online] = Path.Combine(this.currentPath, "online.jpg"),
                    [StatusChat.Bad] = Path.Combine(this.currentPath, "bad.jpg"),
                    [StatusChat.Failed] = Path.Combine(this.currentPath, "failed.jpg"),
                };
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Open Application exception\n\n{exc.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void SendMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            if(this.nickNameTb.Text != string.Empty)
            {
                if(this.msgTb.Text != string.Empty)
                {
                    if(messageController.SendMessage(this.msgTb.Text, this.nickNameTb.Text))
                    {
                        this.msgTb.Text = "";
                    }else
                    {
                        MessageBox.Show("По техническим причинам сообщение не удалось доставить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Тело сообщения не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Укажите свой 'Никнейм'", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateMsg()
        {
            try
            {
                string res = await messageController.UpdateMessages();
                if (!string.IsNullOrEmpty(res))
                {
                    TextRange textRange = new TextRange(this.chatRtb.Document.ContentStart, this.chatRtb.Document.ContentEnd);
                    textRange.Text = res;
                    this.chatRtb.ScrollToEnd();
                    this.Title = "Chat - В сети";
                    this.Icon = new BitmapImage(new Uri(statusImagesPath[StatusChat.Online], UriKind.RelativeOrAbsolute));
                }
                else
                {
                    this.Title = "Chat - Подключение к сети ...";
                    this.Icon = new BitmapImage(new Uri(statusImagesPath[StatusChat.Failed], UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
