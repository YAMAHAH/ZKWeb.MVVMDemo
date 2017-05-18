using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SimpleEasy.Core.lib.Utils
{
    public static class MD5Utils
    {
        //MD5
        public static string GetMD5(string inputString)
        {
            byte[] bytes_md5_out = GetMD5_Hash(inputString);
            string str_md5_out = BitConverter.ToString(bytes_md5_out);
            str_md5_out = str_md5_out.Replace("-", "");
            return str_md5_out;
        }

        public static byte[] GetMD5_Hash(string inputString)
        {
            MD5 md5 = MD5.Create();
            return md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(inputString));
        }


        public static string GetMD5(String input,string format="X2")
        {
            string md5Out = "";
            byte[] s = GetMD5_Hash(input);
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                md5Out = md5Out + s[i].ToString(format); //X2 X3 X4
            }
            return md5Out;
        }
    }
}
