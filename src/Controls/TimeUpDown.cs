﻿using System;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Logic;

namespace Nikse.SubtitleEdit.Controls
{
    public partial class TimeUpDown : UserControl
    {
        const int NumericUpDownValue = 50;

        public EventHandler TimeCodeChanged;

        public TimeUpDown()
        {
            InitializeComponent();
            numericUpDown1.ValueChanged += NumericUpDownValueChanged;
            numericUpDown1.Value = NumericUpDownValue;
        }

        void NumericUpDownValueChanged(object sender, EventArgs e)
        {
            double? millisecs = GetTotalMilliseconds();
            if (millisecs.HasValue)
            {
                if (numericUpDown1.Value > NumericUpDownValue)
                {
                    SetTotalMilliseconds(millisecs.Value + 100);
                }
                else if (numericUpDown1.Value < NumericUpDownValue)
                {
                    if (millisecs.Value - 100 > 0)
                        SetTotalMilliseconds(millisecs.Value - 100);
                    else if (millisecs.Value > 0)
                        SetTotalMilliseconds(0);
                }
                if (TimeCodeChanged != null)
                    TimeCodeChanged.Invoke(this, e);
            }
            numericUpDown1.Value = NumericUpDownValue;
        }

        public MaskedTextBox MaskedTextBox
        {
            get
            {
                return maskedTextBox1;
            }
        }

        public void SetTotalMilliseconds(double milliseconds)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(milliseconds);
            maskedTextBox1.Text = new TimeCode(ts).ToString();
        }

        public double? GetTotalMilliseconds()
        {
            TimeCode tc = TimeCode;
            if (tc != null)
                return tc.TotalMilliseconds;
            return null;
        }

        public TimeCode TimeCode
        {
            get
            {                
                string startTime = maskedTextBox1.Text;
                startTime.Replace(' ', '0');
                if (startTime.EndsWith("."))
                    startTime += "000";

                string[] times = startTime.Split(":,.".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (times.Length == 4)
                {
                    int hours = 0;
                    if (Utilities.IsInteger(times[0]))
                        hours = int.Parse(times[0]);

                    int minutes = 0;
                    if (Utilities.IsInteger(times[1]))
                        minutes = int.Parse(times[1]);

                    int seconds = 0;
                    if (Utilities.IsInteger(times[2]))
                        seconds = int.Parse(times[2]);

                    int milliSeconds = 0;
                    if (Utilities.IsInteger(times[3]))
                        milliSeconds = int.Parse(times[3]);

                    return new TimeCode(hours, minutes, seconds, milliSeconds);
                }
                return null;
            }
            set
            {
                if (value != null)
                    maskedTextBox1.Text = value.ToString();
                else
                    maskedTextBox1.Text = new TimeCode(TimeSpan.FromMilliseconds(0)).ToString();
            }
        }

        private void MaskedTextBox1KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                numericUpDown1.UpButton();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                numericUpDown1.DownButton();
                e.SuppressKeyPress = true;
            }
        }

    }
}
