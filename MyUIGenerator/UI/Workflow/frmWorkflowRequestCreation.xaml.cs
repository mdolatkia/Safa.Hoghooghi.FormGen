using MyUILibrary.EntityArea;

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
using System.Windows.Shapes;

using MyUILibrary.WorkflowArea;
using System.Windows.Threading;
using ProxyLibrary;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmWorkflowRequestCreation.xaml
    /// </summary>
    public partial class frmWorkflowRequestCreation : UserControl, I_View_WorkflowRequestCreator
    {
        //private UIManager uIManager;
        private List<ProcessDTO> processList;

        //public event EventHandler<EntitySelectorArg> EntitySelectorRequested;
        public event EventHandler WorkflowRequestCreate;
        public event EventHandler<ProcessSelectedArg> ProcessSelected;
        public event EventHandler<StateSelectedArg> StateSelected;
        //public event EventHandler<WorkflowAdminSearchArg> WorkflowAdminSearched;
        //public event EventHandler<WorkflowAdminSelectedArg> WorkflowAdminSelected;
        //public event EventHandler<WorkflowStackholderSearchArg> WorkflowStackholderSearched;
        //public event EventHandler<WorkflowStackholderSelectedArg> WorkflowStackholderSelected;
        public event EventHandler<OrganizationPostDTO> CurrentUserOrganizationPostChanged;
        public event EventHandler<PossibleTransitionActionDTO> TargetTransitionActionSelected;
        public event EventHandler<string> OganizationPostsSearchChanged;
        public event EventHandler CloseRequested;

        //CreateRequestDTO RequestMessage;
        public frmWorkflowRequestCreation(List<ProcessDTO> processList)
        {
            InitializeComponent();
            //dtgOutgoingTransitoinActions.SelectionChanged += DtgOutgoingTransitoinActions_SelectionChanged;
            //dtgOutgoingTransitoinActions.RowLoaded += DtgOutgoingTransitoinActions_RowLoaded;
            //RequestMessage = new CreateRequestDTO();
            //ShowRequestFiles();
            this.processList = processList;
            lokProcess.ItemsSource = processList;
            lokProcess.DisplayMember = "Name";
            lokProcess.SelectedValueMember = "ID";

         
        }

        private void DtgOutgoingTransitoinActions_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is PossibleTransitionActionDTO)
            {
                var item = e.DataElement as PossibleTransitionActionDTO;
                if (item.Color == ItemColor.Red)
                    e.Row.Foreground = new SolidColorBrush(Colors.Red);
                else if (item.Color == ItemColor.Green)
                    e.Row.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        //private void ShowRequestFiles()
        //{
        //    dtgFiles.ItemsSource = null;
        //    dtgFiles.ItemsSource = _RequestFiles;
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (WorkflowRequestCreate != null)
                WorkflowRequestCreate(this, null);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }


        public void RemoveEntitySelector()
        {
            if (_SearchView != null)
            {
                grdForm.Children.Remove(_SearchView as UIElement);
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        I_View_TemporaryView _SearchView;
        public I_View_TemporaryView SearchView
        {
            set
            {
                if (_SearchView != null)
                {
                    grdForm.Children.Remove(_SearchView as UIElement);
                }
                _SearchView = value;
                Grid.SetRow(_SearchView as UIElement, 2);
                Grid.SetColumn(_SearchView as UIElement, 1);
                grdForm.Children.Add(_SearchView as UIElement);
            }
            get
            {
                return _SearchView;
            }
        }

        public List<WFStateDTO> States
        {

            set
            {
                cmbState.SelectedValuePath = "ID";
                cmbState.DisplayMemberPath = "Name";
                cmbState.ItemsSource = value;
            }
        }

        //public List<RoleTransitoinActionDTO> RoleTransitoinActions
        //{
        //    get
        //    {

        //        return lstRoleTransitionActions.ItemsSource as List<RoleTransitoinActionDTO>;
        //    }
        //    set
        //    {
        //        lstRoleTransitionActions.DisplayMemberPath = Name;
        //        lstRoleTransitionActions.ItemsSource = value;
        //    }
        //}

        //public List<OrganizationPostDTO> AdminSearchResult
        //{
        //    set
        //    {
        //        lstAdminsSearch.SelectedValuePath = "ID";
        //        lstAdminsSearch.DisplayMemberPath = "Name";
        //        lstAdminsSearch.ItemsSource = value;
        //    }
        //}

        //private void btnAdminSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    if (WorkflowAdminSearched != null)
        //        WorkflowAdminSearched(this, new WorkflowAdminSearchArg() { Search = txtSearchAdmins.Text });
        //}

        //private void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        //{
        //    if (WorkflowAdminSelected != null)
        //    {
        //        if (lstAdminsSearch.SelectedItems != null)
        //        {
        //            List<OrganizationPostDTO> selectedAdmins = new List<OrganizationPostDTO>();
        //            foreach (var item in lstAdminsSearch.SelectedItems)
        //                selectedAdmins.Add(item as OrganizationPostDTO);
        //            WorkflowAdminSelected(this, new WorkflowAdminSelectedArg() { Posts = selectedAdmins });
        //        }
        //    }
        //}

        //public List<OrganizationPostDTO> ProcessAdmins
        //{
        //    get
        //    {
        //        return lstAdmins.ItemsSource as List<OrganizationPostDTO>;
        //    }

        //    set
        //    {
        //        lstAdmins.DisplayMemberPath = "Name";
        //        lstAdmins.SelectedValuePath = "ID";
        //        lstAdmins.ItemsSource = value;
        //    }
        //}


        //private void btnStackholderSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    if (WorkflowStackholderSearched != null)
        //        WorkflowStackholderSearched(this, new WorkflowStackholderSearchArg() { Search = txtSearchStackholders.Text });
        //}

        //private void btnAddStackholder_Click(object sender, RoutedEventArgs e)
        //{
        //    if (WorkflowStackholderSelected != null)
        //    {
        //        if (lstStackholdersSearch.SelectedItems != null)
        //        {
        //            var selectedStackholders = new List<OrganizationPostDTO>();
        //            foreach (OrganizationPostDTO item in lstStackholdersSearch.SelectedItems)
        //                selectedStackholders.Add(item);
        //            WorkflowStackholderSelected(this, new WorkflowStackholderSelectedArg() { Posts = selectedStackholders });
        //        }
        //    }
        //}
        //public List<OrganizationPostDTO> StackholderSearchResult
        //{
        //    set
        //    {
        //        lstStackholdersSearch.SelectedValuePath = "ID";
        //        lstStackholdersSearch.DisplayMemberPath = "Name";
        //        lstStackholdersSearch.ItemsSource = value;
        //    }
        //}

        //public List<OrganizationPostDTO> ProcessStackholders
        //{
        //    get
        //    {
        //        return lstStackholders.ItemsSource as List<OrganizationPostDTO>;
        //    }

        //    set
        //    {
        //        lstStackholders.DisplayMemberPath = "Name";
        //        lstStackholders.SelectedValuePath = "ID";
        //        lstStackholders.ItemsSource = value;
        //    }
        //}
        public ProcessDTO SelectedProcess
        {
            get
            {
                return lokProcess.SelectedItem as ProcessDTO;
            }
        }
        public List<OrganizationPostDTO> CreatorOrganizationPosts
        {
            set
            {
                cmbRequesterRole.SelectedValuePath = "ID";
                cmbRequesterRole.DisplayMemberPath = "Name";
                cmbRequesterRole.ItemsSource = value;
            }
        }

        public OrganizationPostDTO CurrentUserSelectedOrganizationPost
        {
            get
            {
                if (cmbRequesterRole.SelectedItem != null)
                    return cmbRequesterRole.SelectedItem as OrganizationPostDTO;
                else return null;
            }

            set
            {
                cmbRequesterRole.SelectedItem = value;
            }
        }

        public int SelectedStateID
        {
            get
            {
                if (cmbState.SelectedItem != null)
                    return (cmbState.SelectedItem as WFStateDTO).ID;
                else return 0;
            }

            set
            {
                cmbState.SelectedValue = value;
            }
        }

  

        public DateTime? Date
        {
            get
            {
                return txtDate.SelectedDate;
            }

            set
            {
                txtDate.SelectedDate = value;
            }
        }



        public string Title
        {
            get
            {
                return txtTitle.Text;
            }

            set
            {
                txtTitle.Text = value;
            }
        }

        //public string Description
        //{
        //    get
        //    {
        //        return txtDesc.Text;
        //    }

        //    set
        //    {
        //        txtDesc.Text = value;
        //    }
        //}
        //public List<RequestFileDTO> RequestFiles
        //{
        //    get
        //    {
        //        return _RequestFiles;
        //    }
        //}

        public List<RequestNoteDTO> RequestNotes
        {
            get
            {
                if (txtNoteTitle.Text != "" || txtNote.Text != "")
                {
                    var note = new RequestNoteDTO();
                    note.Note = txtNote.Text;
                    note.NoteTitle = txtNoteTitle.Text;
                    return new List<RequestNoteDTO>() { note };
                }
                else
                    return new List<RequestNoteDTO>();
            }
        }
        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbState.SelectedItem != null)
            {
                if (StateSelected != null)
                {
                    StateSelected(this, new StateSelectedArg() { StateID = (cmbState.SelectedItem as WFStateDTO).ID });
                }
            }
        }
      
        private void lokProcess_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (lokProcess.SelectedItem != null)
            {
                if (ProcessSelected != null)
                    ProcessSelected(this, new ProcessSelectedArg() { ProcessID = (int)lokProcess.SelectedValue });
            }
        }


        //System.Windows.Forms.OpenFileDialog fileDialog;
        //private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        //{
        //    if (fileDialog == null)
        //        fileDialog = new System.Windows.Forms.OpenFileDialog();
        //    fileDialog.Filter =
        //  "PDF (*.pdf)|*.pdf";
        //    var result = fileDialog.ShowDialog();
        //    switch (result)
        //    {
        //        case System.Windows.Forms.DialogResult.OK:
        //            txtFilePath.Text = fileDialog.FileName;
        //            break;
        //        case System.Windows.Forms.DialogResult.Cancel:
        //        default:
        //            break;
        //    }
        //}
        //List<RequestFileDTO> _RequestFiles = new List<RequestFileDTO>();
        //private void btnAddFile_Click(object sender, RoutedEventArgs e)
        //{
        //    if (fileDialog != null && txtFilePath.Text != "")
        //    {
        //        var file = new RequestFileDTO();
        //        file.FileName = System.IO.Path.GetFileName(txtFilePath.Text);
        //        file.FileContent = System.IO.File.ReadAllBytes(txtFilePath.Text);
        //        file.FileDesc = txtFileDesc.Text;
        //        file.MIMEType = "";
        //        _RequestFiles.Add(file);
        //        ShowRequestFiles();
        //    }
        //}

        private void cmbRequesterRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentUserOrganizationPostChanged != null)
            {
                CurrentUserOrganizationPostChanged(this, cmbRequesterRole.SelectedItem as OrganizationPostDTO);
            }
        }
     
        public void AddTargetSelectionView(I_View_WorkflowTransitionTargetSelection view)
        {
            tabTargets.Content = view;
        }
    }

}
