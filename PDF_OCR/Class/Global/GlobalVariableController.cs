using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace PDF_OCR.Class.Global
{
    class GlobalVariableController
    {
        /// <summary>
        /// 네이버 클로바 OCR로부터 수신한 표 정보
        /// </summary>
        /// <param name="_jsonString">네이버 클로바 OCR로부터 수신한 JSON</param>
        /// <param name="tableIndex">수신한 테이블 인덱스</param>
        /// <returns>네이버 클로바 OCR이 인식한 표 정보</returns>
        public static DataTable GetJsontoDataTable(string _jsonString, int tableIndex)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("CELL_ID", typeof(string));
            resultTable.Columns.Add("rowSpan", typeof(int));
            resultTable.Columns.Add("rowIndex", typeof(int));
            resultTable.Columns.Add("columnSpan", typeof(int));
            resultTable.Columns.Add("columIndex", typeof(int));
            resultTable.Columns.Add("value", typeof(string));
            try
            {
                JObject jobject = JObject.Parse(_jsonString);
                bool isInferSuccess = jobject["images"][0]["inferResult"].ToString().ToUpper().Equals("SUCCESS");// 이미지 인식 성공 여부
                if (isInferSuccess)
                {
                    JObject Jtbobject = JObject.Parse(jobject["images"][0]["tables"][tableIndex].ToString());
                    JArray jArray = JArray.Parse(Jtbobject["cells"].ToString());
                    int index = 0;
                    foreach (JToken token in jArray)
                    {
                        if (token.HasValues)
                        {
                            string cellid = (++index).ToString();
                            int rowSpan = Convert.ToInt32(token["rowSpan"].ToString());
                            int rowIndex = Convert.ToInt32(token["rowIndex"].ToString());
                            int columnSpan = Convert.ToInt32(token["columnSpan"].ToString());
                            int columnIndex = Convert.ToInt32(token["columnIndex"].ToString());
                            StringBuilder sb = new StringBuilder();
                            string cellTextLines = token["cellTextLines"].ToString();
                            if (token["cellTextLines"].ToString().Equals("[]"))
                            {
                                sb.Append("");
                            }
                            else
                            {
                                int count = token["cellTextLines"][0]["cellWords"].Count<JToken>();
                                for (int i = 0; i < count; i++)
                                {
                                    string tokenContent = token["cellTextLines"][0].ToString();
                                    if (tokenContent.Contains("inferText"))
                                    {
                                        string inferText = token["cellTextLines"][0]["cellWords"][i]["inferText"].ToString();
                                        sb.Append(i != count - 1 ? $"{inferText} " : inferText);
                                    }
                                    else
                                    {
                                        string inferText = "\n";
                                        sb.Append("\n");
                                    }
                                }
                            }
                            resultTable.Rows.Add(cellid, rowSpan, rowIndex, columnSpan, columnIndex, sb.ToString());
                        }
                    }
                }
                else
                {
                    string errMessage = jobject["images"][0]["message"].ToString();
                    resultTable.Rows.Add("ERR", "ERR", "ERR", "ERR", "ERR", $"{errMessage}");
                    return resultTable;
                }
                if (resultTable.Rows.Count > 0)
                {
                    return resultTable;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        /// <summary>
        /// 20240219 TODO
        /// 네이버 클로바 OCR로부터 RETURN 받은 정보의 좌표값을 계산해서 이쁘게 출력
        /// </summary>
        /// <param name="jsonString">네이버 클로바 OCR로부터 리턴받은 정보</param>
        /// <returns> xy 좌표값 보정된 광학인식 문자열.</returns>
        public static string PrettyPrint(string _jsonString)
        {

            StringBuilder sb = new StringBuilder();
            try
            {
                JObject Jnaver = JObject.Parse(_jsonString);
                JEnumerable<JToken> children = Jnaver["images"][0]["fields"].Children();
                foreach (JToken token in children)
                {
                    bool isLinebreak=Boolean.Parse( token.SelectToken("lineBreak").ToString());//라인 분류
                    sb.Append(isLinebreak ? $"{token.SelectToken("inferText")}\n" : $"{token.SelectToken("inferText")} ");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return string.Empty;
        }
    }
}
