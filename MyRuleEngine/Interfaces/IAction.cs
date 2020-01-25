using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
    public interface IAction
    {
        ActionResultEnum Execute(object[] objects);

        event EventHandler<ActionInfoArg> ActionEvent;
    }

    //public class ActionResult
    //{
    //    public ActionResultEnum Result { set; get; }
    //}
    public enum ActionResultEnum
    {
        Successful = 1,
        Failed = 2
    }
  public  class ActionInfoArg : EventArgs
    {
        public string Title;
        public int TotalEventsCount { set; get; }
        public int EventsNumber { set; get; }
    }
}
