using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace MyProject_WPF
{

    public class EntityAndProperties
    {
        public EntityAndProperties()
        {
            Properties = new List<MyPropertyInfo>();
        }
        public TableDrivedEntityDTO Entity { set; get; }
        public List<MyPropertyInfo> Properties { set; get; }
    }
    public class NodeContext
    {
        public NodeContext()
        {

        }
        public NodeContext ParentNode { set; get; }
        // public List<NodeContext> ChildNodes { set; get; }
        //     public string ParentPath { set; get; }
        public object Context { set; get; }
        public string Name { set; get; }
        public string Title { set; get; }
        public NodeType NodeType { set; get; }
        //    public object UIItem { set; get; }
        public int Order1 { set; get; }
    }
    public enum NodeType
    {
        MainVariable,
        HelperProperty,
        RelationshipProperty,
        CustomProperty,
        DotNetProperty,
        DotNetMethod,
        Lambda
    }
    public class MyProp
    {
        public MyProp(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { set; get; }
        public Type Type { set; get; }
    }
    public class NodeSelectedArg
    {
        public string Title { set; get; }
        //public bool Dot { set; get; }
        //public bool Parantese { set; get; }
        //public NodeType PropertyType { set; get; }
        public NodeType NodeType { set; get; }
        //     public string NodePath { set; get; }
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


    public enum FormulaAutoCompleteKeyType
    {
        None,
        EmptyLeftCtrlSpace,
        AfterDotLeftCtrlSpace,
        InTextLeftControlSpace,
        DotOnly
    }
    public class AutoCompleteItem
    {
        public AutoCompleteItem(NodeType nodeType, string title)
        {
            Title = title;
            NodeType = nodeType;
        }

        public string Title { set; get; }
        public NodeType NodeType { set; get; }
    }
    public class WordDetection
    {
        public Tuple<TextPointer, TextPointer, string> CurrentWord { set; get; }
        // public List<Tuple<TextPointer, TextPointer, string>> PreviousWords { set; get; }
        public List<NodeContext> PossibleContexts { set; get; }
    }

    public class CodeBlock
    {
        TextPointer Start;
        public CodeBlock(TextPointer start)
        {
            Start = start;
        }
        public TextPointer StartPointer { set; get; }
        public TextPointer EndPointer { set; get; }
        public string Text { set; get; }

        public bool IsText { get; internal set; }

        public int StartOffset
        {
            get
            {
                return Start.GetOffsetToPosition(StartPointer);
            }
        }
        public int EndOffset
        {
            get

            {
                //if (!IsEndOfDocument)
                //    return Start.GetOffsetToPosition(EndPointer);
                //else
                //{
                //    if (Start.GetOffsetToPosition(EndPointer) - Start.GetOffsetToPosition(StartPointer) > 2)
                //        return Start.GetOffsetToPosition(EndPointer) - 2;
                //    else
                        return Start.GetOffsetToPosition(EndPointer);
                //}
            }
        }

        public bool IsEndOfDocument { get; internal set; }
        public int EmptySpaceAfter { get; internal set; }
    }
    public class NonTextBlock : CodeBlock
    {
        public NonTextBlock(TextPointer start) : base(start)
        {
        }

        public bool IsStartingParantese { set; get; }
        public CodeBlock PairBlock { set; get; }
        public bool IsEndingParantese { get; internal set; }
        public bool IsDot { get; internal set; }

    }
    public class FormulaTextBlock : CodeBlock
    {
        public FormulaTextBlock(TextPointer start) : base(start)
        {
        }

        public List<NodeContext> NextPossibleContexts { set; get; }
        public List<NodeContext> ParentNodeContexts { set; get; }
        public bool IsFunction { get; internal set; }
        //public Tuple<TextPointer, TextPointer> FunctionParantestPointers { get; internal set; }

        public NonTextBlock FunctionParanteseStart { set; get; }
        public NonTextBlock FunctionParanteseEnd { set; get; }

        //public Tuple<int, int> FunctionParantestOffset
        //{
        //    get
        //    {
        //        return new Tuple<int, int>(Start.GetOffsetToPosition(FunctionParantestPointers.Item1), Start.GetOffsetToPosition(FunctionParantestPointers.Item2));
        //    }
        //}
        public int ActualEndOffset
        {
            get
            {
                if (IsFunction)
                    return FunctionParanteseEnd.EndOffset;
                else
                    return EndOffset;
            }
        }
        public List<TextStateChain> TextStateChains { get; internal set; }
        public List<NodeContext> PossibleContexts { get; internal set; }
        public FormulaTextBlock LastItem { get; internal set; }
        public string LambdaText { get; internal set; }
        public bool IsLambdaSign { get; internal set; }
    }
    public class TextStateChain
    {
        public List<FormulaTextBlock> TextStates { set; get; }
        public int StartOffset { get { return TextStates[0].StartOffset; } }
        public int EndOffset { get { return TextStates[TextStates.Count - 1].ActualEndOffset; } }
        public FormulaTextBlock ParentFunction { get; internal set; }
    }


}
