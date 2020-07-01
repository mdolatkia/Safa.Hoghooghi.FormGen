
using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using Telerik.Windows.Documents.FormatProviders.Txt;
using Telerik.Windows.Documents.TextSearch;
using Telerik.Windows.Documents;
using System.Collections;
using ProxyLibrary;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Model.Styles;
using System.Timers;
using System.Windows.Threading;
using MyDataSearchManagerBusiness;
using MyUILibrary.EntityArea;
using MyUIGenerator;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmFormula.xaml
    /// </summary>
    /// 
    public partial class frmFormulaDefinition : UserControl
    {
        public event EventHandler<FormulaDefinedArg> FormulaDefined;
        FormulaAutoComplete formulaAutoComplete;
        FormulaDefinitionInstance FormulaInstance { set; get; }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        int EntityID { set; get; }
        string FormulaString { set; get; }
        //TableDrivedEntityDTO Entity { set; get; }
        public frmFormulaDefinition(string formula, int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            var entity = bizTableDrivedEntity.GetPermissionedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID);
            formulaAutoComplete = new FormulaAutoComplete(MyProjectManager.GetMyProjectManager.GetRequester(), entity);
            formulaAutoComplete.NodeSelected += FormulaAutoComplete_NodeSelected;
            FormulaString = formula;
            ExpressionEditor.ExpressionText = FormulaString;
            //ExpressionEditor.
            SetTimers();
            PrepareFormulaInstance(entity);
            SetParametersTree();
            ExpressionEditor.Loaded += ExpressionEditor_Loaded;

            SetEditEntityArea();

        }



        private void SetEditEntityArea()
        {
            MyUILibrary.AgentUICoreMediator.GetAgentUICoreMediator.SetUIManager(new UIManager());
            var userInfo = new MyUILibrary.UserInfo();
            userInfo.AdminSecurityInfo = new MyUILibrary.AdminSecurityInfo() { IsActive = true, ByPassSecurity = true };
            MyUILibrary.AgentUICoreMediator.GetAgentUICoreMediator.UserInfo = userInfo;


            EditEntityAreaInitializer editEntityAreaInitializer1 = new EditEntityAreaInitializer();
            editEntityAreaInitializer1.EntityID = EntityID;
            editEntityAreaInitializer1.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;
            editEntityAreaInitializer1.DataMode = CommonDefinitions.UISettings.DataMode.One;
            var FirstSideEditEntityAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer1);
            if (FirstSideEditEntityAreaResult.Item1 != null && FirstSideEditEntityAreaResult.Item1 is I_EditEntityAreaOneData)
            {
                EditEntityArea = FirstSideEditEntityAreaResult.Item1 as I_EditEntityAreaOneData;
                EditEntityArea.SetAreaInitializer(editEntityAreaInitializer1);
                grdSelectData.Children.Add(EditEntityArea.TemporaryDisplayView as UIElement);
            }
        }

        DispatcherTimer selectionTimer = new DispatcherTimer();
        private void SetTimers()
        {
            //کل متن را انتخاب میکند
            selectionTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            selectionTimer.Tick += SelectionTimer_Tick;
        }

        private void SelectionTimer_Tick(object sender, EventArgs e)
        {
            //کل متن را انتخاب میکند
            (sender as DispatcherTimer).Stop();
            try
            {
                var document = richTextBox.Document;
                DocumentTextSearch search = new DocumentTextSearch(document);
                List<Telerik.Windows.Documents.TextSearch.TextRange> rangesTrackingDocumentChanges = new List<Telerik.Windows.Documents.TextSearch.TextRange>();
                foreach (var textRange in search.FindAll(selectionText))
                {
                    Telerik.Windows.Documents.TextSearch.TextRange newRange = new Telerik.Windows.Documents.TextSearch.TextRange(new DocumentPosition(textRange.StartPosition, true), new DocumentPosition(textRange.EndPosition, true));
                    rangesTrackingDocumentChanges.Add(newRange);
                }

                foreach (var textRange in rangesTrackingDocumentChanges)
                {
                    richTextBox.Document.Selection.SetSelectionStart(textRange.StartPosition);
                    richTextBox.Document.Selection.AddSelectionEnd(textRange.EndPosition);
                }
            }
            catch
            {

            }
        }

        private void ExpressionEditor_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_loaded)
            {
                CustomizeExpressionEditor();
                //myPopup.Child = formulaAutoComplete;
                //myPopup.IsOpen = false;
                _loaded = true;
            }
        }
        bool _loaded = false;
        private void CustomizeExpressionEditor()
        {

            richTextBox = ExpressionEditor.ChildrenOfType<RadRichTextBox>().FirstOrDefault();
            if (richTextBox != null)
            {
                richTextBox.KeyUp += ExpressionEditor_KeyUp;
                richTextBox.FontSize = 18;
            }
            var treeviews = ExpressionEditor.ChildrenOfType<RadTreeView>();

            //////foreach (var treeview in treeviews)
            //////{
            //////    (treeview.Parent as FrameworkElement).Visibility = Visibility.Collapsed;
            //////    ((treeview.Parent as FrameworkElement).Parent as Grid).RowDefinitions[2].Height = new GridLength(0);
            //////    ((treeview.Parent as FrameworkElement).Parent as Grid).RowDefinitions[4].Height = new GridLength(80);
            //////    var aa = (((treeview.Parent as FrameworkElement).Parent as Grid).Children[4] as System.Windows.Controls.Border).Child;
            //////    ((aa as StackPanel).Children[1] as TextBlock).Width = 800;
            //////    ((aa as StackPanel).Children[1] as TextBlock).FlowDirection = FlowDirection.RightToLeft;
            //////    ((aa as StackPanel).Children[1] as TextBlock).TextWrapping = TextWrapping.Wrap;
            //////    treeview.Visibility = Visibility.Collapsed;
            //////}

            //lstProperties.MaxHeight = 200;
            //lstProperties.MinWidth = 100;
            //lstProperties.MouseDoubleClick += LstProperties_MouseDoubleClick;
            //lstProperties.KeyUp += LstProperties_KeyUp;
        }

        RadRichTextBox richTextBox;
        private void FormulaAutoComplete_NodeSelected(object sender, NodeSelectedArg e)
        {
            if (e.PropertyType == NodeType.Method)
            {
                richTextBox.Insert(e.NodePath);
                if (e.NodePath.Contains("(") && e.NodePath.Contains(")"))
                {
                    var fIndex = e.NodePath.IndexOf("(");
                    var lIndex = e.NodePath.IndexOf(")");
                    var lenght = lIndex - fIndex;
                    var paramStr = e.NodePath.Substring((fIndex + 1), lenght-1);

                    if (!string.IsNullOrEmpty(paramStr))
                    {
                        if (!paramStr.Contains("[]"))
                            SelectText(paramStr);
                    }
                }

            }
            else
            {
                richTextBox.Insert(e.NodePath);
            }
            autoCompleteWindow.Close();
            richTextBox.Focus();

        }
        string selectionText = "";

        RadWindow autoCompleteWindow;


        private void ShowAutoComplete(FormulaAutoCompleteKeyType keyType, string theWord)
        {
            bool currentList = false;
            bool mainEntity = true;
            List<TableDrivedEntityDTO> entities = new List<TableDrivedEntityDTO>();
            if (keyType == FormulaAutoCompleteKeyType.DotOnly || keyType == FormulaAutoCompleteKeyType.AfterDotLeftCtrlSpace)
            {
                if (!string.IsNullOrEmpty(theWord))
                {
                    if (FormulaHepler.IsHelperType(theWord) != null)
                    {
                        var helperType = FormulaHepler.IsHelperType(theWord);
                        formulaAutoComplete.SetCurrentList(helperType);
                        currentList = true;
                    }
                    else if (IsKnownPhraseType(theWord))
                    {
                        if (IsEntity(theWord))
                        {
                            if (IsOneToOneEntity(theWord))
                            {
                                currentList = formulaAutoComplete.SetCurrentList(GetEntityNameFromWord(theWord));
                            }
                        }
                    }
                }
            }

            if (mainEntity || currentList)
            {
                formulaAutoComplete.SetTreeVisiblities(mainEntity, currentList);
                if (autoCompleteWindow == null)
                {
                    autoCompleteWindow = new RadWindow();
                    autoCompleteWindow.Header = "";
                    autoCompleteWindow.Content = formulaAutoComplete;
                    autoCompleteWindow.HideMinimizeButton = true;
                    autoCompleteWindow.HideMaximizeButton = true;
                 //   autoCompleteWindow.SizeToContent = true;
                    autoCompleteWindow.ResizeMode = ResizeMode.NoResize;
                }
                if (richTextBox != null)
                {
                    var aa = richTextBox.Document.CaretPosition;
                    autoCompleteWindow.Left = aa.Location.X + 50;
                    autoCompleteWindow.Top = aa.Location.Y + 200;
                }
                autoCompleteWindow.ShowDialog();
            }
            //myPopup.BringIntoView();
            //myPopup.UpdateLayout();
            //myPopup.Focus();
            //myPopup.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            //myPopup.Height = 400;
            //myPopup.PlacementTarget = ExpressionEditor;
            //myPopup.IsOpen = true;
            //myPopup.StaysOpen = true;

        }

        private string GetEntityNameFromWord(string word)
        {
            word = word.ToLower();
            if (word.StartsWith("otorel"))
            {
                word = word.Substring(6, word.Length - 6);
            }
            else if (word.StartsWith("otmrel"))
            {
                word = word.Substring(6, word.Length - 6);
            }
            else if (word.StartsWith("mtorel"))
            {
                word = word.Substring(6, word.Length - 6);
            }
            if (word.EndsWith("1") || word.EndsWith("2") || word.EndsWith("3") || word.EndsWith("4"))
            {
                word = word.Substring(0, word.Length - 1);
            }
            return word;
        }

        private void ExpressionEditor_KeyUp(object sender, KeyEventArgs e)
        {
            //bool showPropertyMethodList = false;
            //bool rightctrlSpace = false;
            bool ctrlSpace = false;
            bool dot = false;
            if (e.Key == Key.Space && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ctrlSpace = true;
            if (e.Key == Key.OemPeriod && (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
                dot = true;
            if (dot || ctrlSpace)
            {
                FormulaAutoCompleteKeyType keyType;

                string theWord = "";

                if (ctrlSpace && ExpressionEditor.ExpressionText == "")
                {
                    keyType = FormulaAutoCompleteKeyType.EmptyLeftCtrlSpace;
                }
                else
                {
                    DocumentPosition pos = new DocumentPosition(richTextBox.Document);
                    pos.MoveToPosition(richTextBox.Document.CaretPosition);
                    pos.MoveToPrevious();

                    var str = (richTextBox.Document.CaretPosition.GetCurrentWord());
                    if (str == Environment.NewLine)
                        theWord = GetPreviousWord(richTextBox.Document, GetPreviousWord(richTextBox.Document, richTextBox.Document.CaretPosition).Item1).Item2;
                    else
                        theWord = GetPreviousWord(richTextBox.Document, richTextBox.Document.CaretPosition).Item2;

                    if (dot)
                    {
                        keyType = FormulaAutoCompleteKeyType.DotOnly;
                    }
                    else
                    {
                        //leftctrlSpace
                        if (pos.GetCurrentWord() == ".")
                        {
                            keyType = FormulaAutoCompleteKeyType.AfterDotLeftCtrlSpace;
                        }
                        else
                            keyType = FormulaAutoCompleteKeyType.InTextLeftControlSpace;
                    }


                }


                ShowAutoComplete(keyType, theWord);

            }
            else if (e.Key != Key.LeftCtrl
                && e.Key != Key.LeftAlt)
            {
                //myPopup.IsOpen = false;
            }
        }

        //private void DoGeneralAutoComplete()
        //{
        //    var previousEntityWords = GetPreviousWords(richTextBox.Document.CaretPosition).Where(x => IsEntity(x.Item2)).Select(x => x.Item2).ToList();
        //    List<PropertyFunction> list1 = new List<PropertyFunction>();
        //    if (!previousEntityWords.Any())
        //        previousEntityWords.Add(Entity.Name);
        //    foreach (var entity in previousEntityWords)
        //    {
        //        var fItem = FormulaInstance.GetEntityAndProperties(entity);
        //        if (fItem != null)
        //        {
        //            list1.AddRange(GetFromulaObjectPropertiesAndFunctions(fItem.Properties, previousEntityWords.Count > 1 ? fItem.Entity.Name : ""));
        //        }
        //    }
        //    if (list1 != null && list1.Any())
        //    {
        //        ShowPopup(list1);
        //    }
        //}


        //private List<PropertyFunction> GetPropertyOrFunctionList(string theWord)
        //{
        //    //کش شود.دیتابیس هم برداشته شود
        //    List<PropertyFunction> result = null;
        //    if (!string.IsNullOrEmpty(theWord) && FormulaHepler.IsHelperType(theWord) != null)
        //    {
        //        var helperType = FormulaHepler.IsHelperType(theWord);
        //        result = GetDotNetTypePropertiesAndMethods(helperType);
        //    }
        //    else
        //    {
        //        List<TableDrivedEntityDTO> entities = new List<TableDrivedEntityDTO>();
        //        if (!string.IsNullOrEmpty(theWord))
        //        {
        //            if (IsKnownPhraseType(theWord))
        //            {
        //                if (IsEntity(theWord))
        //                {
        //                    if (IsOneToOneEntity(theWord))
        //                    {
        //                        entities = GetPossibleEntity(theWord);

        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            entities.Add(bizTableDrivedEntity.GetPermissionedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID));
        //        }

        //        if (entities.Any())
        //            return GetFromulaObjectPropertiesAndFunctions(GetEntityAndProperties(entities));
        //    }
        //}
        //public TableDrivedEntityDTO GetPossibleEntity(string word)
        //{


        //}


        //private List<PropertyFunction> GetPropertyOrFunctionList()
        //{
        //    List<PropertyFunction> result = null;

        //    List<string> entities = new List<string>();
        //    string property = null;
        //    var str = (richTextBox.Document.CaretPosition.GetCurrentWord());
        //    Tuple<DocumentPosition, string> theFirstWord = null;
        //    Tuple<DocumentPosition, string> theSecondWord = null;
        //    if (str == Environment.NewLine)
        //        theFirstWord = GetPreviousWord(richTextBox.Document, GetPreviousWord(richTextBox.Document, richTextBox.Document.CaretPosition).Item1);
        //    else
        //        theFirstWord = GetPreviousWord(richTextBox.Document, richTextBox.Document.CaretPosition);
        //    if (theFirstWord.Item2 == ".")
        //        theFirstWord = GetPreviousWord(richTextBox.Document, theFirstWord.Item1);
        //    if (theFirstWord != null)
        //    {
        //        var helperType = FormulaHepler.IsHelperType(theFirstWord.Item2);
        //        if (helperType != null)
        //        {
        //            result = GetDotNetTypePropertiesAndMethods(helperType);
        //        }
        //        else
        //        {
        //            var theDotBeforeTheFirstWord = GetPreviousWord(richTextBox.Document, theFirstWord.Item1);
        //            if (theDotBeforeTheFirstWord.Item2 == ".")
        //            {
        //                theSecondWord = GetPreviousWord(richTextBox.Document, theDotBeforeTheFirstWord.Item1);
        //            }

        //            if (IsKnownPropertyType(theFirstWord.Item2))
        //            {
        //                if (IsEntity(theFirstWord.Item2))
        //                {
        //                    entities.Add(theFirstWord.Item2);
        //                }
        //                else if (IsProperty(theFirstWord.Item2))
        //                {
        //                    property = theFirstWord.Item2;
        //                    entities = GetEntitiesBeforeProperty(theSecondWord, theFirstWord);
        //                }
        //            }

        //            if (entities.Any())
        //            {
        //                if (string.IsNullOrEmpty(property))
        //                {
        //                    //اگر پراپرتی نباشد همیشه یکی میاد
        //                    var theEntity = entities.First();
        //                    if (IsOneToOneEntity(theEntity))
        //                    {
        //                        var fItem = FormulaInstance.GetEntityAndProperties(theEntity);
        //                        if (fItem != null)
        //                            result = GetFromulaObjectPropertiesAndFunctions(fItem.Properties);
        //                    }
        //                    else
        //                        result = GetDotNetTypePropertiesAndMethods(typeof(Enumerable));
        //                }
        //                else
        //                {
        //                    if (entities.Count == 1)
        //                    {
        //                        var fItem = FormulaInstance.GetEntityProperty(entities.First(), property);
        //                        if (fItem != null)
        //                            result = GetDotNetTypePropertiesAndMethods(fItem.Type);
        //                    }
        //                    else
        //                    {
        //                        List<PropertyFunction> list1 = new List<PropertyFunction>();
        //                        foreach (var entity in entities)
        //                        {
        //                            var fItem = FormulaInstance.GetEntityProperty(entity, property);
        //                            if (fItem != null)
        //                                list1.AddRange(GetDotNetTypePropertiesAndMethods(fItem.Type));
        //                        }
        //                        result = list1;
        //                    }
        //                }
        //            }

        //        }
        //    }


        //    return result;
        //}
        //private void ShowAutoCompleteMainObject(DocumentPosition caretPosition)
        //{
        //    var CaretPosition = caretPosition;
        //    var list = GetFromulaObjectPropertiesAndFunctions(FormulaInstance.MainFormulaObject.Properties.Select(x => x.Value).ToList());
        //    ShowPopup(list);
        //}

        //private List<string> GetEntitiesBeforeProperty(Tuple<DocumentPosition, string> theWordBefore, Tuple<DocumentPosition, string> thePropertyWord)
        //{
        //    List<string> contextEntity = new List<string>();
        //    if (theWordBefore != null && IsEntity(theWordBefore.Item2))
        //    {
        //        contextEntity.Add(theWordBefore.Item2);
        //    }
        //    else
        //    {
        //        var previousWords = GetPreviousWords(thePropertyWord.Item1);
        //        if (!previousWords.Any(x => IsEntity(x.Item2)))
        //        {
        //            contextEntity.Add(Entity.Name);
        //        }
        //        else
        //        {
        //            var entities = previousWords.Where(x => IsEntity(x.Item2));
        //            foreach (var entity in entities)
        //            {
        //                contextEntity.Add(entity.Item2);
        //            }
        //        }
        //    }
        //    return contextEntity;
        //}

        //private List<PropertyFunction> GetDotNetTypePropertiesAndMethods(Type type)
        //{
        //    List<PropertyFunction> result = new List<PropertyFunction>();
        //    var propertyArray = type.GetProperties();
        //    foreach (var prop in propertyArray)
        //    {
        //        var propertyFunction = new PropertyFunction();
        //        propertyFunction.Title = prop.Name;
        //        propertyFunction.Name = prop.Name;
        //        propertyFunction.Type = Enum_PropertyFunctionType.Property;
        //        propertyFunction.Image = GetPropertyImage();
        //        result.Add(propertyFunction);
        //    }
        //    var methodList = GetMehtodList(type);
        //    result.AddRange(methodList);
        //    return result;


        //}

        private bool IsOneToOneEntity(string item2)
        {
            return item2.ToLower().StartsWith("otorel") || item2.ToLower().StartsWith("mtorel");
        }


        private List<PropertyFunction> GetMehtodList(Type type)
        {
            List<PropertyFunction> result = new List<PropertyFunction>();
            IEnumerable<MethodInfo> methodList = null;
            //if (specificMethod)
            //    methodList = type.GetMethods().Where(x => SpecificMethodNames.Contains(x.Name));
            //else
            methodList = type.GetMethods();
            foreach (var method in methodList)
            {
                var attr = method.Attributes;
                var paramList = method.GetParameters();
                var methodTitle = method.Name;
                var methodName = method.Name;
                var paramsStr = "";
                if (paramList.Count() > 0)
                {

                    foreach (var param in paramList)
                    {
                        if (type == typeof(Enumerable) && param.Name.ToLower() == "source")
                            continue;
                        paramsStr += (paramsStr == "" ? "" : ",") + param.ParameterType.Name + " " + param.Name;
                    }
                    methodTitle += "(" + paramsStr + ")";
                }
                else
                    methodTitle += "()";
                if (!result.Any(x => x.Title == methodTitle))
                {
                    var propertyFunction = new PropertyFunction();
                    propertyFunction.Title = methodTitle;
                    propertyFunction.ParamsStr = paramsStr;
                    propertyFunction.Name = methodName;
                    propertyFunction.Type = Enum_PropertyFunctionType.Method;
                    propertyFunction.Image = GetMethodImage(methodName);

                    result.Add(propertyFunction);
                }
            }

            return result;
        }
        private ImageSource GetMethodImage(string methodName)
        {
            //if (SpecificMethodNames.Contains(methodName))
            //    return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/function.png", UriKind.Relative));
            //else
            return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/function1.png", UriKind.Relative));
        }
        private bool IsKnownPhraseType(string word)
        {
            if (word.ToLower().StartsWith("otorel")
                || word.ToLower().StartsWith("otmrel")
                 || word.ToLower().StartsWith("mtorel")
                     //|| word.StartsWith("cl_")
                     //  || word.StartsWith("st_")
                     //  || word.StartsWith("p_")
                     //  || word.StartsWith("fn_")
                     //  || word.StartsWith("sp_")
                     //    || word.StartsWith("cd_")
                     )
                return true;
            else
                return false;
        }
        private bool IsEntity(string word)
        {
            if (word.ToLower().StartsWith("otorel")
                || word.ToLower().StartsWith("otmrel")
                  || word.ToLower().StartsWith("mtorel"))
                return true;
            else
                return false;
        }
        public bool IsProperty(string word)
        {
            if (word.StartsWith("cl_")
                  || word.StartsWith("st_")
                  || word.StartsWith("p_")
                  || word.StartsWith("fn_")
                  || word.StartsWith("sp_")
                    || word.StartsWith("cd_")
                    || word == "Helper")
                return true;
            else
                return false;
        }

        private Tuple<DocumentPosition, string> GetPreviousWord(RadDocument radDocument, DocumentPosition caretPosition)
        {

            DocumentPosition pos = new DocumentPosition(richTextBox.Document);
            pos.MoveToPosition(caretPosition);
            pos.MoveToPreviousWordStart();
            radDocument.Selection.SetSelectionStart(pos);
            radDocument.Selection.AddSelectionEnd(caretPosition);



            // var text = pos.GetCurrentInlineBox().Text;
            string text = radDocument.Selection.GetSelectedText(); ;// pos.GetCurrentWord();
            if (text.Contains("_"))
            {
                pos.MoveToPreviousWordStart();
                radDocument.Selection.SetSelectionStart(pos);
                radDocument.Selection.AddSelectionEnd(caretPosition);
                text = radDocument.Selection.GetSelectedText();

            }
            if (text != null && text != "" && text != ".")
            {
                if (text.EndsWith("."))
                    text = text.Substring(0, text.Length - 1);
            }
            var previousOfMainCaret = new DocumentPosition(pos);
            radDocument.Selection.Clear();
            return new Tuple<DocumentPosition, string>(previousOfMainCaret, text);
            //////var word = GetText(previousOfMainCaret, caretPosition);// pos.Get
            //////return new Tuple<DocumentPosition, string>(previousOfMainCaret, word);
        }
        private List<Tuple<DocumentPosition, string>> GetPreviousWords(DocumentPosition caretPosition, List<Tuple<DocumentPosition, string>> result = null)
        {
            if (result == null)
                result = new List<Tuple<DocumentPosition, string>>();

            DocumentPosition pos = new DocumentPosition(richTextBox.Document);
            pos.MoveToPosition(caretPosition);
            pos.MoveToPreviousWordStart();
            var text = pos.GetCurrentInlineBox().Text;
            if (text.Contains("_"))
                pos.MoveToPreviousWordStart();
            var previousOfMainCaret = new DocumentPosition(pos);
            if (previousOfMainCaret != caretPosition)
            {
                result.Add(new Tuple<DocumentPosition, string>(previousOfMainCaret, text));

                GetPreviousWords(previousOfMainCaret, result);
            }

            return result;
            //////var word = GetText(previousOfMainCaret, caretPosition);// pos.Get
            //////return new Tuple<DocumentPosition, string>(previousOfMainCaret, word);
        }
        private void SelectText(string text)
        {
            selectionText = text;
            selectionTimer.Start();
        }
        private void PrepareFormulaInstance(TableDrivedEntityDTO mainEntity)
        {
            var dataItem = new ProxyLibrary.DP_DataRepository(EntityID, "");
            dataItem.IsFullData = true;

            FormulaInstance = new FormulaDefinitionInstance(MyProjectManager.GetMyProjectManager.GetRequester(), dataItem, mainEntity);
            ExpressionEditor.Item = FormulaInstance.MainFormulaObject;
        }

        private void SetParametersTree()
        {
            //////frmFormulaTree tree = new frmFormulaTree(EntityID);
            ////////tree.ItemSelected += Tree_ItemSelected;
            //////grdTreeParam.Children.Add(tree);
            ////////ExpressionEditor.Item = ParametersForFormula.BindableTypeDescriptor;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FormulaDefined != null)
                {
                    //var formulaItems = FormulaInstance.GetFormulaItems(ExpressionEditor.ExpressionText);

                    var arg = new FormulaDefinedArg();
                    arg.Expression = ExpressionEditor.ExpressionText;
                    arg.ExpressionResultType = (ExpressionEditor.Expression as LambdaExpression).ReturnType;
                    //if (!arg.ExpressionResultType.IsPrimitive && arg.ExpressionResultType != typeof(string))
                    //{
                    //    MessageBox.Show("فرمول باید یک مقدار را برگداند");
                    //    return;
                    //}
                    //arg.FormulaItems = formulaItems;
                    FormulaDefined(this, arg);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        I_EditEntityArea EditEntityArea { set; get; }
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if (EditEntityArea.GetDataList().Any())
            {
                TestData(EditEntityArea.GetDataList().First());
            }

            //if (lastSelectedData == null)
            //{
            //    btnSelectData_Click(null, null);
            //}
            //else
            //    TestData(lastSelectedData);

        }
        DP_DataRepository lastSelectedData;
        private void TestData(DP_DataRepository data)
        {
            try
            {
                FormulaFunctionHandler handler = new MyFormulaFunctionStateFunctionLibrary.FormulaFunctionHandler();

                ////var child = new ChildRelationshipInfo();
                ////child.Relationship = new BizRelationship().GetRelationship(33);

                ////SearchRequestManager searchProcessor = new SearchRequestManager();
                ////DP_SearchRepository searchItem = new DP_SearchRepository(75);
                ////searchItem.Phrases.Add(new SearchProperty() { ColumnID = 68, Value = "بانک تجارت" });

                ////DP_DataRepository childdata = null;
                ////var requester = GetRequester();
                //////سکوریتی داده اعمال میشود
                ////DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(requester, searchItem);
                ////var searchResult = searchProcessor.Process(request);
                ////if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
                ////    childdata = searchResult.ResultDataItems.First();
                ////else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
                ////    throw (new Exception(searchResult.Message));

                //////var childdata = searchProcessor.GetDataItemsByListOFSearchProperties(GetRequester(), searchItem).First();

                ////child.ParentData = data;

                //childdata.ParantChildRelationshipInfo = child;
                ////childdata.SetProperties(newProp);
                //child.RelatedData.Add(childdata);
                //data.ChildRelationshipInfos.Add(child);
                var result = handler.CalculateFormula(ExpressionEditor.ExpressionText, data, MyProjectManager.GetMyProjectManager.GetRequester());
                if (string.IsNullOrEmpty(result.Exception))
                    MessageBox.Show(result.Result == null ? "null" : result.Result.ToString());
                else
                    MessageBox.Show(result.Exception);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //برنگشتن آبجکت
            //                آیتمهای فرمول
        }
        //frmDataSelect frmDataSelect;
        //private void btnSelectData_Click(object sender, RoutedEventArgs e)
        //{
        //    if (frmDataSelect == null)
        //    {
        //        frmDataSelect = new MyProject_WPF.frmDataSelect(EntityID);
        //        frmDataSelect.DataSelected += View_DataSelected;
        //    }
        //    MyProjectManager.GetMyProjectManager.ShowDialog(frmDataSelect, "انتخاب داده", Enum_WindowSize.Big);
        //}
        //private void View_DataSelected(object sender, DataSelectedArg e)
        //{
        //    SearchRequestManager searchProcessor = new SearchRequestManager();
        //    DP_SearchRepository searchDataItem = new DP_SearchRepository(Entity.ID);
        //    foreach (var property in e.Columns)
        //        searchDataItem.Phrases.Add(new SearchProperty() { ColumnID = property.ColumnID, Value = property.Value });
        //    //سکوریتی داده اعمال میشود
        //    DP_DataRepository foundDataItem = null;
        //    var requester = GetRequester();
        //    DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(requester, searchDataItem);

        //    var searchResult = searchProcessor.Process(request);
        //    if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
        //        foundDataItem = searchResult.ResultDataItems.FirstOrDefault();
        //    else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
        //        throw (new Exception(searchResult.Message));


        //    //var foundDataItem = searchProcessor.GetDataItemsByListOFSearchProperties(GetRequester(), searchDataItem).FirstOrDefault();
        //    if (foundDataItem != null)
        //    {
        //        lastSelectedData = foundDataItem;
        //        TestData(lastSelectedData);
        //    }
        //    else
        //        MessageBox.Show("چنین داده ای یافت نشد");
        //}

    }




    public class FormulaDefinedArg : EventArgs
    {
        public string Expression { set; get; }
        public List<FormulaItemDTO> FormulaItems { set; get; }

        public Type ExpressionResultType { set; get; }
    }

    public enum AutoCompleteMode
    {
        Dot,
        NotDot,
        Urgent
    }
    public class MyAutoComplete
    {
        public TableDrivedEntityDTO MainEntity { set; get; }
        public TableDrivedEntityDTO CurrentEntity { set; get; }
    }
    public class PropertyFunction
    {
        public string Title { set; get; }
        public string ParamsStr { set; get; }
        public string Tooltip { set; get; }
        public string Name { set; get; }
        public ImageSource Image { set; get; }
        public Enum_PropertyFunctionType Type { set; get; }
    }

    public class AutoCompleteItem
    {
        public string Title { set; get; }
        public string ParamsStr { set; get; }
        public string Tooltip { set; get; }
        public string Name { set; get; }
        public ImageSource Image { set; get; }
        public Enum_PropertyFunctionType Type { set; get; }
    }
    public enum FormulaAutoCompleteKeyType
    {
        None,
        EmptyLeftCtrlSpace,
        AfterDotLeftCtrlSpace,
        InTextLeftControlSpace,
        DotOnly
    }
}
