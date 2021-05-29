using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TI_Lab_2;

namespace TI_Lab_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        struct Message
        {
            public string Text { get; set; }
            public long Signature { get; set; }
        }

        Key key = new Key();
        Message message;

        public MainWindow()
        {
            InitializeComponent();
            tbMessage.IsEnabled = false;
            tbSignature.IsEnabled = false;
            bSignature.IsEnabled = false;
            bSend.IsEnabled = false;
        }

        private void bGenerateKeys_Click(object sender, RoutedEventArgs e)
        {
            key.CreateKeys();
            tbPublicKey.Text = (key.PublicKey).ToString() + ", " + (key.Module).ToString();
            tbPrivateKey.Text = (key.PrivateKey).ToString() + ", " + (key.Module).ToString();

            tbMessage.IsEnabled = true;
            tbSignature.IsEnabled = true;
            bSignature.IsEnabled = true;            
        }

        private void bSignature_Click(object sender, RoutedEventArgs e)
        {
            if (tbMessage.Text != "")
            {
                message.Text = tbMessage.Text;
                message.Signature = GenerateSignature(GenerateHash(message.Text), key.PrivateKey, key.Module);
                tbSignature.Text = message.Signature.ToString();
                bSend.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Enter your message", "Info", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
        }

        private long GenerateHash(string message)
        {
            long p = 17, q = 19, n, h = 100;
            n = p * q;
            for (int i = 0; i < message.Length; i++)
            {
                h = RSA.Fast_Exp(h + message[i], 2, n);
            }
            return h;
        }

        private long GenerateSignature(long hash, long privateKey, long module)
        {
            return RSA.Fast_Exp(hash, privateKey, module);
        }

        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            message.Text = tbMessage.Text;
            long hash = GenerateHash(message.Text);
            long hashView = GenerateSignature(message.Signature, key.PublicKey, key.Module);
            if (hash == hashView)
            {
                MessageBox.Show("All OK", "Info", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Something wrong", "Info", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
