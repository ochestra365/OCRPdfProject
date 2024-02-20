using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using System.Xml;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Data;
using PDF_OCR.Class.Global;
using System.Threading;

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
        #region field
        private string url = "https://a76qrjk90b.apigw.ntruss.com/custom/v1/28075/5c4b24044cbadee6b6f258977a87019cbc14e5e63d4cacfee629995432f8691c/general";
        private string clientSecret = "S1B5c2tuY1l1eGF6U0FXelhWTE9WQUdjY0taUVJibkU=";
        private DataSet PDF_TABLE = new DataSet();
        #endregion


        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.picMain.SizeMode = PictureBoxSizeMode.StretchImage;
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// 네이버 OCR 가동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                string base64String = Convert.ToBase64String(File.ReadAllBytes("image.pdf"));// 바이너리 파일 송신 전용 문자열 변환
                HttpClient client = new HttpClient();// 클라이언트 생성자 생성
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://a76qrjk90b.apigw.ntruss.com/custom/v1/28075/5c4b24044cbadee6b6f258977a87019cbc14e5e63d4cacfee629995432f8691c/general");//2번째 인자에 발급받은 Api Invoke URL 입력
                request.Headers.Add("X-OCR-SECRET", "S1B5c2tuY1l1eGF6U0FXelhWTE9WQUdjY0taUVJibkU=");//2번째 인자에 발급받은 시크릿 코드 입력

                StringContent content2 = new StringContent("{\"images\": [{\"format\": \"pdf\",\"name\": \"guide-demo\"," +// 요청 BODY 내용
                   $"\"data\": \"{base64String}" +
                   "\"}]," +
                   "\"lang\": \"ko\"," +
                   "\"requestId\": \"string\"," +
                   "\"resultType\": \"string\"," +
                   "\"timestamp\": 1708009431," +
                   "\"version\": \"V1\"," +
                   "\"enableTableDetection\" : true}", null, "application/json");

                request.Content = content2;// 요청에 값 입력
                var response = await client.SendAsync(request);// 요청(메서드 헤더에 async 추가해야 에러 안남)
                var message = response.EnsureSuccessStatusCode();// Http 결과
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    string responsedString = string.Empty;
                    responsedString = await response.Content.ReadAsStringAsync();// 응답 값의 Body 문자열 입력
                    rtbOcr.Text = responsedString;
                    JObject tempJobject = GetJObject(responsedString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR : {ex}");
            }
        }

        private void btnNaverSearch_Click(object sender, EventArgs e)
        {
            //this.PDF_TABLE = new DataSet();
            #region tesseract
            /*
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
                string result = GetText(Path.Combine(imagePath, "temp.png"), "eng");
                if (!string.IsNullOrEmpty(result))
                {
                    this.rtbOcr.Text = result;
                    MakePDF(result);
                }
                Bitmap bitmap = new Bitmap($@"{imagePath}\temp.png");
                picMain.Image = bitmap;
            }*/
            #endregion
            try
            {
                richTextBox1.Clear();
                drgMain.DataSource = null;
                // 매개변수 설명서 : https://api.ncloud-docs.com/docs/ai-application-service-ocr-ocr#%EC%9A%94%EC%B2%AD-%EB%B0%94%EB%94%94
                JObject tempJobject = new JObject();
                //string jsonString = File.ReadAllText("RESPONSE.json").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                string jsonString = File.ReadAllText("[표22]TextOCR[20240219145919].json").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                string parsedStr=GlobalVariableController.PrettyPrint(jsonString);
                rtbOcr.Text = parsedStr;
                MakePDF(parsedStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// JSON 정보 Jobject 변환 메서드
        /// </summary>
        /// <param name="_response">응답 JSON</param>
        /// <returns>Jobject</returns>
        private JObject GetJObject(string _response)
        {
            JObject result = new JObject();
            try
            {
                result = JObject.Parse(_response);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
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
        private string GetText(string _filePath, string _language = "kor+eng")
        {
            string result = string.Empty;
            try
            {
                Bitmap img = new Bitmap(_filePath);
                TesseractEngine ocr = new TesseractEngine("./tessdata", _language, EngineMode.Default);
                result = ocr.Process(img).GetText();
                //result = ocr.Process(img).GetText();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }
        private void MakePDF(string _text = "")
        {
            rtbOcr.Clear();
            string jsonString = File.ReadAllText("[표22]TextOCR[20240219145919].json").Replace("\r", "").Replace("\n", "").Replace("\t", "");
            string parsedStr = GlobalVariableController.PrettyPrint(jsonString);
            rtbOcr.Text = parsedStr;
            JObject tempJobject = JObject.Parse(jsonString);
            try
            {
                if (File.Exists("output.pdf")) { File.Delete("output.pdf"); Thread.Sleep(10); }
                // DataTable화
                // 표 정보 DataSet에 모두 저장
                JEnumerable<JToken> tempToeken = tempJobject["images"][0]["tables"].Children();
                int tableCount = 0;
                DataSet set = new DataSet();

                foreach (JToken item in tempToeken)
                {
                    DataTable jsonTable = new DataTable();
                    DataTable tempTable = new DataTable();
                    jsonTable = GlobalVariableController.GetJsontoDataTable(jsonString, tableCount);
                    tempTable = jsonTable.Copy();
                    set.Tables.Add(jsonTable);
                    this.PDF_TABLE.Tables.Add(tempTable);
                    //this.PDF_TABLE.Tables.Add(jsonTable);
                    tableCount++;
                }
                if (this.PDF_TABLE.Tables[0] != null)
                {
                    if (this.PDF_TABLE.Tables[0].Rows.Count <= 0) { return; }
                    // 주어진 JSON 정보를 토대로 PDF의 표를 구현.
                    DataTable resultTable = new DataTable();
                    if (int.TryParse(this.PDF_TABLE.Tables[0].Rows[this.PDF_TABLE.Tables[0].Rows.Count - 1][4].ToString().Trim(), out int columnCount))
                    {
                        // 컬럼 추가
                        for (int i = 0; i < columnCount + 1; i++)
                        {
                            resultTable.Columns.Add();
                        }
                        // row 추가
                        int rowCnt = Convert.ToInt32(this.PDF_TABLE.Tables[0].Select(" rowIndex = max(rowIndex) ")[0]["rowIndex"].ToString());
                        for (int i = 0; i < rowCnt + 1; i++)
                        {
                            resultTable.Rows.Add();
                        }
                        // 값 삽입
                        foreach (DataRow row in this.PDF_TABLE.Tables[0].Rows)
                        {
                            int rowIndex = int.Parse(row["rowIndex"].ToString());
                            int colIndex = int.Parse(row["columIndex"].ToString());
                            string value = row["value"].ToString();
                            resultTable.Rows[rowIndex][colIndex] = value;
                            row.AcceptChanges();
                        }
                        this.drgMain.DataSource = resultTable;
                    }
                    // 표
                    // [TODO] 240219 pdf 한글 출력
                    // 한글 출력 가능하게 하는 코드(https://uxtime.tistory.com/entry/C-PDF-Open-Source-ItextSharp-%ED%95%9C%EA%B8%80-%EC%B6%9C%EB%A0%A5)
                    BaseFont.AddToResourceSearch("iTextAsian.dll");
                    Document pdfDocument = new Document(PageSize.A4.Rotate());
                    PdfWriter.GetInstance(pdfDocument, new FileStream("output.pdf", FileMode.Create));
                    pdfDocument.Open();
                    string GulimFont = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\gulim.ttc";
                    FontFactory.Register(GulimFont);
                    PdfPTable table = new PdfPTable(resultTable.Columns.Count);
                    iTextSharp.text.Font DataFont = FontFactory.GetFont("굴림체", BaseFont.IDENTITY_H, 10);
                    for (int i = 0; i < resultTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < resultTable.Columns.Count; j++)
                        {
                            PdfPCell cell_ = new PdfPCell(new Phrase(resultTable.Rows[i][j].ToString(), DataFont));
                            cell_.HorizontalAlignment = 1;// 중앙 정렬
                            table.AddCell(cell_);
                        }
                    }
                    pdfDocument.Add(new Paragraph($"{parsedStr}\n\n\n", DataFont));
                    pdfDocument.Add(table);
                    pdfDocument.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnMakePDF_Click(object sender, EventArgs e)
        {
            // TODO : 20240219 : 표의 ROWSPAN 인식 기능 추가.
            MakePDF();
        }
    }
}
