
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
        DispatcherTimer textChangedCalculationTimer = new DispatcherTimer();
        DispatcherTimer textChangedColorTimer = new DispatcherTimer();

        DispatcherTimer autoCompleteTimer = new DispatcherTimer();
        DispatcherTimer selectionTimer = new DispatcherTimer();
        List<NodeContext> nodeDictionary = new List<NodeContext>();
        public frmNewFormulaDefinition(string formula, int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            Entity = bizTableDrivedEntity.GetPermissionedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID);
            formulaAutoComplete = new FormulaAutoComplete();
            formulaAutoComplete.NodeSelected += FormulaAutoComplete_NodeSelected;
            FormulaString = formula;

            SetTimers();

            DataItem = new ProxyLibrary.DP_DataRepository(EntityID, "");
            DataItem.IsFullData = true;
            ExpressionEvaluator = formulaFunctionHandler.GetExpressionEvaluator(DataItem, MyProjectManager.GetMyProjectManager.GetRequester(), true);

            txtFormula.TextChanged += TxtFormula_TextChanged;
            txtFormula.KeyUp += TxtFormula_KeyUp;
            SetParametersTree();
            SetEditEntityArea();
            //   txtFormula.KeyUp += ExpressionEditor_KeyUp;
            txtFormula.FontSize = 18;
        }

        private void TxtFormula_TextChanged(object sender, TextChangedEventArgs e)
        {
            //textChangedColorTimer.Stop();
            //textChangedColorTimer.Start();
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
                autoCompleteTimer.Stop();
                autoCompleteTimer.Start();
            }


            //        ShowAutoComplete(keyType, theWord);


            //else if (e.Key != Key.LeftCtrl
            //    && e.Key != Key.LeftAlt)
            //{
            //    //myPopup.IsOpen = false;
            //}
        }
        private void AutoCompleteTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            FormulaAutoCompleteKeyType keyType;
            List<NodeContext> nodes = null;
            WordDetection wordDetection = GetWordDetection(txtFormula.CaretPosition);
            MessageBox.Show(wordDetection.CurrentWord.Item3);
            //////if (wordDetection.PreviousWords == null || wordDetection.PreviousWords.Count == 0)
            //////{
            //////    nodes = nodeDictionary.ToList();
            //////}
            //////else
            //////{
            //////    wordDetection.PreviousWords.Reverse();
            //////    //       nodes = GetAutocompleteNodes(wordDetection.PreviousWords, nodeDictionary);
            //////}

            if (nodes != null)
                ShowAutoComplete(nodes);


        }
        private List<NodeContext> GetAutocompleteNodes(List<string> listNames, List<NodeContext> parentNodes)
        {
            if (listNames.Any() && parentNodes != null)
            {
                var name = listNames.First();
                List<string> rest = new List<string>();
                for (int i = 1; i < listNames.Count; i++)
                {
                    rest.Add(listNames[i]);
                }
                //اینجا یه ایرادی داره و اینکه اگر مثلا چند فانکشن با اسامی یکسان اما مقدار برگشتی متفاوت بود چی. اونوقت مسیرها منحرف میشد در حالیکه ما فقط اولی را در نظر میگیریم

                if (parentNodes.Any(x => x.Name == name))
                {
                    var node = parentNodes.First(x => x.Name == name);
                    if (node.ChildNodes == null)
                        SetNodes(node);
                    if (rest.Any())
                    {
                        return GetAutocompleteNodes(rest, node.ChildNodes);
                    }
                    else
                        return node.ChildNodes;
                }
            }
            return null;
        }

        private WordDetection GetWordDetection(TextPointer caretPosition)
        {
            WordDetection result = new WordDetection();
            result.CurrentWord = GetCurrentWord(caretPosition);
            //var prevtext = GetTextRange(currentWord.Item1, -1);
            //if (prevtext != null && prevtext.Item2 == ".")
            var list = new List<Tuple<TextPointer, TextPointer, string>>();
            //       GetBlocks(result.CurrentWord.Item1, list);
            //  List<StatementTree> stTree = GetStatementTree(list, 0);

            return result;
        }
        private List<TextStateChain> GetTextStateChains(TextPointer point)
        {
            List<TextStateChain> result = new List<MyProject_WPF.TextStateChain>();
            var list = GetBlocks(point);
            var statementTree = GetStatementTree(list);
            foreach (var item in statementTree)
            {
                if (item.IsText)
                {
                    TextStateChain rItem = new TextStateChain();
                    //rItem.StartPointer = list[i].Item1;
                    rItem.TextStates = CheckCallChain(statementTree, item);
                }
            }
            return result;
        }

        private List<FormulaTextBlock> CheckCallChain(List<CodeBlock> statementTree, CodeBlock item, CodeBlock lastItem = null, List<FormulaTextBlock> result = null, bool canAcceptOnlyDot = false)
        {
            if (result == null)
                result = new List<FormulaTextBlock>();
            if (canAcceptOnlyDot)
            {
                if ((item is NonTextBlock) && (item as NonTextBlock).IsDot && statementTree.IndexOf(item) != statementTree.Count - 1)
                {
                    CheckCallChain(statementTree, statementTree[statementTree.IndexOf(item) + 1], item, result, false);
                }
            }
            else
            {
                if (item is FormulaTextBlock)
                {
                    result.Add(item as FormulaTextBlock);
                }

                if (statementTree.IndexOf(item) != statementTree.Count - 1)
                {
                    bool canContinue = false;
                    bool nextCanAcceptOnlyDot = false;
                    if (item is FormulaTextBlock)
                    {
                        canContinue = true;
                    }
                    else if ((item is NonTextBlock) && (item as NonTextBlock).IsStartingParantese && (item as NonTextBlock).PairBlock != null)
                    {
                        if (lastItem is FormulaTextBlock)
                        {
                            (lastItem as FormulaTextBlock).IsFunction = true;
                        }
                        canContinue = true;
                        item = (item as NonTextBlock).PairBlock;
                        nextCanAcceptOnlyDot = true;
                    }
                    if (canContinue)
                        CheckCallChain(statementTree, statementTree[statementTree.IndexOf(item) + 1], item, result, nextCanAcceptOnlyDot);
                }
            }



            return result;
        }

        private List<CodeBlock> GetStatementTree(List<Tuple<TextPointer, TextPointer, string>> list, int j = 0)
        {
            List<CodeBlock> result = new List<CodeBlock>();
            for (int i = j; i < list.Count - 1; i++)
            {
                //if (!list[i].Item4)
                //{
                CodeBlock rItem = null;
                ////////if (list[i].Item4)
                ////////{
                ////////    rItem = new FormulaTextBlock();
                ////////}
                ////////else
                ////////{
                ////////    rItem = new NonTextBlock();
                ////////}
                rItem.StartPointer = list[i].Item1;
                rItem.EndPointer = list[i].Item2;
                rItem.Text = list[i].Item3;
                //////         rItem.IsText = list[i].Item4;
                result.Add(rItem);
                //}
                //else
                //{
                //TextStateChain rItem = new TextStateChain();
                //rItem.StartPointer = list[i].Item1;
                //rItem.TextStates = CheckCallChain(list, ref i);
                //string text = "";
                //foreach (var item in rItem.TextStates)
                //{
                //    text += (text == "" ? "" : ".") + item.Text;
                //}
                //rItem.Text = text;
                //rItem.EndPointer = rItem.TextStates.Last().EndPointer;
                //}
            }

            SetNonTextBlockProperties(result.Where(x => x is NonTextBlock).Cast<NonTextBlock>().ToList());
            return result;
        }

        private void SetNonTextBlockProperties(List<NonTextBlock> result)
        {
            foreach (var item in result)
            {
                var block = item as NonTextBlock;
                if (block.Text == "(")
                {
                    block.IsStartingParantese = true;
                }
                else if (block.Text == ")")
                {
                    block.IsEndingParantese = true;
                }
                else if (item.Text == ".")
                {
                    block.IsDot = true;
                }
            }
            foreach (var item in result.Where(x => x.IsStartingParantese))
            {
                SetPairParantese(result, item);
            }
        }

        private void SetPairParantese(List<NonTextBlock> result, NonTextBlock item, int debth = 0)
        {
            for (int i = result.IndexOf(item) + 1; i <= result.Count - 1; i++)
            {

                if (debth == 0 && result[i].IsEndingParantese)
                {
                    item.PairBlock = result[i];
                    result[i].PairBlock = item;
                }
                else if (result[i].IsStartingParantese)
                    debth--;
                else if (result[i].IsEndingParantese)
                {
                    debth++;
                    if (debth > 0)
                        break;
                }
            }
        }



        private List<Tuple<TextPointer, TextPointer, string>> GetBlocks(TextPointer item1, List<Tuple<TextPointer, TextPointer, string>> result = null)
        {
            if (result == null)
                result = new List<Tuple<TextPointer, TextPointer, string>>();
            var previousWord = GetPreviousText(item1, true);
            if (previousWord != null)
            {
                result.Add(new Tuple<TextPointer, TextPointer, string>(previousWord.Item1, previousWord.Item2, previousWord.Item3));
                GetBlocks(previousWord.Item1, result);
            }
            return result;
        }

        //private void SetPreviousWords(WordDetection result, int count, TextPointer item1, int i = 0, int parantesDebth = 0)
        //{
        //    if (i >= count)
        //        return;
        //    if (result.PreviousWords == null)
        //        result.PreviousWords = new List<string>();
        //    var previousWord = GetPreviousText(item1, new List<char>() { '_', '-' }, false);
        //    if (previousWord == null)
        //        return;
        //    else if (previousWord.Item2 == "")
        //    {
        //        previousWord = GetPreviousText(item1, new List<char>() { '_', '-' }, true);
        //        if (previousWord == null || previousWord.Item2 == "")
        //            return;
        //        else
        //        {
        //            if (previousWord.Item2 == ")")
        //                parantesDebth++;
        //            else if (previousWord.Item2 == "(")
        //                parantesDebth--;
        //            SetPreviousWords(result, count, previousWord.Item1, i, parantesDebth);
        //        }


        //    }
        //    else
        //    {
        //        if (parantesDebth == 0)
        //        {
        //            result.PreviousWords.Add(previousWord.Item2);
        //            SetPreviousWords(result, count, previousWord.Item1, i + 1, parantesDebth);
        //        }
        //        else
        //            SetPreviousWords(result, count, previousWord.Item1, i, parantesDebth);

        //    }

        //}
        private Tuple<TextPointer, TextPointer, string> GetCurrentWord(TextPointer item1)
        {
            Tuple<TextPointer, TextPointer, string> prevText;
            Tuple<TextPointer, TextPointer, string> nextText;
            prevText = GetPreviousText(item1, false);
            nextText = GetNextText(item1, false);
            TextPointer start;
            TextPointer end;

            string prevContent = "";
            string nextContent = "";
            if (prevText == null)
                start = item1;
            else
            {
                prevContent = prevText.Item3;
                start = prevText.Item1;
            }
            if (nextText == null)
                end = item1;
            else
            {
                nextContent = nextText.Item3;
                end = nextText.Item2;
            }
            return new Tuple<TextPointer, TextPointer, string>(start, end, prevContent + nextContent);
        }
        private Tuple<TextPointer, TextPointer, string> GetPreviousText(TextPointer start, bool includeSingleNonText)
        {
            var len = GetPreviousTextLen(start, includeSingleNonText);
            if (len == 0)
                return null;
            else
                return GetTextRange(start, len);
        }
        private int GetPreviousTextLen(TextPointer cStart, bool includeSingleNonText, int result = 0)
        {

            if (txtFormula.Document.ContentStart.GetOffsetToPosition(cStart) <= 0)
                return result;

            var nextChar = GetTextRange(cStart, -1);
            if (nextChar.Item3 != "" && IsValidChar(nextChar.Item3[0]))
            {
                result--;
                return GetPreviousTextLen(nextChar.Item1, includeSingleNonText, result);
            }
            else
            {
                if (includeSingleNonText && result == 0)
                    result--;
            }
            return result;
        }

        private Tuple<TextPointer, TextPointer, string> GetNextText(TextPointer start, bool includeSingleNonText)
        {
            var len = GetNextTextLen(start, includeSingleNonText);
            if (len == 0)
                return null;
            else
                return GetTextRange(start, len);
        }
        private int GetNextTextLen(TextPointer cStart, bool includeSingleNonText, int result = 0)
        {
            if (txtFormula.Document.ContentEnd.GetOffsetToPosition(cStart) >= 0)
                return result;
            var nextChar = GetTextRange(cStart, 1);
            if (nextChar.Item3 != "" && IsValidChar(nextChar.Item3[0]))
            {
                result++;
                return GetNextTextLen(nextChar.Item2, includeSingleNonText, result);
            }
            else
            {
                if (includeSingleNonText && result == 0)
                    result++;

            }
            return result;
        }
        private Tuple<TextPointer, TextPointer, string> GetTextRange(TextPointer position, int len)
        {
            TextPointer start;
            TextPointer end;
            if (len >= 0)
            {
                start = position;
                end = start.GetPositionAtOffset(len);
            }
            else
            {
                end = position;
                start = end.GetPositionAtOffset(len);
            }
            TextRange r = new TextRange(start, end);
            String text = r.Text;
            return new Tuple<TextPointer, TextPointer, string>(start, end, text);
        }
        private bool IsValidChar(char ch)
        {
            return Char.IsLetterOrDigit(ch) || new List<char>() { '_' }.Contains(ch);
        }
        private bool IsValidChar(char ch, List<char> validChars)
        {
            return Char.IsLetterOrDigit(ch) || (validChars != null && validChars.Contains(ch));
        }

        private string GetLastWord()
        {
            throw new NotImplementedException();
        }

        private void TextChanged()
        {
            textChangedCalculationTimer.Stop();
            textChangedCalculationTimer.Start();
        }




        private void SetTimers()
        {
            //کل متن را انتخاب میکند
            selectionTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            selectionTimer.Tick += SelectionTimer_Tick;

            textChangedCalculationTimer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
            textChangedCalculationTimer.Tick += TextChanged_Tick;

            textChangedColorTimer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
            textChangedColorTimer.Tick += TextChangedColorTimer_Tick;

            autoCompleteTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            autoCompleteTimer.Tick += AutoCompleteTimer_Tick;
        }

        private void TextChangedColorTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            var list = new List<Tuple<TextPointer, TextPointer, string, bool>>();
            var stTree = GetTextStateChains(txtFormula.Document.ContentEnd);

            foreach (var item in stTree)
            {
                foreach (var block in item.TextStates)
                {
                    var textRange = new TextRange(block.StartPointer, block.EndPointer);
                    if (block.IsFunction)
                        textRange.ApplyPropertyValue(TextElement.ForegroundProperty­, Brushes.Red);
                    else
                        textRange.ApplyPropertyValue(TextElement.ForegroundProperty­, Brushes.Blue);
                }
            }
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
            txtFormula.CaretPosition.InsertTextInRun(e.NodeTitle);
            //if (e.PropertyType == NodeType.Method)
            //{
            //    txtFormula.CaretPosition.InsertTextInRun(e.NodeTitle);
            //    if (e.NodePath.Contains("(") && e.NodePath.Contains(")"))
            //    {
            //        var fIndex = e.NodePath.IndexOf("(");
            //        var lIndex = e.NodePath.IndexOf(")");
            //        var lenght = lIndex - fIndex;
            //        var paramStr = e.NodePath.Substring((fIndex + 1), lenght - 1);

            //        if (!string.IsNullOrEmpty(paramStr))
            //        {
            //            if (!paramStr.Contains("[]"))
            //                SelectText(paramStr);
            //        }
            //    }

            //}
            //else
            //{
            //    txtFormula.Insert(e.NodePath);
            //}
            autoCompleteWindow.Close();
            txtFormula.Focus();
        }
        string selectionText = "";

        RadWindow autoCompleteWindow;


        private void ShowAutoComplete(List<NodeContext> nodes)
        {
            //bool currentList = false;
            //bool mainEntity = true;
            //List<TableDrivedEntityDTO> entities = new List<TableDrivedEntityDTO>();
            //if (keyType == FormulaAutoCompleteKeyType.DotOnly || keyType == FormulaAutoCompleteKeyType.AfterDotLeftCtrlSpace)
            //{
            //    if (!string.IsNullOrEmpty(theWord))
            //    {
            //        if (FormulaHepler.IsHelperType(theWord) != null)
            //        {
            //            var helperType = FormulaHepler.IsHelperType(theWord);
            //            formulaAutoComplete.SetCurrentList(helperType);
            //            currentList = true;
            //        }
            //        else if (IsKnownPhraseType(theWord))
            //        {
            //            if (IsEntity(theWord))
            //            {
            //                if (IsOneToOneEntity(theWord))
            //                {
            //                    currentList = formulaAutoComplete.SetCurrentList(GetEntityNameFromWord(theWord));
            //                }
            //            }
            //        }
            //    }
            //}

            //if (mainEntity || currentList)
            //{
            formulaAutoComplete.SetTree(nodes);
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
            //////if (txtFormula != null)
            //////{
            //////    var aa = txtFormula.Document.CaretPosition;
            //////    autoCompleteWindow.Left = aa.Location.X + 50;
            //////    autoCompleteWindow.Top = aa.Location.Y + 200;
            //////}
            autoCompleteWindow.ShowDialog();
            //}
            //myPopup.BringIntoView();
            //myPopup.UpdateLayout();
            //myPopup.Focus();
            //myPopup.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            //myPopup.Height = 400;
            //myPopup.PlacementTarget = ExpressionEditor;
            //myPopup.IsOpen = true;
            //myPopup.StaysOpen = true;

        }

        private void SetParametersTree()
        {
            var fNodeTitle = FormulaInstanceInternalHelper.GetObjectPrefrix();
            NodeContext rootNodeContext = null;
            if (!string.IsNullOrEmpty(fNodeTitle))
            {
                rootNodeContext = new MyProject_WPF.NodeContext()
                {
                    Context = Entity,
                    Name = fNodeTitle,
                    Title = fNodeTitle,
                    NodeType = NodeType.MainVariable
                };
                var treeItem = AddNodeContext(null, rootNodeContext, false);
                treeItem.IsExpanded = true;
            }
            var entityAndProperties = GetEntityAndProperties(Entity);
            SetNodes(rootNodeContext, entityAndProperties);
            foreach (var item in FormulaInstanceInternalHelper.GetHelpers())
            {
                var nodeContext = new MyProject_WPF.NodeContext() { ParentPath = "", Context = item.Value, Name = item.Key, Title = item.Key, NodeType = NodeType.HelperProperty };
                var node = AddNodeContext(null, nodeContext, false);
                SetNodes(nodeContext, item.Value);
            }

            foreach (var item in FormulaInstanceInternalHelper.GetExpressionBuiltinVariables())
            {
                var nodeContext = new MyProject_WPF.NodeContext() { ParentPath = "", Context = item.Value, Name = item.Key, Title = item.Key, NodeType = NodeType.HelperProperty };
                AddNodeContext(null, nodeContext, false);
                SetNodes(nodeContext, item.Value);
            }

        }
        private RadTreeViewItem AddNodeContext(NodeContext parentNodeContext, NodeContext nodeContext, bool lateExpand)
        {


            string parentPath = "";
            if (parentNodeContext != null)
                parentPath = (string.IsNullOrEmpty(parentNodeContext.ParentPath) ? "" : parentNodeContext.ParentPath + ".") + parentNodeContext.Name;
            nodeContext.ParentPath = parentPath;
            nodeContext.ParentNode = parentNodeContext;

            if (parentNodeContext != null)
            {
                if (parentNodeContext.ChildNodes == null)
                    parentNodeContext.ChildNodes = new List<MyProject_WPF.NodeContext>();
                parentNodeContext.ChildNodes.Add(nodeContext);
            }
            else
                nodeDictionary.Add(nodeContext);



            var fnode = AddNodeToTree(nodeContext, lateExpand);
            nodeContext.UIItem = fnode;
            return fnode;
        }

        private RadTreeViewItem AddNodeToTree(NodeContext nodeContext, bool lateExpand)
        {
            ItemCollection collection;
            if (nodeContext.ParentNode != null)
                collection = (nodeContext.ParentNode.UIItem as RadTreeViewItem).Items;
            else
                collection = treeProperties.Items;

            RadTreeViewItem node = new RadTreeViewItem();
            node.DataContext = nodeContext;
            node.Header = GetHeader(node);

            // node.DoubleClick += Node_DoubleClick;

            if (lateExpand)
            {
                node.Expanded += Node_Expanded;
                node.Items.Add("aaa");
            }
            collection.Add(node);
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
                    SetNodes(nodeContext);
                }
            }
        }

        private void SetNodes(NodeContext parentNodeContext, EntityAndProperties entityAndProperties)
        {

            //if (nodeDictionary.Any(x => x.ParentPath == parentPath))
            //    return nodeDictionary.Where(x => x.ParentPath == parentPath).ToList();
            var result = new List<MyProject_WPF.NodeContext>();
            foreach (var property in entityAndProperties.Properties)
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = property.Name;
                nodeContext.Title = nodeContext.Name;
                nodeContext.Context = property;
                if (property.PropertyType == PropertyType.Relationship)
                    nodeContext.NodeType = NodeType.RelationshipProperty;
                else
                    nodeContext.NodeType = NodeType.CustomProperty;
                result.Add(nodeContext);
            }
            foreach (var item in result)
            {
                AddNodeContext(parentNodeContext, item, true);
            }

        }
        private void SetNodes(NodeContext nodeContext)
        {
            if (nodeContext.NodeType == NodeType.CustomProperty || nodeContext.NodeType == NodeType.RelationshipProperty)
            {
                if ((nodeContext.Context as MyPropertyInfo).PropertyType == ProxyLibrary.PropertyType.Relationship)
                {
                    var entityAndProperties = GetEntityAndProperties((nodeContext.Context as MyPropertyInfo).PropertyRelationship.EntityID2);
                    if (entityAndProperties != null)
                    {
                        SetNodes(nodeContext, entityAndProperties);
                    }
                }
                else
                {
                    SetNodes(nodeContext, (nodeContext.Context as MyPropertyInfo).Type);
                }
            }
            else if (nodeContext.NodeType == NodeType.DotNetProperty)
            {
                SetNodes(nodeContext, (nodeContext.Context as PropertyInfo).PropertyType);
            }
            else if (nodeContext.NodeType == NodeType.DotNetMethod)
            {
                SetNodes(nodeContext, (nodeContext.Context as MethodInfo).ReturnType);
            }
        }
        private void SetNodes(NodeContext parentNodeContext, Type type)
        {
            //if (nodeDictionary.Any(x => x.Key == type.Name))
            //    return nodeDictionary[type.Name];

            List<NodeContext> result = new List<MyProject_WPF.NodeContext>();
            foreach (var prop in type.GetProperties())
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = prop.Name;
                nodeContext.Title = nodeContext.Name;
                nodeContext.NodeType = NodeType.DotNetProperty;
                nodeContext.Context = prop;
                result.Add(nodeContext);
            }

            foreach (var method in type.GetMethods().Where(x => x.IsPublic))
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
                nodeContext.Name = methodName;
                nodeContext.Title = methodName + methodParamStr;
                nodeContext.NodeType = NodeType.DotNetMethod;
                nodeContext.Context = method;
                result.Add(nodeContext);
            }
            //nodeDictionary.Add(type.Name, result);
            //nodeDictionary.AddRange(result);

            foreach (var item in result)
            {
                AddNodeContext(parentNodeContext, item, true);
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
            System.Windows.Controls.TextBlock lbl = new System.Windows.Controls.TextBlock();
            lbl.Text = context.Title;
            Image img = new Image();
            img.Source = GetPropertyImage(context.NodeType);
            img.Width = 15;
            pnl.Children.Add(img);
            pnl.Children.Add(lbl);

            //if (context.Context is MyPropertyInfo)
            //{
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
            //}
            return pnl;
        }

        private void ImgAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, RadTreeViewItem node)
        {
            var context = node.DataContext as NodeContext;
            string exp = context.Name;
            txtFormula.CaretPosition.InsertTextInRun(exp);
            TextChanged();
        }
        private void ImgAddWithPath_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, RadTreeViewItem node)
        {
            var context = node.DataContext as NodeContext;
            var exp = GetNodePath(node, context.Name);
            txtFormula.CaretPosition.InsertTextInRun(exp);
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
            if (propertyType == NodeType.DotNetProperty || propertyType == NodeType.CustomProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/property.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.DotNetMethod)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/method.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.RelationshipProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/type.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.HelperProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/validate.png", UriKind.Relative));
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
            var list = GetBlocks(txtFormula.CaretPosition);
            lstItems.Items.Clear();
            foreach (var item in list)
            {
                lstItems.Items.Add(item.Item3);
            }
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
    public class WordDetection
    {
        public Tuple<TextPointer, TextPointer, string> CurrentWord { set; get; }
        public List<Tuple<TextPointer, TextPointer, string>> PreviousWords { set; get; }

    }

    public class CodeBlock
    {
        public TextPointer StartPointer { set; get; }
        public TextPointer EndPointer { set; get; }
        public string Text { set; get; }

        public bool IsText { get; internal set; }

    }
    public class NonTextBlock : CodeBlock
    {
        public bool IsStartingParantese { set; get; }
        public CodeBlock PairBlock { set; get; }
        public bool IsEndingParantese { get; internal set; }
        public bool IsDot { get; internal set; }

    }
    public class FormulaTextBlock : CodeBlock
    {
        public List<NodeContext> NodeContexts { set; get; }
        public bool IsFunction { get; internal set; }
    }
    public class TextStateChain
    {
        public List<FormulaTextBlock> TextStates { set; get; }
    }



}
