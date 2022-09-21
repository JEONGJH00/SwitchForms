using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchForms
{
    class SensorData
    {
        public String Date { get; set; }
        public String Time { get; set; }
        public int Value { get; set; }

        public SensorData(string date, string time, int value)
        {
            this.Date = date;
            this.Time = time;
            this.Value = value;
        }
    }
}
