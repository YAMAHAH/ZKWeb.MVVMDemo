using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleEasy.Core.lib
{
    public static class AppConsts
    {
        /// <summary>
        /// 客户端默认的AES密钥
        /// </summary>
        public static string DefaultSecretKey = "99b3ad6e";
        /// <summary>
        /// 客户端的RSA公钥KEY
        /// </summary>
        public static string ClientPublicKey = "clientPublicKey";
        /// <summary>
        /// 客户端的AES密钥KEY
        /// </summary>
        public static string ClientSecretKey = "clientSecretKey";
        /// <summary>
        /// 客户端发过来的会话Id
        /// </summary>
        public static string SessionHeaderIn = "X-ZKWeb-SessionId";
        /// <summary>
        /// 发送给客户端的会话Id
        /// </summary>
        public static string SessionHeaderOut = "X-Set-ZKWeb-SessionId";

        public static string ClientDataKey = "ClientData";
    }
}
