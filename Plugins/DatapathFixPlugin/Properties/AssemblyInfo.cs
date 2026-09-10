using DatapathFixPlugin.Actions;
using DatapathFixPlugin.Extensions;
using DatapathFixPlugin.Options;
using Frosty.Core.Attributes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly
)]

[assembly: PluginDisplayName("DatapathFix")]
[assembly: PluginAuthor("andrew_exec")]
[assembly: PluginVersion("1.0.0.0")]

[assembly: RegisterOptionsExtension(typeof(LaunchOptions), Frosty.Core.PluginManagerType.Both)]
[assembly: RegisterExecutionAction(typeof(LaunchExecutionAction))]
[assembly: RegisterMenuExtension(typeof(DatapathFixMenuExtension), Frosty.Core.PluginManagerType.Both)]
