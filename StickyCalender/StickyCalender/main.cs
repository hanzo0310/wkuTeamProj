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

// 프로그램 가이드라인에 따라 데이터 파일 형식 및 달력 구성 요소에 대한 설명을 포함함.

namespace StickyCalender
{
    public partial class main : Form
    {
        // 전역 변수 선언
        private bool isChanging = false; // 라디오 버튼 상태가 변경 중인지 확인하는 플래그 변수
        private bool isRadioChanging = false; // 라디오 버튼의 재귀 호출을 방지하기 위한 플래그 변수
        static int Syear, Smonth, Sday; // 현재 설정된 연도, 월, 일을 나타내는 정적 변수

        //----------------------------------< 공 용 함 수 >----------------------------------
        // 데이터 로딩 메서드
        public string[,] data_loading()
        {
            // 상대 경로로 데이터 폴더 경로 설정
            string relativePath = @"..\..\data";
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            List<string[]> dataList = new List<string[]>(); // 데이터 저장을 위한 리스트

            if (Directory.Exists(folderPath))
            {
                // .txt 파일 목록을 가져옴
                string[] files = Directory.GetFiles(folderPath, "*.txt");

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath); // 파일명에서 확장자 제거

                    // "_"를 기준으로 파일명을 분리하여 날짜와 제목 추출
                    string[] parts = fileName.Split('_');
                    if (parts.Length == 2 && DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                    {
                        string datePart = parts[0]; // 날짜 부분
                        string titlePart = parts[1]; // 제목 부분

                        // 파일 내용을 읽어서 저장
                        string content = File.ReadAllText(filePath);

                        // 날짜, 제목, 내용 배열을 리스트에 추가
                        dataList.Add(new string[] { datePart, titlePart, content });
                    }
                }

                // 날짜를 기준으로 오름차순 정렬
                dataList.Sort((x, y) => x[0].CompareTo(y[0]));

                // 2차원 배열로 변환하여 반환
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
                // 데이터 폴더를 찾지 못한 경우 메시지 출력
                MessageBox.Show("데이터 폴더를 찾을 수 없습니다. 경로를 확인하세요.");
                return new string[,] { { "error", "path", "" } }; // 에러 메시지 반환
            }
        }

        // 파일 쓰기 메서드 (현재 비어있음)
        public void WriteFile(String date, String data)
        {
        }

        // 파일 삭제 메서드
        public void DeleteFile(string fileName)
        {
            // 상대 경로를 사용하여 파일 경로 설정
            string relativePath = Path.Combine("../../data", fileName);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath); // 파일 삭제 시도
                }
                catch (Exception ex)
                {
                    // 오류 발생 시 메시지 출력
                    MessageBox.Show($"파일 삭제에 실패했습니다. 오류 내용 : {ex.Message}");
                }
            }
            else
            {
                // 파일을 찾지 못한 경우 메시지 출력
                Console.WriteLine("파일 찾을 수 없음.");
                return;
            }
        }

        // 파일 이동 메서드 (휴지통으로 이동하거나 복구)
        public void MoveFile(int fromto, string fileName)
        {
            if (fromto == 1) // 휴지통으로 이동
            {
                // 데이터 폴더에서 휴지통 폴더로 이동
                string from = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\data", fileName);
                string to = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\trash", fileName);

                if (File.Exists(from))
                {
                    try
                    {
                        // 대상 디렉터리가 존재하지 않으면 생성
                        if (!Directory.Exists(Path.GetDirectoryName(to)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(to));
                        }

                        File.Move(from, to); // 파일 이동
                    }
                    catch (Exception ex)
                    {
                        // 이동 중 오류 발생 시 메시지 출력
                        MessageBox.Show($"파일 이동 중 오류가 발생했습니다. 오류: {ex.Message}");
                    }
                }
                else
                {
                    // 파일을 찾지 못한 경우 메시지 출력
                    MessageBox.Show($"파일 '{fileName}'을(를) 찾을 수 없습니다.");
                }
            }
            else if (fromto == 0) // 휴지통에서 복구
            {
                // 휴지통 폴더에서 데이터 폴더로 이동
                string from = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\trash", fileName);
                string to = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\data", fileName);

                if (File.Exists(from))
                {
                    try
                    {
                        // 대상 디렉터리가 존재하지 않으면 생성
                        if (!Directory.Exists(Path.GetDirectoryName(to)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(to));
                        }

                        File.Move(from, to); // 파일 이동
                        MessageBox.Show($"파일 '{fileName}'이(가) trash 폴더로 이동되었습니다.");
                    }
                    catch (Exception ex)
                    {
                        // 이동 중 오류 발생 시 메시지 출력
                        MessageBox.Show($"파일 이동 중 오류가 발생했습니다. 오류: {ex.Message}");
                    }
                }
                else
                {
                    // 파일을 찾지 못한 경우 메시지 출력
                    MessageBox.Show($"파일 '{fileName}'을(를) 찾을 수 없습니다.");
                }
            }
        }

        // 비동기 메서드 (현재 지연 시간 추가만 함)
        public async Task blank()
        {
            Thread.Sleep(500); // 500밀리초 동안 지연
        }

        // ListBox에 데이터를 로드하는 메서드
        private List<string> LoadDataToListBox(string[,] data)
        {
            List<string> formattedData = new List<string>();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                string fileName = data[i, 0];  // 파일명
                string content = data[i, 1];  // 파일 내용

                // 파일명과 내용의 형식으로 리스트에 추가
                formattedData.Add($"{fileName}: {content}");
            }

            return formattedData;
        }

        //----------------------------------< 폼 응 용  함 수 >----------------------------------

        // 메모를 미리보기로 추가하는 메서드
        private void memoPreview(string title, string detail = "비어있는 메모") // 기본 메모 값
        {
            GroupBox groupBox = new GroupBox(); // 그룹박스 생성
            groupBox.Text = "";
            groupBox.Location = new Point(10, 10); // 위치 설정
            groupBox.Width = memoPanel.Width - 30; // 너비 설정

            CheckBox checkBox = new CheckBox(); // 체크박스 생성
            checkBox.Text = title; // 체크박스 텍스트 설정
            checkBox.Location = new Point(10, 10); // 위치 설정
            checkBox.Width = groupBox.Width - 20; // 너비 설정

            Label label = new Label(); // 라벨 생성
            label.Text = detail; // 라벨 텍스트 설정
            label.Location = new Point(10, 40); // 위치 설정
            label.Width = groupBox.Width - 20; // 너비 설정

            // 라벨의 크기를 텍스트에 맞추도록 설정
            label.AutoSize = false;
            label.MaximumSize = new Size(groupBox.Width - 20, 0); // 최대 너비 제한
            label.TextAlign = ContentAlignment.TopLeft; // 텍스트 정렬 설정

            // 라벨 높이를 텍스트에 맞게 조정
            label.Height = TextRenderer.MeasureText(label.Text, label.Font).Height + 5;

            // 그룹박스의 높이를 체크박스와 라벨의 높이로 조정
            groupBox.Height = checkBox.Height + label.Height + 20;
            // 그룹박스에 체크박스와 라벨 추가
            groupBox.Controls.Add(checkBox);
            groupBox.Controls.Add(label);

            // memoPanel에 그룹박스 추가 (메모 목록 패널에 그룹박스를 동적으로 추가)
            memoPanel.Controls.Add(groupBox);
        }

        // 특정 날짜에 해당하는 메모를 불러오는 메서드
        private void memoLoad(int year = -1, int month = -1, int day = -1)
        {
            int cnt = 1; // 메모의 개수를 카운트하는 변수

            // 날짜가 제공되지 않은 경우 전역 변수 Syear, Smonth, Sday 값을 사용
            if (year == -1) year = Syear;
            if (month == -1) month = Smonth;
            if (day == -1) day = Sday;

            // 년/월/일을 "YYYYMMDD" 형식의 문자열로 변환
            string targetDate = $"{year:D4}{month:D2}{day:D2}";

            // 데이터 로딩
            string[,] result = data_loading();
            for (int i = 0; i < result.GetLength(0); i++)
            {
                // 로드된 데이터의 날짜와 일치하는 경우에만 메모를 불러옴
                if (result[i, 0] == targetDate)
                {
                    // 메모를 추가 (cnt번째 메모로 표시)
                    memoPreview($"{cnt}번째 메모", result[i, 1]);
                    cnt++;
                }
            }

            // 불러온 메모가 없는 경우 삭제 버튼 비활성화
            if (checkEmpty() == 0) deleteButton.Enabled = false;
        }

        // 날짜 변수를 초기화하는 메서드 (현재 날짜로 설정)
        private void resetVar()
        {
            DateTime date = DateTime.Now; // 현재 날짜와 시간
            Syear = date.Year;  // 연도 설정
            Smonth = date.Month; // 월 설정
            Sday = date.Day;    // 일 설정
        }

        // 메모 패널에 존재하는 메모가 있는지를 확인하는 메서드
        private int checkEmpty()
        {
            // memoPanel의 자식 컨트롤 중 GroupBox가 있는지 확인
            foreach (Control control in memoPanel.Controls)
            {
                if (control is GroupBox)
                {
                    return 1; // 메모가 존재하는 경우 1 반환
                }
            }
            return 0; // 메모가 없는 경우 0 반환
        }

        // 달력의 특정 날짜 칸에 데이터를 삽입하거나 삭제하는 메서드
        private void writeDateBox(int kind, int target, string newText = "")
        {
            if (kind == 0)  // 라디오 버튼 텍스트 수정
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox; // 타겟 GroupBox 가져오기

                if (dateBox != null)
                {
                    RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton; // 타겟 RadioButton 가져오기

                    if (dateRadioButton != null)
                    {
                        // 라디오 버튼의 텍스트를 새로운 값으로 수정
                        dateRadioButton.Text = newText;
                        dateRadioButton.Enabled = true; // 활성화
                    }
                    else
                    {
                        // 해당 라디오 버튼이 존재하지 않을 경우 메시지 출력
                        MessageBox.Show($"dateButton{target} 라디오버튼을 찾을 수 없습니다.");
                    }
                }
                else
                {
                    // 해당 그룹박스가 존재하지 않을 경우 메시지 출력
                    MessageBox.Show($"dateBox{target} 그룹박스를 찾을 수 없습니다.");
                }
            }
            else if (kind == 1)  // 리스트박스 항목 추가
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

                if (dateBox != null)
                {
                    ListBox dateData = dateBox.Controls[$"dateData{target}"] as ListBox; // 타겟 ListBox 가져오기

                    if (dateData != null)
                    {
                        // 줄바꿈을 윈도우 스타일에 맞춰 변경 후 항목 추가
                        string formattedText = newText.Replace("\n", "\r\n");
                        dateData.Items.Add(formattedText);
                    }
                    else
                    {
                        // 해당 ListBox가 존재하지 않을 경우 메시지 출력
                        MessageBox.Show($"dateData{target} 리스트박스를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    // 해당 GroupBox가 존재하지 않을 경우 메시지 출력
                    MessageBox.Show($"dateBox{target} 그룹박스를 찾을 수 없습니다.");
                }
            }
            else if (kind == 2)  // 라디오 버튼 비활성화 및 내용 삭제
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

                if (dateBox != null)
                {
                    RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton;

                    if (dateRadioButton != null)
                    {
                        // 라디오 버튼의 텍스트를 비우고 비활성화
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
                        // ListBox의 모든 항목을 삭제
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
        // ListBox에 데이터를 삽입하는 메서드
        private void InsertDataIntoListBox(int target, string data)
        {
            /*
             * 함수 설명:
             * 리스트 삽입에 적합하도록 데이터를 변환하고 ListBox에 삽입하는 기능을 수행.
             */
            GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;
            if (dateBox != null)
            {
                ListBox dateData = dateBox.Controls[$"dateData{target}"] as ListBox;

                if (dateData != null)
                {
                    // 데이터 문자열을 줄바꿈 기준으로 나누어 각 라인으로 분리
                    string[] dataLines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                    // 각 라인을 ListBox에 추가
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

        // 특정 라디오 버튼의 선택 상태를 변경하는 메서드
        private void changeRadioChecked(int target, bool tf)
        {
            /*
             * 함수 설명:
             * 특정 라디오 버튼의 선택 여부를 프로그램적으로 조정하기 위한 기능을 수행.
             */
            if (isChanging) return; // 상태 변경 중일 경우 중복 호출 방지
            isChanging = true; // 상태 변경 중임을 나타내는 플래그 설정

            GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

            if (dateBox != null)
            {
                RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton;

                if (dateRadioButton != null)
                {
                    dateRadioButton.Checked = tf; // 라디오 버튼의 체크 상태를 설정
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

            isChanging = false; // 상태 변경이 끝났음을 나타내는 플래그 해제
        }

        // 라디오 버튼의 텍스트 값을 확인하는 메서드
        private int[] checkRadioButtonText()
        {
            int[] returnArr = new int[42]; // 달력의 총 42칸에 해당하는 배열 생성

            for (int target = 1; target <= 42; target++)
            {
                GroupBox dateBox = calenderArea.Controls[$"dateBox{target}"] as GroupBox;

                if (dateBox != null)
                {
                    RadioButton dateRadioButton = dateBox.Controls[$"dateButton{target}"] as RadioButton;

                    if (dateRadioButton != null)
                    {
                        // 라디오 버튼의 텍스트를 정수로 변환하고 배열에 저장
                        if (int.TryParse(dateRadioButton.Text, out int parsedValue))
                        {
                            returnArr[target - 1] = parsedValue - 1;
                        }
                        else
                        {
                            // 정수로 변환되지 않는 경우 -1로 설정
                            returnArr[target - 1] = -1;
                        }
                    }
                }
            }
            return returnArr;
        }

        // 현재 시스템의 설정된 년/월과 일치하는 파일을 찾는 메서드
        private int[] CheckDateColumn()
        {
            /*
             * 함수 설명:
             * 설정된 년/월과 일치하는 파일이 있는지를 확인하고 해당 파일의 행 번호를 반환.
             */
            List<int> matchingRows = new List<int>();

            string[,] result = data_loading(); // 데이터를 불러옴
            int rowCount = result.GetLength(0);

            for (int i = 0; i < rowCount; i++)
            {
                string dateStr = result[i, 0]; // 파일의 날짜 정보

                if (dateStr.Length == 8 && int.TryParse(dateStr, out _))
                {
                    int dataYear = int.Parse(dateStr.Substring(0, 4)); // 연도 추출
                    int dataMonth = int.Parse(dateStr.Substring(4, 2)); // 월 추출

                    // 설정된 연도와 월과 일치하는 경우 리스트에 추가
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

        // 캘린더를 지정된 날짜의 모양으로 초기화하는 메서드
        private void calenderReset(int year = -1, int month = -1, int day = -1)
        {
            /*
             * 함수 설명 (순차적 동작):
             * 
             * 1. 캘린더 날짜 표기 라벨의 위치 조정
             * 2. 입력된 연도, 월, 일이 유효하지 않은 경우 현재 날짜로 설정
             * 3. 달력의 비사용 영역을 비활성화
             * 4. 달력의 사용 영역을 활성화 및 라디오 버튼에 날짜 값 설정
             * 5. 날짜 라벨 텍스트 업데이트
             * 6. 전역 변수 Syear, Smonth, Sday 값 갱신
             * 7. 해당 월의 메모 데이터를 ListBox에 삽입
             */

            Calender_Date.Location = new Point((Calender_DateArea.Width / 2 - Calender_Date.Width / 2), 17); // 캘린더 날짜 위치 조정

            // 입력값이 유효하지 않을 경우 현재 날짜로 설정
            DateTime date = DateTime.Now;
            if (year > 2099 || year < 1990) year = -1;
            if (month > 12) month = -1;
            if (day > 31) day = -1;

            if (year == -1) year = date.Year;
            if (month == -1) month = date.Month;
            if (day == -1) day = date.Day;

            date = new DateTime(year, month, day);

            // 1일의 요일을 기준으로 시작 지점을 설정
            date = new DateTime(year, month, 1);
            DayOfWeek dayOfWeek = date.DayOfWeek;
            int StartP = (int)dayOfWeek + 1; // 1부터 시작하도록 조정

            int lastDay = DateTime.DaysInMonth(year, month); // 해당 월의 마지막 일

            // 모든 달력 칸을 비활성화 및 초기화
            for (int i = 1; i <= 42; i++)
            {
                writeDateBox(2, i);
            }

            // 달력의 활성화 영역을 설정 및 날짜 추가
            int j = 1;
            for (int i = StartP; i < lastDay + StartP; i++)
            {
                writeDateBox(0, i, j.ToString());
                j++;
            }

            // 비활성화 영역을 초기화
            for (int i = lastDay + StartP; i <= 42; i++)
            {
                writeDateBox(2, i);
            }

            // 날짜 라벨을 업데이트
            String newDate = year + "년 " + month + "월 " + day + "일";
            changeDateLabel(newDate);

            // 전역 변수 갱신
            Syear = year; Smonth = month; Sday = day;

            // 해당 월의 데이터 로드 및 ListBox에 삽입
            string[,] loadingData = data_loading();
            int[] match = CheckDateColumn();

            foreach (int index in match)
            {
                string dateStr = loadingData[index, 0];
                int target = int.Parse(dateStr.Substring(6, 2));
                string data = loadingData[index, 1];
                InsertDataIntoListBox(target + StartP - 1, data);
            }
        }

        // 날짜 라벨의 텍스트를 변경하는 메서드
        private void changeDateLabel(String text)
        {
            // Calender_DateArea 그룹박스 안에 있는 Calender_Date라는 이름의 LinkLabel을 찾음
            LinkLabel calenderDate = Calender_DateArea.Controls["Calender_Date"] as LinkLabel;

            if (calenderDate != null)
            {
                calenderDate.Text = text; // 새로운 텍스트로 업데이트
            }
            else
            {
                MessageBox.Show("Calender_Date 라는 LinkLabel을 찾을 수 없습니다.");
            }
        }

        // 메모 목록을 새로고침하는 메서드
        private void refreshActive()
        {
            foreach (Control control in memoPanel.Controls.Cast<Control>().ToList())
            {
                if (control is GroupBox groupBox)
                {
                    memoPanel.Controls.Remove(groupBox); // 메모 제거
                    groupBox.Dispose(); // 리소스 해제
                }
            }

            memoLoad(); // 메모 불러오기

            // 메모가 없으면 삭제 버튼 비활성화, 있으면 활성화
            if (checkEmpty() == 0) deleteButton.Enabled = false;
            else deleteButton.Enabled = true;
        }
        //----------------------------------< 폼  함 수 >----------------------------------

        // 생성자: 폼 초기화 메서드
        public main()
        {
            InitializeComponent(); // 폼의 구성 요소를 초기화
        }

        // 메인 폼이 로드될 때 실행되는 이벤트 핸들러
        private void mainForm_Load(object sender, EventArgs e)
        {
            resetVar(); // 현재 날짜로 연도, 월, 일 설정
            calenderReset(); // 캘린더를 현재 날짜에 맞게 초기화
            checkRadioButtonText(); // 라디오 버튼의 텍스트 값을 확인
            memoLoad(Syear, Smonth, Sday); // 해당 연도, 월, 일의 메모를 불러오기
        }

        // 검색 버튼이 클릭되었을 때 실행되는 이벤트 핸들러 (현재 구현되지 않음)
        private void Search_clicked(object sender, EventArgs e)
        {
        }

        // 추가 버튼이 클릭되었을 때 실행되는 이벤트 핸들러
        private void Add_Clicked(object sender, EventArgs e)
        {
            // 기본 메모 추가 형태 (현재 빈 파일 작성)
            WriteFile("", "");
            add_memo add_Memo = new add_memo(); // 메모 추가 폼 인스턴스 생성
            add_Memo.ShowDialog(); // 메모 추가 폼을 모달로 표시
        }

        // 설정 버튼이 클릭되었을 때 실행되는 이벤트 핸들러
        private void Setting_Clicked(object sender, EventArgs e)
        {
            // 설정 폼 인스턴스 생성 및 모달로 표시
            setting setting = new setting();
            setting.ShowDialog();
        }

        // 삭제 버튼이 클릭되었을 때 실행되는 이벤트 핸들러
        private void Delete_Clicked(object sender, EventArgs e)
        {
            List<GroupBox> groupBoxesToDelete = new List<GroupBox>(); // 삭제할 그룹박스 목록

            foreach (Control control in memoPanel.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    CheckBox checkBox = groupBox.Controls.OfType<CheckBox>().FirstOrDefault();

                    // 체크박스가 체크되어 있으면 그룹박스를 삭제 목록에 추가
                    if (checkBox != null && checkBox.Checked)
                    {
                        groupBoxesToDelete.Add(groupBox);
                    }
                }
            }

            // 선택된 그룹박스 삭제 처리
            foreach (GroupBox groupBox in groupBoxesToDelete)
            {
                CheckBox checkBox = groupBox.Controls.OfType<CheckBox>().FirstOrDefault();
                if (checkBox != null)
                {
                    string fileName = checkBox.Text + ".txt"; // 체크박스 텍스트를 파일 이름으로 사용
                    MoveFile(1, fileName); // 파일을 휴지통으로 이동
                }

                memoPanel.Controls.Remove(groupBox); // 패널에서 그룹박스 제거
                groupBox.Dispose(); // 리소스 해제
            }

            // 메모가 없으면 삭제 버튼 비활성화
            if (checkEmpty() == 0) deleteButton.Enabled = false;
            else deleteButton.Enabled = true;

            // 캘린더 리셋
            calenderReset(Syear, Smonth, Sday);
        }

        // 새로고침 버튼이 클릭되었을 때 실행되는 이벤트 핸들러
        private void refresh_Click(object sender, EventArgs e)
        {
            refreshActive(); // 메모 목록을 새로고침
        }

        // 휴지통 버튼이 클릭되었을 때 실행되는 이벤트 핸들러
        private void trashButton_Click(object sender, EventArgs e)
        {
            memo_trash memo_Trash = new memo_trash(); // 휴지통 폼 인스턴스 생성
            memo_Trash.ShowDialog(); // 휴지통 폼을 모달로 표시
        }

        // 리스트박스가 더블 클릭되었을 때 실행되는 이벤트 핸들러 (현재 메시지 박스만 표시)
        private void ListDoubleClicked(object sender, EventArgs e)
        {
            MessageBox.Show("통과");
        }

        // 리스트박스가 클릭되었을 때 실행되는 이벤트 핸들러
        private void ListClick(object sender, EventArgs e)
        {
            ListBox LB = sender as ListBox;
            LB.ClearSelected(); // 선택된 항목을 해제
        }

        // 월 이동 버튼이 클릭되었을 때 실행되는 이벤트 핸들러
        private void MonthButtonClicked(object sender, EventArgs e)
        {
            Button B = sender as Button;
            if (B != null)
            {
                if (B == DateLeftShift) // 왼쪽 이동 버튼 클릭
                {
                    if (Smonth == 1)
                    {
                        Syear--; // 연도 감소
                        Smonth = 12; // 12월로 설정
                    }
                    else Smonth--; // 월 감소

                    calenderReset(Syear, Smonth, Sday); // 캘린더 리셋
                }
                if (B == DateRightShift) // 오른쪽 이동 버튼 클릭
                {
                    if (Smonth == 12)
                    {
                        Syear++; // 연도 증가
                        Smonth = 1; // 1월로 설정
                    }
                    else Smonth++; // 월 증가
                    calenderReset(Syear, Smonth, Sday); // 캘린더 리셋
                }
            }
        }

        // 라디오 버튼이 선택되었을 때 실행되는 이벤트 핸들러
        private void radioChecked(object sender, EventArgs e)
        {
            if (isRadioChanging) return; // 재귀 호출 방지
            isRadioChanging = true; // 플래그 설정

            RadioButton R = sender as RadioButton;
            string radioButtonName;
            int buttonNumber = 0;

            if (R != null)
            {
                // "dateButton" 문자열을 제거하여 숫자만 남기고 정수로 변환
                radioButtonName = R.Name.Replace("dateButton", "");

                if (int.TryParse(radioButtonName, out buttonNumber))
                {
                    // 모든 다른 라디오 버튼을 비활성화
                    for (int i = 1; i <= 42; i++)
                    {
                        if (buttonNumber != i)
                        {
                            changeRadioChecked(i, false); // changeRadioChecked 메서드 호출
                        }
                    }
                }
            }

            // 라디오 버튼 값 확인
            int[] radioButtonValue = checkRadioButtonText();
            Sday = radioButtonValue[buttonNumber]; // 선택된 날짜를 설정

            refreshActive(); // 메모 목록 새로고침

            isRadioChanging = false; // 플래그 해제
        }
    }
}
