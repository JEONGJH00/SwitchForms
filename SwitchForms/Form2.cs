using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;

namespace SwitchForms
{
    public partial class Form2 : Form
    {
        private SerialPort sPort;
        private int cnt; // 심박센서 버리지 않은 측정 횟수
        private int v1 = 50; // s1(문자열로 넘어오는 심박센서값)을 Int32.Parse(s1)(정수형으로 변환)값을 넣을 변수
                             // 심박센서의 최소값을 구하기 위해 50으로 초기화

        private int HeartMax; // 심박센서의 최대값
        private int HeartMin = 110; // 심박센서의 최소값. 최소값을 구하기 위해 Int32.Parse(s1)의 범위 중 최대값으로 설정
        private int PIRMax;  // PIR센서의 최대값 ( 간격 1시간 )
        private int PIRMin = 3600; // PIR센서의 최소값( 간격 1시간 )
                                   // 최소값을 구하기 위해 1시간동안 측정할 수 있는 범위 중 최대값으로 설정 

        // 10시간을 1시간 단위로 나눠서 배열에 센서값을 넣음
        int[] HeartData = new int[10]; // 1시간 단위로 심박센서 값을 넣을 배열
        int[] PIRData = new int[10]; // 1시간 단위로 PIR센서 값을 넣을 배열

        public Form2()
        {
            InitializeComponent();

            // Combobox
            foreach (var port in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(port);
            }
            comboBox1.Text = "Select Port";

            // Chart 세팅
            ChartSetting();

            // Connect, DistConnect 버튼
            button3.Enabled = false;
            button4.Enabled = false;

            // 화면 label 텍스트
            label1.Text = "당신의 더 좋은 수면";
            label2.Text = "Connection Time : ";
            label3.Text = "포트를 선택해주세요";

            // 불면증 자가진단 테스트의 텍스트
            label4.Text = "불면증 자가진단 테스트";
            checkBox1.Text = " 잠에 들기까지 30분 이상 걸리는 것 같다. ";
            checkBox2.Text = " 잠을 잘 자기 위해서 노력을 한다. ";
            checkBox3.Text = " 잠들기 위해 술을 마시거나 약국에서 수면제를 사서 먹어본 적이 있다.";
            checkBox4.Text = " 휴일에는 실컷 자는 수가 있다.  ";
            checkBox5.Text = " 잠자리가 바뀌면 잠을 오히려 더 잘잔다. ";
            checkBox6.Text = " 자는 도중에 두 세 차례 이상 잠을 깨고 다시 잠들기가 쉽지않다. ";
            checkBox7.Text = " 자다가 중간에 깨면 얼마나 잤는지를 확인하기 위해 시계를 본다. ";
            checkBox8.Text = " 낮에 항상 졸리고 특히 점심식후에는 정신이 없을 정도로 졸린다. ";
            checkBox9.Text = " 항상 많은 꿈을 꾸고, 깨고 나서도 대개는 기억이 난다. ";
            checkBox10.Text = " 평소보다 훨씬 일찍깨서 (예를들어 새벽3, 4시) 더 이상 잠들기가 어렵다. ";
            button2.Text = "확인";

            //수면장애 자가진단 텍스트
            button5.Text = "확인";
            button6.Text = "수면장애 자가검진";
            label5.Text = "오늘 당신의 수면은?";
            label6.Text = "심박수 45이하, 움직임 5이하 = RAM수면으로 판단" + "\n" + "심박수 65이상, 움직임 10이상 = 비RAM수면으로 판단"
                + "\n" + "* 심박수가 35이하로 떨어졌을 시 병원정밀검사 필요!";
            label7.Text = "심박수 최대값 : ";
            label8.Text = "심박수 최소값 : ";
            label9.Text = "움직임 최대값 : ";
            label10.Text = "움직임 최소값 : ";
        }

        private void ChartSetting()
        {
            // 심박센서 차트
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("draw");
            chart1.ChartAreas["draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["draw"].AxisX.Maximum = 17;
            chart1.ChartAreas["draw"].AxisX.Interval = 1;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineColor = Color.White;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].AxisY.Minimum = 0;
            chart1.ChartAreas["draw"].AxisY.Maximum = 120;
            chart1.ChartAreas["draw"].AxisY.Interval = 10;
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineColor = Color.White;
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].BackColor = Color.White;

            chart1.ChartAreas["draw"].CursorX.AutoScroll = true;

            chart1.ChartAreas["draw"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonColor = Color.LightSteelBlue;

            chart1.Series.Clear();
            chart1.Series.Add("HeartSensor");
            chart1.Series["HeartSensor"].ChartType = SeriesChartType.Line;
            chart1.Series["HeartSensor"].Color = Color.Black;
            chart1.Series["HeartSensor"].BorderWidth = 10;
            if (chart1.Legends.Count > 0)
                chart1.Legends.RemoveAt(0);

            // PIR 센서 차트
            chart2.ChartAreas.Clear();
            chart2.ChartAreas.Add("draw2");
            chart2.ChartAreas["draw2"].AxisX.Minimum = 0;
            chart2.ChartAreas["draw2"].AxisX.Maximum = 17;
            chart2.ChartAreas["draw2"].AxisX.Interval = 1;
            chart2.ChartAreas["draw2"].AxisX.MajorGrid.LineColor = Color.White;
            chart2.ChartAreas["draw2"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart2.ChartAreas["draw2"].AxisY.Minimum = 0;
            chart2.ChartAreas["draw2"].AxisY.Maximum = 50;
            chart2.ChartAreas["draw2"].AxisY.Interval = 10;
            chart2.ChartAreas["draw2"].AxisY.MajorGrid.LineColor = Color.White;
            chart2.ChartAreas["draw2"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart2.ChartAreas["draw2"].BackColor = Color.White;

            chart2.ChartAreas["draw2"].CursorX.AutoScroll = true;

            chart2.ChartAreas["draw2"].AxisX.ScaleView.Zoomable = true;
            chart2.ChartAreas["draw2"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart2.ChartAreas["draw2"].AxisX.ScrollBar.ButtonColor = Color.LightSteelBlue;

            chart2.Series.Clear();
            chart2.Series.Add("활동량");
            chart2.Series["활동량"].ChartType = SeriesChartType.Column;
            chart2.Series["활동량"].Color = Color.Black;
            chart2.Series["활동량"].BorderWidth = 10;
            if (chart2.Legends.Count > 0)
                chart2.Legends.RemoveAt(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            sPort = new SerialPort(cb.SelectedItem.ToString());
            sPort.Open();
            sPort.DataReceived += SPort_DataReceived;

            label2.Text = "Connection Time : " + DateTime.Now.ToString();
            button4.Enabled = true;
        }

        private void SPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string s1 = sPort.ReadLine(); // 심박센서값을 불러와서 s1에 넣음
            string s2 = sPort.ReadLine(); // PIR센서값을 불러와서 s2에 넣음
            this.BeginInvoke(new Action(delegate { ShowValue1(s1); })); // 심박센서의 ShowValue를 호출
            this.BeginInvoke(new Action(delegate { ShowValue2(s2); })); // PIR센서의 ShowValue를 호출
        }

        // 심박 센서 ShowValue
        private void ShowValue1(string s1)
        {

            // 심박 센서값이 35보다 작거나 110보다 크면 버린다.
            // 버리지 않은 값의 개수와 측정 횟수가 동일해야 평균값을 구할 수 있다.
            if (Int32.Parse(s1) < 35 || Int32.Parse(s1) > 110)
                return;
            else
            {
                // v1과 s1 두 숫자를 받고 HeartMax와 두 숫자를 비교하여 더 큰 수를 return
                HeartMax = ((v1 > Int32.Parse(s1)) ? v1 : Int32.Parse(s1)) > HeartMax ?
                    ((v1 > Int32.Parse(s1) ? v1 : Int32.Parse(s1))) : HeartMax;

                // v1과 s1 두 숫자를 받고 HeartMin과 두 숫자를 비교하여 더 작은 수를 return
                HeartMin = ((v1 < Int32.Parse(s1)) ? v1 : Int32.Parse(s1)) < HeartMin ?
                    ((v1 < Int32.Parse(s1) ? v1 : Int32.Parse(s1))) : HeartMin;

                // 심박센서값을 INT값으로 변환해서 v1에 넣음
                v1 = Int32.Parse(s1);

                try // 오류 점검 코드
                {
                    v1 = Int32.Parse(s1);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Unable to convert {0}", s1);
                    return;
                }

                if (DateTime.Now.Hour >= 22 || DateTime.Now.Hour < 8) // 22시 이전, 8시 이후 불필요한 cnt값 증가 방지
                {
                    cnt++; // 심박 센서값이 35~110사이일 경우 측정 횟수 1씩 증가
                }
            }

            // 측정된 센서 값을 한시간 단위로 나눈다음 HeartData[] 배열에 저장
            if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 23)
                HeartData[0] += v1;
            else if (DateTime.Now.Hour >= 23 && DateTime.Now.Hour < 24)
                HeartData[1] += v1;
            else if (DateTime.Now.Hour >= 24 && DateTime.Now.Hour < 1)
                HeartData[2] += v1;
            else if (DateTime.Now.Hour >= 1 && DateTime.Now.Hour < 2)
                HeartData[3] += v1;
            else if (DateTime.Now.Hour >= 2 && DateTime.Now.Hour < 3)
                HeartData[4] += v1;
            else if (DateTime.Now.Hour >= 3 && DateTime.Now.Hour < 4)
                HeartData[5] += v1;
            else if (DateTime.Now.Hour >= 4 && DateTime.Now.Hour < 5)
                HeartData[6] += v1;
            else if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour < 6)
                HeartData[7] += v1;
            else if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 7)
                HeartData[8] += v1;
            else if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 8)
                HeartData[9] += v1;

            // HeartData[] 배열에 저장된 값을 cnt(측정횟수)로 나눠 평균값을 정각에 Chart로 나타냄
            // 다음 시간대에서 정확한 평균값을 구하기 위해 cnt 초기화
            // cnt 초기화를 안해줄 시 이전 시간대의 cnt값에서 cnt++이 시작됨
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("22시", HeartData[0] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 24 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("23시", HeartData[1] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 1 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("24시", HeartData[2] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 2 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("1시", HeartData[3] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 3 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("2시", HeartData[4] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 4 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("3시", HeartData[5] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 5 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("4시", HeartData[6] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 6 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("5시", HeartData[7] / cnt);
                cnt = 0;

            }
            else if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("6시", HeartData[8] / cnt);
                cnt = 0;
            }
            else if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                chart1.Series["HeartSensor"].Points.AddXY("7시", HeartData[9] / cnt);
                cnt = 0;
            }

        }

        // PIR 센서 ShowValue
        private void ShowValue2(string s2)
        {

            int v2 = Int32.Parse(s2); // PIR값을 INT값으로 변환해서 v2에 넣음
            try // 오류 점검 코드
            {
                v2 = Int32.Parse(s2);
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to convert {0}", s2);
                return;
            }
            // 센서값이 0보다 작거나 1보다 크면 버린다.
            if (v2 < 0 || v2 > 1)
                return;

            // 측정된 센서 값을 한시간 단위로 나눈다음 배열에 저장
            if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 23)
                PIRData[0] += v2;
            else if (DateTime.Now.Hour >= 23 && DateTime.Now.Hour < 24)
                PIRData[1] += v2;
            else if (DateTime.Now.Hour >= 24 && DateTime.Now.Hour < 1)
                PIRData[2] += v2;
            else if (DateTime.Now.Hour >= 1 && DateTime.Now.Hour < 2)
                PIRData[3] += v2;
            else if (DateTime.Now.Hour >= 2 && DateTime.Now.Hour < 3)
                PIRData[4] += v2;
            else if (DateTime.Now.Hour >= 3 && DateTime.Now.Hour < 4)
                PIRData[5] += v2;
            else if (DateTime.Now.Hour >= 4 && DateTime.Now.Hour < 5)
                PIRData[6] += v2;
            else if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour < 6)
                PIRData[7] += v2;
            else if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 7)
                PIRData[8] += v2;
            else if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 8)
                PIRData[9] += v2;

            // 정각에 배열에 저장된 값을 Chart로 나타냄
            // PIR차트에서 cnt(측정횟수)를 안쓰는 이유는 센서값들이 모인 PIRData리스트 값 자체가 활동량임.
            // 예를 들어 PIRData[0] = 50일 경우 1시간동안 50번 움직였다는 의미이므로 굳이 cnt(측정횟수)로 연산을 하지않음.
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("22시", PIRData[0]);
            else if (DateTime.Now.Hour == 24 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("23시", PIRData[1]);
            else if (DateTime.Now.Hour == 1 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("24시", PIRData[2]);
            else if (DateTime.Now.Hour == 2 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("1시", PIRData[3]);
            else if (DateTime.Now.Hour == 3 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("2시", PIRData[4]);
            else if (DateTime.Now.Hour == 4 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("3시", PIRData[5]);
            else if (DateTime.Now.Hour == 5 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("4시", PIRData[6]);
            else if (DateTime.Now.Hour == 6 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("5시", PIRData[7]);
            else if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("6시", PIRData[8]);
            else if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                chart2.Series["활동량"].Points.AddXY("7시", PIRData[9]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //폼전환
            this.Hide();
            Form1 newForm = new Form1();
            newForm.Show();
            Program.ac.MainForm = newForm;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //불면증 자가진단 확인버튼
            MessageBox.Show("4가지 이상 해당된다면 불면증 가능성 ↑");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sPort.Open();

            button3.Enabled = false;
            button4.Enabled = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            sPort.Close();

            button3.Enabled = true;
            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label7.Text = string.Format("심박수 최대값 : " + HeartMax);
            label8.Text = string.Format("심박수 최소값 : " + HeartMin);


            // PIRData[] 배열의 인덱스 중 최대값을 찾음. 
            PIRMax = PIRData.Max();

            // PIRData[] 배열의 인덱스 중 0이 아닌 최소값을 찾음.
            for (int i = 0; i < PIRData.Length; i++)
            {
                if (PIRData[i] == 0) { }
                else if (PIRData[i] < PIRMin)
                    PIRMin = PIRData[i];
            }

            label9.Text = string.Format("움직임 최대값 : " + PIRMax);
            label10.Text = string.Format("움직임 최소값 : " + PIRMin);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" 수면장애 자가검진! \n 1. 잠 드는 데 걸리는 시간이 30분 이상 \n 2.밤마다 5분 넘게 깨어있는 횟수가 1번 이상 \n 3.최초로 잠이 든 후 중간에 완전히 깨어있는 시간이 총 20분 이상" +
                " \n\n * 위 3가지 모두 해당한다면 수면장애이므로 병원의 진료가 필요합니다. \n * 1가지만 해당할 경우 적신호로 판단되니 주의가 필요합니다.");
        }
    }
}