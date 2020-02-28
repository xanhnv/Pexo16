using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Pexo16
{
    public class Channel
    {
        public Channel()
        {
        }

        private float[] data;
        public float[] Data
        {
            get { return data; }
            set { data = value; }
        }

        private int dataLenght;//for pexo 35
        public int DataLenght
        {
            get { return dataLenght; }
            set { dataLenght = value; }
        }
        
        byte sensor;
        public byte Sensor
        {
            get { return sensor; }
            set { sensor = value; }
        }


        byte unit;
        public byte Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        string desc;
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
       

        byte divNum;
        public byte DivNum
        {
            get { return divNum; }
            set { divNum = value; }
        }


        int alarmMax;
        public int AlarmMax
        {
            get { return alarmMax; }
            set { alarmMax = value; }
        }


        int alarmMin;
        public int AlarmMin
        {
            get { return alarmMin; }
            set { alarmMin = value; }
        }
       
        double highest;
        public double Highest
        {
            get { return highest; }
            set { highest = value; }
        }

        double lowest;
        public double Lowest
        {
            get { return lowest; }
            set { lowest = value; }
        }
  
        bool noAlarm;
        public bool NoAlarm
        {
            get { return noAlarm; }
            set { noAlarm = value; }
        }

        Color lineColor;
        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        Color lineColorY;
        public Color LineColorY
        {
            get { return lineColorY; }
            set { lineColorY = value; }
        }

        Color lineColorZ;
        public Color LineColorZ
        {
            get { return lineColorZ; }
            set { lineColorZ = value; }
        }

        private int maxCount;
        public int MaxCount
        {
            get { return maxCount; }
            set { maxCount = value; }
        }

        private int minCount;
        public int MinCount
        {
            get { return minCount; }
            set { minCount = value; }
        }

        private double val;
        public double Val
        {
            get { return val; }
            set { val = value; }
        }


        public float high_suminfo = new float();
        public float low_suminfo = new float();
        public double ave_frm_suminfo = new double();
    }
}
