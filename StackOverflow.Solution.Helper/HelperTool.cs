using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Helper
{
    public static class HelperTool
    {
        public static string Decrypt(string input)
        {
            return EncryptDecrypt(input, Config.EncryptionKey, EncryptMode.DECRYPT, Config.VectorKey);
        }

        public static string Encrypt(string input)
        {
            return EncryptDecrypt(input, Config.EncryptionKey, EncryptMode.ENCRYPT, Config.VectorKey);
        }
        public static string EncryptObject(object input)
        {
            var dtoJson = ConvertObjectToJson(input);
            return EncryptDecrypt(dtoJson, Config.EncryptionKey, EncryptMode.ENCRYPT, Config.VectorKey);
        }


        public static string ConvertObjectToJson(object input)
        {
            var jsonString = JsonConvert.SerializeObject(input);
            return jsonString;
        }

        public static T ConvertJsonToObject<T>(string input)
        {
            //JsonSerializer.Deserialize<Department>(jsonData);
            var jsonObject = JsonConvert.DeserializeObject<T>(input);
            return jsonObject;
        }



        private static string EncryptDecrypt(string inputText, string encryptionKey, EncryptMode mode, string vectorKey)
        {
            UTF8Encoding _enc;
            RijndaelManaged _rcipher;
            byte[] _key, _pwd, _ivBytes, _iv;

            _enc = new UTF8Encoding();
            _rcipher = new RijndaelManaged();
            _rcipher.Mode = CipherMode.CBC;
            _rcipher.Padding = PaddingMode.PKCS7;
            _rcipher.KeySize = 256;
            _rcipher.BlockSize = 128;
            _key = new byte[32];
            _iv = new byte[_rcipher.BlockSize / 8]; //128 bit / 8 = 16 bytes
            _ivBytes = new byte[16];

            string _out = "";// output string
            _pwd = Encoding.UTF8.GetBytes(encryptionKey);
            _ivBytes = Encoding.UTF8.GetBytes(vectorKey);
            int len = _pwd.Length;
            if (len > _key.Length)
            {
                len = _key.Length;
            }
            int ivLenth = _ivBytes.Length;
            if (ivLenth > _iv.Length)
            {
                ivLenth = _iv.Length;
            }
            Array.Copy(_pwd, _key, len);
            Array.Copy(_ivBytes, _iv, ivLenth);
            _rcipher.Key = _key;
            _rcipher.IV = _iv;
            if (mode.Equals(EncryptMode.ENCRYPT))
            {
                //encrypt
                byte[] plainText = _rcipher.CreateEncryptor().TransformFinalBlock(_enc.GetBytes(inputText), 0, _enc.GetBytes(inputText).Length);
                _out = Convert.ToBase64String(plainText);
            }
            if (mode.Equals(EncryptMode.DECRYPT))
            {
                //decrypt
                byte[] plainText = _rcipher.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(inputText), 0, Convert.FromBase64String(inputText).Length);
                _out = _enc.GetString(plainText);
            }
            _rcipher.Dispose();
            return _out;// return encrypted/decrypted string
        }
    }
}
