using MyUILibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

namespace MyUIGenerator.UIControlHelper.Controls
{
    /// <summary>
    /// Interaction logic for MyDateTimePicker.xaml
    /// </summary>
    public partial class MyDateTimePicker : UserControl
    {
        public MyDateTimePicker()
        {
            InitializeComponent();
            txtShamsiDatePicker.SelectedDateChanged += TxtShamsiDatePicker_SelectedDateChanged;
            txtMiladiDatePicker.SelectedDateChanged += TxtMiladiDatePicker_SelectedDateChanged;
            txtTimePicker.SelectionChanged += TxtTimePicker_SelectionChanged;
        }



        private void SetSelectedDateTime()
        {
            miladiIsChangingProperty = true;
            if (txtMiladiDatePicker.SelectedDate != null)
            {
                if (txtTimePicker.SelectedValue != null)
                    SelectedDateTime = txtMiladiDatePicker.SelectedDate.Value.Add(txtTimePicker.SelectedValue.Value.TimeOfDay);
                else
                    SelectedDateTime = txtMiladiDatePicker.SelectedDate.Value;
            }
            else
                SelectedDateTime = txtTimePicker.SelectedDate;
            miladiIsChangingProperty = false;
        }
        bool miladiIsChangingProperty = false;
        bool shamsiIsChangingMiladi = false;
        bool miladiIsChangingShamsi = false;
        bool propertyIsChangingMiladi = false;


        private void TxtShamsiDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!shamsiIsChangingMiladi && !miladiIsChangingShamsi)
            {
                shamsiIsChangingMiladi = true;
                if (string.IsNullOrEmpty(txtShamsiDatePicker.SelectedPersianDate))
                    txtMiladiDatePicker.SelectedDate = null;
                else
                    txtMiladiDatePicker.SelectedDate = AgentHelper.GetMiladiDateFromShamsi(txtShamsiDatePicker.SelectedPersianDate);
                shamsiIsChangingMiladi = false;
            }
        }

        private void TxtMiladiDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!shamsiIsChangingMiladi && !miladiIsChangingShamsi)
            {
                miladiIsChangingShamsi = true;
                if (txtMiladiDatePicker.SelectedDate == null)
                    txtShamsiDatePicker.SelectedPersianDate = null;
                else
                    txtShamsiDatePicker.SelectedPersianDate = AgentHelper.GetShamsiDateFromMiladi(txtMiladiDatePicker.SelectedDate.Value);
                miladiIsChangingShamsi = false;
            }
            if (!propertyIsChangingMiladi)
                SetSelectedDateTime();
        }
        private void TxtTimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!propertyIsChangingMiladi)
            {
                SetSelectedDateTime();
            }
        }
        public bool MiladiDatePickerVisiblity
        {
            set
            {
                txtMiladiDatePicker.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
            get
            {
                return txtMiladiDatePicker.Visibility == Visibility.Visible;
            }
        }
        public bool ShamsiDatePickerVisiblity
        {
            set
            {
                txtShamsiDatePicker.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
            get
            {
                return txtShamsiDatePicker.Visibility == Visibility.Visible;
            }
        }
        //DatePickerMode _DatePickerMode;
        //public DatePickerMode DatePickerMode
        //{
        //    set
        //    {
        //        _DatePickerMode = value;
        //        if (_DatePickerMode == DatePickerMode.Miladi)
        //        {
        //            txtShamsiDatePicker.Visibility = Visibility.Collapsed;
        //            txtMiladiDatePicker.Visibility = Visibility.Visible;
        //        }
        //        else if (_DatePickerMode == DatePickerMode.Shamsi)
        //        {
        //            txtShamsiDatePicker.Visibility = Visibility.Visible;
        //            txtMiladiDatePicker.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            txtShamsiDatePicker.Visibility = Visibility.Visible;
        //            txtMiladiDatePicker.Visibility = Visibility.Visible;
        //        }
        //    }
        //    get
        //    {
        //        return _DatePickerMode;
        //    }
        //}
        public CultureInfo TimePickeCulture
        {
            set
            {
                txtTimePicker.Culture = value;
            }
            get
            {
                return txtTimePicker.Culture;
            }
        }
        public bool TimePickerVisiblity
        {
            set
            {
                txtTimePicker.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
            get
            {
                return txtTimePicker.Visibility == Visibility.Visible;
            }
        }
        public bool TimePicker24Hours
        {
            set
            {
                if (value)
                {
                    txtTimePicker.Culture.DateTimeFormat.ShortTimePattern = "H:mm";
                    txtTimePicker.Culture.DateTimeFormat.LongTimePattern = "H:mm";
                }
                else
                {
                    txtTimePicker.Culture.DateTimeFormat.ShortTimePattern = "";
                    txtTimePicker.Culture.DateTimeFormat.LongTimePattern = "";
                }
            }
            get
            {
                return txtTimePicker.Culture.DateTimeFormat.ShortTimePattern == "H:mm";
            }
        }

        public string ShamsiDate
        {
            set
            {
                txtShamsiDatePicker.SelectedPersianDate = value;
            }
            get
            {
                return txtShamsiDatePicker.SelectedPersianDate;
            }

        }
        public DateTime? MiladiDate
        {
            set
            {
                txtMiladiDatePicker.SelectedDate = value;
                txtTimePicker.SelectedDate = value;
            }
            get
            {
                return txtMiladiDatePicker.SelectedDate;
            }

        }

        public static readonly DependencyProperty SelectedDateTimeProperty =
    DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(MyDateTimePicker)
, new PropertyMetadata(new PropertyChangedCallback(OnSelectedDateTimeChanged)));

        private static void OnSelectedDateTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            MyDateTimePicker myDateTimePicker = (MyDateTimePicker)sender;
            if (!myDateTimePicker.miladiIsChangingProperty)
            {
                myDateTimePicker.propertyIsChangingMiladi = true;
                if (e.NewValue != null)
                {
                    myDateTimePicker.txtMiladiDatePicker.SelectedDate = (DateTime)e.NewValue;
                    myDateTimePicker.txtTimePicker.SelectedTime = ((DateTime)e.NewValue).TimeOfDay;
                }
                else
                {
                    myDateTimePicker.txtMiladiDatePicker.SelectedDate = null;
                    myDateTimePicker.txtTimePicker.SelectedValue = null;
                }
                myDateTimePicker.propertyIsChangingMiladi = false;
            }
        }

        public DateTime? SelectedDateTime
        {
            set
            {
                SetValue(SelectedDateTimeProperty, value);
            }
            get
            {
                return (DateTime?)GetValue(SelectedDateTimeProperty);
                //if (txtMiladiDatePicker.SelectedDate != null)
                //{
                //    if (txtTimePicker.SelectedDate != null)
                //        return txtMiladiDatePicker.SelectedDate.Value.Add(txtTimePicker.SelectedDate.Value.TimeOfDay);
                //    else
                //        return txtMiladiDatePicker.SelectedDate.Value;
                //}
                //else
                //    return txtTimePicker.SelectedDate;
            }

        }
        public DateTime? SelectedTime
        {
            set
            {
                txtTimePicker.SelectedDate = value;

            }
            get
            {
                return txtTimePicker.SelectedValue;
            }
        }
        public string TimeText
        {
            set
            {
                txtTimePicker.DateTimeText = value;

            }
            get
            {
                return txtTimePicker.DateTimeText;
            }

        }
    }
    public enum DatePickerMode
    {
        Both,
        Miladi,
        Shamsi

    }
}
