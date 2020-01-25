using MyDataManagerService;

using MyUILibrary.EntityArea;
using MyWorkflowService;
using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.WorkflowArea
{
    public class WorkflowNoteArea : I_WorkflowNoteArea
    {

        public I_View_RequestNote View
        {
            set; get;
        }
        int RequestID { set; get; }
        public WorkflowNoteArea(int requestID)
        {
            RequestID = requestID;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetRequestNoteForm();
            ShowRequestNotes();
            View.RequestNoteSelected += View_RequestNoteSelected;
            View.RequestNoteConfirmed += View_RequestNoteConfirmed;
            View.RequestNoteClear += View_RequestNoteClear;
            View.CloseRequested += View_CloseRequested;
        }

        private void View_CloseRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }
        private void View_RequestNoteClear(object sender, EventArgs e)
        {
          ClearDataEntry();
            
        }

        
        private void View_RequestNoteConfirmed(object sender, EventArgs e)
        {
            RequestNoteDTO message = new RequestNoteDTO();
            message.ID = View.ID;
            message.NoteTitle = View.Title;
            message.Note = View.Note;
            AgentUICoreMediator.GetAgentUICoreMediator.workflowService.SaveRequestNote(message, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            ShowRequestNotes();
            ClearDataEntry();
        }

    

        private void View_RequestNoteSelected(object sender, RequestNoteSelectedArg e)
        {
            ShowRequestNote(e.RequestNote);
            if (e.RequestNote.UserID == AgentUICoreMediator.GetAgentUICoreMediator.GetRequester().Identity)
            {
                if (e.RequestNote.ID != 0)
                {
                    View.EditEnabled = true;
                }
                else
                {
                    View.ShowMessage("متن های اقدامات انجام شده قابل اصلاح نمی باشند");
                    View.EditEnabled = false;
                }
            }
            else
            {
                View.ShowMessage("تنها کاربر ایجاد کننده قادر به تغییر متن می باشد");
                View.EditEnabled = false;
            }
        }
        private void ShowRequestNotes()
        {
            var list = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetRequestNotes(RequestID, true);
            View.ShowRequestNotes(list);
        }
        private void ClearDataEntry()
        {
            View.ID = 0;
            View.Title = "";
            View.Note = "";
            View.EditEnabled = true;
            View.ShowMessage("");
        }

        private void ShowRequestNote(RequestNoteDTO requestNote)
        {
            View.ID = requestNote.ID;
            View.Title = requestNote.NoteTitle;
            View.Note = requestNote.Note;
        }
    }
}
