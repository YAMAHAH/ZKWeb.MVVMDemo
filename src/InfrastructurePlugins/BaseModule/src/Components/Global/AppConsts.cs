namespace InfrastructurePlugins.BaseModule.Components.Global
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
        public static string SessionContextKey = "ZKWeb.Session";
        /// <summary>
        /// 发送给客户端的会话Id
        /// </summary>
        public static string SessionHeaderOut = "X-Set-ZKWeb-SessionId";

        public static string ClientDataKey = "ClientData";

        //openssl genrsa -out rsa_1024_priv.pem 1024
        public static readonly string ServerRSAPrivateKey = @"MIICXgIBAAKBgQC0xP5HcfThSQr43bAMoopbzcCyZWE0xfUeTA4Nx4PrXEfDvybJ
                                                      EIjbU/rgANAty1yp7g20J7+wVMPCusxftl/d0rPQiCLjeZ3HtlRKld+9htAZtHFZ
                                                      osV29h/hNE9JkxzGXstaSeXIUIWquMZQ8XyscIHhqoOmjXaCv58CSRAlAQIDAQAB
                                                      AoGBAJtDgCwZYv2FYVk0ABw6F6CWbuZLUVykks69AG0xasti7Xjh3AximUnZLefs
                                                      iuJqg2KpRzfv1CM+Cw5cp2GmIVvRqq0GlRZGxJ38AqH9oyUa2m3TojxWapY47zye
                                                      PYEjWwRTGlxUBkdujdcYj6/dojNkm4azsDXl9W5YaXiPfbgJAkEA4rlhSPXlohDk
                                                      FoyfX0v2OIdaTOcVpinv1jjbSzZ8KZACggjiNUVrSFV3Y4oWom93K5JLXf2mV0Sy
                                                      80mPR5jOdwJBAMwciAk8xyQKpMUGNhFX2jKboAYY1SJCfuUnyXHAPWeHp5xCL2UH
                                                      tjryJp/Vx8TgsFTGyWSyIE9R8hSup+32rkcCQBe+EAkC7yQ0np4Z5cql+sfarMMm
                                                      4+Z9t8b4N0a+EuyLTyfs5Dtt5JkzkggTeuFRyOoALPJP0K6M3CyMBHwb7WsCQQCi
                                                      TM2fCsUO06fRQu8bO1A1janhLz3K0DU24jw8RzCMckHE7pvhKhCtLn+n+MWwtzl/
                                                      L9JUT4+BgxeLepXtkolhAkEA2V7er7fnEuL0+kKIjmOm5F3kvMIDh9YC1JwLGSvu
                                                      1fnzxK34QwSdxgQRF1dfIKJw73lClQpHZfQxL/2XRG8IoA==".Replace("\n", "");

        ////openssl rsa -pubout -in rsa_1024_priv.pem -out rsa_1024_pub.pem
        public static readonly string ServerRSAPublicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC0xP5HcfThSQr43bAMoopbzcCy
                                                     ZWE0xfUeTA4Nx4PrXEfDvybJEIjbU/rgANAty1yp7g20J7+wVMPCusxftl/d0rPQ
                                                     iCLjeZ3HtlRKld+9htAZtHFZosV29h/hNE9JkxzGXstaSeXIUIWquMZQ8XyscIHh
                                                     qoOmjXaCv58CSRAlAQIDAQAB".Replace("\n", "");
    }
}
