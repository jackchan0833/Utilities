using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JC.Utilities.WPF.UserControls
{
    /// <summary>
    /// TimePicker.xaml, a timer picker to display as "00:00:00" and can increse or decrese the vlaues.
    /// </summary>
    public partial class TimePicker : UserControl, INotifyPropertyChanged
    {
        private enum TimeFocusType
        {
            None = 0,
            Hour = 1,
            Minute = 2,
            Second = 3,
        }
        private TimeFocusType _FocusType =  TimeFocusType.None;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _ShowSeconds = true;
        /// <summary>
        /// Whether to show the seconds.
        /// </summary>
        public bool ShowSeconds
        {
            get
            {
                return _ShowSeconds;
            }
            set
            {
                _ShowSeconds = value;
                SetValue(ShowSecondsProperty, value);
            }
        }

        private string _Text;
        /// <summary>
        /// Gets or sets the display text. Format as "00:00:00" or "00:00";
        /// </summary>
        public string Text
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Text))
                {
                    if (_ShowSeconds)
                    {
                        _Text = "00:00:00";
                    }
                    else
                    {
                        _Text = "00:00";
                    }
                }
                return _Text;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    if (_ShowSeconds)
                    {
                        _Text = "00:00:00";
                    }
                    else
                    {
                        _Text = "00:00";
                    }
                }
                else
                {
                    _Text = value;
                }
                SetValue(TextProperty, _Text);
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TimePicker), new PropertyMetadata(default(string), OnTextPropertyChanged));

        public static readonly DependencyProperty ShowSecondsProperty =
            DependencyProperty.Register(nameof(ShowSeconds), typeof(bool), typeof(TimePicker), new PropertyMetadata(true, OnShowSecondsPropertyChanged));

        public TimePicker()
        {
            InitializeComponent();
        }
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (TimePicker)d;
            var value = e.NewValue as string;
            string strHour = "00";
            string strMinute = "00";
            string strSecond = "00";
            if (!string.IsNullOrWhiteSpace(value))
            {
                var arrTime = value.Split(':');
                if (arrTime.Length > 0)
                {
                    strHour = arrTime[0];
                }
                else
                {
                    throw new InvalidOperationException("Invalid time picker text.");
                }
                if (arrTime.Length > 1)
                {
                    strMinute = arrTime[1];
                }
                else
                {
                    throw new InvalidOperationException("Invalid time picker text.");
                }
                if (picker.ShowSeconds)
                {
                    if (arrTime.Length > 2)
                    {
                        strSecond = arrTime[2];
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid time picker text that is missing seconds.");
                    }
                }
            }
            picker.txtHour.Text = strHour;
            picker.txtMinute.Text = strMinute;
            picker.txtSecond.Text = strSecond;
        }
        private static void OnShowSecondsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (TimePicker)d;
            var value = e.NewValue as string;
            bool showSeconds = true;
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (!bool.TryParse(value, out showSeconds))
                {
                    throw new InvalidOperationException($"Invalid property value for {nameof(ShowSeconds)}.");
                }
            }
            picker.stackPanelSeconds.Visibility = showSeconds ? Visibility.Visible : Visibility.Collapsed;
        }
        
        private string GetTextValue()
        {
            string hour = string.IsNullOrWhiteSpace(txtHour.Text) ? "00" : txtHour.Text.Trim();
            string minute = string.IsNullOrWhiteSpace(txtMinute.Text) ? "00" : txtMinute.Text.Trim();
            string second = string.IsNullOrWhiteSpace(txtSecond.Text) ? "00" : txtSecond.Text.Trim();
            if (this.ShowSeconds)
            {
                return $"{hour}:{minute}:{second}";
            }
            else
            {
                return $"{hour}:{minute}";
            }
        }
        protected void txtHour_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Text = GetTextValue();
                }));
            }
            catch
            {
            }
        }
        protected void txtMinute_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Text = GetTextValue();
                }));
            }
            catch
            {
            }
        }
        protected void txtSecond_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Text = GetTextValue();
                }));
            }
            catch
            {
            }
        }
        protected void txtHour_Focused(object sender, RoutedEventArgs e)
        {
            try
            {
                _FocusType = TimeFocusType.Hour;
            }
            catch
            {
            }
        }
        protected void txtMinute_Focused(object sender, RoutedEventArgs e)
        {
            try
            {
                _FocusType = TimeFocusType.Minute;
            }
            catch
            {
            }
        }
        protected void txtSecond_Focused(object sender, RoutedEventArgs e)
        {
            try
            {
                _FocusType = TimeFocusType.Second;
            }
            catch
            {
            }
        }
        protected void btnTimeIncrement_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bool showSeconds = this.ShowSeconds;
                if (_FocusType == TimeFocusType.Hour)
                {
                    int hour = 0;
                    if (int.TryParse(txtHour.Text.Trim(), out hour))
                    {
                        hour++;
                    }
                    if (hour > 23)
                    {
                        hour = 0;
                    }
                    txtHour.Text = hour.ToString().PadLeft(2, '0');
                }
                else if (_FocusType == TimeFocusType.Minute || !showSeconds)
                {
                    int minute = 0;
                    if (int.TryParse(txtMinute.Text.Trim(), out minute))
                    {
                        minute++;
                    }
                    if (minute > 59)
                    {
                        minute = 0;
                    }
                    txtMinute.Text = minute.ToString().PadLeft(2, '0');
                }
                else if (_FocusType == TimeFocusType.Second || showSeconds)
                {
                    int second = 0;
                    if (int.TryParse(txtSecond.Text.Trim(), out second))
                    {
                        second++;
                    }
                    if (second > 59)
                    {
                        second = 0;
                    }
                    txtSecond.Text = second.ToString().PadLeft(2, '0');
                }
            }
            catch
            {
            }
        }
        protected void btnTimeDecrement_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bool showSeconds = this.ShowSeconds;
                if (_FocusType == TimeFocusType.Hour)
                {
                    int hour = 0;
                    if (int.TryParse(txtHour.Text.Trim(), out hour))
                    {
                        hour--;
                    }
                    if (hour < 0)
                    {
                        hour = 23;
                    }
                    txtHour.Text = hour.ToString().PadLeft(2, '0');
                }
                else if (_FocusType == TimeFocusType.Minute || !showSeconds)
                {
                    int minute = 0;
                    if (int.TryParse(txtMinute.Text.Trim(), out minute))
                    {
                        minute--;
                    }
                    if (minute < 0)
                    {
                        minute = 59;
                    }
                    txtMinute.Text = minute.ToString().PadLeft(2, '0');
                }
                else if (_FocusType == TimeFocusType.Second || showSeconds)
                {
                    int second = 0;
                    if (int.TryParse(txtSecond.Text.Trim(), out second))
                    {
                        second--;
                    }
                    if (second < 0)
                    {
                        second = 59;
                    }
                    txtSecond.Text = second.ToString().PadLeft(2, '0');
                }
            }
            catch
            {
            }
        }
    }
}
