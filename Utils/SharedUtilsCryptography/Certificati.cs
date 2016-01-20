using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilsCryptography
{
    public static class Certificati
    {
        public static void FindCertInStore()
        {
            /*var store = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);


            var cert_AdfsSigning = store.Certificates.Find(X509FindType.FindByThumbprint, "677CCB7A5F9878038696A11ABD59084597471C1E", false);
            var cert_AdfsEncryption = store.Certificates.Find(X509FindType.FindByThumbprint, "9849F9486A510AB646D44B165DC04E275C84DAFA", false);

            //CA o CA-int???
            var cert_CoreCA = store.Certificates.Find(X509FindType.FindByThumbprint, "6A6C92DF218251BA650053D6E7E64EFF8854EC94", false);

            var cert_Comm_asd = store.Certificates.Find(X509FindType.FindByThumbprint, "5280E0C0F9B1A64AB92666EF33B77E53C1E2EB5B", false);


            var storeAddress = new X509Store(StoreName.AddressBook, StoreLocation.CurrentUser);
            storeAddress.Open(OpenFlags.ReadOnly);
            var cert_Comm = storeAddress.Certificates.Find(X509FindType.FindByThumbprint, "5280E0C0F9B1A64AB92666EF33B77E53C1E2EB5B", false);


            var listSubjects = new List<string>();


            foreach (var item in store.Certificates)
            {
                if (item.HasPrivateKey == true)
                {
                    Debugger.Break();
                }

                listSubjects.Add(item.Subject);

            }*/

            throw new NotImplementedException("metodo commentato");
        }
    }
}
