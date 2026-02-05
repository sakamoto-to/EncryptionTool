using System.Windows;
using System.Windows.Controls;

namespace EncryptionTool
{
    public partial class MainWindow : Window
    {
        private EncryptionHelper encryptionHelper;
        private string selectedAlgorithm = "AES";

        public MainWindow()
        {
            InitializeComponent();
            encryptionHelper = new EncryptionHelper();
        }

        private void AlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgorithmComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedAlgorithm = selectedItem.Tag.ToString() ?? "AES";
                
                // ハッシュアルゴリズムの場合は復号化セクションを非表示
                bool isHashAlgorithm = selectedAlgorithm == "MD5" || 
                                       selectedAlgorithm == "SHA256" || 
                                       selectedAlgorithm == "SHA512";
                
                if (DecryptionGroupBox != null)
                {
                    DecryptionGroupBox.Visibility = isHashAlgorithm ? Visibility.Collapsed : Visibility.Visible;
                }

                // ラベルの更新
                UpdateLabels();
            }
        }

        private void UpdateLabels()
        {
            if (PlainTextLabel == null || EncryptedTextLabel == null || 
                CipherTextLabel == null || DecryptedTextLabel == null ||
                EncryptButton == null || DecryptButton == null) return;

            switch (selectedAlgorithm)
            {
                case "MD5":
                case "SHA256":
                case "SHA512":
                    PlainTextLabel.Text = "ハッシュ化する文字列を入力してください：";
                    EncryptedTextLabel.Text = "ハッシュ結果：";
                    EncryptButton.Content = "ハッシュ化";
                    break;
                case "Base64":
                    PlainTextLabel.Text = "エンコードする文字列を入力してください：";
                    EncryptedTextLabel.Text = "エンコード結果：";
                    CipherTextLabel.Text = "デコードする文字列を入力してください：";
                    DecryptedTextLabel.Text = "デコード結果：";
                    EncryptButton.Content = "エンコード";
                    DecryptButton.Content = "デコード";
                    break;
                default:
                    PlainTextLabel.Text = "平文を入力してください：";
                    EncryptedTextLabel.Text = "暗号化結果：";
                    CipherTextLabel.Text = "暗号文を入力してください：";
                    DecryptedTextLabel.Text = "復号化結果：";
                    EncryptButton.Content = "暗号化";
                    DecryptButton.Content = "復号化";
                    break;
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string plainText = PlainTextBox.Text;
                
                if (string.IsNullOrEmpty(plainText))
                {
                    MessageBox.Show("処理する文字列を入力してください。", "入力エラー", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string result = selectedAlgorithm switch
                {
                    "AES" => encryptionHelper.Encrypt(plainText),
                    "DES" => encryptionHelper.EncryptDES(plainText),
                    "TripleDES" => encryptionHelper.EncryptTripleDES(plainText),
                    "MD5" => encryptionHelper.GenerateMD5Hash(plainText),
                    "SHA256" => encryptionHelper.GenerateSHA256Hash(plainText),
                    "SHA512" => encryptionHelper.GenerateSHA512Hash(plainText),
                    "Base64" => encryptionHelper.EncodeBase64(plainText),
                    _ => encryptionHelper.Encrypt(plainText)
                };

                EncryptedTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"処理中にエラーが発生しました：\n{ex.Message}", "エラー", 
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
                    MessageBox.Show("処理する文字列を入力してください。", "入力エラー", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string result = selectedAlgorithm switch
                {
                    "AES" => encryptionHelper.Decrypt(cipherText),
                    "DES" => encryptionHelper.DecryptDES(cipherText),
                    "TripleDES" => encryptionHelper.DecryptTripleDES(cipherText),
                    "Base64" => encryptionHelper.DecodeBase64(cipherText),
                    _ => encryptionHelper.Decrypt(cipherText)
                };

                DecryptedTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"処理中にエラーが発生しました：\n{ex.Message}", "エラー", 
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
