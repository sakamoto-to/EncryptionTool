using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionTool
{
    public class EncryptionHelper
    {
        /// <summary>秘密鍵</summary>
        private string KEY = "testTobeAesKey01";
        /// <summary>初期化ベクトル</summary>
        private string VEC = "testTobeAesIv001";

        /// <summary>
        /// AES復合化
        /// </summary>
        /// <param name="str">復号化対象文字列</param>
        /// <returns>復号化済文字列</returns>
        public string Decrypt(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                string code = "";

                //対象を16進バイト列化
                byte[] data = hexString2Byte(str);

                //暗号用のキー情報をセットする
                byte[] aseKey = Encoding.UTF8.GetBytes(KEY);
                byte[] aseVec = Encoding.UTF8.GetBytes(VEC);

                //暗号化オブジェクトとストリームを作成する
                using (Aes aes = Aes.Create())
                {
                    aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(aseKey, aseVec),
                               CryptoStreamMode.Write))
                    {
                        // ストリームに複合化するデータを出力
                        cs.Write(data, 0, data.Length);
                        cs.Close();

                        // 複合化されたデータを取得
                        code = Encoding.UTF8.GetString(ms.ToArray());

                        ms.Close();
                    }
                }
                return code;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// AES暗号化し文字列を返す
        /// </summary>
        /// <param name="str">暗号化対象文字列</param>
        /// <returns>暗号化済文字列</returns>
        public string Encrypt(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                string code = "";

                //対象を16進バイト列化
                byte[] data = Encoding.UTF8.GetBytes(str);

                //暗号用のキー情報をセットする
                byte[] aseKey = Encoding.UTF8.GetBytes(KEY);
                byte[] aseVec = Encoding.UTF8.GetBytes(VEC);

                //暗号化オブジェクトとストリームを作成する
                using (Aes aes = Aes.Create())
                {
                    aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(aseKey, aseVec), CryptoStreamMode.Write))
                    {
                        // ストリームに暗号化するデータを出力
                        cs.Write(data, 0, data.Length);
                        cs.Close();

                        // 暗号化されたデータを取得
                        code = byte2HexString(ms.ToArray());
                        ms.Close();
                    }
                }
                return code;
            }
            catch (Exception)
            {
                return str;
            }
        }

        /// <summary>
        /// 16進数の文字列をバイト列に変換する
        /// </summary>
        /// <param name="data">対象文字列</param>
        /// <returns>バイト配列</returns>
        private byte[] hexString2Byte(string data)
        {

            int size = data.Length / 2;
            byte[] result = new byte[size];

            //2文字ずつ進み、バイトに変換
            for (int i = 0; i < size; i++)
            {
                string target = data.Substring(i * 2, 2);
                result[i] = Convert.ToByte(target, 16);
            }

            return result;
        }

        /// <summary>
        /// バイト列を16進数の文字列に変換する
        /// </summary>
        /// <param name="data">対象バイト配列</param>
        /// <returns>文字列</returns>
        private string byte2HexString(byte[] data)
        {
            //00-11-22形式を001122に変換
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}
