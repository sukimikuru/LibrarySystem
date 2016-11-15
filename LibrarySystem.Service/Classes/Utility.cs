using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Configuration;
using System.Reflection;

using System.IO;
using System.Security.Cryptography;
using System.Text;

using LibrarySystem.Service.Classes;

namespace LibrarySystem.Service.Classes
{
    public class Utility
    {
        private static Dictionary<long, int> _rkCache = new Dictionary<long, int>();
        private static object _locker = new object();

        /// <summary>
        /// 分页最大返回行数
        /// </summary>
        public static int PageMaxCount = 50000;




        public static SQLiteConnection GetConn()
        {
            string constr = "";
            constr = "Data Source=" + ConfigurationManager.AppSettings["DataBasePath"] + ";Version=3";
            return new SQLiteConnection(constr);
        }

        public static void LogError(string position, string input, string msg)
        { }


        /// <summary>        
        /// DateTime时间格式转换为Unix时间戳格式        
        /// </summary>        
        /// <param name=”time”></param>        
        /// <returns></returns>       
        public static long CreateRowKey()
        {
            lock (_locker)
            {
                long row_key = DateTime.Now.Ticks;
                while (_rkCache.ContainsKey(row_key))
                {
                    row_key = DateTime.Now.Ticks;
                }
                _rkCache.Add(row_key, 0);
                int maxCount = 100000;
                if (_rkCache.Count >= maxCount)
                {
                    List<long> keyList = _rkCache.Keys.ToList();
                    for (int i = keyList.Count - 1; i >= keyList.Count / 2; --i)
                    {
                        _rkCache.Remove(keyList[i]);
                    }
                    if (!_rkCache.ContainsKey(row_key))
                    {
                        _rkCache.Add(row_key, 0);
                    }
                }
                return row_key;
            }
        }










        #region 加解密相关
        /// <summary>
        /// Rijndael加密（可解密）
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string RijndaelEncrypt(string sourceString)
        {
            string str = "";
            string keyStr = "oseage_cxxx_2015";
            byte[] keyIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            using (MemoryStream mStream = new MemoryStream())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(keyStr);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(sourceString);
                Rijndael rij = Rijndael.Create();
                using (CryptoStream cStream = new CryptoStream(mStream, rij.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write))
                {
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    str = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return str.Replace("+", "%2B");
        }
        /// <summary>
        ///  Rijndael解密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string RijndaelDecrypt(string sourceString)
        {
            sourceString = sourceString.Replace(' ', '+');
            string keyStr = "oseage_cxxx_2015";
            byte[] keyIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            using (MemoryStream mStream = new MemoryStream())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(keyStr);
                byte[] inputByteArray = Convert.FromBase64String(sourceString);
                Rijndael rij = Rijndael.Create();
                using (CryptoStream cStream = new CryptoStream(mStream, rij.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write))
                {
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
        }
        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA256Encrypt(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.Unicode.GetBytes(str));
            s256.Clear();
            return Convert.ToBase64String(byte1);
        }
        #endregion
    }
}
