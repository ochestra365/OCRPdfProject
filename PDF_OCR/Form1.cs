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
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Data;
using PDF_OCR.Class.Global;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

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
        private string url = "";
        private string clientSecret = "";
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
           
        }
        private async void GetNaver(string _path)
        {
            try
            {
                string base64String = Convert.ToBase64String(File.ReadAllBytes(_path));// 바이너리 파일 송신 전용 문자열 변환
                HttpClient client = new HttpClient();// 클라이언트 생성자 생성
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");//2번째 인자에 발급받은 Api Invoke URL 입력
                request.Headers.Add("X-OCR-SECRET", "");//2번째 인자에 발급받은 시크릿 코드 입력

                StringContent content2 = new StringContent("{\"images\": [{\"format\": \"pdf\",\"name\": \"guide-demo\"," +// 요청 BODY 내용
                   $"\"data\": \"{base64String}" +
                   "\"}]," +
                   "\"lang\": \"ko\"," +
                   "\"requestId\": \"string\"," +
                   "\"resultType\": \"string\"," +
                   "\"timestamp\": 1708009431," +
                   "\"version\": \"V2\"," +
                   "\"enableTableDetection\" : true}", null, "application/json");

                request.Content = content2;// 요청에 값 입력
                var response = await client.SendAsync(request);// 요청(메서드 헤더에 async 추가해야 에러 안남)
                var message = response.EnsureSuccessStatusCode();// Http 결과
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    string responsedString = string.Empty;
                    responsedString = await response.Content.ReadAsStringAsync();// 응답 값의 Body 문자열 입력
                    rtbOcr.Text = responsedString;
                    MakePDF(responsedString);
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
                string parsedStr = GlobalVariableController.PrettyPrint(jsonString);
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
        /// <summary>
        /// PDF 문서 제작.
        ///  한글 출력 가능하게 하는 코드(https://uxtime.tistory.com/entry/C-PDF-Open-Source-ItextSharp-%ED%95%9C%EA%B8%80-%EC%B6%9C%EB%A0%A5)
        /// </summary>
        /// <param name="_text">보정된 JSON 문자열</param>
        private void MakePDF(string _text = "")
        {
            this.drgMain.DataSource = null;
            rtbOcr.Clear();
            string jsonString = (!string.IsNullOrEmpty(_text)) ? _text : File.ReadAllText("사업계획서_낙서된거.json").Replace("\r", "").Replace("\n", "").Replace("\t", "");
            string parsedStr = GlobalVariableController.PrettyPrint(jsonString);
            if (string.IsNullOrEmpty(parsedStr)) { return; }
            rtbOcr.Text = parsedStr;
            JObject tempJobject = JObject.Parse(jsonString);
            try
            {
                if (File.Exists("ocroutput.pdf")) { File.Delete("ocroutput.pdf"); Thread.Sleep(10); }
                JEnumerable<JToken> isContainTables = tempJobject["images"][0].Children();
                bool isContainTable = false;
                FontFactory.Register(Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\gulim.ttc");
                Document pdfDocument = new Document(PageSize.A4.Rotate());
                PdfWriter.GetInstance(pdfDocument, new FileStream("ocroutput.pdf", FileMode.Create));
                iTextSharp.text.Font DataFont = FontFactory.GetFont("굴림체", BaseFont.IDENTITY_H, 9);
                pdfDocument.Open();
                pdfDocument.Add(new Paragraph($"{parsedStr}\n\n\n", DataFont));
                foreach (JToken token in isContainTables) { if (token.Path.Split('.')[1].Equals("tables")) { isContainTable = true; break; } }
                if (isContainTable)
                {
                    JEnumerable<JToken> tempToeken = tempJobject["images"][0]["tables"].Children();
                    int tableCount = 0;
                    DataSet set = new DataSet();
                    DataTable resultTable = new DataTable();
                    foreach (JToken item in tempToeken)
                    {
                        DataTable jsonTable = new DataTable();
                        jsonTable = GlobalVariableController.GetJsontoDataTable(jsonString, tableCount);
                        if (jsonTable != null) { set.Tables.Add(jsonTable); tableCount++; }
                    }
                    int selectedTable = 0;
                    if (set != null)
                    {
                        bool isSpanExist = false;
                        bool isRowSpanExist = false;
                        bool isColSpanExist = false;
                        // 20240222 행, 열의 스판 존재 여부 확인 기능 작동 하지 않는다.
                        foreach (DataRow row in set.Tables[selectedTable].Rows) { if (int.Parse(row[1].ToString()).Equals(1)) { isRowSpanExist = true; break; } }
                        foreach (DataRow row in set.Tables[selectedTable].Rows) { if (int.Parse(row[3].ToString()).Equals(1)) { isColSpanExist = true; break; } }
                        isSpanExist = isRowSpanExist || isColSpanExist;


                        while (selectedTable < tableCount)
                        {
                            if (selectedTable == 1) { string hi = string.Empty; }


                            if (isSpanExist)
                            {
                                DataTable tempTable = set.Tables[selectedTable];
                                int columnCountMax = tempTable.Rows.Cast<DataRow>().Max<DataRow>(row => (int)row[4]);
                                int rowConutMax = tempTable.Rows.Cast<DataRow>().Max<DataRow>(row => (int)row[2]);
                                PdfPTable table = new PdfPTable(columnCountMax + 1);

                                for (int i = 0; i <= rowConutMax; i++)
                                {
                                    for (int j = 0; j < columnCountMax + 1; j++)
                                    {
                                        try
                                        {
                                            DataRow[] sameCoordinate = tempTable.Select($" {tempTable.Columns[2].ColumnName} = '{i}' AND {tempTable.Columns[4].ColumnName}='{j}' ");
                                            bool isOnlyCoordinate = sameCoordinate.Length == 1;
                                            if (isOnlyCoordinate)
                                            {
                                                DataRow row = tempTable.AsEnumerable().First(x => int.Parse(x[2].ToString()).Equals(i) && int.Parse(x[4].ToString()).Equals(j));
                                                int rowSpan = (int)row.ItemArray[1];
                                                int colSpan = (int)row.ItemArray[3];
                                                string value = row.ItemArray[5].ToString();
                                                value = value.Length > 560 ? value.Substring(0, 560) : value;
                                                PdfPCell cell = new PdfPCell(new Phrase(value, DataFont));
                                                cell.HorizontalAlignment = 1;
                                                cell.Rowspan = rowSpan;
                                                cell.Colspan = colSpan;
                                                if (i == 0) { cell.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY; }
                                                table.AddCell(cell);
                                            }
                                            else
                                            {
                                                int rowSpan = (int)sameCoordinate[0].ItemArray[1];
                                                int colSpan = (int)sameCoordinate[0].ItemArray[3];
                                                StringBuilder sb = new StringBuilder();
                                                for (int k = 0; k < sameCoordinate.Length; k++)
                                                {
                                                    string value = sameCoordinate[k].ItemArray[5].ToString();
                                                    sb.Append(k != sameCoordinate.Length - 1 ? $"{value}\n" : $"{value}");
                                                }
                                                string finalValue = sb.ToString();
                                                finalValue = finalValue.Length > 560 ? finalValue.Substring(0, 560) : finalValue;
                                                PdfPCell cell = new PdfPCell(new Phrase(finalValue, DataFont));
                                                cell.HorizontalAlignment = 1;
                                                cell.Rowspan = rowSpan;
                                                cell.Colspan = colSpan;
                                                if (i == 0) { cell.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY; }
                                                table.AddCell(cell);
                                            }
                                        }
                                        catch { continue; }
                                    }
                                }

                                pdfDocument.Add(table);
                                pdfDocument.Add(new Paragraph($"\n\n\n", DataFont));
                            }
                            else
                            {
                                resultTable = GlobalVariableController.MakeNoSpanResultTable(set.Tables[selectedTable]);
                                if (resultTable == null) { return; }

                                PdfPTable table = new PdfPTable(resultTable.Columns.Count);
                                for (int i = 0; i < resultTable.Rows.Count; i++)
                                {
                                    for (int j = 0; j < resultTable.Columns.Count; j++)
                                    {
                                        PdfPCell cell_ = new PdfPCell(new Phrase(resultTable.Rows[i][j].ToString(), DataFont));
                                        cell_.HorizontalAlignment = 1;// 중앙 정렬
                                        if (i == 0) { cell_.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY; }
                                        table.AddCell(cell_);
                                    }
                                }
                                pdfDocument.Add(table);
                                pdfDocument.Add(new Paragraph($"\n\n\n", DataFont));
                            }
                            selectedTable++;
                        }
                    }
                }
                else { pdfDocument.Add(new Paragraph($"{parsedStr}\n\n\n", DataFont)); }
                pdfDocument.Close();
            }
            catch (IOException ex)
            {
                // 나중에 경로 파일로 유효성 검증한다.
                if (ex.Message.Contains(Application.StartupPath))
                {
                    MessageBox.Show("파일이 열려 있습니다.\n파일을 닫아주세요", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void btnMakePDF_Click(object sender, EventArgs e)
        {
            // TODO : 20240219 : 표의 ROWSPAN 인식 기능 추가.
            MakePDF();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            MakePDF();
        }
    }
}