using System;

namespace SimpleEasy.Core.lib.Utils
{
    public static class HEXUtils
    {
        public static string ByteToHexString(byte[] bytes)
        {
            string result = "";
            // if (bytes != null)
            // {
            //     for (int i = 0; i < bytes.Length; i++)
            //     {
            //         result += bytes[i].ToString("X2");
            //     }
            // }
            foreach (var item in bytes)
            {
                //result += string.Format("{0:X2}", item);
                result += item.ToString("X2");
            }
            return result;
        }
        public static byte[] HexStringToByte(string hexString)
        {
            string[] byteStrings = hexString.Split(" ".ToCharArray());
            byte[] byteOuts = new byte[byteStrings.Length - 1];
            for (int i = 0; i < byteStrings.Length - 1; i++)
            {
                byteOuts[i] = Convert.ToByte(("0x" + byteStrings[i]));
            }
            return byteOuts;
        }
    }
}
