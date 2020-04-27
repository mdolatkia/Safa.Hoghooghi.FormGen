using ModelEntites;
using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.FormulaCalculationArea
{


    public interface I_FormulaCalculationArea
    {
        FormulaCalculationAreaInitializer AreaInitializer { set; get; }
        I_View_FormulaCalculationArea View { set; get; }
    }

    public interface I_View_FormulaCalculationArea
    {
        string FromulaExpression { get; set; }
        object ResultString { get; set; }

        void ParameterTabSelect();
        void ExceptionTabSelect();
        event EventHandler CloseRequested;
        object AddTreeNode(object parentNode, string title);
        void ClearTree();

        void AddException(string exception);
    }

    public class FormulaCalculationAreaInitializer
    {
        public SimpleColumnControl ColumnControl { get; set; }
        public DP_DataRepository DataItem { set; get; }
        public FormulaDTO  Formula { set; get; }

        public I_UIFomulaManager FomulaManager { set; get; }
    }
}
