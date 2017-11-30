using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Krypto_Example_ITS_Hiesmair_Buchner
{
    public partial class FormMain : Form
    {

        private KryptoHelper crypt;
        private string keyName;
        private OpenFileDialog openFileDialog;

        public FormMain()
        {
            InitializeComponent();
            crypt = new KryptoHelper();
            keyName =  "BuchmairKeys";
            openFileDialog = new OpenFileDialog();
        }

        private void btnCreateKeys_Click(object sender, EventArgs e)
        {
            string status;
            crypt.CreateRSAKeys(keyName, out status);
            lbStatus.Items.Add(status);
        }

        private void btnExportKeys_Click(object sender, EventArgs e)
        {
            string status;
            crypt.ExportRSAPublicKey(out status);
            lbStatus.Items.Add(status);

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string status;
            if (crypt.rsaKeysCreated == false)
            {
                status = "ERROR: Key not set!";
            }
            else
            {
                // Display a dialog box to select a file to encrypt.
                openFileDialog.InitialDirectory = crypt.documentsDirPath;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    crypt.EncryptFile(openFileDialog.FileName, out status);
                }
                else
                {
                    status = "ERROR: File not chosen!";
                }
            }
            lbStatus.Items.Add(status);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string status;
            if (crypt.rsaKeysCreated == false)
            {
                status = "ERROR: Key not set!";
            }
            else
            {
                // Display a dialog box to select a file to encrypt.
                openFileDialog.InitialDirectory = crypt.encryptDirPath;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    crypt.DecryptFile(openFileDialog.FileName, out status);
                }
                else
                {
                    status = "ERROR: File not chosen!";
                }
            }
            lbStatus.Items.Add(status);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string status = "ERROR: Could not clear!";
            crypt.Clear(out status);
            lbStatus.Items.Clear();
            lbStatus.Items.Add(status);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Do you really want to close the application?","Exit")
            
            DialogResult dialogResult = MessageBox.Show("Do you really want to close the application?", "About to exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
