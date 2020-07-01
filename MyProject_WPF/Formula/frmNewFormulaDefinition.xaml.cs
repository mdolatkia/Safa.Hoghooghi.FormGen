
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using Telerik.Windows.Documents.FormatProviders.Txt;
using System.Collections;
using ProxyLibrary;
using Telerik.Windows.Documents.Model.Styles;
using System.Timers;
using System.Windows.Threading;
using MyDataSearchManagerBusiness;
using MyUILibrary.EntityArea;
using MyUIGenerator;
using System.Windows.Documents;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmFormula.xaml
    /// </summary>
    /// 
    public partial class frmNewFormulaDefinition : UserControl
    {
        FormulaFunctionHandler formulaFunctionHandler = new FormulaFunctionHandler();
        //public event EventHandler<FormulaDefinedArg> FormulaDefined;
        FormulaAutoComplete formulaAutoComplete;
        //  FormulaDefinitionInstance FormulaInstance { set; get; }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        int EntityID { set; get; }
        string FormulaString { set; get; }
        List<EntityAndProperties> EntityAndProperties = new List<EntityAndProperties>();

        TableDrivedEntityDTO Entity { set; get; }
        private I_ExpressionEvaluator ExpressionEvaluator;

        DP_DataRepository DataItem { set; get; }
        DispatcherTimer textChangedTimer = new DispatcherTimer();
        DispatcherTimer selectionTimer = new DispatcherTimer();
        public frmNewFormulaDefinition(string formula, int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            Entity = bizTableDrivedEntity.GetPermissionedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID);
            formulaAutoComplete = new FormulaAutoComplete(MyProjectManager.GetMyProjectManager.GetRequester(), Entity);
            formulaAutoComplete.NodeSelected += FormulaAutoComplete_NodeSelected;
            FormulaString = formula;

            SetTimers();

            DataItem = new ProxyLibrary.DP_DataRepository(EntityID, "");
            DataItem.IsFullData = true;
            ExpressionEvaluator = formulaFunctionHandler.GetExpressionEvaluator(DataItem, MyProjectManager.GetMyProjectManager.GetRequester(), true);


            txtFormula.KeyUp += TxtFormula_KeyUp;
            SetParametersTree();
            SetEditEntityArea();
            //   txtFormula.KeyUp += ExpressionEditor_KeyUp;
            txtFormula.FontSize = 18;
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
        //private void ExpressionEditor_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (!_loaded)
        //    {
        //        CustomizeExpressionEditor();
        //        //myPopup.Child = formulaAutoComplete;
        //        //myPopup.IsOpen = false;
        //        _loaded = true;
        //    }
        //}

        //bool _loaded = false;
        //private void CustomizeExpressionEditor()
        //{
        //    //txtFormula = new RadRichTextBox();
        //    //if (txtFormula != null)
        //    //{

        //    //}
        //}
        private void TxtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            TextChanged();

            bool ctrlSpace = false;
            bool dot = false;
            if (e.Key == Key.Space && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ctrlSpace = true;
            if (e.Key == Key.OemPeriod)//&& (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
                dot = true;
            if (dot || ctrlSpace)
            {
                FormulaAutoCompleteKeyType keyType;

                string theCurrentWord = "";
                string theWordBeforeLastDot = "";
                if (ctrlSpace && GetFormulaText() == "")
                {
                    keyType = FormulaAutoCompleteKeyType.EmptyLeftCtrlSpace;
                }
                else
                {

                    //DocumentPosition pos = new DocumentPosition(txtFormula.Document);
                    //pos.MoveToPosition(txtFormula.Document.CaretPosition);
                    //pos.MoveToPrevious();
                    theCurrentWord = GetCurrentWord();

                    //    var str = (txtFormula.Document.CaretPosition.GetCurrentWord());
                    if (theCurrentWord == Environment.NewLine)
                        theWord = GetPreviousWord(txtFormula.Document, GetPreviousWord(txtFormula.Document, txtFormula.Document.CaretPosition).Item1).Item2;
                    else
                        theWord = GetPreviousWord(txtFormula.Document, txtFormula.Document.CaretPosition).Item2;

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

        private string GetCurrentWord()
        {
            TextPointer start = txtFormula.CaretPosition;  // this is the variable we will advance to the left until a non-letter character is found
            TextPointer end = txtFormula.CaretPosition;    // this is the variable we will advance to the right until a non-letter character is found

            String stringBeforeCaret = start.GetTextInRun(LogicalDirection.Backward);   // extract the text in the current run from the caret to the left
            String stringAfterCaret = start.GetTextInRun(LogicalDirection.Forward);     // extract the text in the current run from the caret to the left

            Int32 countToMoveLeft = 0;  // we record how many positions we move to the left until a non-letter character is found
            Int32 countToMoveRight = 0; // we record how many positions we move to the right until a non-letter character is found

            for (Int32 i = stringBeforeCaret.Length - 1; i >= 0; --i)
            {
                // if the character at the location CaretPosition-LeftOffset is a letter, we move more to the left
                if (Char.IsLetter(stringBeforeCaret[i]))
                    ++countToMoveLeft;
                else break; // otherwise we have found the beginning of the word
            }


            for (Int32 i = 0; i < stringAfterCaret.Length; ++i)
            {
                // if the character at the location CaretPosition+RightOffset is a letter, we move more to the right
                if (Char.IsLetter(stringAfterCaret[i]))
                    ++countToMoveRight;
                else break; // otherwise we have found the end of the word
            }



            start = start.GetPositionAtOffset(-countToMoveLeft);    // modify the start pointer by the offset we have calculated
            end = end.GetPositionAtOffset(countToMoveRight);        // modify the end pointer by the offset we have calculated


            // extract the text between those two pointers
            TextRange r = new TextRange(start, end);
            String text = r.Text;


            return text;
        }

        private string GetLastWord()
        {
            throw new NotImplementedException();
        }

        private void TextChanged()
        {
            textChangedTimer.Stop();
            textChangedTimer.Start();
        }




        private void SetTimers()
        {
            //کل متن را انتخاب میکند
            selectionTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            selectionTimer.Tick += SelectionTimer_Tick;

            textChangedTimer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
            textChangedTimer.Tick += TextChanged_Tick;
        }

        private void TextChanged_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            var output = GetFormulaText();
            // var text = new TextRange(new DocumentPosition()., txtFormula.Document.ContentEnd).Text;
            try
            {
                var res = ExpressionEvaluator.Calculate(output);
                if (res != null)
                    lblResult.Text = res.ToString();
                else
                    lblResult.Text = "Null";
            }
            catch (Exception ex)
            {
                lblResult.Text = "Exception:" + " " + ex.Message;
            }

        }

        private string GetFormulaText()
        {
            //   TxtFormatProvider provider = new TxtFormatProvider();
            //return provider.Export(txtFormula.str);

            TextRange textRange = new TextRange(
              // TextPointer to the start of content in the RichTextBox.
              txtFormula.Document.ContentStart,
          // TextPointer to the end of content in the RichTextBox.
          txtFormula.Document.ContentEnd
             );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }

        private void SelectionTimer_Tick(object sender, EventArgs e)
        {
            //کل متن را انتخاب میکند
            (sender as DispatcherTimer).Stop();
            //try
            //{
            //    var document = txtFormula.Document;
            //    DocumentTextSearch search = new DocumentTextSearch(document);
            //    List<Telerik.Windows.Documents.TextSearch.TextRange> rangesTrackingDocumentChanges = new List<Telerik.Windows.Documents.TextSearch.TextRange>();
            //    foreach (var textRange in search.FindAll(selectionText))
            //    {
            //        Telerik.Windows.Documents.TextSearch.TextRange newRange = new Telerik.Windows.Documents.TextSearch.TextRange(new DocumentPosition(textRange.StartPosition, true), new DocumentPosition(textRange.EndPosition, true));
            //        rangesTrackingDocumentChanges.Add(newRange);
            //    }

            //    foreach (var textRange in rangesTrackingDocumentChanges)
            //    {
            //        txtFormula.Document.Selection.SetSelectionStart(textRange.StartPosition);
            //        txtFormula.Document.Selection.AddSelectionEnd(textRange.EndPosition);
            //    }
            //}
            //catch
            //{

            //}
        }



        private void FormulaAutoComplete_NodeSelected(object sender, NodeSelectedArg e)
        {
            if (e.PropertyType == NodeType.Method)
            {
                txtFormula.Insert(e.NodePath);
                if (e.NodePath.Contains("(") && e.NodePath.Contains(")"))
                {
                    var fIndex = e.NodePath.IndexOf("(");
                    var lIndex = e.NodePath.IndexOf(")");
                    var lenght = lIndex - fIndex;
                    var paramStr = e.NodePath.Substring((fIndex + 1), lenght - 1);

                    if (!string.IsNullOrEmpty(paramStr))
                    {
                        if (!paramStr.Contains("[]"))
                            SelectText(paramStr);
                    }
                }

            }
            else
            {
                txtFormula.Insert(e.NodePath);
            }
            autoCompleteWindow.Close();
            txtFormula.Focus();

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
                if (txtFormula != null)
                {
                    var aa = txtFormula.Document.CaretPosition;
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

        //private Tuple<DocumentPosition, string> GetPreviousWord(RadDocument radDocument, DocumentPosition caretPosition)
        //{

        //    DocumentPosition pos = new DocumentPosition(txtFormula.Document);
        //    pos.MoveToPosition(caretPosition);
        //    pos.MoveToPreviousWordStart();
        //    radDocument.Selection.SetSelectionStart(pos);
        //    radDocument.Selection.AddSelectionEnd(caretPosition);



        //    // var text = pos.GetCurrentInlineBox().Text;
        //    string text = radDocument.Selection.GetSelectedText(); ;// pos.GetCurrentWord();
        //    if (text.Contains("_"))
        //    {
        //        pos.MoveToPreviousWordStart();
        //        radDocument.Selection.SetSelectionStart(pos);
        //        radDocument.Selection.AddSelectionEnd(caretPosition);
        //        text = radDocument.Selection.GetSelectedText();

        //    }
        //    if (text != null && text != "" && text != ".")
        //    {
        //        if (text.EndsWith("."))
        //            text = text.Substring(0, text.Length - 1);
        //    }
        //    var previousOfMainCaret = new DocumentPosition(pos);
        //    radDocument.Selection.Clear();
        //    return new Tuple<DocumentPosition, string>(previousOfMainCaret, text);
        //    //////var word = GetText(previousOfMainCaret, caretPosition);// pos.Get
        //    //////return new Tuple<DocumentPosition, string>(previousOfMainCaret, word);
        //}
        //private List<Tuple<DocumentPosition, string>> GetPreviousWords(DocumentPosition caretPosition, List<Tuple<DocumentPosition, string>> result = null)
        //{
        //    if (result == null)
        //        result = new List<Tuple<DocumentPosition, string>>();

        //    DocumentPosition pos = new DocumentPosition(txtFormula.Document);
        //    pos.MoveToPosition(caretPosition);
        //    pos.MoveToPreviousWordStart();
        //    var text = pos.GetCurrentInlineBox().Text;
        //    if (text.Contains("_"))
        //        pos.MoveToPreviousWordStart();
        //    var previousOfMainCaret = new DocumentPosition(pos);
        //    if (previousOfMainCaret != caretPosition)
        //    {
        //        result.Add(new Tuple<DocumentPosition, string>(previousOfMainCaret, text));

        //        GetPreviousWords(previousOfMainCaret, result);
        //    }

        //    return result;
        //    //////var word = GetText(previousOfMainCaret, caretPosition);// pos.Get
        //    //////return new Tuple<DocumentPosition, string>(previousOfMainCaret, word);
        //}
        private void SelectText(string text)
        {
            selectionText = text;
            selectionTimer.Start();
        }


        private void SetParametersTree()
        {
            var entityAndProperties = GetEntityAndProperties(Entity);
            SetTree(treeProperties.Items, entityAndProperties);
            foreach (var item in FormulaInstanceInternalHelper.GetHelpers())
            {
                AddPropertyNode(treeProperties.Items, item);
            }
        }

        private void AddPropertyNode(ItemCollection items, Type type)
        {
            var node = AddNode(items, new MyProject_WPF.NodeContext() { Context = type, Name = type.Name, NodeType = NodeType.HelperProperty });
            SetTree(node.Items, type);
        }

        private RadTreeViewItem AddNode(ItemCollection items, NodeContext nodeContext)
        {
            RadTreeViewItem node = new RadTreeViewItem();
            node.DataContext = nodeContext;
            node.Header = GetHeader(node);
            node.Expanded += Node_Expanded;
            // node.DoubleClick += Node_DoubleClick;

            if (nodeContext.Context is MyPropertyInfo)
            {
                var myPropertyInfo = (nodeContext.Context as MyPropertyInfo);
                if (myPropertyInfo.PropertyType == ProxyLibrary.PropertyType.Helper)
                {
                    node.Items.Add("aaa");
                }
                else if (myPropertyInfo.PropertyType == ProxyLibrary.PropertyType.Relationship)
                {
                    node.Items.Add("aaa");
                }
            }
            items.Add(node);
            return node;
        }
        private void Node_Expanded(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (sender is RadTreeViewItem)
            {
                var node = (sender as RadTreeViewItem);
                var nodeContext = (node.DataContext as NodeContext);
                if (node.Items.Count == 1 && node.Items[0].ToString() == "aaa")
                {
                    node.Items.Clear();
                    if (nodeContext.Context is MyPropertyInfo)
                    {
                        if ((nodeContext.Context as MyPropertyInfo).PropertyType == ProxyLibrary.PropertyType.Helper)
                        {
                            SetTree(node.Items, (nodeContext.Context as MyPropertyInfo).Type);
                        }
                        else if ((nodeContext.Context as MyPropertyInfo).PropertyType == ProxyLibrary.PropertyType.Relationship)
                        {
                            var entityAndProperties = GetEntityAndProperties((nodeContext.Context as MyPropertyInfo).PropertyRelationship.EntityID2);
                            if (entityAndProperties != null)
                            {
                                SetTree(node.Items, entityAndProperties);
                            }
                        }
                    }
                }
            }
        }
        private void SetTree(ItemCollection items, EntityAndProperties entityAndProperties)
        {
            items.Clear();
            foreach (var property in entityAndProperties.Properties)
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = property.Name;
                nodeContext.Context = property;
                nodeContext.NodeType = NodeType.Property;
                AddNode(items, nodeContext);
            }
        }
        private void SetTree(ItemCollection items, Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = prop.Name;
                nodeContext.NodeType = NodeType.Property;
                nodeContext.Context = prop;
                AddNode(items, nodeContext);
            }

            foreach (var method in type.GetMethods())
            {
                var methodParamStr = "";
                var paramList = method.GetParameters();
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
                    methodParamStr += "(" + paramsStr + ")";
                }
                else
                    methodParamStr += "()";


                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = methodName + methodParamStr;
                nodeContext.NodeType = NodeType.Method;
                nodeContext.Context = method;
                AddNode(items, nodeContext);
            }
        }
        EntityAndProperties GetEntityAndProperties(string entityName)
        {
            if (!EntityAndProperties.Any(x => x.Entity.Name.ToLower() == entityName.ToLower()))
            {
                var entity = bizTableDrivedEntity.GetPermissionedEntityByName(MyProjectManager.GetMyProjectManager.GetRequester(), Entity.DatabaseID, entityName);
                if (entity != null)
                {
                    return GetEntityAndProperties(entity);
                }
                else
                    return null;
            }
            else
            {
                return EntityAndProperties.First(x => x.Entity.Name == entityName);
            }
        }
        EntityAndProperties GetEntityAndProperties(int entityID)
        {
            if (!EntityAndProperties.Any(x => x.Entity.ID == entityID))
            {
                var entity = bizTableDrivedEntity.GetPermissionedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID);
                if (entity != null)
                {
                    return GetEntityAndProperties(entity);
                }
                else
                    return null;
            }
            else
            {
                return EntityAndProperties.First(x => x.Entity.ID == entityID);
            }
        }
        EntityAndProperties GetEntityAndProperties(TableDrivedEntityDTO entity)
        {
            EntityAndProperties entityAndProperties = null;
            if (!EntityAndProperties.Any(x => x.Entity.ID == entity.ID))
            {
                var properties = FormulaInstanceInternalHelper.GetProperties(entity, null, true).Select(x => x.Value).ToList();
                entityAndProperties = new EntityAndProperties() { Entity = entity, Properties = properties };
                EntityAndProperties.Add(entityAndProperties);
            }
            else
            {
                entityAndProperties = EntityAndProperties.First(x => x.Entity.ID == entity.ID);
            }
            return entityAndProperties;
        }
        private object GetHeader(RadTreeViewItem node)
        {
            var context = node.DataContext as NodeContext;
            StackPanel pnl = new StackPanel();
            pnl.Orientation = Orientation.Horizontal;
            TextBlock lbl = new TextBlock();
            lbl.Text = context.Name;
            Image img = new Image();
            img.Source = GetPropertyImage(context.NodeType);
            img.Width = 15;
            pnl.Children.Add(img);
            pnl.Children.Add(lbl);

            if (context.Context is MyPropertyInfo)
            {
                Image imgAdd = new Image();
                imgAdd.Source = new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/addnew.png", UriKind.Relative));
                imgAdd.Width = 15;
                imgAdd.Cursor = Cursors.Hand;
                imgAdd.MouseLeftButtonUp += (sender, e) => ImgAdd_MouseLeftButtonUp(sender, e, node);
                pnl.Children.Add(imgAdd);

                Image imgAddWithPath = new Image();
                imgAddWithPath.Source = new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/datalink.png", UriKind.Relative));
                imgAddWithPath.Width = 15;
                imgAddWithPath.Cursor = Cursors.Hand;
                imgAddWithPath.MouseLeftButtonUp += (sender, e) => ImgAddWithPath_MouseLeftButtonUp(sender, e, node);

                pnl.Children.Add(imgAddWithPath);
            }
            return pnl;
        }

        private void ImgAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, RadTreeViewItem node)
        {
            var context = node.DataContext as NodeContext;
            string exp = "";
            if (node.ParentItem == null)
                exp = "x." + context.Name;
            else
                exp = context.Name;
            txtFormula.Insert(exp);
            TextChanged();
        }
        private void ImgAddWithPath_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, RadTreeViewItem node)
        {
            var context = node.DataContext as NodeContext;
            var exp = "x." + GetNodePath(node, context.Name);
            txtFormula.Insert(exp);
            TextChanged();
        }
        private string GetNodePath(RadTreeViewItem node, string currentPath = "")
        {
            if (currentPath == "")
                currentPath = (node.DataContext as NodeContext).Name;
            if (node.ParentItem != null)
            {
                currentPath = (node.ParentItem.DataContext as NodeContext).Name + "." + currentPath;
                return GetNodePath(node.ParentItem, currentPath);
            }
            else
            {
                return currentPath;
            }
        }
        private ImageSource GetPropertyImage(NodeType propertyType)
        {
            if (propertyType == NodeType.Property)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/property.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.Method)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/method.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.RelationshipProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/type.png", UriKind.Relative));
            }

            return null;
        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //////if (FormulaDefined != null)
                //////{
                //////    var formulaItems = FormulaInstance.GetFormulaItems(ExpressionEditor.ExpressionText);

                //////    var arg = new FormulaDefinedArg();
                //////    arg.Expression = ExpressionEditor.ExpressionText;
                //////    arg.ExpressionResultType = (ExpressionEditor.Expression as LambdaExpression).ReturnType;
                //////    //if (!arg.ExpressionResultType.IsPrimitive && arg.ExpressionResultType != typeof(string))
                //////    //{
                //////    //    MessageBox.Show("فرمول باید یک مقدار را برگداند");
                //////    //    return;
                //////    //}
                //////    arg.FormulaItems = formulaItems;
                //////    FormulaDefined(this, arg);

                //////}
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





                //////var result = handler.CalculateFormula(ExpressionEditor.ExpressionText, data, MyProjectManager.GetMyProjectManager.GetRequester());
                //////if (string.IsNullOrEmpty(result.Exception))
                //////    MessageBox.Show(result.Result == null ? "null" : result.Result.ToString());
                //////else
                //////    MessageBox.Show(result.Exception);
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
