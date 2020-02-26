using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ClientChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MessageController messageController;

        public MainWindow()
        {
            InitializeComponent();
            messageController = new MessageController();
            _ = messageController.RunPeriodicallyAsync(this.UpdateMsg, new TimeSpan(0, 0, 0, 0, 300), CancellationToken.None);
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
                    this.Icon = new BitmapImage(new Uri(@"./../../../Content/StatusChat/online.jpg", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    this.Title = "Chat - Подключение к сети ...";
                    this.Icon = new BitmapImage(new Uri(@"./../../../Content/StatusChat/faild.jpg", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
