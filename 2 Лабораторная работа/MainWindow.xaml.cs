using System;
using System.Windows;
using LW_2.Encryption;

namespace LW_2
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            InitializeComboBox();
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            RSA rsa;
            if (ComboBoxMode.SelectedIndex == 0)
            {
                rsa = new RSA(false, 0, 0, 0);
                var result = rsa.Encrypt(InputText.Text, rsa.e, rsa.r);

                TextBoxResult.Text = result;
                TextBoxSecretNumberP.Text = rsa.p.ToString();
                TextBoxSecretNumberQ.Text = rsa.q.ToString();
                TextBoxNumberR.Text = rsa.r.ToString();
                TextBoxPublicKey.Text = rsa.e.ToString();
                TextBoxPrivateKey.Text = rsa.d.ToString();
            }
            else
            {
                rsa = new RSA(true, GetSecretNumberP(), GetSecretNumberQ(), GetNumberE());
                var result = rsa.Encrypt(InputText.Text, rsa.e, rsa.r);

                TextBoxResult.Text = result;
                TextBoxNumberR.Text = rsa.r.ToString();
                TextBoxPublicKey.Text = rsa.e.ToString();
                TextBoxPrivateKey.Text = rsa.d.ToString();
            }
        }
        
        private void Decrypt(object sender, RoutedEventArgs e)
        {
            RSA rsa = new RSA(true, GetSecretNumberP(), GetSecretNumberQ(), GetNumberE());
            string[] cipherText = InputText.Text.Trim(' ').Split(' ');
            TextBoxResult.Text = rsa.Decrypt(cipherText, GetSecretNumberD(), rsa.r);
        }

        private long GetSecretNumberP()
        {
            return Convert.ToInt64(TextBoxSecretNumberP.Text);
        }

        private long GetSecretNumberQ()
        {
            return Convert.ToInt64(TextBoxSecretNumberQ.Text);
        }

        private long GetSecretNumberD()
        {
            return Convert.ToInt64(TextBoxPrivateKey.Text);
        }

        private long GetNumberE()
        {
            return Convert.ToInt64(TextBoxPublicKey.Text);
        }

        private void InitializeComboBox()
        {
            ComboBoxMode.SelectedIndex = 0;
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}