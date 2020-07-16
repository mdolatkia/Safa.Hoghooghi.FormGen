
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
        DispatcherTimer textChangedTimer = new DispatcherTimer();

        DispatcherTimer selectionTimer = new DispatcherTimer();
        List<NodeContext> nodeDictionary = new List<NodeContext>();
        List<FormulaTextBlock> AllTextStates = new List<FormulaTextBlock>();
        public frmNewFormulaDefinition(string formula, int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            Entity = bizTableDrivedEntity.GetPermissionedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID);
            formulaAutoComplete = new FormulaAutoComplete();
            formulaAutoComplete.NodeSelected += FormulaAutoComplete_NodeSelected;
            FormulaString = formula;
            SetTimers();
            txtFormula.SelectionChanged += TxtFormula_SelectionChanged;
            DataItem = new ProxyLibrary.DP_DataRepository(EntityID, "");
            DataItem.IsFullData = true;
            ExpressionEvaluator = formulaFunctionHandler.GetExpressionEvaluator(DataItem, MyProjectManager.GetMyProjectManager.GetRequester(), true);

            txtFormula.TextChanged += TxtFormula_TextChanged;
            txtFormula.KeyUp += TxtFormula_KeyUp;
            SetParametersTree();
            SetEditEntityArea();
            txtFormula.FontSize = 18;
        }

        private void TxtFormula_SelectionChanged(object sender, RoutedEventArgs e)
        {
            lblPointer.Text = txtFormula.CaretPosition.GetOffsetToPosition(txtFormula.Document.ContentStart).ToString();
        }

        private void txtClear_Click(object sender, RoutedEventArgs e)
        {
            txtFormula.SelectAll();
            txtFormula.Selection.Text = "";
        }

        bool skipTextChanged = false;
        private void TxtFormula_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!skipTextChanged)
            {
                textChangedCalculationTimer.Stop();
                textChangedCalculationTimer.Start();

                textChangedTimer.Stop();
                textChangedTimer.Start();
            }

        }
        private void textChangedCalculationTimer_Tick(object sender, EventArgs e)
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
        private void TextChangedTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            var list = GetBlocks(txtFormula.Document.ContentEnd);
            list = list.Where(x => x.Item3 != "").ToList();
            list.Reverse();

            var chains = GetTextStateChains(txtFormula.Document.ContentEnd);
            AllTextStates = chains.Item2;

            ClearColor();
            if (AllTextStates != null)
                SetColor(AllTextStates, true);
        }

        private void ClearColor()
        {
            skipTextChanged = true;
            var alltextRange = new TextRange(txtFormula.Document.ContentStart, txtFormula.Document.ContentEnd);
            alltextRange.ApplyPropertyValue(TextElement.ForegroundProperty­, Brushes.Black);
            alltextRange.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            skipTextChanged = false;
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
        private void TxtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            bool ctrlSpace = false;
            bool dot = false;
            if (e.Key == Key.Space && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ctrlSpace = true;
            if (e.Key == Key.OemPeriod)//&& (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
                dot = true;
            if (dot || ctrlSpace)
            {
                if (dot)
                {
                    CheckAutoComplete(txtFormula.CaretPosition);
                }
                else
                {
                    var prev = GetPreviousText(txtFormula.CaretPosition, true);
                    if (prev!=null && prev.Item3 == ".")
                    {
                        CheckAutoComplete(prev.Item2);
                    }
                    else
                        ShowAutoComplete(nodeDictionary);
                }
            }
        }
        private void CheckAutoComplete(TextPointer pointer)
        {
            if (AllTextStates != null)
            {
                var state = AllTextStates.FirstOrDefault(x => IsSameOffset(x.ActualEndPointer, pointer));
                if (state != null)
                {
                    if (state.NextPossibleContexts != null && state.NextPossibleContexts.Any())
                    {
                        ShowAutoComplete(state.NextPossibleContexts);
                    }
                }
            }
        }

        private Tuple<List<TextStateChain>, List<FormulaTextBlock>> GetTextStateChains(TextPointer point)
        {
            var list = GetBlocks(point);
            list = list.Where(x => x.Item3 != "").ToList();
            list.Reverse();



            var statementTree = GetStatementTree(list);
            var chains = SetChainLevels(statementTree, txtFormula.Document.ContentStart, txtFormula.Document.ContentEnd);
            SetChainContext(chains.Item1, nodeDictionary);
            return chains;
        }

        private void SetChainContext(List<TextStateChain> chains, List<NodeContext> nodeDictionary)
        {
            foreach (var chain in chains)
            {
                SetTextBlocksContext(chain.TextStates, nodeDictionary);
            }
        }

        private void SetTextBlocksContext(List<FormulaTextBlock> textBlocks, List<NodeContext> parentNodes, int i = 0)
        {
            var sItem = textBlocks[i];
            sItem.NodeContexts = parentNodes;
            if (parentNodes != null && parentNodes.Any())
            {
                List<NodeContext> foundContext = null;

                if (sItem.IsFunction)
                {
                    foundContext = parentNodes.Where(x => x.NodeType == NodeType.DotNetMethod && x.Name == sItem.Text).ToList();
                }
                else
                {
                    foundContext = parentNodes.Where(x => x.NodeType != NodeType.DotNetMethod && x.Name == sItem.Text).ToList();
                }
                sItem.FoundContexts = foundContext;
                List<NodeContext> nextContext = new List<MyProject_WPF.NodeContext>();
                foreach (var node in foundContext)
                {
                    if (node.ChildNodes == null)
                        SetNodes(node);
                    if (node.ChildNodes != null)
                        nextContext.AddRange(node.ChildNodes);
                }
                //اینجا چون فعلا تشخیص نداریم که مثلا فانکشن چند پارامتری هست نمیتونیم خروجی دقیق رو تشخیص بدیم بنابراین همه رو میاریم
                sItem.NextPossibleContexts = nextContext;
                if (textBlocks.Count - 1 > i)
                    SetTextBlocksContext(textBlocks, nextContext, i + 1);
            }
            if (sItem.TextStateChains != null && sItem.TextStateChains.Any())
            {
                SetChainContext(sItem.TextStateChains, GetFunctionNodeContext(sItem));
            }
        }

        private List<NodeContext> GetFunctionNodeContext(FormulaTextBlock sItem)
        {
            //اینجااونجاست که لامبدا اکپرشن ها میتونن اضافه بشن y=> y.
            return nodeDictionary;
        }

        private Tuple<List<TextStateChain>, List<FormulaTextBlock>> SetChainLevels(List<CodeBlock> statementTree, TextPointer start, TextPointer end, int level = 0, List<FormulaTextBlock> textBlocks = null)
        {
            List<TextStateChain> chains = new List<MyProject_WPF.TextStateChain>();
            if (textBlocks == null)
                textBlocks = new List<FormulaTextBlock>();
            //var result = new List<MyProject_WPF.TextStateChain>();

            foreach (var item in statementTree.Where(x => x.IsText))
            {
                if (RangeContainsBlock(start, end, item))
                {
                    if (!chains.Any(x => RangeContainsBlock(x.StartPointer, x.EndPointer, item)))
                    {
                        TextStateChain rItem = new TextStateChain();
                        rItem.TextStates = CheckCallChain(statementTree, item);
                        textBlocks.AddRange(rItem.TextStates);
                        if (rItem.TextStates.Any())
                        {
                            rItem.StartPointer = rItem.TextStates[0].StartPointer;
                            rItem.EndPointer = rItem.TextStates[rItem.TextStates.Count - 1].ActualEndPointer;
                        }
                        chains.Add(rItem);
                    }
                }
            }
            foreach (var item in chains)
            {
                foreach (var fItem in item.TextStates.Where(x => x.IsFunction))
                {
                    var fResult = SetChainLevels(statementTree, fItem.FunctionParantestPointers.Item1, fItem.FunctionParantestPointers.Item2, level + 1);
                    fItem.TextStateChains = fResult.Item1;
                }
            }
            return new Tuple<List<MyProject_WPF.TextStateChain>, List<MyProject_WPF.FormulaTextBlock>>(chains, textBlocks);
        }

        private bool RangeContainsBlock(TextPointer start, TextPointer end, CodeBlock item)
        {
            return txtFormula.Document.ContentStart.GetOffsetToPosition(start) <= txtFormula.Document.ContentStart.GetOffsetToPosition(item.StartPointer)
                  && txtFormula.Document.ContentStart.GetOffsetToPosition(end) >= txtFormula.Document.ContentStart.GetOffsetToPosition(item.EndPointer);
        }

        private List<FormulaTextBlock> CheckCallChain(List<CodeBlock> statementTree, CodeBlock item, CodeBlock lastItem = null, List<FormulaTextBlock> result = null)
        {
            if (result == null)
                result = new List<FormulaTextBlock>();

            if (item is FormulaTextBlock)
            {
                result.Add(item as FormulaTextBlock);
            }

            if (statementTree.IndexOf(item) != statementTree.Count - 1)
            {
                var nextItem = statementTree[statementTree.IndexOf(item) + 1];
                bool canContinue = false;
                //bool nextCanAcceptOnlyDot = false;
                if (item is FormulaTextBlock)
                {
                    canContinue = true;
                }
                else if ((item is NonTextBlock) && (item as NonTextBlock).IsDot)
                {
                    if (nextItem is FormulaTextBlock)
                        CheckCallChain(statementTree, nextItem, item, result);
                }
                else if ((item is NonTextBlock) && (item as NonTextBlock).IsStartingParantese && (item as NonTextBlock).PairBlock != null)
                {
                    if (lastItem is FormulaTextBlock)
                    {
                        (lastItem as FormulaTextBlock).IsFunction = true;
                        (lastItem as FormulaTextBlock).FunctionParantestPointers = new Tuple<TextPointer, TextPointer>(item.StartPointer, (item as NonTextBlock).PairBlock.EndPointer);
                    }
                    item = (item as NonTextBlock).PairBlock;
                    if (statementTree.IndexOf(item) != statementTree.Count - 1)
                    {
                        nextItem = statementTree[statementTree.IndexOf(item) + 1];
                        if (nextItem is NonTextBlock)
                            if ((nextItem as NonTextBlock).IsDot)
                                canContinue = true;
                    }
                }
                if (canContinue)
                    CheckCallChain(statementTree, statementTree[statementTree.IndexOf(item) + 1], item, result);
                //}
            }



            return result;
        }

        private List<CodeBlock> GetStatementTree(List<Tuple<TextPointer, TextPointer, string, int, int>> list, int j = 0)
        {
            List<CodeBlock> result = new List<CodeBlock>();
            for (int i = j; i < list.Count - 1; i++)
            {
                //if (!list[i].Item4)
                //{
                CodeBlock rItem = null;
                bool isText = IsText(list[i].Item3);
                if (isText)
                {
                    rItem = new FormulaTextBlock();
                }
                else
                {
                    rItem = new NonTextBlock();
                }
                rItem.IsText = isText;

                rItem.StartPointer = list[i].Item1;
                rItem.Offset = txtFormula.Document.ContentStart.GetOffsetToPosition(rItem.StartPointer);
                rItem.EndPointer = list[i].Item2;
                rItem.Text = list[i].Item3;

                result.Add(rItem);

            }

            SetNonTextBlockProperties(result.Where(x => x is NonTextBlock).Cast<NonTextBlock>().ToList());
            return result;
        }

        private bool IsText(string item3)
        {
            return item3.Any() && IsValidChar(item3[0]);
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



        private List<Tuple<TextPointer, TextPointer, string, int, int>> GetBlocks(TextPointer item1, List<Tuple<TextPointer, TextPointer, string, int, int>> result = null)
        {
            if (result == null)
                result = new List<Tuple<TextPointer, TextPointer, string, int, int>>();
            var previousWord = GetPreviousText(item1, true);
            if (previousWord != null)
            {
                result.Add(new Tuple<TextPointer, TextPointer, string, int, int>(previousWord.Item1, previousWord.Item2, previousWord.Item3, txtFormula.Document.ContentStart.GetOffsetToPosition(previousWord.Item1), txtFormula.Document.ContentStart.GetOffsetToPosition(previousWord.Item2)));
                GetBlocks(previousWord.Item1, result);
            }
            return result;
        }

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
                return GetTextRangeTuple(start, len);
        }
        private int GetPreviousTextLen(TextPointer cStart, bool includeSingleNonText, int result = 0)
        {

            if (txtFormula.Document.ContentStart.GetOffsetToPosition(cStart) <= 0)
                return result;

            var nextChar = GetTextRangeTuple(cStart, -1);
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
                return GetTextRangeTuple(start, len);
        }
        private int GetNextTextLen(TextPointer cStart, bool includeSingleNonText, int result = 0)
        {
            if (txtFormula.Document.ContentEnd.GetOffsetToPosition(cStart) >= 0)
                return result;
            var nextChar = GetTextRangeTuple(cStart, 1);
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
        private TextRange GetTextRange(TextPointer start, int len)
        {
            var end = start.GetPositionAtOffset(len);
            return new TextRange(start, end);
        }

        private Tuple<TextPointer, TextPointer, string> GetTextRangeTuple(TextPointer position, int len)
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

                start = position.GetPositionAtOffset(len);
                end = position;
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

        private void SetTimers()
        {
            //کل متن را انتخاب میکند
            selectionTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            selectionTimer.Tick += SelectionTimer_Tick;

            textChangedCalculationTimer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
            textChangedCalculationTimer.Tick += textChangedCalculationTimer_Tick;

            textChangedTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            textChangedTimer.Tick += TextChangedTimer_Tick;


        }

        private bool IsSameOffset(TextPointer actualEndPointer, TextPointer autoCompletePointer)
        {
            var res = actualEndPointer.GetOffsetToPosition(txtFormula.Document.ContentStart) - autoCompletePointer.GetOffsetToPosition(txtFormula.Document.ContentStart);
            return res <= 1 && res >= -1;

        }

        private void SetColor(List<FormulaTextBlock> textStates, bool first)
        {
            skipTextChanged = true;
            List<Tuple<TextRange, int, SolidColorBrush, bool>> colors = new List<Tuple<TextRange, int, SolidColorBrush, bool>>();
            foreach (var textstate in textStates)
            {
                var textRange = new TextRange(textstate.StartPointer, textstate.EndPointer);
                if (textstate.FoundContexts != null && textstate.FoundContexts.Any())
                {
                    if (textstate.IsFunction)
                    {
                        var parEnd = GetTextRange(textstate.FunctionParantestPointers.Item2, -1);
                        colors.Add(new Tuple<TextRange, int, SolidColorBrush, bool>(parEnd, txtFormula.Document.ContentStart.GetOffsetToPosition(parEnd.Start), Brushes.Red, false));
                        var parStart = GetTextRange(textstate.FunctionParantestPointers.Item1, 1);
                        colors.Add(new Tuple<TextRange, int, SolidColorBrush, bool>(parStart, txtFormula.Document.ContentStart.GetOffsetToPosition(parStart.Start), Brushes.Red, false));
                        colors.Add(new Tuple<TextRange, int, SolidColorBrush, bool>(textRange, textstate.Offset, Brushes.Red, false));
                    }
                    else
                        colors.Add(new Tuple<TextRange, int, SolidColorBrush, bool>(textRange, textstate.Offset, Brushes.Blue, false));
                }
                else
                {
                    colors.Add(new Tuple<TextRange, int, SolidColorBrush, bool>(textRange, textstate.Offset, null, true));
                }
            }
            //اینجا لیست رو میسازیم و از ته شروع میکنیم. چون اگه رنج های قبل رو مثلا رنگی کنیم
            // رنج های بعدی جابجا میشن. زیرا خود رنگ یک رنج حساب میشه
            foreach (var color in colors.OrderByDescending(x => x.Item2))
            {
                if (color.Item4)
                {
                    color.Item1.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
                else
                {
                    color.Item1.ApplyPropertyValue(TextElement.ForegroundProperty­, color.Item3);
                }
            }
            skipTextChanged = false;
        }


        private string GetFormulaText()
        {
            TextRange textRange = new TextRange(
                 txtFormula.Document.ContentStart,
                txtFormula.Document.ContentEnd
                   );
            return textRange.Text;
        }

        private void FormulaAutoComplete_NodeSelected(object sender, NodeSelectedArg e)
        {
            NodeSelected(e.Title);
        }
        string selectedNodeText;
        TextPointer selectedNodePosition;

        private void NodeSelected(string text)
        {
            selectedNodePosition = txtFormula.CaretPosition;
          

            selectedNodeText = text;
            txtFormula.CaretPosition.InsertTextInRun(text);
            //return;
            selectionTimer.Start();


            if (autoCompleteWindow != null && autoCompleteWindow.IsOpen)
                autoCompleteWindow.Close();
            txtFormula.Focus();
        }

        private void SelectionTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            var newPos = selectedNodePosition.GetPositionAtOffset(selectedNodeText.Length);
            if (newPos != null)
                txtFormula.CaretPosition = newPos;

            if (selectedNodeText.Contains("(") && selectedNodeText.Contains(")"))
            {
                var fIndex = selectedNodeText.IndexOf("(");
                var lIndex = selectedNodeText.IndexOf(")");
                var lenght = lIndex - fIndex;
                var paramStr = selectedNodeText.Substring((fIndex + 1), lenght - 1);

                if (!string.IsNullOrEmpty(paramStr))
                {
                    if (!paramStr.Contains("[]"))
                    {
                        try
                        {
                            var paramRange = GetTextRange(selectedNodePosition.GetPositionAtOffset(fIndex + 1), paramStr.Length);
                            // paramRange.Select(paramRange.Start, paramRange.End);
                            txtFormula.Selection.Select(paramRange.Start, paramRange.End);
                            //         paramRange.ApplyPropertyValue(TextElement.ForegroundProperty­, Brushes.Green);

                        }
                        catch
                        {

                        }
                    }
                }
            }

        }

        RadWindow autoCompleteWindow;


        private void ShowAutoComplete(List<NodeContext> nodes)
        {
           
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

            autoCompleteWindow.ShowDialog();
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
                    Order1 = 0,
                    NodeType = NodeType.MainVariable
                };
                var treeItem = AddNodeContext(null, rootNodeContext, false);
                treeItem.IsExpanded = true;
            }
            var entityAndProperties = GetEntityAndProperties(Entity);
            SetNodes(rootNodeContext, entityAndProperties);
            foreach (var item in FormulaInstanceInternalHelper.GetHelpers().OrderBy(x => x.Key))
            {
                var nodeContext = new MyProject_WPF.NodeContext() { Order1 = 1, ParentPath = "", Context = item.Value, Name = item.Key, Title = item.Key, NodeType = NodeType.HelperProperty };
                var node = AddNodeContext(null, nodeContext, false);
                SetNodes(nodeContext, item.Value);
            }

            foreach (var item in FormulaInstanceInternalHelper.GetExpressionBuiltinVariables().OrderBy(x => x.Key))
            {
                var nodeContext = new MyProject_WPF.NodeContext() { Order1 = 2, ParentPath = "", Context = item.Value, Name = item.Key, Title = item.Key, NodeType = NodeType.HelperProperty };
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
            int i = 0;
            foreach (var property in entityAndProperties.Properties.OrderBy(x => x.Name))
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = property.Name;
                nodeContext.Title = nodeContext.Name;
                nodeContext.Context = property;
                if (property.PropertyType == PropertyType.Relationship)
                    nodeContext.NodeType = NodeType.RelationshipProperty;
                else
                    nodeContext.NodeType = NodeType.CustomProperty;
                nodeContext.Order1 = i;
                i++;
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
            int i = 0;
            List<NodeContext> result = new List<MyProject_WPF.NodeContext>();
            foreach (var prop in type.GetProperties().OrderBy(x => x.Name))
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = prop.Name;
                nodeContext.Title = nodeContext.Name;
                nodeContext.NodeType = NodeType.DotNetProperty;
                nodeContext.Context = prop;
                nodeContext.Order1 = i;
                i++;
                result.Add(nodeContext);
            }

            foreach (var method in type.GetMethods().Where(x => x.IsPublic).OrderBy(x => x.Name))
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
                nodeContext.Order1 = i;
                i++;
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
            NodeSelected(context.Title);
        }
        private void ImgAddWithPath_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, RadTreeViewItem node)
        {
            var context = node.DataContext as NodeContext;
            var text = GetNodePath(context, context.Title);
            NodeSelected(text);
        }
        private string GetNodePath(NodeContext node, string currentPath = "")
        {
            if (currentPath == "")
                currentPath = node.Title;
            if (node.ParentNode != null)
            {
                currentPath = node.ParentNode.Title + "." + currentPath;
                return GetNodePath(node.ParentNode, currentPath);
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



}
