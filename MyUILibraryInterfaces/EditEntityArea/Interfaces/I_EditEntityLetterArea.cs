using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using MyUILibraryInterfaces.DataTreeArea;
using MyUILibraryInterfaces.ContextMenu;

namespace MyUILibrary.EntityArea
{

    public class LettersAreaInitializer
    {
        public int EntityID { set; get; }
        //     public bool UserCanChangeDataItem { set; get; }
        //public bool LoadRelatedItems { set; get; }
        public DP_DataView DataInstance { set; get; }
    }

    public class LetterAreaInitializer
    {
        public int LetterID { set; get; }
        public DP_DataView DataInstance { set; get; }
    }

    public interface I_EditEntityLetterArea
    {
        //int EntityID { set; get; }
        LetterAreaInitializer AreaInitializer { set; get; }
        I_View_LetterArea View { set; get; }
        event EventHandler LetterUpdated;
        //void EditLetterRequested(int letterID);
        //    void NewLetterRequested();
    }
    public interface I_EntityLettersArea
    {
        //int EntityID { set; get; }
        //DP_DataRepository DataItem { set; get; }
        LettersAreaInitializer AreaInitializer { set; get; }
        //List<LetterDTO> Letters { set; get; }
        //I_View_EntityLettersArea View { set; get; }
        object MainView { set; get; }
        //I_EditEntityLetterArea EditLetterArea { set; get; }
    }
    public interface I_View_LetterArea
    {
        List<MainLetterTemplateDTO> LetterTemplates { set; }
        List<LetterTypeDTO> LetterTypes { set; }
        List<Tuple<string, List<string>>> Extentions { get; set; }

        bool ConvertToExternalVisibility { get; set; }
        bool CreateDateAndUserVisibility { get; set; }
        bool ExternalCodeDisabled { get; set; }
        bool ExternalInfoPanelVisibility { get; set; }
        //bool InternalPanelVisibility { get; set; }
        bool FileGenerationPanelVisibility { get; set; }
        bool FileSelectionPanelVisibility { get; set; }
        bool ExternalInternalPanelEnabled { get; set; }
        bool GenerateOrSelectPanelVisible { get; set; }

        bool ShowGeneretedFileVisibility { get; set; }
        //bool ConvertToExternalPanelVisibility { get; set; }
        //bool ExternalInfoPanelVisibility { get; set; }
        bool ViewExistingFilePanelVisibility { get; set; }

        //event EventHandler SelectFileClicked;
        event EventHandler<LetterGenerateArg> GenerateFileClicked;
        event EventHandler<LetterConfirmedArg> LetterConfirmed;
        event EventHandler ShowFileClicked;
        event EventHandler DownloadFileClicked;
        event EventHandler ShowGeneratedFileClicked;

        event EventHandler ShowExternalFileClicked;
        event EventHandler<LetterInternalExternalArg> ExternalOrInternalClicked;
        event EventHandler<LetterFileSelectGenerateArg> GenerateOrSelectClicked;
        event EventHandler<LetterExternalInfoRequestedArg> ExternalInfoRequested;

        event EventHandler ConvertToExternalClicked;
        event EventHandler CloseRequested;
        //  void MakeGenerateModeHidden();

        event EventHandler NewClicked;
        void ShowMessage(LetterDTO letter);
        void UpdateMessage();
        void AddRelatedLetterSelector(object control);
        //void DisableFileAttachment();
        //void EnableFileAttachmentToDefault();
        GenerateResult ShowGeneratedFile(byte[] generatedFile, string fileName, string fileExtension);
        void OpenFile(byte[] content, string fileName);
        void ViewExistingFile(string fileName);
        void ViewExternalFileInfo(string fileName);
        bool ViewExternalFilePanelVisibility { get; set; }
        void ClearSelectedFiles();
        GenerateResult ShowGeneratedFile();
    }
    public class GenerateResult
    {
        public bool Result { set; get; }
        public string Exception { set; get; }
    }
    public interface I_View_EntityLettersArea
    {
        event EventHandler<ContextMenuArg> ContextMenuLoaded;
        bool EnableAdd { get; set; }
        //bool AddButtonVisibility { get; set; }
       // bool EnableDelete { get; set; }
      //  bool EnableEdit { get; set; }
        bool DataTreeAreaEnabled { get; set; }
        bool DataTreeVisibility { get; set; }

        event EventHandler DataTreeRequested;

        //event EventHandler<EditLetterArg> DeleteLetterClicked;

        //event EventHandler<EditLetterArg> EditLetterClicked;
        event EventHandler NewLetterClicked;
        void ShowList(List<LetterDTO> letter);
        void EnableDisable(bool v);
        void ShowDataTree(I_DataTreeView view);
    }
    public class LetterConfirmedArg : EventArgs
    {
        //public LetterDTO Letter { set; get; }
    }
    public class LetterGenerateArg : EventArgs
    {
        public MainLetterTemplateDTO LetterTemplate { set; get; }
    }
    public class EditLetterArg : EventArgs
    {
        public int LetterID { set; get; }
    }
    public class LetterInternalExternalArg : EventArgs
    {
        public bool ExternalInternal { set; get; }
    }
    public class LetterFileSelectGenerateArg : EventArgs
    {
        public bool GenerateOrSelect { set; get; }
    }
    public class LetterExternalInfoRequestedArg : EventArgs
    {
        public string ExternalCode { set; get; }
    }
    //public class EntityLetterRelationshipTail
    //{
    //    public EntityRelationshipTailDTO RelationshipTail { set; get; }
    //    public AssignedPermissionDTO Permission { set; get; }

    //}
}
