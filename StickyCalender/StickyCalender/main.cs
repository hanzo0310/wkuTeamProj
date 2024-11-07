using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


/*
프로그램 작성 가이드라인

1. 데이터 파일 작성 형식
날짜에 맞는 파일이 있는지 먼저 체크.
내용 줄 바꿈시 **로 표기하겠음.

2.
*/

namespace StickyCalender
{
    public partial class main : Form
    {
        //----------------------------------< 공 용 함 수 >----------------------------------
        public string[,] data_loading()
        {
            string relativePath = @"..\..\data";
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            List<string[]> dataList = new List<string[]>();

            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath, "*.txt");

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    if (DateTime.TryParseExact(fileName, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                    {
                        string content = File.ReadAllText(filePath);
                        dataList.Add(new string[] { fileName, content });
                    }
                    else
                    {
                        dataList.Add(new string[] { fileName, "filename_error" });
                    }
                }

                dataList.Sort((x, y) => x[0].CompareTo(y[0]));

                string[,] sortedData = new string[dataList.Count, 2];
                for (int i = 0; i < dataList.Count; i++)
                {
                    sortedData[i, 0] = dataList[i][0];
                    sortedData[i, 1] = dataList[i][1];
                }

                return sortedData;
            }
            else
            {
                MessageBox.Show("데이터 폴더를 찾을 수 없습니다. 경로를 확인하세요.");
                return new string[,] { { "error", "path" } };
            }
        }

        public void WriteFile(String date, String data)
        {

        }

        public void DeleteFile(string fileName)
        {
            string relativePath = Path.Combine("../../data", fileName);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"파일 삭제에 실패했습니다. 오류 내용 : {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("파일 찾을 수 없음.");
                return;
            }
        }

        public void MoveFile(int fromto,string fileName)
        {
            if (fromto == 1) // 휴지통으로 이동
            {
                string from = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\data", fileName);
                string to = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\trash", fileName);

                if (File.Exists(from))
                {
                    try
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(to)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(to));
                        }

                        File.Move(from, to);
                        //MessageBox.Show($"파일 '{fileName}'이(가) trash 폴더로 이동되었습니다.");
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"파일 이동 중 오류가 발생했습니다. 오류: {ex.Message}");
                    }
                }
                else
                {
                    //MessageBox.Show($"파일 '{fileName}'을(를) 찾을 수 없습니다.");
                }
            }
            else if(fromto == 0) // 휴지통에서 복구
            {
                string from = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\trash", fileName);
                string to = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\data", fileName);

                if (File.Exists(from))
                {
                    try
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(to)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(to));
                        }

                        File.Move(from, to);
                        MessageBox.Show($"파일 '{fileName}'이(가) trash 폴더로 이동되었습니다.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"파일 이동 중 오류가 발생했습니다. 오류: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show($"파일 '{fileName}'을(를) 찾을 수 없습니다.");
                }
            }
        }


        public async Task blank()
        {
            Thread.Sleep(500);
        }

        //----------------------------------< 폼 응 용  함 수 >----------------------------------
        private void memoPreview(string date, string detail = "비어있는 메모")
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Text = "";
            groupBox.Location = new Point(10, 10);
            groupBox.Width = memoPanel.Width - 20;

            CheckBox checkBox = new CheckBox();
            checkBox.Text = date;
            checkBox.Location = new Point(10, 10);
            checkBox.Width = groupBox.Width - 20;

            Label label = new Label();
            label.Text = detail;
            label.Location = new Point(10, 40);
            label.Width = groupBox.Width - 20;

            label.AutoSize = false;
            label.MaximumSize = new Size(groupBox.Width - 20, 0);
            label.TextAlign = ContentAlignment.TopLeft;

            label.Height = TextRenderer.MeasureText(label.Text, label.Font).Height + 5;

            groupBox.Height = checkBox.Height + label.Height + 20;

            groupBox.Controls.Add(checkBox);
            groupBox.Controls.Add(label);

            memoPanel.Controls.Add(groupBox);
        }

        private void memoLoad()
        {
            string[,] result = data_loading();
            for (int i = 0; i < result.GetLength(0); i++)
            {
                if (result[i, 0] == null) break;
                memoPreview(result[i, 0], result[i, 1]);
            }

            if (checkEmpty() == 0) deleteButton.Enabled = false;
        }

        private int checkEmpty()
        {
            foreach (Control control in memoPanel.Controls)
            {
                if (control is GroupBox)
                {
                    return 1;
                }
            }
            return 0;
        }


        //----------------------------------< 폼  함 수 >----------------------------------
        public main()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            memoLoad();
        }

        private void Search_clicked(object sender, EventArgs e)
        {

        }

        private void Add_Clicked(object sender, EventArgs e)
        {

        }

        private void Setting_Clicked(object sender, EventArgs e)
        {

        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            List<GroupBox> groupBoxesToDelete = new List<GroupBox>();

            foreach (Control control in memoPanel.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    CheckBox checkBox = groupBox.Controls.OfType<CheckBox>().FirstOrDefault();

                    if (checkBox != null && checkBox.Checked)
                    {
                        groupBoxesToDelete.Add(groupBox);
                    }
                }
            }

            foreach (GroupBox groupBox in groupBoxesToDelete)
            {
                CheckBox checkBox = groupBox.Controls.OfType<CheckBox>().FirstOrDefault();
                if (checkBox != null)
                {
                    string fileName = checkBox.Text + ".txt";
                    MoveFile(1, fileName);
                }

                memoPanel.Controls.Remove(groupBox);
                groupBox.Dispose();
            }

            if (checkEmpty() == 0) deleteButton.Enabled=false;
            else deleteButton.Enabled=true;
        }


        private void refresh_Click(object sender, EventArgs e)
        {
            foreach (Control control in memoPanel.Controls.Cast<Control>().ToList())
            {
                if (control is GroupBox groupBox)
                {
                    memoPanel.Controls.Remove(groupBox);
                    groupBox.Dispose();
                }
            }
            
            memoLoad();

            if (checkEmpty() == 0) deleteButton.Enabled = false;
            else deleteButton.Enabled = true;

        }
    }
}
