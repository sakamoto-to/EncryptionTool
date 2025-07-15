using System.Windows;
using System.Windows.Controls;

namespace EncryptionTool
{
    public partial class MainWindow : Window
    {
        private EncryptionHelper encryptionHelper;

        public MainWindow()
        {
            InitializeComponent();
            encryptionHelper = new EncryptionHelper();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string plainText = PlainTextBox.Text;
                
                if (string.IsNullOrEmpty(plainText))
                {
                    MessageBox.Show("暗号化する文字列を入力してください。", "入力エラー", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string encryptedText = encryptionHelper.Encrypt(plainText);
                EncryptedTextBox.Text = encryptedText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"暗号化中にエラーが発生しました：\n{ex.Message}", "エラー", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cipherText = CipherTextBox.Text;
                
                if (string.IsNullOrEmpty(cipherText))
                {
                    MessageBox.Show("復号化する文字列を入力してください。", "入力エラー", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string decryptedText = encryptionHelper.Decrypt(cipherText);
                DecryptedTextBox.Text = decryptedText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"復号化中にエラーが発生しました：\n{ex.Message}", "エラー", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            PlainTextBox.Text = "";
            EncryptedTextBox.Text = "";
            CipherTextBox.Text = "";
            DecryptedTextBox.Text = "";
        }

        private void CopyEncryptedButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(EncryptedTextBox.Text))
            {
                Clipboard.SetText(EncryptedTextBox.Text);
                MessageBox.Show("暗号化文字列をクリップボードにコピーしました。", "コピー完了", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("コピーする暗号化文字列がありません。", "コピーエラー", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CopyDecryptedButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DecryptedTextBox.Text))
            {
                Clipboard.SetText(DecryptedTextBox.Text);
                MessageBox.Show("復号化文字列をクリップボードにコピーしました。", "コピー完了", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("コピーする復号化文字列がありません。", "コピーエラー", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
