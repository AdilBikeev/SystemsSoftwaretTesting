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
            if(messageController.SendMessage(this.msgTb.Text, this.nickNameTb.Text))
            {
                MessageBox.Show("Сообщение успешно доставлено", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                this.msgTb.Text = "";
            }else
            {
                MessageBox.Show("По техническим причинам сообщение не удалось доставить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateMsg()
        {
            string res = await messageController.UpdateMessages();
            if( !string.IsNullOrEmpty(res))
            {
                TextRange textRange = new TextRange(this.chatRtb.Document.ContentStart, this.chatRtb.Document.ContentEnd);
                textRange.Text = res;
                this.Title = "Chat - В сети";
            } else
            {
                this.Title = "Chat - Связь с сервером потеряно";
            }
        }
    }
}
