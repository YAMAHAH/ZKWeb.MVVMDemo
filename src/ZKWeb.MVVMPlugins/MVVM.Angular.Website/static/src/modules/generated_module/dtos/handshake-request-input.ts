/** 握手请求 */
export class HandshakeRequestInput {
    /** 数据加密私钥 */
    public SecretKey: string;
    /** 客户RSA公私 */
    public PublicKey: string;
}
