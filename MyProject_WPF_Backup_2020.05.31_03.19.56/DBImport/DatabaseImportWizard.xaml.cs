using ModelEntites;
using MyModelGenerator;
using MyModelManager;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for DatabaseImportWizard.xaml
    /// </summary>
    public partial class DatabaseImportWizard : UserControl
    {
        BizDatabase bizDatabase = new BizDatabase();
        DatabaseDTO Database { set; get; }
        //   LinkedList<ImportWizardStep> WizardSteps = new LinkedList<ImportWizardStep>();
        public DatabaseImportWizard(int databaseID)
        {
            InitializeComponent();
            ucSteps.StepClicked += UcSteps_StepClicked;
            Database = bizDatabase.GetDatabase(databaseID);

            SetWizardSteps();


        }

        private void Steps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //     WizardSteps = new LinkedList<MyProject_WPF.ImportWizardStep>();
            //foreach (var item in steps.OrderBy(X => X.Index))
            //{
            //    WizardSteps.AddLast(item);
            //}
            ucSteps.SetSteps(steps);
        }

        private void UcSteps_StepClicked(object sender, ImportWizardStep e)
        {
            ShowContent(e.Form);
        }
        ObservableCollection<MyProject_WPF.ImportWizardStep> steps = new ObservableCollection<ImportWizardStep>();
        private void SetWizardSteps()
        {
            steps.CollectionChanged -= Steps_CollectionChanged;
            steps.Add(new MyProject_WPF.ImportWizardStep() { Index = 1, State = WizrdState.ImportTables, Title = "بروزرسانی جداول", Form = FrmImportTables });
            steps.Add(new MyProject_WPF.ImportWizardStep() { Index = 2, State = WizrdState.ImportRelationships, Title = "بروزرسانی روابط", Form = FrmImportRelationships });
            steps.Add(new MyProject_WPF.ImportWizardStep() { Index = 3, State = WizrdState.ImportViewAndFunctions, Title = "بروزرسانی نماها و توابع", Form = FrmImportViewsAndFunctions });

            ucSteps.SetSteps(steps);

            steps.CollectionChanged += Steps_CollectionChanged;

            FrmImportTables.NewEntitiesAdded += FrmImportTables_NewEntitiesAdded;
            FrmEntityIsIndependent.DataUpdated += FrmEntityIsIndependent_DataUpdated;

            CheckEntityIsDataReference();
            CheckEntityIsIndependent();
            CheckNavigationTree();
            CheckEntitySettings();

            //   WizardSteps.AddAfter(WizardSteps.Last,
            //ImposeWizardState(WizardSteps.First);

        }

        private void FrmEntityIsIndependent_DataUpdated(object sender, EventArgs e)
        {
            CheckNavigationTree();
        }

        private async void CheckEntitySettings()
        {
            if (!steps.Any(x => x.Form == FrmEntitySettings))
            {
                if (await FrmEntitySettingsHasData())
                {
                    steps.Add(new MyProject_WPF.ImportWizardStep() { Index = 7, State = WizrdState.TableFinalSettings, Title = "تنظیمات نهایی موجودیتها", Form = FrmEntitySettings });
                }
            }
        }
        BizEntitySettings bizEntitySettings = new BizEntitySettings();
        private Task<bool> FrmEntitySettingsHasData()
        {
            return Task.Run(() =>
            {
                return bizEntitySettings.EntityWithoutSetting(Database.ID);
            });
        }

        private async void CheckNavigationTree()
        {
            if (!steps.Any(x => x.Form == FrmNavigationTree))
            {
                if (await FrmNavigationTreeHasData())
                {
                    steps.Add(new MyProject_WPF.ImportWizardStep() { Index = 6, State = WizrdState.NavigationTree, Title = "درخت منو", Form = FrmNavigationTree });
                }
            }
        }
        BizNavigationTree bizNavigationTree = new BizNavigationTree();
        private Task<bool> FrmNavigationTreeHasData()
        {
            return Task.Run(() =>
            {
                return bizNavigationTree.HasEntityNotInNavigationTree(Database.ID);
            });
        }

        private async void CheckEntityIsIndependent()
        {
            if (!steps.Any(x => x.Form == FrmEntityIsIndependent))
            {
                if (await FrmEntityIsIndependentHasData())
                {
                    steps.Add(new MyProject_WPF.ImportWizardStep() { Index = 5, State = WizrdState.EntityIsIndependant, Title = "تعیین جداول مستقل", Form = FrmEntityIsIndependent });
                }
            }
        }
        private Task<bool> FrmEntityIsIndependentHasData()
        {
            return Task.Run(() =>
            {
                return bizTableDrivedEntity.ExistsEnabledEntitiesWithNullIndependentProperty(Database.ID);
            });
        }

        private async void CheckEntityIsDataReference()
        {
            if (!steps.Any(x => x.Form == FrmEntityIsDataReference))
            {
                if (await FrmEntityIsDataReferenceHasData())
                {
                    steps.Add(new MyProject_WPF.ImportWizardStep() { Index = 4, State = WizrdState.EntityIsDataReference, Title = "تعیین جداول پایه", Form = FrmEntityIsDataReference });
                }
            }
        }

        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        private Task<bool> FrmEntityIsDataReferenceHasData()
        {
            return Task.Run(() =>
            {
                return bizTableDrivedEntity.ExistsEnabledEntitiesWithNullDataReference(Database.ID);
            });
        }


        private void FrmImportTables_NewEntitiesAdded(object sender, EventArgs e)
        {

            CheckEntityIsDataReference();
            CheckEntityIsIndependent();
            CheckNavigationTree();
            CheckEntitySettings();
        }

        frmImportTables _FrmImportTables;
        frmImportTables FrmImportTables
        {
            get
            {
                if (_FrmImportTables == null)
                {
                    _FrmImportTables = new frmImportTables(Database);
                    SetEvents(_FrmImportTables);
                }
                return _FrmImportTables;
            }
        }

        private void SetEvents(ImportWizardForm importWizardForm)
        {
            importWizardForm.InfoUpdated += _FrmImportTables_InfoUpdated;
            importWizardForm.FormIsBusy += ImportWizardForm_FormIsBusy;
            importWizardForm.FormIsFree += ImportWizardForm_FormIsFree;
        }

        private void ImportWizardForm_FormIsFree(object sender, EventArgs e)
        {
            (sender as UIElement).IsEnabled = true;
        }

        private void ImportWizardForm_FormIsBusy(object sender, EventArgs e)
        {
            (sender as UIElement).IsEnabled = false;
        }

        frmImportRelationships _FrmImportRelationships;
        frmImportRelationships FrmImportRelationships
        {
            get
            {
                if (_FrmImportRelationships == null)
                {
                    _FrmImportRelationships = new frmImportRelationships(Database);
                    SetEvents(_FrmImportRelationships);
                }
                return _FrmImportRelationships;
            }
        }


        frmImportViewsAndFunctions _FrmImportViewsAndFunctions;
        frmImportViewsAndFunctions FrmImportViewsAndFunctions
        {
            get
            {
                if (_FrmImportViewsAndFunctions == null)
                {
                    _FrmImportViewsAndFunctions = new frmImportViewsAndFunctions(Database);
                    SetEvents(_FrmImportViewsAndFunctions);
                }
                return _FrmImportViewsAndFunctions;
            }
        }

        //ImportWizardForm _FrmImportViews;
        //ImportWizardForm FrmImportViews
        //{
        //    get
        //    {
        //        if (_FrmImportViews == null)
        //        {
        //            _FrmImportViews = new frmImportViews(Database);
        //            _FrmImportViews.InfoUpdated += _FrmImportTables_InfoUpdated;
        //        }
        //        return _FrmImportViews;
        //    }
        //}

        //ImportWizardForm _FrmImportFunctions;
        //ImportWizardForm FrmImportFunctions
        //{
        //    get
        //    {
        //        if (_FrmImportFunctions == null)
        //        {
        //            _FrmImportFunctions = new frmImportFunctions(Database);
        //            _FrmImportFunctions.InfoUpdated += _FrmImportTables_InfoUpdated;
        //        }
        //        return _FrmImportFunctions;
        //    }
        //}


        frmEntityIsDataReference _FrmEntityIsDataReference;
        frmEntityIsDataReference FrmEntityIsDataReference
        {
            get
            {
                if (_FrmEntityIsDataReference == null)
                {
                    _FrmEntityIsDataReference = new frmEntityIsDataReference(Database);
                    SetEvents(_FrmEntityIsDataReference);
                }
                return _FrmEntityIsDataReference;
            }
        }

        frmEntityIsIndependent _FrmEntityIsIndependent;
        frmEntityIsIndependent FrmEntityIsIndependent
        {
            get
            {
                if (_FrmEntityIsIndependent == null)
                {
                    _FrmEntityIsIndependent = new frmEntityIsIndependent(Database);
                    SetEvents(_FrmEntityIsIndependent);
                }
                return _FrmEntityIsIndependent;
            }
        }

        frmNavigationTree _FrmNavigationTree;
        frmNavigationTree FrmNavigationTree
        {
            get
            {
                if (_FrmNavigationTree == null)
                {
                    _FrmNavigationTree = new frmNavigationTree(Database, true);
                    SetEvents(_FrmNavigationTree);
                }
                return _FrmNavigationTree;
            }
        }
        frmEntitySettings _FrmEntitySettings;
        frmEntitySettings FrmEntitySettings
        {
            get
            {
                if (_FrmEntitySettings == null)
                {
                    _FrmEntitySettings = new frmEntitySettings(Database);
                    SetEvents(_FrmEntitySettings);
                }
                return _FrmEntitySettings;
            }
        }

        public delegate void UpdateDetailInfoDelegate(string message, int currentProgress, int totalProgress);

        private void _FrmImportTables_InfoUpdated(object sender, ItemImportingStartedArg e)
        {

            //چون از یک ترد دیگه (اسینک) اومده
            lblJobDetail.Dispatcher.Invoke(
                     new UpdateDetailInfoDelegate(this.UpdateDetailInfo),
                     new object[] { e.ItemName, e.CurrentProgress, e.TotalProgressCount }
                 );
        }
        private void UpdateDetailInfo(string message, int currentProgress, int totalProgress)
        {
            lblJobDetail.Text = message;
            if (totalProgress != 0 && currentProgress != 0)
            {
                int percent = (currentProgress * 100) / totalProgress;
                lblPercent.Text = percent.ToString() + "%";
                if (lblPercent.Visibility == Visibility.Collapsed)
                    lblPercent.Visibility = Visibility.Visible;
            }
            else if (currentProgress != 0)
            {
                lblPercent.Text = currentProgress.ToString();
                if (lblPercent.Visibility == Visibility.Collapsed)
                    lblPercent.Visibility = Visibility.Visible;
            }
            else
                lblPercent.Visibility = Visibility.Collapsed;
        }
        //private void ImposeWizardState(LinkedListNode<ImportWizardStep> step)
        //{
        // //   CurrentState = step;
        //    ShowContent(step.Value.Form);
        //}

        private void ShowContent(object userControl)
        {
            foreach (var item in grdContent.Children)
            {
                if (item is UIElement)
                    (item as UIElement).Visibility = Visibility.Collapsed;
            }
            UIElement element = null;
            foreach (var item in grdContent.Children)
            {
                if (item == userControl)
                    element = item as UIElement;
            }
            if (element == null)
            {
                element = userControl as UIElement;
                grdContent.Children.Add(element);
            }
            element.Visibility = Visibility.Visible;
        }


        //private WizrdState GetNextState()
        //{
        //    if (CurrentState == WizrdState.ImportTables)
        //    {
        //        return WizrdState.ImportRelationships;
        //    }
        //    else if (CurrentState == WizrdState.ImportRelationships)
        //    {
        //        return WizrdState.ImportViewAndFunctions;
        //    }
        //    //else if (CurrentState == WizrdState.ImportViewAndFunctions)
        //    //{
        //    //    return WizrdState.ImportFunctions;
        //    //}
        //    else if (CurrentState == WizrdState.ImportViewAndFunctions)
        //    {
        //        return WizrdState.TablePropertiesIsDataReference;
        //    }
        //    else if (CurrentState == WizrdState.TablePropertiesIsDataReference)
        //    {
        //        return WizrdState.TableProperties;
        //    }
        //    else if (CurrentState == WizrdState.TableProperties)
        //    {
        //        return WizrdState.NavigationTree;
        //    }
        //    else if (CurrentState == WizrdState.NavigationTree)
        //    {
        //        return WizrdState.TableFinalSettings;
        //    }
        //    return WizrdState.TableFinalSettings;
        //}
        //private WizrdState GetPrevState()
        //{
        //    if (CurrentState == WizrdState.ImportRelationships)
        //    {
        //        return WizrdState.ImportTables;
        //    }
        //    else if (CurrentState == WizrdState.ImportViewAndFunctions)
        //    {
        //        return WizrdState.ImportRelationships;
        //    }
        //    //else if (CurrentState == WizrdState.ImportFunctions)
        //    //{
        //    //    return WizrdState.ImportViews;
        //    //}
        //    else if (CurrentState == WizrdState.TablePropertiesIsDataReference)
        //    {
        //        return WizrdState.ImportViewAndFunctions;
        //    }
        //    else if (CurrentState == WizrdState.TableProperties)
        //    {
        //        return WizrdState.TablePropertiesIsDataReference;
        //    }
        //    else if (CurrentState == WizrdState.NavigationTree)
        //    {
        //        return WizrdState.TableProperties;
        //    }
        //    else if (CurrentState == WizrdState.TableFinalSettings)
        //    {
        //        return WizrdState.NavigationTree;
        //    }
        //    return WizrdState.ImportTables;
        //}

    }

    public enum WizrdState
    {
        ImportTables,
        ImportRelationships,
        ImportViewAndFunctions,
        //  ImportViews,
        //   ImportFunctions,
        EntityIsIndependant,
        NavigationTree,
        EntityIsDataReference,
        TableFinalSettings
    }
    public interface ImportWizardForm
    {
        event EventHandler<ItemImportingStartedArg> InfoUpdated;
        event EventHandler FormIsBusy;
        event EventHandler FormIsFree;
        //  bool HasData();
    }
    public class ImportWizardStep
    {
        public int Index { set; get; }
        public WizrdState State { set; get; }
        public string Title { set; get; }
        public ImportWizardForm Form { set; get; }
    }
}
