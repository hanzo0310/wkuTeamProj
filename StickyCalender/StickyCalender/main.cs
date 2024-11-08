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


/*for (int i = 1; i <= 42; i++)
            {
                // 각 ListBox의 이름을 동적으로 설정
                string listBoxName = $"DateBox{i}";

                // calenderArea GroupBox에서 해당 이름의 ListBox를 찾음
                ListBox listBox = calenderArea.Controls.Find(listBoxName, true).FirstOrDefault() as ListBox;

                if (listBox != null)
                {
                    // 원하는 작업 수행
                    listBox.Items.Add("Some data");
                }
            }*/

/*
 
프로그램 작성 가이드라인

1. 데이터 파일 작성 형식
날짜에 맞는 파일이 있는지 먼저 체크.
내용 줄 바꿈시 **로 표기하겠음.

2. 달력 한칸 
종류 뒤에 숫자를 붙혀서 구분
ex) dateBox1

종류 :
칸 ( 그룹박스 ) - dateBox
라디오버튼 - dateButton
데이터 - dateData
*/

namespace StickyCalender
{
    public partial class main : Form
    {

        private bool isChanging = false;
        static int Syear, Smonth;


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

        public void MoveFile(int fromto, string fileName)
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
            else if (fromto == 0) // 휴지통에서 복구
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

        private List<string> LoadDataToListBox(string[,] data)
        {
            List<string> formattedData = new List<string>();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                string fileName = data[i, 0];  // 파일명
                string content = data[i, 1];  // 파일 내용

                // 파일명과 내용이 합쳐진 형식으로 리스트에 추가
                formattedData.Add($"{fileName}: {content}");
            }

            return formattedData;
        }


        //----------------------------------< 폼 응 용  함 수 >----------------------------------
        private void memoPreview(string date, string detail = "비어있는 메모") //메모 추가
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Text = "";
            groupBox.Location = new Point(10, 10);
            groupBox.Width = memoPanel.Width - 30;

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

        private void memoLoad()//메모 불러오기 memoPreview를 이용하여 존재하는 파일 전부를 읽어드림.
        {
            string[,] result = data_loading();
            for (int i = 0; i < result.GetLength(0); i++)
            {
                if (result[i, 0] == null) break;
                memoPreview(result[i, 0], result[i, 1]);
            }

            if (checkEmpty() == 0) deleteButton.Enabled = false;
        }

        private int checkEmpty() // 메모 패널에 존재하는 메모가 있는지를 확인함.
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

        private void writeDateBox(int kind, int target, string newText)
        {
            if (kind == 0)  // 라디오버튼 텍스트 수정
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

                if (dateBox != null)
                {
                    RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton;

                    if (dateRadioButton != null)
                    {
                        // 라디오버튼의 텍스트 수정
                        dateRadioButton.Text = newText;
                        dateRadioButton.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show($"dateButton{target} 라디오버튼을 찾을 수 없습니다.");
                    }
                }
                else
                {
                    MessageBox.Show($"dateBox{target} 그룹박스를 찾을 수 없습니다.");
                }
            }
            else if (kind == 1)  // 리스트박스 항목 추가
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

                if (dateBox != null)
                {
                    ListBox dateData = dateBox.Controls[$"dateData{target}"] as ListBox;

                    if (dateData != null)
                    {
                        // 줄바꿈을 포함한 텍스트를 그대로 추가
                        // 만약 데이터를 여러 줄로 구분해야 한다면 \r\n을 사용하여 구분
                        // 예시: newText에서 줄바꿈이 있을 경우 그대로 삽입
                        string formattedText = newText.Replace("\n", "\r\n");
                        dateData.Items.Add(formattedText);
                    }
                    else
                    {
                        MessageBox.Show($"dateData{target} 리스트박스를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    MessageBox.Show($"dateBox{target} 그룹박스를 찾을 수 없습니다.");
                }
            }
        }

        private void InsertDataIntoListBox(int target, string data)
        {
            GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;
            if (dateBox != null)
            {
                ListBox dateData = dateBox.Controls[$"dateData{target}"] as ListBox;

                if (dateData != null)
                {
                    // 줄바꿈을 기준으로 데이터를 나누어 각 줄을 ListBox에 추가
                    string[] dataLines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                    foreach (string line in dataLines)
                    {
                        dateData.Items.Add(line); // 각 줄을 ListBox에 추가
                    }
                }
                else
                {
                    MessageBox.Show($"ListBox를 찾을 수 없습니다. target: {target}");
                }
            }
            else
            {
                MessageBox.Show($"GroupBox를 찾을 수 없습니다. target: {target}");
            }
        }

        private void changeRadioChecked(int target, bool tf)
        {
            if (isChanging) return; // 이미 상태가 변경 중이면 함수를 종료

            isChanging = true; // 상태 변경 중임을 표시

            GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

            if (dateBox != null)
            {
                RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton;

                if (dateRadioButton != null)
                {
                    dateRadioButton.Checked = tf;
                }
                else
                {
                    MessageBox.Show($"dateButton{target} 라디오버튼을 찾을 수 없습니다.");
                }
            }
            else
            {
                MessageBox.Show($"dateBox{target} 그룹박스를 찾을 수 없습니다.");
            }

            isChanging = false; // 상태 변경이 끝났음을 표시
        }

        private int[] CheckDateColumn()
        {
            List<int> matchingRows = new List<int>();  // 일치하는 행 번호를 저장할 리스트

            string[,] result = data_loading();  // 데이터를 불러옴
            int rowCount = result.GetLength(0); // 배열의 행의 수

            // result 배열의 1열을 순회
            for (int i = 0; i < rowCount; i++)
            {
                string dateStr = result[i, 0]; // 1열의 데이터 가져오기

                // YYYYMMDD 형식인지 정규 표현식으로 확인
                if (dateStr.Length == 8 && int.TryParse(dateStr, out _))  // 길이가 8이고, 숫자로만 구성되어 있는지 확인
                {
                    int dataYear = int.Parse(dateStr.Substring(0, 4)); // 년도 추출
                    int dataMonth = int.Parse(dateStr.Substring(4, 2)); // 월 추출

                    // 년도와 월이 일치하는지 확인
                    if (dataYear == Syear && dataMonth == Smonth)
                    {
                        matchingRows.Add(i);  // 일치하는 행 번호를 리스트에 추가
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid date format: {dateStr}");
                }
            }

            // 리스트를 배열로 변환하여 반환
            return matchingRows.ToArray();
        }

        private void calenderReset(int year = -1, int month = -1, int day = -1) // 캘린더를 지정한 날짜의 모양으로 초기화 함 ( 미입력시 오늘 날짜로 설정함 )
        {
            string dayName = "Monday";
            int targetP;
            DateTime date;
            if (year > 2099 || year < 1990) year = -1;
            if (month > 12) month = -1;
            if (day > 31) day = -1;


            if (year == -1 || month == -1 || day == -1)
            {
                date = DateTime.Now;
            }
            else
            {
                date = new DateTime(year, month, day);
            }

            DayOfWeek dayOfWeek = date.DayOfWeek;
            year = date.Year;
            month = date.Month;
            day = date.Day;
            dayName = dayOfWeek.ToString();
            switch (dayName)
            {
                case "Sunday":
                    targetP = 1;
                    break;
                case "Monday":
                    targetP = 2;
                    break;
                case "Tuesday":
                    targetP = 3;
                    break;
                case "Wednesday":
                    targetP = 4;
                    break;
                case "Thursday":
                    targetP = 5;
                    break;
                case "Friday":
                    targetP = 6;
                    break;
                case "Saturday":
                    targetP = 7;
                    break;
                default:
                    targetP = -1;
                    break;
            }

            date = new DateTime(year, month, 1);
            dayOfWeek = date.DayOfWeek;
            String startDayName = dayOfWeek.ToString();
            int StartP;
            switch (startDayName)
            {
                case "Sunday":
                    StartP = 1;
                    break;
                case "Monday":
                    StartP = 2;
                    break;
                case "Tuesday":
                    StartP = 3;
                    break;
                case "Wednesday":
                    StartP = 4;
                    break;
                case "Thursday":
                    StartP = 5;
                    break;
                case "Friday":
                    StartP = 6;
                    break;
                case "Saturday":
                    StartP = 7;
                    break;
                default:
                    StartP = -1;
                    break;
            }
            int lastDay = DateTime.DaysInMonth(year, month);
            int j = 1;

            for (int i = StartP; i < lastDay + StartP; i++)
            {
                writeDateBox(0, i, j.ToString());
                j++;
            }


            String newDate = year + "년 " + month + "월 " + day + "일";
            changeDateLabel(newDate);

            Syear = year; Smonth = month;

            string[,] loadingData = data_loading();
            int[] match = CheckDateColumn();

            foreach (int index in match)
            {
                string dateStr = loadingData[index, 0];  // 일치하는 날짜의 데이터 (YYYYMMDD 형식)

                // 각 날짜별로 데이터를 삽입 (예: dateStr이 '20231109'이라면, 해당 날짜에 맞는 데이터 삽입)
                int target = int.Parse(dateStr.Substring(6, 2)); // 날짜에서 일 부분을 추출하여 target에 맞게 설정 (09일이면 target=9)

                // 데이터를 삽입 (예: 'data'는 해당 날짜에 대한 상세 정보)
                string data = loadingData[index, 1];  // 예시로 2번째 열이 데이터라 가정

                // 데이터를 줄바꿈을 포함하여 해당 날짜의 ListBox에 추가
                InsertDataIntoListBox(target, data);  // writeDateBox 대신 InsertDataIntoListBox 사용
            }


        }
        private void changeDateLabel(String text)
        {
            // Calender_DateArea 그룹박스 안에서 Calender_Date라는 이름을 가진 LinkLabel을 찾음
            LinkLabel calenderDate = Calender_DateArea.Controls["Calender_Date"] as LinkLabel;

            // LinkLabel이 존재하는지 확인
            if (calenderDate != null)
            {
                calenderDate.Text = text; // 새로운 텍스트 값으로 변경
            }
            else
            {
                MessageBox.Show("Calender_Date 라는 LinkLabel을 찾을 수 없습니다.");
            }
        }



        //----------------------------------< 폼  함 수 >----------------------------------
        public main()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            memoLoad();
            Calender_Date.Location = new Point((Calender_DateArea.Width / 2 - Calender_Date.Width / 2), 17); // 캘린더 날짜 위치 조정
            calenderReset(2024, 11, 15);

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

            if (checkEmpty() == 0) deleteButton.Enabled = false;
            else deleteButton.Enabled = true;
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

        private void radioChecked(object sender, EventArgs e)
        {
            RadioButton R = sender as RadioButton;

            if (R != null)
            {
                // "dateButton"을 제거하여 숫자만 남기고 정수로 변환
                string radioButtonName = R.Name.Replace("dateButton", "");
                int buttonNumber;
                if (int.TryParse(radioButtonName, out buttonNumber))
                {
                    // 다른 모든 라디오 버튼을 끄는 코드
                    for (int i = 1; i <= 42; i++)
                    {
                        changeRadioChecked(i, false);
                    }
                    // 현재 라디오 버튼만 켬
                    changeRadioChecked(buttonNumber, true);
                }
            }
        }
    }
}
