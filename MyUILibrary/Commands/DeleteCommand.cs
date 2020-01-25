using CommonDefinitions.UISettings;
using MyUILibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class DeleteCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public DeleteCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    CommandManager.SetTitle("Delete");
            //else
            CommandManager.SetTitle("حذف از پایگاه");

            CommandManager.ImagePath = "Images//Remove.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }
        List<DP_DataRepository> dataList = new List<DP_DataRepository>();
        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            //Enabled = false;
            dataList = new List<DP_DataRepository>();
            if (EditArea.AreaInitializer.DataMode == DataMode.One)
            {
                var data = EditArea.AreaInitializer.Datas.FirstOrDefault();
                if (data != null)
                    dataList.Add(data);
            }
            else if (EditArea.AreaInitializer.DataMode == DataMode.Multiple)
            {
                dataList = (EditArea as I_EditEntityAreaMultipleData).GetSelectedData();
            }
            if (dataList.Count != 0 && !dataList.Any(x => x.IsNewItem == true))
            {
                var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                requester.SkipSecurity = true;
                DR_DeleteInquiryRequest request = new DR_DeleteInquiryRequest(requester);
                request.DataItems = dataList.Cast<DP_BaseData>().ToList(); ;
                var reuslt = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendDeleteInquiryRequest(request);
                I_ViewDeleteInquiry view = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDeleteInquiryView();
                view.ButtonClicked += View_ButtonClicked;
                view.SetTreeItems(reuslt.DataTreeItems);
                if (reuslt.Loop == true)
                {
                    view.SetMessage("بعلت وجود حلقه وابستگی بین داده ها امکان حذف داده (داده های) انتخاب شده وجود ندارد");
                    view.SetUserConfirmMode(UserDialogMode.Ok);
                }
                else
                {
                    view.SetUserConfirmMode(UserDialogMode.YesNo);
                    if (reuslt.DataTreeItems.Any(x => x.ChildRelationshipInfos.Any(y => y.RelationshipDeleteOption == ModelEntites.RelationshipDeleteOption.DeleteCascade && y.RelatedData.Any())))
                        view.SetMessage("داده های وابسته نمایش داده شده نیز حذف خواهند شد. آیا مطمئن هستید؟");
                    else
                        view.SetMessage("داده های نمایش داده شده حذف خواهد شد. آیا مطمئن هستید؟");

                }
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(view, "تایید", Enum_WindowSize.Big);
                //////if (AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowConfirm("تائید", "داده موجود به همراه تمامی اطلاعات وابسته حذف خواهند شد" +
                //////       Environment.NewLine + "آیا مطمئن هستید؟") == MyUILibrary.Temp.ConfirmResul.Yes)
                //////{

                //////    DR_DeleteRequest request = new DR_DeleteRequest();
                //////    request.EditPackages = dataList;

                //////    var reuslt = AgentUICoreMediator.GetAgentUICoreMediator.SendDeleteRequest(request);
                //////    if (reuslt.Result == Enum_DR_ResultType.SeccessfullyDone)
                //////    {
                //////        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(EditArea.SimpleEntity.Alias + " : " + "عملیات حذف با موفقیت انجام شد", "", MyUILibrary.Temp.InfoColor.Green);
                //////        if (EditArea.AreaInitializer.DataMode == DataMode.One)
                //////            EditArea.AreaInitializer.Datas.Remove(EditArea.AreaInitializer.Datas.First());
                //////        else if (EditArea.AreaInitializer.DataMode == DataMode.Multiple)
                //////        {
                //////            //foreach (var item in dataList)
                //////            //(EditArea as I_EditEntityAreaMultipleData).ClearData(item);
                //////            EditArea.AreaInitializer.Datas.Clear();
                //////        }


                //////    }
                //////}
            }
            //Enabled = true;
        }

        private void View_ButtonClicked(object sender, ConfirmModeClickedArg e)
        {
            I_ViewDeleteInquiry view = sender as I_ViewDeleteInquiry;
            if (view != null)
            {
                if (e.Result == UserDialogResult.Ok || e.Result == UserDialogResult.No)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(view);
                }
                else if (e.Result == UserDialogResult.Yes)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(view);
                    var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                    DR_DeleteRequest request = new DR_DeleteRequest(requester);
                    request.DataItems = dataList;

                    var reuslt = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendDeleteRequest(request);
                    if (reuslt.Result == Enum_DR_ResultType.SeccessfullyDone)
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(EditArea.SimpleEntity.Alias + " : " + "عملیات حذف با موفقیت انجام شد", reuslt.Details, MyUILibrary.Temp.InfoColor.Green);
                    else if (reuslt.Result == Enum_DR_ResultType.JustMajorFunctionDone)
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(EditArea.SimpleEntity.Alias + " : " + "عملیات حذف با موفقیت انجام شد اما برخی عملیات جانبی کامل انجام نشد", reuslt.Details, MyUILibrary.Temp.InfoColor.Blue);
                    else if (reuslt.Result == Enum_DR_ResultType.ExceptionThrown)
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(EditArea.SimpleEntity.Alias + " : " + "عملیات حذف با خطا همراه بود", reuslt.Details, MyUILibrary.Temp.InfoColor.Red);

                    if (reuslt.Result == Enum_DR_ResultType.SeccessfullyDone)
                    {
                        if (EditArea.AreaInitializer.DataMode == DataMode.One)
                            (EditArea as I_EditEntityAreaOneData).ClearData(true);
                        else if (EditArea.AreaInitializer.DataMode == DataMode.Multiple)
                        {
                            (EditArea as I_EditEntityAreaMultipleData).ClearData(false);
                        }
                    }
                    //if (reuslt.ResultItems.All(x => x.Result == Enum_DR_ResultType.SeccessfullyDone))
                    //{
                    //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("", "عملیات حذف داده/داده های منتخب با موفقیت انجام شد");
                    //}
                    //else if (reuslt.ResultItems.Any(x => x.Result == Enum_DR_ResultType.SeccessfullyDone)
                    //    && reuslt.ResultItems.Any(x => x.Result == Enum_DR_ResultType.ExceptionThrown))
                    //{
                    //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("", "عملیات حذف برخی از داده ها با موفقیت و برخی با خطا همراه بود");
                    //}
                    //else if (reuslt.ResultItems.All(x => x.Result == Enum_DR_ResultType.ExceptionThrown))
                    //{
                    //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("", "به علت وقوع خطا عملیات حذف داده/داده های منتخب انجام نشد");
                    //}
                    //foreach (var item in reuslt.ResultItems)
                    //{
                    //    DP_DataRepository data = AgentHelper.GetEquivalentDataItem(EditArea, item.DataItem);

                    //    if (item.Result == Enum_DR_ResultType.ExceptionThrown)
                    //    {
                    //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("خطا در حذف" + " " + data.ViewInfo, item.Message, MyUILibrary.Temp.InfoColor.Green);

                    //    }
                    //    else if (item.Result == Enum_DR_ResultType.SeccessfullyDone)
                    //    {
                    //        if (EditArea.AreaInitializer.DataMode == DataMode.One)
                    //            (EditArea as I_EditEntityAreaOneData).ClearData(true, true);
                    //        else if (EditArea.AreaInitializer.DataMode == DataMode.Multiple)
                    //        {
                    //            (EditArea as I_EditEntityAreaMultipleData).RemoveData(data, true);
                    //        }
                    //    }
                    //}
                }


            }
        }
    }
}

