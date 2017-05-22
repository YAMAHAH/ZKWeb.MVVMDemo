export class ClientDataModel {
    constructor(public SecretKey: string,
        public RsaPublickKey: string,
        public RsaPrivateKey: string,
        public ServerRsaPublicKey: string) {
    }
}