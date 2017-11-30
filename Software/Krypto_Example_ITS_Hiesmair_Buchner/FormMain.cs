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

        public FormMain()
        {
            InitializeComponent();
            crypt = new KryptoHelper();
            keyName =  "BuchmairKeys";
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

        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
