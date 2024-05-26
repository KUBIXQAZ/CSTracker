using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamItemsStatsViewer.Commands
{
    public class RefreshDataCommand : CommandBase
    {
        private Action _action;

        public RefreshDataCommand(Action action)
        {
            _action = action;
        }

        public override void Execute(object? parameter)
        {
            _action?.Invoke();
        }
    }
}
