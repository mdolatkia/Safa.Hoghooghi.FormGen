using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for ucWizardSteps.xaml
    /// </summary>
    public partial class ucWizardSteps : UserControl
    {
        LinkedListNode<ImportWizardStep> CurrentState { set; get; }

        public ucWizardSteps()
        {
            InitializeComponent();
        }
        public event EventHandler<ImportWizardStep> StepClicked;

        LinkedList<ImportWizardStep> WizardSteps { set; get; }
        List<Tuple<ImportWizardStep, Grid>> StepsAndGrids = new List<Tuple<ImportWizardStep, Grid>>();
        public void SetSteps(ObservableCollection<MyProject_WPF.ImportWizardStep> steps)
        {
            if (WizardSteps == null)
                WizardSteps = new LinkedList<ImportWizardStep>();

            List<MyProject_WPF.ImportWizardStep> newsteps = new List<MyProject_WPF.ImportWizardStep>();
            foreach (var item in steps.OrderBy(X => X.Index))
            {
                if (!WizardSteps.Any(x => x == item))
                {

                    var last = WizardSteps.OrderBy(x => x.Index).LastOrDefault(x => x.Index <= item.Index);
                    if (last == null)
                        WizardSteps.AddFirst(item);
                    else
                    {
                        WizardSteps.AddAfter(WizardSteps.Find(last), item);
                    }
                    newsteps.Add(item);
                }
            }
            //foreach (var item in pnlSteps.Children)
            //    pnlSteps.Children.Clear();
            foreach (var item in newsteps)
            {
                Grid grid = new Grid();
                grid.Tag = item;
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                var textblock = new System.Windows.Controls.TextBlock();
                Hyperlink link = new Hyperlink();
                link.Click += (sender, e) => Link_Click(sender, e, WizardSteps.Find(item));
                link.Inlines.Add(item.Title);
                textblock.Inlines.Add(link);
                textblock.Tag = item;
                grid.Children.Add(textblock);

                var textblockSpace = new System.Windows.Controls.TextBlock();
                textblockSpace.Inlines.Add(">");
                Grid.SetColumn(textblockSpace, 1);
                grid.Children.Add(textblockSpace);

                var index = 0;

                if (StepsAndGrids.Any())
                {
                    var lastgrid = StepsAndGrids.OrderBy(x => x.Item1.Index).LastOrDefault(x => x.Item1.Index <= item.Index);
                    if (lastgrid != null)
                    {
                        index = pnlSteps.Children.IndexOf(lastgrid.Item2) + 1;
                    }
                }
                pnlSteps.Children.Insert(index, grid);
                StepsAndGrids.Add(new Tuple<ImportWizardStep, Grid>(item, grid));
                foreach (var stepGrid in StepsAndGrids)
                {
                    Visibility delimiterVisibility;
                    if (pnlSteps.Children.IndexOf(stepGrid.Item2) == pnlSteps.Children.Count - 1)
                        delimiterVisibility = Visibility.Collapsed;
                    else
                        delimiterVisibility = Visibility.Visible;
                    stepGrid.Item2.Children[1].Visibility = delimiterVisibility;
                }
            }
            if (CurrentState == null)
                changeState(WizardSteps.First);
        }

        private void Link_Click(object sender, RoutedEventArgs e, LinkedListNode<ImportWizardStep> step)
        {
            changeState(step);
        }

        private void changeState(LinkedListNode<ImportWizardStep> step)
        {
            CurrentState = step;
            var texts = pnlSteps.ChildrenOfType<System.Windows.Controls.TextBlock>();
            foreach (var item in texts)
            {
                if (item.Tag != null)
                {
                    if ((item.Tag as ImportWizardStep) == step.Value)
                        item.FontWeight = FontWeights.Bold;
                    else
                        item.FontWeight = FontWeights.Normal;
                }
            }
            if (StepClicked != null)
                StepClicked(this, CurrentState.Value);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentState != WizardSteps.Last)
                changeState(CurrentState.Next);
        }
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentState != WizardSteps.First)
                changeState(CurrentState.Previous);
        }

    }
}
