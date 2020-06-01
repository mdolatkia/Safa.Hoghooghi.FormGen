
using MyModelManager;

using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for UC_ConnectionContent.xaml
    /// </summary>
    /// 

    public partial class UC_ConnectionContent : UserControl
    {
        public event EventHandler<TransitionEditArg> TransitionInfo;
        //public event EventHandler<TransitionEditArg> TransitionEditActions;
        //public event EventHandler<TransitionEditArg> TransitionEditActivities;
        //public event EventHandler<TransitionEditArg> TransitionEditForms;
        TransitionDTO Transition { set; get; }
        public UC_ConnectionContent(TransitionDTO transition)
        {
            //بهتره همه دکمه ها یکی بشه
            InitializeComponent();
            Transition = transition;

            SetInfo(Transition);
            BizProcess bizProcess = new BizProcess();
            var process = bizProcess.GetProcess(MyProjectManager.GetMyProjectManager.GetRequester(), transition.ProcessID, false);
            //if (process.EntityID == 0)
            //{
            //    btnEditTransitionActionForms.Visibility = Visibility.Collapsed;
            //}
        }

        //private void btnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    if (TransitionEditActions != null)
        //    {
        //        TransitionEditActions(this, new TransitionEditArg() { Transition = Transition });
        //    }
        //}


        internal void SetInfo(TransitionDTO transition)
        {
            //بهتره همه دکمه ها یکی بشه
            lblTitle.Text = transition.Name + Environment.NewLine + "اقدام" + " : " + transition.TransitionActions.Count + "-" + "فعالیت" + " : " + transition.TransitionActivities.Count;

            var actionTooltip = "";
            foreach (var item in transition.TransitionActions)
            {
                if (item.Action == null)
                {
                    if (item.ActionID != 0)
                    {
                        BizAction bizAction = new MyModelManager.BizAction();
                        item.Action = bizAction.GetAction(item.ActionID);

                    }
                }
                actionTooltip += (actionTooltip == "" ? "" : Environment.NewLine) + item.Action.Name;
            }

            var activityTooltip = "";
            foreach (var item in transition.TransitionActivities)
            {
                activityTooltip += (activityTooltip == "" ? "" : Environment.NewLine) + item.Name;
            }
            lblTitle.ToolTip = "اقدامات" + Environment.NewLine + actionTooltip + Environment.NewLine + "فعالیتها" + Environment.NewLine + activityTooltip;
        }

        //private void btnEditActivities_Click(object sender, RoutedEventArgs e)
        //{
        //    if (TransitionEditActivities != null)
        //    {
        //        TransitionEditActivities(this, new TransitionEditArg() { Transition = Transition });
        //    }
        //}

        private void btnEditTransition_Click(object sender, RoutedEventArgs e)
        {
            if (TransitionInfo != null)
            {
                TransitionInfo(this, new TransitionEditArg() { Transition = Transition });
            }
        }

        //private void btnEditTransitionActionForms_Click(object sender, RoutedEventArgs e)
        //{
        //    if (TransitionEditForms != null)
        //        TransitionEditForms(this, new TransitionEditArg() { Transition = Transition });
        //}
    }

    public class TransitionEditArg : EventArgs
    {
        public TransitionDTO Transition { set; get; }
    }
}
