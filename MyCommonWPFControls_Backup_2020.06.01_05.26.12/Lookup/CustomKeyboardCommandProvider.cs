using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyCommonWPFControls
{
    public class CustomKeyboardCommandProvider : DefaultKeyboardCommandProvider
    {

        public CustomKeyboardCommandProvider(GridViewDataControl grid) : base(grid)
        {
            this.parentGrid = grid;
        }

        public GridViewDataControl parentGrid { get; private set; }

        public override IEnumerable<ICommand> ProvideCommandsForKey(Key key)
        {
            List<ICommand> commandsToExecute = base.ProvideCommandsForKey(key).ToList();

            if (key == Key.Tab)
            {
                commandsToExecute.Remove(RadGridViewCommands.SelectCurrentItem);
            }
            else if (key == Key.Enter)
            {
                //if (parentGrid.CurrentCell.IsInEditMode)
                //{
                    commandsToExecute.Clear();
                    commandsToExecute.Remove(RadGridViewCommands.MoveDown);
                //}

            }

            return commandsToExecute;
        }
    }
}
