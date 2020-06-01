using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;

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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmRequestAction.xaml
    /// </summary>
    public partial class frmRequestNote : UserControl, I_View_RequestNote
    {
        //public event EventHandler RequestActionUpdated;

        public frmRequestNote()
        {
            InitializeComponent();
        }

        public event EventHandler RequestNoteConfirmed;
        public event EventHandler<RequestNoteSelectedArg> RequestNoteSelected;
        public event EventHandler RequestNoteClear;
        public event EventHandler CloseRequested;

        RequestNoteDTO requestNoteMessage;

        public int RequestID
        {
            set; get;
        }

        public string Title
        {
            get
            {
                return txtNoteTitle.Text;
            }

            set
            {
                txtNoteTitle.Text = value;
            }
        }

        public string Note
        {
            get
            {
                return txtNote.Text;
            }

            set
            {
                txtNote.Text = value;
            }
        }

        public int ID
        {
            get
            {
                if (txtID.Text == "")
                    return 0;
                else return Convert.ToInt32(txtID.Text);
            }

            set
            {
                txtID.Text = value.ToString(); 
            }
        }

        public bool EditEnabled
        {
            get
            {
                return btnSave.IsEnabled;
            }

            set
            {
                btnSave.IsEnabled = value;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (RequestNoteConfirmed != null)
                RequestNoteConfirmed(this, null);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }

        public void ShowRequestNote(RequestNoteDTO requestNote)
        {
            requestNoteMessage = requestNote;
            txtNote.Text = requestNoteMessage.Note;
            txtNoteTitle.Text = requestNoteMessage.NoteTitle;
        }

        private void dtgNotes_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var selectedNote = dtgNotes.SelectedItem as RequestNoteDTO;
            if (selectedNote != null)
            {
                if (RequestNoteSelected != null)
                    RequestNoteSelected(this, new RequestNoteSelectedArg() { RequestNote = selectedNote });
            }
        }

        public void ShowRequestNotes(List<RequestNoteDTO> requestNotes)
        {
            dtgNotes.ItemsSource = requestNotes;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (RequestNoteClear != null)
                RequestNoteClear(this, null);
        }

        public void ShowMessage(string message)
        {
            lblMessage.Text = message;
        }
    }
}
