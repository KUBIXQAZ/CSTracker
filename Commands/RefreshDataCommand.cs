using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SteamItemsStatsViewer.Commands
{
    public class RefreshDataCommand : CommandBase
    {
        private Action<string> _action;
        private string _filePath;

        public RefreshDataCommand(Action<string> action, string filePath)
        {
            _action = action;
            _filePath = filePath;
        }

        public override void Execute(object? parameter)
        {
            _action?.Invoke(_filePath);
        }
    }
}
