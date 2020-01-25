using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;
using MyCodeFunctionLibrary;
using MyCommonWPFControls;


namespace MyUILibrary.EntityArea
{
    class EditLetterArea : I_EditEntityLetterArea
    {
        LetterSettingDTO LetterSetting { set; get; }
        //public DP_DataRepository DataInstance { set; get; }

        CodeFunctionHandler codeFunctionHandler = new CodeFunctionHandler();
        private object entitySearchLookup;
        private MySearchLookup relatedLetterSearchLookup;

        LetterDTO LetterMessage { set; get; }
        public LetterAreaInitializer AreaInitializer { set; get; }
        public EditLetterArea(LetterAreaInitializer areaInitializer)
        {
            AreaInitializer = areaInitializer;
            //EntityID = entityId;

            var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), areaInitializer.DataInstance.TargetEntityID, false);
            if (!permissions.GrantedActions.Any(x => x == SecurityAction.LetterEdit))
            {
                return;
            }
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfLetterArea();
            SetExtensions();
            View.GenerateFileClicked += View_GenerateFileClicked;
            View.LetterConfirmed += View_LetterConfirmed;
            View.NewClicked += View_NewClicked;
            View.ShowExternalFileClicked += View_ShowExternalFileClicked;
            View.ShowFileClicked += View_ShowFileClicked;
            View.DownloadFileClicked += View_DownloadFileClicked;
            View.ConvertToExternalClicked += View_ConvertToExternalClicked;
            View.GenerateOrSelectClicked += View_GenerateOrSelectClicked;
            View.ExternalOrInternalClicked += View_ExternalOrInternalClicked;
            View.ExternalInfoRequested += View_ExternalInfoRequested;
            View.ShowGeneratedFileClicked += View_ShowGeneratedFileClicked;
            relatedLetterSearchLookup = new MySearchLookup();
            relatedLetterSearchLookup.DisplayMember = "Title";
            relatedLetterSearchLookup.SelectedValueMember = "ID";
            relatedLetterSearchLookup.SearchFilterChanged += RelatedLetterSearchLookup_SearchFilterChanged;
            relatedLetterSearchLookup.SelectionChanged += RelatedLetterSearchLookup_SelectionChanged;
            View.AddRelatedLetterSelector(relatedLetterSearchLookup);

            if (AreaInitializer.LetterID != 0)
            {
                LetterMessage = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetter(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.LetterID, true);
                AreaInitializer.DataInstance = LetterMessage.DataItem;
            }
            else
            {
                LetterMessage = new LetterDTO();
            }
            ShowLetter();
            var letterTemplates = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetMainLetterTemplates(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.DataInstance.TargetEntityID);
            View.LetterTemplates = letterTemplates;

            View.LetterTypes = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetterTypes();
            LetterSetting = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetterSettings();
            //if(LetterSetting==null||LetterSetting.LetterExternalInfoCode==null)

        }

        private void View_DownloadFileClicked(object sender, EventArgs e)
        {
            if (LetterMessage.AttechedFile != null)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.DownloadFile(LetterMessage.AttechedFile, false);
            }
        }

        private void RelatedLetterSearchLookup_SelectionChanged(object sender, SelectionChangedArg e)
        {

        }

        private void RelatedLetterSearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var letter = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetter(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), Convert.ToInt32(e.SingleFilterValue), false);
                    e.ResultItemsSource = new List<LetterDTO> { letter };
                }
                else
                {
                    var letters = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.SearchLetters(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.SingleFilterValue);
                    e.ResultItemsSource = letters;
                }
            }
        }

        private void View_ConvertToExternalClicked(object sender, EventArgs e)
        {
            if (LetterMessage.AttechedFileID == 0)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("فایلی برای این نامه موجود نمی باشد");
                return;
            }
            if (LetterSetting != null && LetterSetting.LetterSendToExternalCodeID != 0)
            {
                var result = codeFunctionHandler.GetLetterSendingCodeFunctionResult(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), LetterSetting.LetterSendToExternalCodeID, LetterMessage);
                if (result.ExecutionException == null)
                {

                    if (Convert.ToBoolean(result.Result))
                    {
                        var convertresult = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.ConvertLetterToExternal(LetterMessage.ID, result.ExternalCode);
                        if (convertresult)
                        {
                            LetterMessage.ExternalCode = result.ExternalCode;
                            ShowLetter();
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    //پیام خطا
                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("رویه ای به منظور تبدیل نامه به نامه خارجی موجود نمی باشد");
                return;
            }
        }

        private void View_ExternalInfoRequested(object sender, LetterExternalInfoRequestedArg e)
        {
            if (LetterSetting != null && LetterSetting.LetterExternalInfoCodeID != 0)
            {


                View.UpdateMessage();

                LetterDTO sendingLetter = new LetterDTO();
                sendingLetter.ExternalCode = e.ExternalCode;
                var result = codeFunctionHandler.GetCodeFunctionResult(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), LetterSetting.LetterExternalInfoCodeID, sendingLetter);
                if (result.ExecutionException == null)
                {



                    if (!string.IsNullOrEmpty(sendingLetter.Title))
                        LetterMessage.Title = sendingLetter.Title;
                    LetterMessage.LetterDate = sendingLetter.LetterDate;
                    if (string.IsNullOrEmpty(LetterMessage.Desc) && !string.IsNullOrEmpty(sendingLetter.Desc))
                        LetterMessage.Desc = sendingLetter.Desc;


                    LetterMessage.AttechedFile = sendingLetter.AttechedFile;
                    if (LetterMessage.AttechedFile != null && LetterMessage.AttechedFile.Content != null)
                    {
                        if (LetterMessage.AttechedFile.FileName == null)
                            LetterMessage.AttechedFile.FileName = "فایل ضمیمه دارد";
                        if (LetterMessage.AttechedFile.FileExtension == null)
                            LetterMessage.AttechedFile.FileExtension = "???";
                        View.ViewExternalFileInfo(LetterMessage.AttechedFile.FileName + "." + LetterMessage.AttechedFile.FileExtension);
                        View.ViewExternalFilePanelVisibility = true;
                    }
                    else
                    {
                        View.ViewExternalFilePanelVisibility = false;
                        LetterMessage.AttechedFile = null;
                    }
                    //اگر فایل از منبع خارجی کلا حذف شده بود چی؟
                    //LetterMessage.IsExternalOrInternal = true;
                    View.ShowMessage(LetterMessage);


                }
                else
                {

                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("رویه ای به منظور دربافت اطلاعات نامه خارجی موجود نمی باشد");
                return;
            }
        }

        private void View_ExternalOrInternalClicked(object sender, LetterInternalExternalArg e)
        {
            if (LetterMessage.ID == 0)
            {
                if (e.ExternalInternal)
                {
                    View.ExternalInfoPanelVisibility = true;
                    View.GenerateOrSelectPanelVisible = false;
                }
                else
                {
                    View.ExternalInfoPanelVisibility = false;
                    View.GenerateOrSelectPanelVisible = true;
                }
            }
        }

        private void View_GenerateOrSelectClicked(object sender, LetterFileSelectGenerateArg e)
        {


            if (e.GenerateOrSelect)
            {
                View.FileGenerationPanelVisibility = true;
                View.FileSelectionPanelVisibility = false;
            }
            else
            {
                View.FileGenerationPanelVisibility = false;
                View.FileSelectionPanelVisibility = true;
            }

        }

        private void View_ShowFileClicked(object sender, EventArgs e)
        {
            if (LetterMessage.AttechedFile != null)
            {
                var fileName = LetterMessage.AttechedFile.FileName + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + LetterMessage.AttechedFile.FileExtension;
                View.OpenFile(LetterMessage.AttechedFile.Content, fileName);
            }
        }
        private void View_ShowExternalFileClicked(object sender, EventArgs e)
        {
            if (LetterMessage.AttechedFile != null)
            {
                var fileName = LetterMessage.AttechedFile.FileName + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + LetterMessage.AttechedFile.FileExtension;
                View.OpenFile(LetterMessage.AttechedFile.Content, fileName);
            }
        }
        string genereatedFilePath = "";
        private void View_GenerateFileClicked(object sender, LetterGenerateArg e)
        {
            //var letterTemplete = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetterTemplate(e.LetterTemplate.);
            byte[] result = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GenereateLetter(e.LetterTemplate.ID, AreaInitializer.DataInstance.KeyProperties, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            //var generatedFile = GenerateLetterTemplate(letterTemplete);
            genereatedFilePath = e.LetterTemplate.Name + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "");
            var showresult = View.ShowGeneratedFile(result, genereatedFilePath, e.LetterTemplate.FileExtension);
            if (showresult.Result)
                View.ShowGeneretedFileVisibility = true;
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("خطا", "خطا در نمایش فایل تولید شده" + Environment.NewLine + showresult.Exception);
            }
        }

        private void View_ShowGeneratedFileClicked(object sender, EventArgs e)
        {
            var showresult = View.ShowGeneratedFile();
            if (!showresult.Result)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("خطا", "خطا در نمایش فایل تولید شده" + Environment.NewLine + showresult.Exception);
            }
        }

        private void SetExtensions()
        {
            View.Extentions = new List<Tuple<string, List<string>>>();
            View.Extentions.Add(new Tuple<string, List<string>>("MS Word", new List<string>() { "*.doc", "*.docx" }));
            View.Extentions.Add(new Tuple<string, List<string>>("PDF", new List<string>() { "*.pdf", "*.pdf" }));
            View.Extentions.Add(new Tuple<string, List<string>>("JPEG", new List<string>() { "JPG", "*.jpg", "*.jpeg" }));
        }

        public event EventHandler LetterUpdated;

        private void View_NewClicked(object sender, EventArgs e)
        {
            LetterMessage = new LetterDTO();
            ShowLetter();
        }

        private void View_LetterConfirmed(object sender, LetterConfirmedArg e)
        {
            View.UpdateMessage();
            if (relatedLetterSearchLookup.SelectedItem != null)
                LetterMessage.RelatedLetterID = (relatedLetterSearchLookup.SelectedItem as LetterDTO).ID;
            else
                LetterMessage.RelatedLetterID = 0;
            if (LetterMessage.LetterTypeID == 0)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("نوع نامه مشخص نشده است");
                return;
            }

            if (LetterMessage.ID == 0)
            {
                LetterMessage.DataItem = AreaInitializer.DataInstance;
                //LetterMessage.CreatorDataItem.KeyProperties = KeyProperties;
                //LetterMessage.CreatorDataItem.TableDrivedEntityID = EntityID;
            }


            if (LetterMessage.IsExternalOrInternal)
            {
                if (string.IsNullOrEmpty(LetterMessage.ExternalCode))
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("کد نامه خارجی مشخص نشده است");
                    return;
                }
            }

            var result = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.UpdateLetter(LetterMessage, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("پیام", result.Message, result.Details);
            if (result.Result)
            {
                LetterMessage = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetter(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), result.SavedID, true);
                ShowLetter();
                if (LetterUpdated != null)
                    LetterUpdated(this, null);
            }

        }

        //public void NewLetterRequested()
        //{
        //    ClearForm();
        //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(View, "نامه");
        //}

        //private void ClearForm()
        //{

        //    View.ConvertToExternalPanelVisibility = false;
        //    View.ShowMessage(LetterMessage);
        //    View.ExternalInternalPanelEnabled = true;
        //    View.GenerateOrSelectPanelVisible = true;
        //    View.ViewExistingFilePanelVisibility = false;
        //    View.FileGenerationPanelVisibility = false;
        //    View.FileSelectionPanelVisibility = false;
        //    View.ViewExternalFilePanelVisibility = false;
        //    View.ClearSelectedFiles();

        //}

        public void ShowLetter()
        {


            //  ClearForm();
            //View.ConvertToExternalVisibility = false;
            ////View.ConvertToExternalPanelVisibility = false;
            //View.GenerateOrSelectPanelVisible = true;
            //View.ViewExistingFilePanelVisibility = false;
            //View.FileGenerationPanelVisibility = false;
            //View.FileSelectionPanelVisibility = false;
            //View.ViewExternalFilePanelVisibility = false;
            View.ClearSelectedFiles();

            //فقط زمانی دیده میشود که دریافت اطلاعات خارجی زده شود و فایل هم دریافت شود.موقتا فایل را نمایش میدهد
            View.ViewExternalFilePanelVisibility = false;

            View.ShowGeneretedFileVisibility = false;

            relatedLetterSearchLookup.SelectedValue = LetterMessage.RelatedLetterID;
            if (LetterMessage.ID == 0)
            {
                View.ExternalInternalPanelEnabled = true;
                View.ConvertToExternalVisibility = false;
                View.CreateDateAndUserVisibility = false;
            }
            else
            {
                View.ExternalInternalPanelEnabled = false;
                if (LetterMessage.IsExternalOrInternal || !string.IsNullOrEmpty(LetterMessage.ExternalCode) || LetterMessage.AttechedFileID == 0)
                    View.ConvertToExternalVisibility = false;
                else
                    View.ConvertToExternalVisibility = true;
                View.CreateDateAndUserVisibility = true;
            }

            if (LetterMessage.IsExternalOrInternal)
            {
                View.GenerateOrSelectPanelVisible = false;
            }
            //else
            //{

            //}

            if (string.IsNullOrEmpty(LetterMessage.ExternalCode))
            {
                View.ExternalCodeDisabled = false;
                if (!LetterMessage.IsExternalOrInternal)
                    View.ExternalInfoPanelVisibility = false;
            }
            else
            {
                View.ExternalInfoPanelVisibility = true;
                View.ExternalCodeDisabled = true;
            }

            if (LetterMessage.AttechedFileID != 0)
            {
                View.ViewExistingFilePanelVisibility = true;

                View.GenerateOrSelectPanelVisible = false;
                if (LetterMessage.AttechedFile != null)
                    View.ViewExistingFile(LetterMessage.AttechedFile.FileName + "." + LetterMessage.AttechedFile.FileExtension);
            }
            else
            {
                View.ViewExistingFilePanelVisibility = false;
            }

            if (View.GenerateOrSelectPanelVisible == true)
            {
                if (LetterMessage.IsGeneratedOrSelected == true)
                    View.FileSelectionPanelVisibility = false;
                else if (LetterMessage.IsGeneratedOrSelected == false)
                    View.FileGenerationPanelVisibility = true;
                else
                {
                    View.FileSelectionPanelVisibility = false;
                    View.FileGenerationPanelVisibility = false;
                }
            }
            if (LetterMessage.UserID != 0)
            {
                var user = AgentUICoreMediator.GetAgentUICoreMediator.userManagerService.GetUser(LetterMessage.UserID);
                LetterMessage.vwUser = user.FullName;
            }
            //if (LetterMessage.IsExternalOrInternal == false)
            //{
            //    if (LetterMessage.AttechedFileID != 0)
            //    {
            //        View.GenerateOrSelectPanelVisible = false;
            //        View.FileGenerationPanelVisibility = false;
            //        View.FileSelectionPanelVisibility = false;

            //        //موقتی چون چیز دیگه ای تو پنل اینترنال موجود نیست
            //        View.InternalPanelVisibility = false;
            //    }
            //    else
            //    {
            //        View.GenerateOrSelectPanelVisible = true;
            //        View.FileGenerationPanelVisibility = LetterMessage.IsGeneratedOrSelected == true;
            //        View.FileSelectionPanelVisibility = LetterMessage.IsGeneratedOrSelected == false;
            //    }
            //}
            View.ShowMessage(LetterMessage);
        }
        //public int EntityID
        //{
        //    set; get;
        //}

        //public List<EntityInstanceProperty> KeyProperties
        //{
        //    set; get;
        //}

        public I_View_LetterArea View
        {
            set; get;
        }
    }
}
