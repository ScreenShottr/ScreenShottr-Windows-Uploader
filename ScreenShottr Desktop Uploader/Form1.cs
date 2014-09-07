using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Web;

namespace ScreenShottr_Desktop_Uploader {
	public partial class Form1: Form {
		public static string Base64Encode(string plainText) {
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData) {
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public Form1() {
			InitializeComponent();
		}

        public void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public void openFileDialog1_FileOk(object sender, EventArgs e)
        {

        }

		private void button1_Click(object sender, EventArgs e) {
			string chosenFile = "";
            pictureBox1.Image = null;
            resultBox.Enabled = false;
            resultBox.Text = "";
			openFD.Title = "Select an Image to Upload";
			openFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			openFD.Filter = "PNG Images|*.png|JPG Images|*.jpg|JPEG Images|*.jpeg";

			openFD.ShowDialog();
			chosenFile = openFD.FileName;
			if (chosenFile != "openFD") {

				System.Drawing.Image imageData = Image.FromFile(chosenFile);
				pictureBox1.Image = imageData;
                UploadFile(chosenFile);

			}
            openFD.FileName = "openFD";

		}


        public void UploadFile(string chosenFile)
        {
			var client = new WebClient();
			client.UploadFileCompleted += client_UploadDataCompleted;
			client.UploadProgressChanged += client_UploadDataChanged;
            client.UploadFileAsync(new Uri("https://www.screenshottr.us/upload?uploadAr=file"), "POST", @"" + chosenFile + "");
		}

		void client_UploadDataCompleted(object sender, UploadFileCompletedEventArgs e) {
			if (e.Error != null) {
				MessageBox.Show(e.Error.Message);
			}

			byte[] response = e.Result;
            progressBar1.Value = 100;
            resultBox.Enabled = true;
            resultBox.Text = System.Text.Encoding.Default.GetString(response);
            Clipboard.SetText(System.Text.Encoding.Default.GetString(response));
		}
        void client_UploadDataChanged(object sender, UploadProgressChangedEventArgs e)
        {
			int percent = e.ProgressPercentage;
            progressBar1.Value = percent;
		}
	}

}
