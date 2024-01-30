using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
namespace PDF_OCR
{
    public partial class Form1 : Form
    {
        /*
            1. reference URL : https://virusheo.blogspot.com/2023/04/230422.html
            2. License 1: Aphace-2.0[Conversion Of image to text]
            3. License 2: GhostScript[AGPL(https://www.gnu.org/licenses/agpl-3.0.html)]
             -> https://www.oss.kr/oss_license_qna/show/222bcc65-ddc5-48e5-8d98-c162ef9f9598?page=80
        ==================================================================================================
            1. fileName should be written by English.
            2. The Save Path should be Startup Path of Application.
        ===================================================================================================
        logic
        1. Choose pdf.
        2. Convert pdf to image(GhostScript)
        3. Convert image to text(tesseract)
        */
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.picMain.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            picMain.Image = null;
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "IMAGE");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                #region extension check
                this.rtbOcr.Text = string.Empty;
                if (!Path.GetExtension(dialog.FileName).ToUpper().Replace(".", string.Empty).Equals("PDF"))
                {
                    MessageBox.Show("Please select PDF");
                    return;
                }
                #endregion
                #region folder and file check
                DirectoryInfo dir = new DirectoryInfo(imagePath);
                if (!dir.Exists) { dir.Create(); }
                FileInfo[] infos = dir.GetFiles();
                if (infos.Length > 0)
                {
                    foreach (FileInfo file in infos)
                    {
                        if (File.Exists(file.FullName))
                        {
                            try
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                File.Delete(file.FullName);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"TIME : {DateTime.Now}. CONTENT : {ex}");
                            }
                        }
                    }
                }
                #endregion
                MakePDFtoImage(dialog.SafeFileName, int.TryParse(this.textBox1.Text, out int page) ? page : 1);
                string result = GetText(Path.Combine(imagePath,"temp.png"), "kor+eng");
                if (!string.IsNullOrEmpty(result))
                {
                    this.rtbOcr.Text = result;
                }
                Bitmap bitmap = new Bitmap($@"{imagePath}\temp.png");
                picMain.Image = bitmap;
            }
        }

        private bool MakePDFtoImage(string _pdfPath, int _page)
        {
            try
            {
                GhostscriptVersionInfo gvi = new GhostscriptVersionInfo($@"{Directory.GetCurrentDirectory()}\gsdll32.dll");
                using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
                {
                    rasterizer.Open(_pdfPath, gvi, false);
                    if (rasterizer.PageCount < _page) { return false; }
                    rasterizer.GetPage(200, _page).Save(Path.Combine(Directory.GetCurrentDirectory(), "IMAGE", "temp.png"));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 이미지에 적힌 텍스트를 분석하는 메서드
        /// </summary>
        /// <param name="_filePath">파일 경로</param>
        /// <param name="_language">변환할 언어</param>
        /// <returns>변환이 완료된 언어</returns>
        private string GetText(string _filePath, string _language = "eng")
        {
            string result = string.Empty;
            try
            {
                Bitmap img = new Bitmap(_filePath);
                TesseractEngine ocr = new TesseractEngine("./tessdata", _language, EngineMode.Default);
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
