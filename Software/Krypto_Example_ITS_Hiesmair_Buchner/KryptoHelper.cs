using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Krypto_Example_ITS_Hiesmair_Buchner
{
    class KryptoHelper
    {

        // Directory Paths 
        private string dataDirPath = "";
        private string encryptDirPath = "";
        private string decryptDirPath = "";
        private string keysDirPath = "";
        private string publicKeyFileName = "";
        private string symmetricKeysFileName = "";

        // Needed Crypto-Objects
        private CspParameters cspParam;
        private RSACryptoServiceProvider rsaProv;
        private bool rsaKeysCreated;

        // Needed Parameter
        private string rsaKeyName = "";

        public KryptoHelper()
        {
            dataDirPath = GetDataDirectory();
            encryptDirPath = dataDirPath + "\\Encrypt";
            decryptDirPath = dataDirPath + "\\Decrypt";
            keysDirPath = dataDirPath + "\\Keys";
            publicKeyFileName = "rsaPublicKey.txt";
            symmetricKeysFileName = "aesKeys.txt";

            cspParam = new CspParameters();
            rsaKeysCreated = false;


            if (!Directory.Exists(encryptDirPath))
            {
                Directory.CreateDirectory(encryptDirPath);
            }
            if (!Directory.Exists(decryptDirPath))
            {
                Directory.CreateDirectory(decryptDirPath);
            }
            if (!Directory.Exists(keysDirPath))
            {
                Directory.CreateDirectory(keysDirPath);
            }
        }

        private string GetDataDirectory()
        {
            string ret = "";
            // assuming that executing folder is debug folder in application context
            // -> goto root folder of application
            string currentdir = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 5; i++)
            {
                currentdir = Path.GetDirectoryName(currentdir);
            }

            currentdir += "\\Data";

            if (Directory.Exists(currentdir))
            {
                // Data-Dir exists next to Application-Dir
                ret = currentdir;
            }
            else
            {
                FolderBrowserDialog folderdialog = new FolderBrowserDialog();
                if (folderdialog.ShowDialog() == DialogResult.OK)
                {
                    ret = folderdialog.SelectedPath;
                }
            }

            return ret;
        }

        public void CreateRSAKeys(string keyname, out string status)
        {
            StringBuilder str = new StringBuilder();
            // Stores a key pair in the key container.
            cspParam.KeyContainerName = rsaKeyName;
            rsaProv = new RSACryptoServiceProvider(cspParam);
            rsaProv.PersistKeyInCsp = true;

            str.Append("RSA-Keys Created: ");
            str.Append(cspParam.KeyContainerName);
            if (rsaProv.PublicOnly == true)
            {
                str.Append(" - Public Only");
            }
            else
            {
                str.Append(" - Full Key Pair");
            }

            status = str.ToString();
            rsaKeysCreated = true;
        }

        private void CreateAESKeys(out RijndaelManaged rjndl) {
            rjndl = new RijndaelManaged();
            rjndl.KeySize = 256;
            rjndl.BlockSize = 256;
            rjndl.Mode = CipherMode.CBC;
        }

        private void ExportAESKeys(RijndaelManaged rjndl, string filename)
        {
            if (!rsaKeysCreated)
                return;

            string outFile = filename.Substring(0, filename.LastIndexOf(".")) + ".key";
            string filepath = keysDirPath + "\\" + outFile;

            byte[] symmkeyEncrypted = rsaProv.Encrypt(rjndl.Key, false);

            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            int lKey = symmkeyEncrypted.Length;
            LenK = BitConverter.GetBytes(lKey);
            int lIV = rjndl.IV.Length;
            LenIV = BitConverter.GetBytes(lIV);

            using (FileStream outFs = new FileStream(filepath, FileMode.Create))
            {
                outFs.Write(LenK, 0, 4);
                outFs.Write(LenIV, 0, 4);
                outFs.Write(symmkeyEncrypted, 0, lKey);
                outFs.Write(rjndl.IV, 0, lIV);

                outFs.Close();
            }
        }

        private void ImportAESKeys(string filename, byte[] IV, byte[] key)
        {
            if (!rsaKeysCreated)
                return;

            RijndaelManaged rjndl;
            CreateAESKeys(out rjndl);
            rjndl.Padding = PaddingMode.None;

            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            string inFile = filename.Substring(0, filename.LastIndexOf(".")) + ".key";
            string filepath = keysDirPath + "\\" + inFile;

            if (!File.Exists(filepath))
                return;

            using (FileStream inFs = new FileStream(filepath, FileMode.Open))
            {
                inFs.Seek(0, SeekOrigin.Begin);
                inFs.Seek(0, SeekOrigin.Begin);
                inFs.Read(LenK, 0, 3);
                inFs.Seek(4, SeekOrigin.Begin);
                inFs.Read(LenIV, 0, 3);

                // Convert the lengths to integer values.
                int lenK = BitConverter.ToInt32(LenK, 0);
                int lenIV = BitConverter.ToInt32(LenIV, 0);

                // Determine the start postition of
                // the ciphter text (startC)
                // and its length(lenC).
                int startC = lenK + lenIV + 8;
                int lenC = (int)inFs.Length - startC;

                // Create the byte arrays for
                // the encrypted Rijndael key,
                // the IV, and the cipher text.
                byte[] KeyEncrypted = new byte[lenK];
                IV = new byte[lenIV];

                // Extract the key and IV
                // starting from index 8
                // after the length values.
                inFs.Seek(8, SeekOrigin.Begin);
                inFs.Read(KeyEncrypted, 0, lenK);
                inFs.Seek(8 + lenK, SeekOrigin.Begin);
                inFs.Read(IV, 0, lenIV);

                // Use RSACryptoServiceProvider
                // to decrypt the Rijndael key.
                key = rsaProv.Decrypt(KeyEncrypted, false);
            }
        }

        public void ExportRSAPublicKey(out string status) {
            if (!rsaKeysCreated)
            {
                status = "ERROR - NO RSA Keys!";
                return;
            }

            string filepath = keysDirPath + "\\" + publicKeyFileName;

            // Save the public key created by the RSA
            // to a file. Caution, persisting the
            // key to a file is a security risk.
            StreamWriter sw = new StreamWriter(filepath);
            sw.Write(rsaProv.ToXmlString(false));
            sw.Close();

            status = "PublicKey Exported: " + publicKeyFileName;
        }

        // Benötigt ????????????? 

        public void ImportPublicKey(string keyName, out string status)
        {
            StringBuilder str = new StringBuilder();
            string filepath = keysDirPath + "\\" + publicKeyFileName;
            if (!File.Exists(filepath))
            {
                status = "ERROR: Public Key File Not Found!";
                return;
            }

            StreamReader sr = new StreamReader(filepath);
            cspParam.KeyContainerName = keyName;
            rsaProv = new RSACryptoServiceProvider(cspParam);
            string keytxt = sr.ReadToEnd();
            rsaProv.FromXmlString(keytxt);
            rsaProv.PersistKeyInCsp = true;

            str.Append("Key Loaded: ");
            str.Append(cspParam.KeyContainerName);
            if (rsaProv.PublicOnly == true)
                str.Append(" - Public Only");
            else
                str.Append(" - Full Key Pair"); ;

            sr.Close();

            status = str.ToString();
        }










    }
}
