using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using System.Reflection.Emit;


namespace SwitchForms
{
    public partial class Form2 : Form
    {
        private SerialPort mySerial; //시리얼 포트 선언
        private double xCount = 200;  // 차트에 보여지는 데이터 갯수(아래 차트세팅부분이랑 연결1)
 //       List<SensorData> myData = new List<SensorData>();   // mydata라는 리스트 객체가 만들어짐/저장하기 위한 자료구조(아두이노 값을 저장하기 위한) 솔루션 탐색기에 sensordata만드는거 데이터베이스인가본.?(아래랑 연결2)
        SqlConnection conn;
        private int cnt;
 //       string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\KHJ\Desktop\555\SensorData.mdf;Integrated Security = True";

        // private int cnt;
        int c1;
        int c2;
        int c3;
        int c4;
        int c5;
        int c6;
        int c7;
        int c8;
        int c9;
        int c10;
        int c11;
        int c12;
        int c13;
        int c14;
        int c15;
        int c16;
        int c17;
        int f;
        int v;
        int b;
        int z;
        Timer t = new Timer();
        Timer t1 = new Timer();
        private int cnt_1;
        private int cnt_0;







        public Form2()
        {
            InitializeComponent();

            t.Interval = 1000;
            t.Tick += T_Tick;
            t1.Tick += T1_Tick;
            t1.Interval = 1000;



            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;

            label18.Text = DateTime.Now.ToString("hh:mm:ss");


            // ComboBox
            foreach (var ports in SerialPort.GetPortNames()) //포트읽기
            {
                comboBox1.Items.Add(ports); //선택할수있는 포트들이 다 콤보박스에 들어가진다.

            }
            comboBox1.Text = "Select port";

            // Connect, Disconnect 버튼 비활성화
            button3.Enabled = false;
            button4.Enabled = false;


            // 차트 모양 세팅
            ChartSetting(); //메소드 


            // 화면 label 텍스트
            label4.Text = "당신의 더 좋은 수면";
            label5.Text = "포트를 선택해주세요";


            // 불면증 자가진단 테스트의 텍스트
            label3.Text = "불면증 자가진단 테스트";
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
            label1.Text = "오늘 당신의 수면은?";
            label2.Text = "심박수 45이하, 움직임 5이하 = RAM수면으로 판단" + "\n" + "심박수 65이상, 움직임 10이상 = 비RAM수면으로 판단"
                +"\n"+"* 심박수가 35이하로 떨어졌을 시 병원정밀검사 필요!";
        }

        private void T_Tick(object sender, EventArgs e)
        {

        }

        private void T1_Tick(object sender, EventArgs e)
        {
            if (v == 1)
            {
                b++;
                cnt_1 = b;
                /*
                if (DateTime.Now.Hour >= 21 && DateTime.Now.Hour < 22)
                {

                    c1++;
                    txt1.Text = (((float)c1 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 23)
                {
                    c2++;
                    txt2.Text = (((float)c2 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 23 && DateTime.Now.Hour < 24)
                {
                    c3++;
                    txt3.Text = (((float)c3 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 24 && DateTime.Now.Hour < 01)
                {
                    c4++;
                    txt4.Text = (((float)c4 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 01 && DateTime.Now.Hour < 02)
                {
                    c5++;
                    txt5.Text = (((float)c5 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 02 && DateTime.Now.Hour < 03)
                {
                    c6++;
                    txt6.Text = (((float)c6 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 03 && DateTime.Now.Hour < 04)
                {
                    c7++;
                    txt7.Text = (((float)c7 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 04 && DateTime.Now.Hour < 05)
                {
                    c8++;
                    txt8.Text = (((float)c8 / 3600.0) * 100.0).ToString();
                }
                else if (DateTime.Now.Hour >= 05 && DateTime.Now.Hour < 06)
                {
                    c9++;
                    txt9.Text = (((float)c9 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 06 && DateTime.Now.Hour < 07)
                {
                    c10++;
                    txt10.Text = (((float)c10 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 07 && DateTime.Now.Hour < 08)
                {
                    c11++;
                    txt11.Text = (((float)c11 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 08 && DateTime.Now.Hour < 09)
                {
                    c12++;
                    txt12.Text = (((float)c12 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 09 && DateTime.Now.Hour < 10)
                {
                    c13++;
                    txt13.Text = (((float)c13 / 3600.0) * 100.0).ToString();
                }

                else if (DateTime.Now.Hour >= 10 && DateTime.Now.Hour < 11)
                {
                    c14++;
                    txt14.Text = (((float)c14 / 3600.0) * 100.0).ToString();
                }

                else
                    cnt_0++;
                */
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            label18.Text = DateTime.Now.ToString("hh:mm:ss");


        }

        private void ChartSetting()
        {
            //PIR 차트
            chart2.ChartAreas.Clear();
            chart2.ChartAreas.Add("draw");
            chart2.ChartAreas["draw"].AxisX.Minimum = 0;
            chart2.ChartAreas["draw"].AxisX.Maximum = 15;
            chart2.ChartAreas["draw"].AxisX.Interval = 1;
            chart2.ChartAreas["draw"].AxisX.MajorGrid.LineColor = Color.Gray;
            chart2.ChartAreas["draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart2.ChartAreas["draw"].AxisY.Minimum = 0;
            chart2.ChartAreas["draw"].AxisY.Maximum = 100;
            chart2.ChartAreas["draw"].AxisY.Interval = 10;
            chart2.ChartAreas["draw"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart2.ChartAreas["draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart2.ChartAreas["draw"].BackColor = Color.Ivory;
            chart2.ChartAreas["draw"].CursorX.AutoScroll = true;
            chart2.ChartAreas["draw"].AxisX.ScaleView.Zoomable = true;
            chart2.ChartAreas["draw"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart2.ChartAreas["draw"].AxisX.ScrollBar.ButtonColor = Color.LightSteelBlue;




            chart2.Series.Clear();
            chart2.Series.Add("PhotoCell");
            chart2.Series["PhotoCell"].ChartType = SeriesChartType.Column;
            chart2.Series["PhotoCell"].Color = Color.DarkSalmon;
            chart2.Series["PhotoCell"].BorderWidth = 7;
            if (chart2.Legends.Count > 0) // 범례를 안보이게 한다

                chart2.Legends.RemoveAt(0);


 /*           chart2.Series["PhotoCell"].Points.AddXY("21시", txt1.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("22시", txt2.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("23시", txt3.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("24시", txt4.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("01시", txt5.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("02시", txt6.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("03시", txt7.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("04시", txt8.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("05시", txt9.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("06시", txt10.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("07시", txt11.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("08시", txt12.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("09시", txt13.Text.ToString());
            chart2.Series["PhotoCell"].Points.AddXY("10시", txt14.Text.ToString());
 */



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

        private void Form2_Load(object sender, EventArgs e)
        {
            mySerial = new SerialPort();
            comboBox1.DataSource = SerialPort.GetPortNames();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //불면증 자가진단 확인버튼
            MessageBox.Show("4가지 이상 해당된다면 불면증 가능성 ↑");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            mySerial = new SerialPort(cb.SelectedItem.ToString());
            mySerial.Open();
            mySerial.DataReceived += SPort_DataReceived;


            label18.Text = DateTime.Now.ToString("hh:mm:ss");
            label19.Text = "Connection Time : " + DateTime.Now.ToString();

            button3.Enabled = true;
            button4.Enabled = true;





        }


        private void SPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {


            string data = mySerial.ReadLine();
            this.BeginInvoke((new Action(delegate { ShowValue(data); })));

        }

        private void ShowValue(string s)
        {
            //z = int.Parse(txt_1.Text);
            cnt = 0;
            int v = Int32.Parse(s);
            if (v == 1)
            {
                cnt += 1;
            }
            else if (v == 0)
            {
                cnt += 0;
            }


 //           int v = Int32.Parse(s);
            if (int.TryParse(s, out v) == false)
                return;
            if (v < 0 || v > 140)
                return;

            t1.Start();
            t.Start();







 //           SensorData data = new SensorData(DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss"), v);
 //           myData.Add(data);






            chart2.Series["PhotoCell"].Points.Add(cnt);

            chart2.ChartAreas["draw"].AxisX.Minimum = 0;
 //           chart2.ChartAreas["draw"].AxisX.Maximum = (myData.Count >= xCount) ? myData.Count : xCount;




        }



        private void button3_Click(object sender, EventArgs e)
        {


            mySerial.Open();

            button3.Enabled = false;
            button4.Enabled = true;

        }


        private void button4_Click(object sender, EventArgs e)
        {
            mySerial.Close();

            button3.Enabled = true;
            button4.Enabled = false;


        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
 //           label20.Text = string.Format("심박수 최대값 : " + myData.Max());
 //           label21.Text = string.Format("심박수 최소값 : " + myData.Min());
        }

        private void button6_Click(object sender, EventArgs e)
        {

            MessageBox.Show(" 수면장애 자가검진! \n 1. 잠 드는 데 걸리는 시간이 30분 이상 \n 2.밤마다 5분 넘게 깨어있는 횟수가 1번 이상 \n 3.최초로 잠이 든 후 중간에 완전히 깨어있는 시간이 총 20분 이상" +
                " \n\n * 위 3가지 모두 해당한다면 수면장애이므로 병원의 진료가 필요합니다. \n * 1가지만 해당할 경우 적신호로 판단되니 주의가 필요합니다.");

        }
    }
}