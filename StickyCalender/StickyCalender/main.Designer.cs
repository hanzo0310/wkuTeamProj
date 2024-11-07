namespace StickyCalender
{
    partial class main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.searchArea = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.memoArea = new System.Windows.Forms.GroupBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.settingArea = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.calenderArea = new System.Windows.Forms.GroupBox();
            this.memoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.refreshArea = new System.Windows.Forms.GroupBox();
            this.refresh = new System.Windows.Forms.Button();
            this.memoButtonArea = new System.Windows.Forms.GroupBox();
            this.trashCanArea = new System.Windows.Forms.GroupBox();
            this.trashButton = new System.Windows.Forms.Button();
            this.searchArea.SuspendLayout();
            this.memoArea.SuspendLayout();
            this.settingArea.SuspendLayout();
            this.refreshArea.SuspendLayout();
            this.memoButtonArea.SuspendLayout();
            this.trashCanArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(131, 21);
            this.textBox1.TabIndex = 0;
            // 
            // searchArea
            // 
            this.searchArea.Controls.Add(this.button1);
            this.searchArea.Controls.Add(this.textBox1);
            this.searchArea.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.searchArea.Location = new System.Drawing.Point(6, 619);
            this.searchArea.Name = "searchArea";
            this.searchArea.Size = new System.Drawing.Size(209, 50);
            this.searchArea.TabIndex = 1;
            this.searchArea.TabStop = false;
            this.searchArea.Text = "검색";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(136, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 21);
            this.button1.TabIndex = 2;
            this.button1.Text = "검색";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Search_clicked);
            // 
            // memoArea
            // 
            this.memoArea.Controls.Add(this.memoButtonArea);
            this.memoArea.Controls.Add(this.memoPanel);
            this.memoArea.Controls.Add(this.searchArea);
            this.memoArea.Location = new System.Drawing.Point(12, 68);
            this.memoArea.Name = "memoArea";
            this.memoArea.Size = new System.Drawing.Size(383, 675);
            this.memoArea.TabIndex = 2;
            this.memoArea.TabStop = false;
            this.memoArea.Text = "메모";
            // 
            // deleteButton
            // 
            this.deleteButton.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.deleteButton.Location = new System.Drawing.Point(78, 18);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "메모 삭제";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.Delete_Clicked);
            // 
            // addButton
            // 
            this.addButton.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.addButton.Location = new System.Drawing.Point(3, 18);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "메모 추가";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.Add_Clicked);
            // 
            // settingArea
            // 
            this.settingArea.Controls.Add(this.button2);
            this.settingArea.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.settingArea.Location = new System.Drawing.Point(343, 12);
            this.settingArea.Name = "settingArea";
            this.settingArea.Size = new System.Drawing.Size(52, 50);
            this.settingArea.TabIndex = 3;
            this.settingArea.TabStop = false;
            this.settingArea.Text = "설정";
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ImageKey = "setting";
            this.button2.ImageList = this.imageList1;
            this.button2.Location = new System.Drawing.Point(6, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 25);
            this.button2.TabIndex = 0;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Setting_Clicked);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "setting");
            this.imageList1.Images.SetKeyName(1, "refresh.png");
            this.imageList1.Images.SetKeyName(2, "trash.png");
            // 
            // calenderArea
            // 
            this.calenderArea.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.calenderArea.Location = new System.Drawing.Point(401, 12);
            this.calenderArea.Name = "calenderArea";
            this.calenderArea.Size = new System.Drawing.Size(887, 731);
            this.calenderArea.TabIndex = 4;
            this.calenderArea.TabStop = false;
            this.calenderArea.Text = "달력";
            // 
            // memoPanel
            // 
            this.memoPanel.Location = new System.Drawing.Point(6, 20);
            this.memoPanel.Name = "memoPanel";
            this.memoPanel.Size = new System.Drawing.Size(371, 593);
            this.memoPanel.TabIndex = 0;
            // 
            // refreshArea
            // 
            this.refreshArea.Controls.Add(this.refresh);
            this.refreshArea.Font = new System.Drawing.Font("굴림", 8.5F, System.Drawing.FontStyle.Bold);
            this.refreshArea.Location = new System.Drawing.Point(227, 12);
            this.refreshArea.Name = "refreshArea";
            this.refreshArea.Size = new System.Drawing.Size(52, 50);
            this.refreshArea.TabIndex = 4;
            this.refreshArea.TabStop = false;
            this.refreshArea.Text = "새로고침";
            // 
            // refresh
            // 
            this.refresh.FlatAppearance.BorderSize = 0;
            this.refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refresh.ImageKey = "refresh.png";
            this.refresh.ImageList = this.imageList1;
            this.refresh.Location = new System.Drawing.Point(6, 19);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(40, 25);
            this.refresh.TabIndex = 0;
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // memoButtonArea
            // 
            this.memoButtonArea.Controls.Add(this.addButton);
            this.memoButtonArea.Controls.Add(this.deleteButton);
            this.memoButtonArea.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.memoButtonArea.Location = new System.Drawing.Point(218, 619);
            this.memoButtonArea.Name = "memoButtonArea";
            this.memoButtonArea.Size = new System.Drawing.Size(159, 50);
            this.memoButtonArea.TabIndex = 3;
            this.memoButtonArea.TabStop = false;
            this.memoButtonArea.Text = "메모 조정";
            // 
            // trashCanArea
            // 
            this.trashCanArea.Controls.Add(this.trashButton);
            this.trashCanArea.Font = new System.Drawing.Font("굴림", 8.5F, System.Drawing.FontStyle.Bold);
            this.trashCanArea.Location = new System.Drawing.Point(285, 12);
            this.trashCanArea.Name = "trashCanArea";
            this.trashCanArea.Size = new System.Drawing.Size(52, 50);
            this.trashCanArea.TabIndex = 5;
            this.trashCanArea.TabStop = false;
            this.trashCanArea.Text = "휴지통";
            // 
            // trashButton
            // 
            this.trashButton.FlatAppearance.BorderSize = 0;
            this.trashButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.trashButton.ImageKey = "trash.png";
            this.trashButton.ImageList = this.imageList1;
            this.trashButton.Location = new System.Drawing.Point(6, 19);
            this.trashButton.Name = "trashButton";
            this.trashButton.Size = new System.Drawing.Size(40, 25);
            this.trashButton.TabIndex = 0;
            this.trashButton.UseVisualStyleBackColor = true;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 755);
            this.Controls.Add(this.trashCanArea);
            this.Controls.Add(this.refreshArea);
            this.Controls.Add(this.calenderArea);
            this.Controls.Add(this.settingArea);
            this.Controls.Add(this.memoArea);
            this.Name = "main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.searchArea.ResumeLayout(false);
            this.searchArea.PerformLayout();
            this.memoArea.ResumeLayout(false);
            this.settingArea.ResumeLayout(false);
            this.refreshArea.ResumeLayout(false);
            this.memoButtonArea.ResumeLayout(false);
            this.trashCanArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox searchArea;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox memoArea;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.GroupBox settingArea;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox calenderArea;
        private System.Windows.Forms.FlowLayoutPanel memoPanel;
        private System.Windows.Forms.GroupBox refreshArea;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.GroupBox memoButtonArea;
        private System.Windows.Forms.GroupBox trashCanArea;
        private System.Windows.Forms.Button trashButton;
    }
}

