
namespace PDF_OCR
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbOcr = new System.Windows.Forms.RichTextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.picMain = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnNaverSearch = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.drgMain = new System.Windows.Forms.DataGridView();
            this.btnMakePDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.drgMain)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbOcr
            // 
            this.rtbOcr.Location = new System.Drawing.Point(3, 12);
            this.rtbOcr.Name = "rtbOcr";
            this.rtbOcr.Size = new System.Drawing.Size(515, 708);
            this.rtbOcr.TabIndex = 0;
            this.rtbOcr.Text = "";
            // 
            // btnConvert
            // 
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConvert.Location = new System.Drawing.Point(1072, 46);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(324, 368);
            this.btnConvert.TabIndex = 1;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(524, 12);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(521, 708);
            this.picMain.TabIndex = 2;
            this.picMain.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1072, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(324, 21);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnNaverSearch
            // 
            this.btnNaverSearch.Location = new System.Drawing.Point(1072, 444);
            this.btnNaverSearch.Name = "btnNaverSearch";
            this.btnNaverSearch.Size = new System.Drawing.Size(136, 33);
            this.btnNaverSearch.TabIndex = 4;
            this.btnNaverSearch.Text = "NaverSearch";
            this.btnNaverSearch.UseVisualStyleBackColor = true;
            this.btnNaverSearch.Click += new System.EventHandler(this.btnNaverSearch_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1072, 496);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(432, 273);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // drgMain
            // 
            this.drgMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.drgMain.Location = new System.Drawing.Point(1402, 61);
            this.drgMain.Name = "drgMain";
            this.drgMain.RowTemplate.Height = 23;
            this.drgMain.Size = new System.Drawing.Size(410, 353);
            this.drgMain.TabIndex = 6;
            // 
            // btnMakePDF
            // 
            this.btnMakePDF.Location = new System.Drawing.Point(1224, 444);
            this.btnMakePDF.Name = "btnMakePDF";
            this.btnMakePDF.Size = new System.Drawing.Size(136, 33);
            this.btnMakePDF.TabIndex = 7;
            this.btnMakePDF.Text = "MakePDF";
            this.btnMakePDF.UseVisualStyleBackColor = true;
            this.btnMakePDF.Click += new System.EventHandler(this.btnMakePDF_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1815, 842);
            this.Controls.Add(this.btnMakePDF);
            this.Controls.Add(this.drgMain);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnNaverSearch);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.picMain);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.rtbOcr);
            this.Enabled = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.drgMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbOcr;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnNaverSearch;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridView drgMain;
        private System.Windows.Forms.Button btnMakePDF;
    }
}

