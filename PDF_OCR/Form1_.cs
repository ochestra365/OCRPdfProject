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
        //"https://a76qrjk90b.apigw.ntruss.com/custom/v1/28075/5c4b24044cbadee6b6f258977a87019cbc14e5e63d4cacfee629995432f8691c/general");
        // "S1B5c2tuY1l1eGF6U0FXelhWTE9WQUdjY0taUVJibkU=");
        private async void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                string base64String = Convert.ToBase64String(File.ReadAllBytes("image.pdf"));// 바이너리 파일 송신 전용 문자열 변환
                HttpClient client = new HttpClient();// 클라이언트 생성자 생성
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://a76qrjk90b.apigw.ntruss.com/custom/v1/28075/5c4b24044cbadee6b6f258977a87019cbc14e5e63d4cacfee629995432f8691c/general");//2번째 인자에 발급받은 Api Invoke URL 입력
                request.Headers.Add("X-OCR-SECRET", "S1B5c2tuY1l1eGF6U0FXelhWTE9WQUdjY0taUVJibkU=");//2번째 인자에 발급받은 시크릿 코드 입력
                //StringContent content = new StringContent("{\"images\": [{\"format\": \"pdf\",\"name\": \"guide-demo\"," +// 요청 BODY 내용
                //   $"\"data\": \"{base64String}" +
                //   "\"}]," +
                //   "\"lang\": \"ko\"," +
                //   "\"requestId\": \"string\"," +
                //   "\"resultType\": \"string\"," +
                //   "\"timestamp\": 1708009431," +
                //   "\"version\": \"V1\"}", null, "application/json");

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

                tempJobject = GetJObject(jsonString);
                if (tempJobject != null)
                {
                    #region 나중에 지울 내용
                    richTextBox1.AppendText($"속성 수량 : {tempJobject.Count}\n");
                    richTextBox1.AppendText($"VERSION : {tempJobject["version"]}\n");// 버전 정보 확인
                    richTextBox1.AppendText($"requestID : {tempJobject["requestId"]}\n");//요청 아이디(내가 보낸 것)-> TimeStamp로 놓고 verify 비교하면 될듯
                    richTextBox1.AppendText($"TIMESTAMP : {tempJobject["timestamp"]}\n");
                    richTextBox1.AppendText($"UID : {tempJobject["images"][0]["uid"]}\n");// 고유 아이디
                    richTextBox1.AppendText($"name : {tempJobject["images"][0]["name"]}\n");// 잘 모르겠음-> 기술지원 필요 혹은 문서 확인
                    richTextBox1.AppendText($"inferResult : {tempJobject["images"][0]["inferResult"]}\n");// 성공 여부
                    richTextBox1.AppendText($"message : {tempJobject["images"][0]["message"]}\n");//성공여부
                    richTextBox1.AppendText($"validationResult : {tempJobject["images"][0]["validationResult"]["result"]}\n");// 잘 모르겠음-> 기술지원 필요 혹은 문서 확인
                    #endregion
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
                    //jsonTable로 받은내용 표로 변환 추가.
                    int cnt = Convert.ToInt32(textBox1.Text);

                    if (set.Tables[cnt] != null)
                    {
                        if (set.Tables[cnt].Rows.Count <= 0) { return; }
                        DataTable resultTable = new DataTable();
                        if (int.TryParse(set.Tables[cnt].Rows[set.Tables[cnt].Rows.Count - 1][4].ToString().Trim(), out int columnCount))
                        {
                            // 컬럼 추가
                            for (int i = 0; i < columnCount + 1; i++)
                            {
                                resultTable.Columns.Add();
                            }
                            // row 추가
                            int rowCnt = Convert.ToInt32(set.Tables[cnt].Select(" rowIndex = max(rowIndex) ")[0]["rowIndex"].ToString());

                            for (int i = 0; i < rowCnt + 1; i++)
                            {
                                resultTable.Rows.Add();
                            }
                            // 값 삽입
                            foreach (DataRow row in set.Tables[cnt].Rows)
                            {
                                int rowIndex = int.Parse(row["rowIndex"].ToString());
                                int colIndex = int.Parse(row["columIndex"].ToString());
                                string value = row["value"].ToString();
                                resultTable.Rows[rowIndex][colIndex] = value;
                                row.AcceptChanges();
                            }
                            // 첫 번째 행이, 인식한 표의 컬럼명이다. 컬럼명이 없다면 지워서 다음 행을 헤더로 인식하게 한다.
                            // foreach (var item in resultTable.Rows[0].ItemArray) { if (String.IsNullOrEmpty(item.ToString())) { resultTable.Rows.RemoveAt(0); } }
                            this.drgMain.DataSource = resultTable;
                        }
                    }
                }
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
            try
            {
                if (File.Exists("image2.pdf")) { File.Delete("image2.pdf"); Thread.Sleep(10); }
                // DataTable화
                if (this.PDF_TABLE.Tables[0] != null)
                {


                    if (this.PDF_TABLE.Tables[0].Rows.Count <= 0) { return; }
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
                    PdfWriter.GetInstance(pdfDocument, new FileStream("image2.pdf", FileMode.Create));
                    pdfDocument.Open();
                    string GulimFont = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\gulim.ttc";
                    FontFactory.Register(GulimFont);
                    PdfPTable table = new PdfPTable(resultTable.Columns.Count);
                    iTextSharp.text.Font DataFont = FontFactory.GetFont("굴림체", BaseFont.IDENTITY_H, 10);
                    for (int i = 0; i < resultTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < resultTable.Columns.Count; j++)
                        {
                            PdfPCell cell_ = new PdfPCell(new Phrase(""));
                            cell_.HorizontalAlignment = 1;
                            string value =   resultTable.Rows[i][j].ToString();
                            new Paragraph(value, DataFont);
                            //table.AddCell(value);
                            table.AddCell(new Paragraph(value, DataFont));
                        }
                    }
                    pdfDocument.Add(new Paragraph("hihi this is example. hihihihadfasdfasdfaihi한글입력\n\n",DataFont));
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
            MakePDF();
        }
    }
}
