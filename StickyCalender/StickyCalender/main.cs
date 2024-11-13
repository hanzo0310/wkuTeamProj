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
        private bool isRadioChanging = false;
        static int Syear, Smonth, Sday;


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

                    // 파일 이름을 "_" 기준으로 분리하여 날짜와 제목을 추출
                    string[] parts = fileName.Split('_');
                    if (parts.Length == 2 && DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                    {
                        string datePart = parts[0]; // 날짜 부분
                        string titlePart = parts[1]; // 제목 부분

                        // 파일 내용을 읽어와서 변수에 저장
                        string content = File.ReadAllText(filePath);

                        // 날짜, 제목, 내용으로 구성된 배열을 리스트에 추가
                        dataList.Add(new string[] { datePart, titlePart, content });
                    }
                }

                // 날짜(파일 이름)를 기준으로 리스트를 오름차순 정렬
                dataList.Sort((x, y) => x[0].CompareTo(y[0]));

                // 정렬된 데이터를 2차원 배열로 변환
                string[,] sortedData = new string[dataList.Count, 3];
                for (int i = 0; i < dataList.Count; i++)
                {
                    sortedData[i, 0] = dataList[i][0]; // 날짜
                    sortedData[i, 1] = dataList[i][1]; // 제목
                    sortedData[i, 2] = dataList[i][2]; // 내용
                }

                return sortedData;
            }
            else
            {
                MessageBox.Show("데이터 폴더를 찾을 수 없습니다. 경로를 확인하세요.");
                return new string[,] { { "error", "path", "" } };
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
        private void memoPreview(string title, string detail = "비어있는 메모") //메모 추가
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Text = "";
            groupBox.Location = new Point(10, 10);
            groupBox.Width = memoPanel.Width - 30;

            CheckBox checkBox = new CheckBox();
            checkBox.Text = title;
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

        private void memoLoad(int year = -1, int month = -1, int day = -1) // 특정 날짜에 해당하는 메모만 불러오기
        {
            int cnt = 1;

            if (year == -1) year = Syear;
            if (month == -1) month = Smonth;
            if(day == -1) day = Sday;

            // year, month, day를 YYYYMMDD 형식의 문자열로 변환
            string targetDate = $"{year:D4}{month:D2}{day:D2}";

            string[,] result = data_loading();
            for (int i = 0; i < result.GetLength(0); i++)
            {
                // 해당 날짜와 일치하는 경우에만 메모 불러오기
                if (result[i, 0] == targetDate)
                {
                    memoPreview($"{cnt}번째 메모", result[i, 1]);
                    cnt++;
                }
            }

            // 불러온 메모가 없는 경우 삭제 버튼 비활성화
            if (checkEmpty() == 0) deleteButton.Enabled = false;
        }

        private void resetVar()
        {
            DateTime date = DateTime.Now;
            Syear = date.Year;
            Smonth = date.Month;
            Sday = date.Day;
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

        private void writeDateBox(int kind, int target, string newText = "")
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
            else if (kind == 2)  // 라디오버튼 비활성화 그리고 내용 삭제
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

                if (dateBox != null)
                {
                    RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton;

                    if (dateRadioButton != null)
                    {
                        dateRadioButton.Text = "";
                        dateRadioButton.Enabled = false;
                        
                    }
                    else
                    {
                        MessageBox.Show($"dateButton{target} 라디오버튼을 찾을 수 없습니다.");
                    }

                    ListBox dateData = dateBox.Controls[$"dateData{target}"] as ListBox;

                    if (dateData != null)
                    {
                        dateData.Items.Clear();
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
            /*
             * 함수 내용 :
             * 리스트 삽입에 알맞도록 불러온 데이터를 변환하고 삽입하는 함수.
             */
            GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;
            if (dateBox != null)
            {
                ListBox dateData = dateBox.Controls[$"dateData{target}"] as ListBox;

                if (dateData != null)
                {
                    string[] dataLines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                    foreach (string line in dataLines)
                    {
                        dateData.Items.Add(line);
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
            /*
             * 함수 내용 :
             * 라디오 버튼을 특정하여 해당 라디오 버튼의 선택여부를 임의로 조정하는 함수.
             */
            if (isChanging) return;

            isChanging = true;

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

        private int[] checkRadioButtonText()
        {
            int[] returnArr = new int[42];

            for(int target = 1; target <= 42; target++)
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

                if (dateBox != null)
                {
                    RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton;

                    if (dateRadioButton != null)
                    {
                        if (int.TryParse(dateRadioButton.Text, out int parsedValue))
                        {
                            returnArr[target-1] = parsedValue-1;
                        }
                        else
                        {
                            returnArr[target-1] = -1;
                        }
                    }
                }
            }
            return returnArr;
        }

        private int[] CheckDateColumn()
        {
            /*
             * 함수 내용 :
             * 현재 시스템에 설정되어 있는 년/월과 일치하는 파일이 있는지를 찾는 함수.
             * 설정시간과 동일한 시점의 파일이 있는 행의 번호를 리스트로 모아서 리턴함.
             */
            List<int> matchingRows = new List<int>();

            string[,] result = data_loading();
            int rowCount = result.GetLength(0);

            for (int i = 0; i < rowCount; i++)
            {
                string dateStr = result[i, 0];

                if (dateStr.Length == 8 && int.TryParse(dateStr, out _))
                {
                    int dataYear = int.Parse(dateStr.Substring(0, 4));
                    int dataMonth = int.Parse(dateStr.Substring(4, 2));

                    if (dataYear == Syear && dataMonth == Smonth)
                    {
                        matchingRows.Add(i);
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

        private void calenderReset(int year = -1, int month = -1, int day = -1) // 캘린더를 지정한 날짜의 모양으로 초기화 함 ( 미입력시 오늘 날짜로 설정함 ㅡ )
        {
            /*
             * 함수 내용 ( 순차별 ) :
             * 
             * 1. 캘린더 날짜 표기 라벨 위치 조정
             * 2. 각종 변수 초기화
             * -> 미기입 or 오류값 입력 시, 현재 날짜를 기준으로 수정 ( ex : 2024년 08월 12일에 실행 했는데 월을 미기입 시, month 값이 08로 설정됨 )
             * 3. 달력 미사용부 비활성화
             * 4. 달력 사용부 활성화 및 라디오버튼에 날짜 값 기입
             * 5. 달력 미사용부 비활성화
             * 6. 날짜 표기 라벨 텍스트 값 새로 조정
             * 7. 전역변수 Syear / Smonth / Sday 값 변경
             * 8. 해당 월에 있는 메모들 데이터 칸에 입력.
             */
            Calender_Date.Location = new Point((Calender_DateArea.Width / 2 - Calender_Date.Width / 2), 17); // 캘린더 날짜 위치 조정

            string dayName = "Monday";
            int targetP;

            DateTime date = DateTime.Now;
            if (year > 2099 || year < 1990) year = -1;
            if (month > 12) month = -1;
            if (day > 31) day = -1;

            if (year == -1)year = date.Year;
            if (month == -1)month = date.Month;
            if (day == -1)day = date.Day;
            
            date = new DateTime(year, month, day);
           

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

            for (int i = 1; i <= 42; i++)
            {
                writeDateBox(2, i);
            }

            int j = 1;

            for (int i = 1; i <= StartP; i++)
            {
                writeDateBox(2, i);
            }

            for (int i = StartP; i < lastDay + StartP; i++)
            {
                writeDateBox(0, i, j.ToString());
                j++;
            }

            for(int i = lastDay+StartP; i<=42; i++)
            {
                writeDateBox(2, i);
            }


            String newDate = year + "년 " + month + "월 " + day + "일";
            changeDateLabel(newDate);

            Syear = year; Smonth = month; Sday = day;

            string[,] loadingData = data_loading();
            int[] match = CheckDateColumn();

            foreach (int index in match)
            {
                string dateStr = loadingData[index, 0];

                
                int target = int.Parse(dateStr.Substring(6, 2));

                
                string data = loadingData[index, 1];

                
                InsertDataIntoListBox(target+StartP-1, data);
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

        private void refreshActive()
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



        //----------------------------------< 폼  함 수 >----------------------------------
        public main()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            resetVar();
            calenderReset();
            checkRadioButtonText();
            memoLoad(Syear, Smonth,Smonth);

        }

        private void Search_clicked(object sender, EventArgs e)
        {

        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            // 기본 형태는 다음과 같음.
            WriteFile("","");
            add_memo add_Memo = new add_memo();
            add_Memo.ShowDialog();
        }

        private void Setting_Clicked(object sender, EventArgs e)
        {
            //기본 형태는 다음과 같음.
            setting setting =  new setting();
            setting.ShowDialog();
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

            calenderReset(Syear,Smonth,Sday);
        }


        private void refresh_Click(object sender, EventArgs e)
        {
            refreshActive();
        }

        private void trashButton_Click(object sender, EventArgs e)
        {
            memo_trash memo_Trash = new memo_trash();
            memo_Trash.ShowDialog();
        }

        private void ListDoubleClicked(object sender, EventArgs e)
        {
            MessageBox.Show("통과");
        }

        private void ListClick(object sender, EventArgs e)
        {
            ListBox LB = sender as ListBox;
            LB.ClearSelected();
        }

        private void MonthButtonClicked(object sender, EventArgs e)
        {
            Button B = sender as Button;
            if (B != null)
            {
                if (B == DateLeftShift)
                {
                    if (Smonth == 1)
                    {
                        Syear--;
                        Smonth = 12;
                    }
                    else Smonth--;
                    
                    
                    calenderReset(Syear, Smonth, Sday); 
                }
                if (B == DateRightShift)
                {
                    if(Smonth == 12)
                    {
                        Syear++;
                        Smonth = 1;
                    }
                    else Smonth++;
                    calenderReset(Syear, Smonth, Sday);
                }
            }
            
        }

        private void radioChecked(object sender, EventArgs e)
        {
            if (isRadioChanging) return; // 재귀 호출 방지
            isRadioChanging = true; // 플래그 설정

            RadioButton R = sender as RadioButton;
            string radioButtonName;
            int buttonNumber = 0;

            if (R != null)
            {
                // "dateButton"을 제거하여 숫자만 남기고 정수로 변환
                radioButtonName = R.Name.Replace("dateButton", "");

                if (int.TryParse(radioButtonName, out buttonNumber))
                {
                    // 다른 모든 라디오 버튼을 끄는 코드
                    for (int i = 1; i <= 42; i++)
                    {
                        if (buttonNumber != i)
                        {
                            changeRadioChecked(i, false); // changeRadioChecked에서는 기존의 isChanging 플래그를 사용
                        }
                    }                    
                }
            }

            int[] radioButtonValue = checkRadioButtonText();
            Sday = radioButtonValue[buttonNumber];

            refreshActive(); // 추가 작업 호출

            isRadioChanging = false; // 플래그 해제
        }
    }
}
