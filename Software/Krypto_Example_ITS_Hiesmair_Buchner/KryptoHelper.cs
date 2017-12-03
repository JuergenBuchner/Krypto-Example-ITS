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
        public string documentsDirPath { get; private set; }
        public string encryptDirPath { get; private set; }
        private string decryptDirPath = "";
        private string keysDirPath = "";
        private string publicKeyFileName = "";
        private string symmetricKeysFileName = "";

        // Needed Crypto-Objects
        private CspParameters cspParam;
        private RSACryptoServiceProvider rsaProv;
        public bool rsaKeysCreated { get; private set; }

        // Needed Parameter
        private string rsaKeyName = "";

        public KryptoHelper()
        {
            dataDirPath = GetDataDirectory();
            documentsDirPath = dataDirPath + "\\00_Documents";
            encryptDirPath = dataDirPath + "\\01_Encrypt";
            decryptDirPath = dataDirPath + "\\02_Decrypt";
            keysDirPath = dataDirPath + "\\03_Keys";
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

            using (FileStream outFStream = new FileStream(filepath, FileMode.Create))
            {
                outFStream.Write(LenK, 0, 4);
                outFStream.Write(LenIV, 0, 4);
                outFStream.Write(symmkeyEncrypted, 0, lKey);
                outFStream.Write(rjndl.IV, 0, lIV);

                outFStream.Close();
            }
        }

        private void ImportAESKeys(string filename, out byte[] IV, out byte[] key, out RijndaelManaged rjndl)
        {
            IV = null;
            key = null;
            rjndl = null;

            if (!rsaKeysCreated)
                return;

            CreateAESKeys(out rjndl);
            //rjndl.Padding = PaddingMode.None;

            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            string inFile = filename.Substring(0, filename.IndexOf(".")) + ".key";
            string filepath = keysDirPath + "\\" + inFile;

            if (!File.Exists(filepath))
                return;

            using (FileStream inFStream = new FileStream(filepath, FileMode.Open))
            {
                inFStream.Seek(0, SeekOrigin.Begin);
                inFStream.Seek(0, SeekOrigin.Begin);
                inFStream.Read(LenK, 0, 3);
                inFStream.Seek(4, SeekOrigin.Begin);
                inFStream.Read(LenIV, 0, 3);

                // Convert the lengths to integer values.
                int lenK = BitConverter.ToInt32(LenK, 0);
                int lenIV = BitConverter.ToInt32(LenIV, 0);

                // Determine the start postition of
                // the ciphter text (startC)
                // and its length(lenC).
                int startC = lenK + lenIV + 8;
                int lenC = (int)inFStream.Length - startC;

                // Create the byte arrays for
                // the encrypted Rijndael key,
                // the IV, and the cipher text.
                byte[] KeyEncrypted = new byte[lenK];
                IV = new byte[lenIV];

                // Extract the key and IV
                // starting from index 8
                // after the length values.
                inFStream.Seek(8, SeekOrigin.Begin);
                inFStream.Read(KeyEncrypted, 0, lenK);
                inFStream.Seek(8 + lenK, SeekOrigin.Begin);
                inFStream.Read(IV, 0, lenIV);

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

        public void EncryptFile(string inFilepath, out string status)
        {
            FileInfo fileInfo = new FileInfo(inFilepath);           
            string name = fileInfo.Name; // File name without path
            
            RijndaelManaged rjndl; // Object for symetric encryption
            CreateAESKeys(out rjndl); // defining keysize, blocksize, mode
            ICryptoTransform cryptoTransform = rjndl.CreateEncryptor();

            // Exporting the AES keys
            ExportAESKeys(rjndl, name);

            string outFilepath = encryptDirPath + "\\" + name + ".enc";

            using (FileStream outFStrem = new FileStream(outFilepath, FileMode.Create))
            {

                // CryptoStream: Stream for encrypting.
                using (CryptoStream outStremEnc = new CryptoStream(outFStrem, cryptoTransform, CryptoStreamMode.Write))
                {
                    int cnt = 0;

                    int blockSizeBytes = rjndl.BlockSize / 8; //From bitsize to bytes
                    byte[] data = new byte[blockSizeBytes];
                    int bytesRead = 0;

                    using (FileStream inFStream = new FileStream( inFilepath, FileMode.Open))
                    {
                        do
                        {
                            cnt = inFStream.Read(data, 0, blockSizeBytes);
                            outStremEnc.Write(data, 0, cnt);
                            bytesRead += blockSizeBytes;
                        }
                        while (cnt > 0);
                        inFStream.Close();
                    }
                    outStremEnc.FlushFinalBlock();
                    outStremEnc.Close();
                }
                outFStrem.Close();
            }
            status = "File \"" + name +"\" encrypted.";
        }

        public void DecryptFile(string inFilepath, out string status)
        {
            FileInfo fileInfo = new FileInfo(inFilepath);
            string name = fileInfo.Name; // File name without path

            RijndaelManaged rjndl; // Object for symetric encryption

            string outFilepath = decryptDirPath + "\\" + name.Substring(0, name.LastIndexOf("."));

            // Importing the AES key from file
            byte[] IV;
            byte[] KeyDecrypted;
            ImportAESKeys(name, out IV, out KeyDecrypted, out rjndl);            

            // Decrypting the key.
            ICryptoTransform transform = rjndl.CreateDecryptor(KeyDecrypted, IV);

            // Reading the encrypted file -> decryption using AES -> writing the decrypted data to file 
            using (FileStream inFStream = new FileStream(inFilepath, FileMode.Open))
            {
                using (FileStream outFStream = new FileStream(outFilepath, FileMode.Create))
                {
                    int cnt = 0;

                    int blockSizeBytes = rjndl.BlockSize / 8; //From bitsize to bytes
                    byte[] data = new byte[blockSizeBytes];

                    inFStream.Seek(0, SeekOrigin.Begin);
                    using (CryptoStream outStreamDecrypted = new CryptoStream(outFStream, transform, CryptoStreamMode.Write))
                    {
                        do
                        {
                            cnt = inFStream.Read(data, 0, blockSizeBytes);
                            outStreamDecrypted.Write(data, 0, cnt);
                        }
                        while (cnt > 0);

                        outStreamDecrypted.FlushFinalBlock();
                        outStreamDecrypted.Close();
                    }
                    outFStream.Close();
                }
                inFStream.Close();
            }
            status = "File \"" + name + "\" decrypted.";
        }

        public void Clear(out string status)
        {
            status = "ERROR: Could not clear!";
            System.IO.DirectoryInfo diEnc = new DirectoryInfo(encryptDirPath);
            System.IO.DirectoryInfo diDec = new DirectoryInfo(decryptDirPath);
            System.IO.DirectoryInfo diKey = new DirectoryInfo(keysDirPath);

            foreach (FileInfo file in diEnc.GetFiles())
            {
                file.Delete();
            }
            foreach (FileInfo file in diDec.GetFiles())
            {
                file.Delete();
            }
            foreach (FileInfo file in diKey.GetFiles())
            {
                file.Delete();
            }
            rsaKeysCreated = false;
            status = "Cleared folders \"01_Encrypt\" \"02_Decrypt\" and \"03_Keys\".";
        }
    }
}
