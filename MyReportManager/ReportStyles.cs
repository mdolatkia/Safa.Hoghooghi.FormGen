using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;

namespace MyReportManager
{
    public class ReportStyles
    {
        //public static StyleRule GetReportTitleStyle()
        //{
        //    var style = new Telerik.Reporting.Drawing.StyleRule();

        //    return style;
        //}

        internal static void SetReportTitleStyle(Style style)
        {
            style.Font.Name = "B Yekan";
            style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(2);
            style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
        }

        internal static void SetReportHeaderStyle(Style style, int reportLevel)
        {
            //style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(5);
            if (reportLevel == 0)
                style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            else if (reportLevel == 1)
                style.BackgroundColor = System.Drawing.Color.LightPink;
            else if (reportLevel == 2)
                style.BackgroundColor = System.Drawing.Color.LightGreen;
            style.BorderStyle.Bottom = BorderType.Solid;
        }


        internal static void SetGeoupColumnHeaderStyle(Style style, int reportLevel)
        {
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            //style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(5);
            if (reportLevel == 0)
                style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            else if (reportLevel == 1)
                style.BackgroundColor = System.Drawing.Color.LightPink;
            else if (reportLevel == 2)
                style.BackgroundColor = System.Drawing.Color.LightYellow;
            //style.BorderStyle.Bottom = BorderType.Solid;
        }

        internal static void SetColumnHeaderTextboxStyle(Style style)
        {
            //   style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(2);
            //style.BorderStyle.Top = BorderType.Solid;
            //if (!isFirstColumn)
            style.BorderStyle.Left = BorderType.Solid;
        }

        internal static void SetGeoupHeaderStyle(Style style)
        {
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            //style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(5);

            style.BackgroundColor = System.Drawing.Color.FromArgb(154, 216, 157);
            //else if (reportLevel == 1)
            //    style.BackgroundColor = System.Drawing.Color.LightPink;
            //else if (reportLevel == 2)
            //style.BackgroundColor = System.Drawing.Color.LightGreen;
            //style.BorderStyle.Bottom = BorderType.Solid;
        }
        internal static void SetGroupHeaderTextboxStyle(Style style)
        {
            style.BackgroundColor = System.Drawing.Color.FromArgb(132, 176, 141);
            style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(2);
            //style.BorderStyle.Top = BorderType.Solid;
            //if (!isFirstColumn)
            style.BorderStyle.Left = BorderType.Solid;
            style.BorderStyle.Right = BorderType.Solid;
        }

        internal static void SetDetailTextboxStyle(Style style)
        {
            //style.BorderStyle.Bottom = BorderType.Solid;
            style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(2);
            //if (!isFirstColumn)
            style.BorderStyle.Right = BorderType.Solid;
            style.BorderStyle.Left = BorderType.Solid;
        }

        internal static void SetReportDetailStyle(Style style)
        {
            //   style.BorderStyle.Bottom = BorderType.Solid;
        }

        internal static void SetSubreportStyle(Style style)
        {
            style.Padding.Bottom = Unit.Cm(0.5);
        }

        internal static void SetDetailPanelStyle(Style style, bool hasSub)
        {
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            if (hasSub)
                style.BackgroundColor = System.Drawing.Color.LightBlue;

        }











        internal static void SetChartReportHeaderStyle(Style style)
        {
            ////style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(5);
            //if (reportLevel == 0)
            //    style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            //else if (reportLevel == 1)
            //    style.BackgroundColor = System.Drawing.Color.LightPink;
            //else if (reportLevel == 2)
            //    style.BackgroundColor = System.Drawing.Color.LightGreen;
            style.BorderStyle.Bottom = BorderType.Solid;
        }







        internal static void SetCrosstabReportHeaderStyle(Style style)
        {
            style.BorderStyle.Bottom = BorderType.Solid;
        }

        internal static void SetCrossTabEmptyCornerTextboxStyle(Style style)
        {
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            style.BorderStyle.Left = BorderType.Solid;
            style.BorderStyle.Right = BorderType.Solid;
            style.TextAlign = HorizontalAlign.Center;
        }
        internal static void SetCrossTabRowHeader(Style style)
        {
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            style.BorderStyle.Left = BorderType.Solid;
            style.BorderStyle.Right = BorderType.Solid;
            style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(174)))), ((int)(((byte)(173)))));
            style.TextAlign = HorizontalAlign.Center;
        }

        internal static void SetCrossTabBodyTextboxStyle(Style style)
        {
            //مقادیر نتیجه در بدنه
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            style.BorderStyle.Left = BorderType.Solid;
            style.BorderStyle.Right = BorderType.Solid;
            style.TextAlign = HorizontalAlign.Center;
        }

        internal static void SetCrossTabRowColumnStyle(Style style)
        {
            //عناوین دسته بندی ها ..چه ستون چه ردیف
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            style.BorderStyle.Left = BorderType.Solid;
            style.BorderStyle.Right = BorderType.Solid;
            style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            style.TextAlign = HorizontalAlign.Center;
        }

        internal static void SetCrossTabTotalRowColumnStyle(Style style)
        {
            //نتیجه جمهعای داخل بدنه
            style.BorderStyle.Top = BorderType.Solid;
            style.BorderStyle.Bottom = BorderType.Solid;
            style.BorderStyle.Left = BorderType.Solid;
            style.BorderStyle.Right = BorderType.Solid;
            style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            style.TextAlign = HorizontalAlign.Center;
        }

        //internal static void SetCrossTabColumnMultiValueStyle(Style style)
        //{
        //    style.BorderStyle.Top = BorderType.Solid;
        //    style.BorderStyle.Bottom = BorderType.Solid;
        //    style.BorderStyle.Left = BorderType.Solid;
        //    style.BorderStyle.Right = BorderType.Solid;
        //    style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(174)))), ((int)(((byte)(173)))));
        //}


        //internal static void SetCrossTabTotalMultiValueStyle(Style style)
        //{
        //    style.BorderStyle.Top = BorderType.Solid;
        //    style.BorderStyle.Bottom = BorderType.Solid;
        //    style.BorderStyle.Left = BorderType.Solid;
        //    style.BorderStyle.Right = BorderType.Solid;
        //    style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(174)))), ((int)(((byte)(173)))));
        //}
        //internal static void SetCrossTabBodyRowStyle(Style style)
        //{
        //    style.BorderStyle.Top = BorderType.Solid;
        //    style.BorderStyle.Bottom = BorderType.Solid;
        //    style.BorderStyle.Left = BorderType.Solid;
        //    style.BorderStyle.Right = BorderType.Solid;
        //    style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));

        //}
        //internal static void SetCrossTabTotalRowStyle(Style style)
        //{
        //    style.BorderStyle.Top = BorderType.Solid;
        //    style.BorderStyle.Bottom = BorderType.Solid;
        //    style.BorderStyle.Left = BorderType.Solid;
        //    style.BorderStyle.Right = BorderType.Solid;
        //    style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));

        //}
        //internal static void SetCrossTabBodyTotalRowStyle(Style style)
        //{//ردیف آخر جمع در بدنه
        //    style.BorderStyle.Top = BorderType.Solid;
        //    style.BorderStyle.Bottom = BorderType.Solid;
        //    style.BorderStyle.Left = BorderType.Solid;
        //    style.BorderStyle.Right = BorderType.Solid;
        //    style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(174)))), ((int)(((byte)(173)))));

        //}
    }
}