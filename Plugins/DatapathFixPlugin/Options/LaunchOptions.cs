using Frosty.Core;
using FrostySdk.Attributes;

namespace DatapathFixPlugin.Options
{
    [DisplayName("DatapathFix Options")]
    public class LaunchOptions : OptionsExtension
    {
        [Category("General")]
        [DisplayName("Enabled")]
        [Description("Fixes mods not applying when launching through Steam, Epic Games Store, or EA Desktop. Not needed if only the EA App/Origin is used.")]
        [EbxFieldMeta(FrostySdk.IO.EbxFieldType.Boolean)]
        public bool DatapathFixEnabled { get; set; } = true;

        public override void Load()
        {
            DatapathFixEnabled = Config.Get("DatapathFixEnabled", true);
        }

        public override void Save()
        {
            Config.Add("DatapathFixEnabled", DatapathFixEnabled);

            if (DatapathFixEnabled && Config.Get("PlatformLaunchingEnabled", false, ConfigScope.Game))
                Config.Add("PlatformLaunchingEnabled", false, ConfigScope.Game);
        }
    }
}
