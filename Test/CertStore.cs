



using Bnnsoft.Sdk;
using Newtonsoft.Json;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class CertStore : IDisposable
    {
        private readonly string appid;
        private readonly string secret;
        private readonly string region;
        private SignClient signClient;
        private bool _disposed;

        public CertStore(string appid, string secret, string region = "demo")
        {
            this.appid = appid;
            this.secret = secret;
            this.region = region;
        }

        public CertStore(SignClient signClient)
        {
            this.signClient = signClient;
        }

        public X509Certificate getX509()
        {

            if (signClient == null)
            {
                signClient = new SignClient(appid, secret, "sign", region);
            }

            var responseString = signClient.UploadString("/api/v2/account/cert", "POST", "");

            var verifyResult = JsonConvert.DeserializeObject<ApiResp>(responseString);
            if (verifyResult.status == 0)
            {
                var certdata = Convert.FromBase64String(verifyResult.obj.ToString());
                return new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(certdata);
            }


            return null;
        }

        public X509Certificate[] getX509Chain()
        {

            if (signClient == null)
            {
                signClient = new SignClient(appid, secret, "sign", region);
            }

            var responseString = signClient.UploadString("/api/v2/account/chain", "POST", "");
            var chainRs = JsonConvert.DeserializeObject<ApiResp>(responseString);
            if (chainRs.status == 0)
            {
                ChainJson chain = JsonConvert.DeserializeObject<ChainJson>(chainRs.obj.ToString());
               
                //ChainJson chain = JsonConvert.DeserializeObject<ChainJson>(chainRs.obj.ToString());
                //return chain.toArray();
            }
            return null;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        /// <param name="disposing">Flag indicating whether managed resources should be disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                // Dispose managed objects
                if (disposing)
                {
                    signClient.Dispose();
                }

                // Dispose unmanaged objects

                _disposed = true;
            }
        }
    }

}
