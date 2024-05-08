using Bnn.SignLib;
using Bnnsoft.Sdk;
using iTextSharp.text.pdf.security;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Properties;
using Org.BouncyCastle.X509;

namespace Test
{
    public class PdfHash : IDisposable
    {
        private readonly PdfReader _pdfReader;
        private bool _disposed;
        string default_font = "segoeui.tff";
        public PdfHash(byte[] pdfinputBytes)
        {
            _pdfReader = new PdfReader(pdfinputBytes);
        }

        public byte[] getHash(Org.BouncyCastle.X509.X509Certificate cer, ICollection<X509Certificate> chain, IByteSigner signer, string sHash, int typeSignature, string base64Image, string textOut, string font, string signatureName, int pageSign = 1,
            int xPoint = 100, int yPoint = 200, int width = 300, int height = 400)
        {
            PdfStamper stp = null;
            AcroFields af = _pdfReader.AcroFields;
            var signatureNameNames = af.GetSignatureNames();
            var signatureNumber = signatureNameNames.Count + 1;
            MemoryStream baos = new MemoryStream();

            if (signatureNumber > 1)
            {

                stp = PdfStamper.CreateSignature(_pdfReader, baos, '\0', null, true);
            }
            else
            {
                stp = PdfStamper.CreateSignature(_pdfReader, baos, '\0');
            }

            var sap = stp.SignatureAppearance;
            if (af.GetFieldItem(signatureName) == null)
            {
                Rectangle rectangle1 = new Rectangle((float)xPoint, (float)yPoint, (float)width, (float)height);
                sap.SetVisibleSignature(rectangle1, pageSign, signatureName);
            }
            else
            {
                sap.SetVisibleSignature(signatureName);
            }

            sap.Certificate = cer;
            sap.SignDate = DateTime.Now;

            switch (typeSignature)
            {
                case 1:
                    {
                        byte[] bytePng = Convert.FromBase64String(base64Image);
                        Image image = Image.GetInstance(bytePng);
                        sap.Image = image;
                        sap.Acro6Layers = true;
                        sap.Layer2Text = "";
                        break;
                    }
                case 2:
                    {

                        if (font == null)
                        {
                            font = default_font;
                            BaseColor colorSign = new BaseColor(0, 128, 0);
                            var bytes = File.ReadAllBytes("../segoeui.ttf");
                            BaseFont bf = BaseFont.CreateFont(font, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, true, bytes, null);
                            sap.Layer2Font = new Font(bf, 9, Font.NORMAL, colorSign);
                        }
                        string noidung = "Ký bởi: " + textOut + "\n";
                        noidung += "Ký ngày: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        sap.Layer2Text = noidung;
                        //sap.Layer2Text.PadLeft(100);
                        break;
                    }
                case 3:
                    {

                        if (font != null)
                        {

                        }

                        Font f = null;
                        if (String.Compare(font.ToLower(), "segoeui.ttf") == 0)
                        {
                            BaseColor colorSign = new BaseColor(0, 128, 0);
                            var bytes = File.ReadAllBytes("../segoeui.ttf");
                            BaseFont bf = BaseFont.CreateFont("segoeui.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED,
                                true, bytes, null);
                            f = new Font(bf, 9, Font.NORMAL, colorSign);
                        }
                        else
                        {
                            BaseColor colorSign = new BaseColor(0, 128, 0);
                            var bytes = File.ReadAllBytes("../segoeui.ttf");
                            BaseFont bf = BaseFont.CreateFont("segoeui.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED,
                                true, bytes, null);
                            f = new Font(bf, 9, Font.NORMAL, colorSign);
                        }
                        sap.Layer2Font = f;
                        string noidung = "Ký bởi: " + textOut + "\n";
                        noidung += "Ký ngày: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                        byte[] bytePng = Convert.FromBase64String(base64Image);
                        Image image = Image.GetInstance(bytePng);
                        image.ScalePercent(50);
                        image.SetAbsolutePosition(100f, 150f);
                        sap.Image = image;
                        sap.Image.Alignment = 0;
                        sap.ImageScale = 0.3f;
                        //sap.Image.ScaleAbsoluteHeight(height);
                        //Rectangle rSignature1 = new Rectangle(x, y, height, height);
                        //sap.Image.ScaleToFit(rSignature1);

                        sap.Acro6Layers = true;
                        sap.Layer2Text = noidung;
                        //sap.Layer2Text.PadLeft(100);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }


            sap.CryptoDictionary = (PdfDictionary)new PdfSignature((PdfName)PdfName.ADOBE_PPKLITE, (PdfName)PdfName.ADBE_PKCS7_DETACHED)
            {
                Reason = sap.Reason,
                Location = sap.Location,
                Contact = sap.Contact,
                Date = new PdfDate(sap.SignDate)
            };

            Dictionary<PdfName, int> dictionary = new Dictionary<PdfName, int>();
            dictionary.Add((PdfName)PdfName.CONTENTS, 16386);
            sap.PreClose(dictionary);
            var digest = DigestAlgorithms.Digest(sap.GetRangeStream(), sHash);


            //var externalSignature = new HsmRsaSignature(signer, sHash);
            //MakeSignature.SignDetached(sap, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

            var bytearr = baos.ToArray();
            return bytearr;
        }
        //    @Override
        //public synchronized byte[] signHashDataPdf(byte[] hash, String sHash, Long providerId, String privateKeyName, Certificate[] chain) throws Exception
        //    {
        //        Calendar cal = Calendar.getInstance();
        //    byte[] ocsp = null;
        //    ExternalSignature externalSignature = new PrivateKeySignature(sHash, "BC");
        //    String hashAlgorithm = externalSignature.getHashAlgorithm();
        //    Collection<byte[]> crlBytes = null;
        //    TSAClient tsaClient = null;
        //    String slot1password = configService.GetStringValue("Slot1Password");
        //    KeyStore keyStore = getKeystore(providerId);
        //    Provider prov = getProv(providerId);
        //    PrivateKey privateKey = (PrivateKey)keyStore.getKey(privateKeyName, slot1password.toCharArray());
        //    try {
        //        //log.info(chain[0].toString());
        //        log.info("hashAlgorithm " + hashAlgorithm);
        //        PdfPKCS7 sgn1 = new PdfPKCS7(privateKey, chain, hashAlgorithm, prov.getName(), null, false);
        //    byte[] sh = sgn1.getAuthenticatedAttributeBytes(hash, cal, ocsp, crlBytes, MakeSignature.CryptoStandard.CMS);
        //    sgn1.update(sh, 0, sh.length);
        //        return sgn1.getEncodedPKCS7(hash, cal, tsaClient, ocsp, crlBytes, MakeSignature.CryptoStandard.CMS);
        //    } catch (Exception ex) {
        //        ex.printStackTrace();
        //        log.error("signHashData Error:" + ex.toString());
        //        throw ex;
        //    }
        //}

        public byte[] getHashPkcs(Org.BouncyCastle.X509.X509Certificate cer, ICollection<X509Certificate> chain, SignClient client, string sHash, int typeSignature, string base64Image, string textOut, string font, string signatureName, int pageSign = 1,
            int xPoint = 100, int yPoint = 200, int width = 300, int height = 400)
        {
            PdfStamper stp = null;
            AcroFields af = _pdfReader.AcroFields;
            var signatureNameNames = af.GetSignatureNames();
            var signatureNumber = signatureNameNames.Count + 1;
            MemoryStream baos = new MemoryStream();

            if (signatureNumber > 1)
            {

                stp = PdfStamper.CreateSignature(_pdfReader, baos, '\0', null, true);
            }
            else
            {
                stp = PdfStamper.CreateSignature(_pdfReader, baos, '\0');
            }

            var sap = stp.SignatureAppearance;
            if (af.GetFieldItem(signatureName) == null)
            {
                Rectangle rectangle1 = new Rectangle((float)xPoint, (float)yPoint, (float)width, (float)height);
                sap.SetVisibleSignature(rectangle1, pageSign, signatureName);
            }
            else
            {
                sap.SetVisibleSignature(signatureName);
            }

            sap.Certificate = cer;
            sap.SignDate = DateTime.Now;

            switch (typeSignature)
            {
                case 1:
                    {
                        byte[] bytePng = Convert.FromBase64String(base64Image);
                        Image image = Image.GetInstance(bytePng);
                        sap.Image = image;
                        sap.Acro6Layers = true;
                        sap.Layer2Text = "";
                        break;
                    }
                case 2:
                    {

                        if (font == null)
                        {
                            font = default_font;
                            BaseColor colorSign = new BaseColor(0, 128, 0);
                            var bytes = File.ReadAllBytes("../segoeui.ttf");
                            BaseFont bf = BaseFont.CreateFont(font, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, true, bytes, null);
                            sap.Layer2Font = new Font(bf, 9, Font.NORMAL, colorSign);
                        }
                        string noidung = "Ký bởi: " + textOut + "\n";
                        noidung += "Ký ngày: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        sap.Layer2Text = noidung;
                        //sap.Layer2Text.PadLeft(100);
                        break;
                    }
                case 3:
                    {

                        if (font != null)
                        {

                        }

                        Font f = null;
                        if (String.Compare(font.ToLower(), "segoeui.ttf") == 0)
                        {
                            BaseColor colorSign = new BaseColor(0, 128, 0);
                            var bytes = File.ReadAllBytes("../segoeui.ttf");
                            BaseFont bf = BaseFont.CreateFont("segoeui.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED,
                                true, bytes, null);
                            f = new Font(bf, 9, Font.NORMAL, colorSign);
                        }
                        else
                        {
                            BaseColor colorSign = new BaseColor(0, 128, 0);
                            var bytes = File.ReadAllBytes("../segoeui.ttf");
                            BaseFont bf = BaseFont.CreateFont("segoeui.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED,
                                true, bytes, null);
                            f = new Font(bf, 9, Font.NORMAL, colorSign);
                        }
                        sap.Layer2Font = f;
                        string noidung = "Ký bởi: " + textOut + "\n";
                        noidung += "Ký ngày: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                        byte[] bytePng = Convert.FromBase64String(base64Image);
                        Image image = Image.GetInstance(bytePng);
                        image.ScalePercent(50);
                        image.SetAbsolutePosition(100f, 150f);
                        sap.Image = image;
                        sap.Image.Alignment = 0;
                        sap.ImageScale = 0.3f;
                        //sap.Image.ScaleAbsoluteHeight(height);
                        //Rectangle rSignature1 = new Rectangle(x, y, height, height);
                        //sap.Image.ScaleToFit(rSignature1);

                        sap.Acro6Layers = true;
                        sap.Layer2Text = noidung;
                        //sap.Layer2Text.PadLeft(100);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }


            sap.CryptoDictionary = (PdfDictionary)new PdfSignature((PdfName)PdfName.ADOBE_PPKLITE, (PdfName)PdfName.ADBE_PKCS7_DETACHED)
            {
                Reason = sap.Reason,
                Location = sap.Location,
                Contact = sap.Contact,
                Date = new PdfDate(sap.SignDate)
            };

            Dictionary<PdfName, int> dictionary = new Dictionary<PdfName, int>();
            dictionary.Add((PdfName)PdfName.CONTENTS, 16386);
            sap.PreClose(dictionary);
            var digest = DigestAlgorithms.Digest(sap.GetRangeStream(), sHash);

            var binDataJson = new
            {
                hashalg = "SHA-1",
                base64hash = Convert.ToBase64String(digest)
            };
            string pdfStringJson = JsonConvert.SerializeObject(binDataJson);
            var responseString = client.UploadString("/api/v2/pdf/sign/hashdata", "POST", pdfStringJson);
            var dataSigned = JsonConvert.DeserializeObject<ApiResp>(responseString);
            if (dataSigned.status == 0)
            {
                var bytesout = Convert.FromBase64String(dataSigned.obj.ToString());
                addExternalSignature(bytesout, sap);
                return baos.ToArray();
            }

            return null;
        }
        public static void addExternalSignature(byte[] extSignature, PdfSignatureAppearance sap)
        {
            byte[] numArray = new byte[8192];
            extSignature.CopyTo((Array)numArray, 0);
            PdfDictionary pdfDictionary = new PdfDictionary();
            pdfDictionary.Put((PdfName)PdfName.CONTENTS, (PdfObject)new PdfString(numArray).SetHexWriting(true));
            sap.Close(pdfDictionary);
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
                    _pdfReader.Dispose();
                }

                // Dispose unmanaged objects

                _disposed = true;
            }
        }
    }

    public enum Pkcs11InteropHashAlgorithm
    {
        /// <summary>
        /// The SHA1 hash algorithm
        /// </summary>
        SHA1,

        /// <summary>
        /// The SHA256 hash algorithm
        /// </summary>
        SHA256,

        /// <summary>
        /// The SHA384 hash algorithm
        /// </summary>
        SHA384,

        /// <summary>
        /// The SHA512 hash algorithm
        /// </summary>
        SHA512
    }

    public class HsmRsaSignature : IExternalSignature
    {
        private Pkcs11InteropHashAlgorithm _hashAlgorihtm = Pkcs11InteropHashAlgorithm.SHA256;
        public string GetHashAlgorithm()
        {
            return _hashAlgorihtm.ToString();
        }

        private String hash = "RSA";
        private readonly IByteSigner _byteSigner;
        private readonly string algHash;
        public HsmRsaSignature(IByteSigner byteSigner, String algHash)
        {
            this._byteSigner = byteSigner;
            this.algHash = algHash;
            if (algHash.ToLower().Equals("sha-1") || algHash.ToLower().Equals("sha1"))
            {
                _hashAlgorihtm = Pkcs11InteropHashAlgorithm.SHA1;
            }
            else if (algHash.ToLower().Equals("sha-256") || algHash.ToLower().Equals("sha256"))
            {
                _hashAlgorihtm = Pkcs11InteropHashAlgorithm.SHA256;
            }
            else if (algHash.ToLower().Equals("sha-384") || algHash.ToLower().Equals("sha384"))
            {
                _hashAlgorihtm = Pkcs11InteropHashAlgorithm.SHA384;
            }
            else if (algHash.ToLower().Equals("sha-512") || algHash.ToLower().Equals("sha512"))
            {
                _hashAlgorihtm = Pkcs11InteropHashAlgorithm.SHA512;
            }
        }
#pragma warning disable CA1024 // Use properties where appropriate
        public string GetEncryptionAlgorithm()
#pragma warning restore CA1024 // Use properties where appropriate
        {
            return "RSA";
        }
        /// <summary>
        /// Computes hash of the data
        /// </summary>
        /// <param name="digest">Hash algorithm implementation</param>
        /// <param name="data">Data that should be processed</param>
        /// <returns>Hash of data</returns>
        private byte[] ComputeDigest(IDigest digest, byte[] data)
        {
            if (digest == null)
                throw new ArgumentNullException("digest");

            if (data == null)
                throw new ArgumentNullException("data");

            byte[] hash = new byte[digest.GetDigestSize()];

            digest.Reset();
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);

            return hash;
        }
        /// <summary>
        /// Creates PKCS#1 DigestInfo
        /// </summary>
        /// <param name="hash">Hash value</param>
        /// <param name="hashOid">Hash algorithm OID</param>
        /// <returns>DER encoded PKCS#1 DigestInfo</returns>
        private byte[] CreateDigestInfo(byte[] hash, string hashOid)
        {
            DerObjectIdentifier derObjectIdentifier = new DerObjectIdentifier(hashOid);
            AlgorithmIdentifier algorithmIdentifier = new AlgorithmIdentifier(derObjectIdentifier, null);
            DigestInfo digestInfo = new DigestInfo(algorithmIdentifier, hash);
            return digestInfo.GetDerEncoded();
        }
        public byte[] Sign(byte[] message)
        {
            byte[] digest = null;
            byte[] digestInfo = null;

            switch (_hashAlgorihtm)
            {
                case Pkcs11InteropHashAlgorithm.SHA1:
                    digest = ComputeDigest(new Sha1Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "1.3.14.3.2.26");
                    break;
                case Pkcs11InteropHashAlgorithm.SHA256:
                    digest = ComputeDigest(new Sha256Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "2.16.840.1.101.3.4.2.1");
                    break;
                case Pkcs11InteropHashAlgorithm.SHA384:
                    digest = ComputeDigest(new Sha384Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "2.16.840.1.101.3.4.2.2");
                    break;
                case Pkcs11InteropHashAlgorithm.SHA512:
                    digest = ComputeDigest(new Sha512Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "2.16.840.1.101.3.4.2.3");
                    break;
            }
            return _byteSigner.Sign(digestInfo);
        }

    }


}
