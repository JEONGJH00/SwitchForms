using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label1.Text = "Sleep Monitoring System";
            label2.Text = "심박수 센서는 손가락에 " + '\n' + "PIR 센서는 침대위 가지런히" + '\n' + " 준비가 끝났다면 시작버튼을 눌러주세요.";
            label3.Text = "당신의 더 좋은 수면";
            label4.Text = "수면 중 움직임을" + '\n' + "측정하여 분석할 수 있습니다.";
            label5.Text = "심박수를 측정하여" + '\n' + "수면 효율을 파악할 수 있습니다.";
            label6.Text = "불면증 자가진단" + '\n' + "테스트를 제공합니다.";
            button1.Text = "START";

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form2 newForm = new Form2();
            newForm.Show();
            Program.ac.MainForm = newForm;
            this.Close();
        }

    }
}
