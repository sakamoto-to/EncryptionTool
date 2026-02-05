using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionTool
{
    public class EncryptionHelper
    {
        /// <summary>秘密鍵（AES用）</summary>
        private string KEY = "testTobeAesKey01";
        /// <summary>初期化ベクトル（AES用）</summary>
        private string VEC = "testTobeAesIv001";
        /// <summary>秘密鍵（DES用）</summary>
        private string DES_KEY = "testDes1";
        /// <summary>初期化ベクトル（DES用）</summary>
        private string DES_VEC = "testDes1";
        /// <summary>秘密鍵（TripleDES用）</summary>
        private string TRIPLE_DES_KEY = "testTobeTripleDes0123456";
        /// <summary>初期化ベクトル（TripleDES用）</summary>
        private string TRIPLE_DES_VEC = "testDes1";

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

        /// <summary>
        /// DES暗号化し文字列を返す
        /// </summary>
        /// <param name="str">暗号化対象文字列</param>
        /// <returns>暗号化済文字列</returns>
        public string EncryptDES(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = Encoding.UTF8.GetBytes(str);
                byte[] desKey = Encoding.UTF8.GetBytes(DES_KEY);
                byte[] desVec = Encoding.UTF8.GetBytes(DES_VEC);

                using (DES des = DES.Create())
                {
                    des.Mode = CipherMode.CBC;
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(desKey, desVec), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                        return byte2HexString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                return str;
            }
        }

        /// <summary>
        /// DES復号化
        /// </summary>
        /// <param name="str">復号化対象文字列</param>
        /// <returns>復号化済文字列</returns>
        public string DecryptDES(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = hexString2Byte(str);
                byte[] desKey = Encoding.UTF8.GetBytes(DES_KEY);
                byte[] desVec = Encoding.UTF8.GetBytes(DES_VEC);

                using (DES des = DES.Create())
                {
                    des.Mode = CipherMode.CBC;
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(desKey, desVec), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// TripleDES暗号化し文字列を返す
        /// </summary>
        /// <param name="str">暗号化対象文字列</param>
        /// <returns>暗号化済文字列</returns>
        public string EncryptTripleDES(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = Encoding.UTF8.GetBytes(str);
                byte[] tripleDesKey = Encoding.UTF8.GetBytes(TRIPLE_DES_KEY);
                byte[] tripleDesVec = Encoding.UTF8.GetBytes(TRIPLE_DES_VEC);

                using (TripleDES tripleDes = TripleDES.Create())
                {
                    tripleDes.Mode = CipherMode.CBC;
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, tripleDes.CreateEncryptor(tripleDesKey, tripleDesVec), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                        return byte2HexString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                return str;
            }
        }

        /// <summary>
        /// TripleDES復号化
        /// </summary>
        /// <param name="str">復号化対象文字列</param>
        /// <returns>復号化済文字列</returns>
        public string DecryptTripleDES(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = hexString2Byte(str);
                byte[] tripleDesKey = Encoding.UTF8.GetBytes(TRIPLE_DES_KEY);
                byte[] tripleDesVec = Encoding.UTF8.GetBytes(TRIPLE_DES_VEC);

                using (TripleDES tripleDes = TripleDES.Create())
                {
                    tripleDes.Mode = CipherMode.CBC;
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, tripleDes.CreateDecryptor(tripleDesKey, tripleDesVec), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// MD5ハッシュ生成
        /// </summary>
        /// <param name="str">ハッシュ化対象文字列</param>
        /// <returns>ハッシュ文字列</returns>
        public string GenerateMD5Hash(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = Encoding.UTF8.GetBytes(str);
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(data);
                    return byte2HexString(hash);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// SHA256ハッシュ生成
        /// </summary>
        /// <param name="str">ハッシュ化対象文字列</param>
        /// <returns>ハッシュ文字列</returns>
        public string GenerateSHA256Hash(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = Encoding.UTF8.GetBytes(str);
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(data);
                    return byte2HexString(hash);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// SHA512ハッシュ生成
        /// </summary>
        /// <param name="str">ハッシュ化対象文字列</param>
        /// <returns>ハッシュ文字列</returns>
        public string GenerateSHA512Hash(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = Encoding.UTF8.GetBytes(str);
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hash = sha512.ComputeHash(data);
                    return byte2HexString(hash);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Base64エンコード
        /// </summary>
        /// <param name="str">エンコード対象文字列</param>
        /// <returns>エンコード済文字列</returns>
        public string EncodeBase64(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = Encoding.UTF8.GetBytes(str);
                return Convert.ToBase64String(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Base64デコード
        /// </summary>
        /// <param name="str">デコード対象文字列</param>
        /// <returns>デコード済文字列</returns>
        public string DecodeBase64(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return string.Empty;

                byte[] data = Convert.FromBase64String(str);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
