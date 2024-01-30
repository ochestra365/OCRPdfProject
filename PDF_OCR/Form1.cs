using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;
namespace PDF_OCR
{
    public partial class Form1 : Form
    {
        /*
            1. reference URL : https://virusheo.blogspot.com/2023/04/230422.html
            2. License : Aphace-2.0
        */
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string startUpPath = Directory.GetCurrentDirectory() + "\\";
            string imageName = "c_lang.bmp";

            this.rtbOcr.Text = string.Empty;

            string result = GetText(startUpPath + imageName, "kor+eng");
            if (!string.IsNullOrEmpty(result))
            {
                this.rtbOcr.Text = result;
            }
        }

        private string GetText(string _filePath, string _language = "eng")
        {


            string result = string.Empty;
            try
            {
                Bitmap img = new Bitmap(_filePath);
                var ocr = new TesseractEngine("./tessdata", _language, EngineMode.Default);
                result = ocr.Process(img).GetText();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }
    }
}
