using UnityEngine.Networking;

namespace ET
{
    public class WebReqSkipCert : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}