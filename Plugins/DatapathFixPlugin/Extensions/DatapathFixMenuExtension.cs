using DatapathFixPlugin.Actions;
using Frosty.Core;

namespace DatapathFixPlugin.Extensions
{
    public class DatapathFixMenuExtension : MenuExtension
    {
        public override string TopLevelMenuName => "Tools";
        public override string SubLevelMenuName => "DatapathFix";

        public override string MenuItemName => "Reset Game Installation";

        public override RelayCommand MenuItemClicked => new RelayCommand((o) => LaunchExecutionAction.ResetGameDirectory(App.Logger));
    }
}
