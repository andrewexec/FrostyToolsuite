using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Frosty.Controls;
using Frosty.Core;
using FrostySdk;
using FrostySdk.Interfaces;

namespace DatapathFixPlugin.Actions
{
    // Fixes mods not applying on Steam, Epic Games, and the EA App
    public class LaunchExecutionAction : ExecutionAction
    {
        private static string DatapathFixExe => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DatapathFix", "DatapathFix.exe");

        private static string Game => Path.Combine(App.FileSystem.BasePath, $"{ProfilesLibrary.ProfileName}.exe");
        private static string Par => Path.Combine(App.FileSystem.BasePath, $"{ProfilesLibrary.ProfileName}.par");

        public override Action<ILogger, PluginManagerType, CancellationToken> PreLaunchAction => new Action<ILogger, PluginManagerType, CancellationToken>((ILogger logger, PluginManagerType type, CancellationToken cancelToken) =>
        {
            if (!Config.Get("DatapathFixEnabled", true))
                return;

            if (!File.Exists(DatapathFixExe))
            {
                logger.LogError($"DatapathFix: cannot find {DatapathFixExe}");

                Task.Run(() =>
                {
                    SystemSounds.Exclamation.Play();
                    FrostyMessageBox.Show($"Cannot find Plugins\\DatapathFix\\{Path.GetFileName(DatapathFixExe)}. Rebuild the DatapathFix project.", "DatapathFix", MessageBoxButton.OK);
                });
                return;
            }

            ResetGameDirectory(logger);

            Thread.Sleep(1000);

            string dataPath = Path.Combine(App.FileSystem.BasePath, $"ModData\\{App.SelectedPack}");
            string cmdArgs = $"-dataPath \"{dataPath}\" {Config.Get("CommandLineArgs", "", ConfigScope.Game)}".Trim();

            try
            {
                File.WriteAllText(Path.Combine(App.FileSystem.BasePath, "tmp"), cmdArgs);
                File.Move(Game, Game.Replace(".exe", ".orig.exe"));
                if (File.Exists(Par))
                    File.Copy(Par, Par.Replace(".par", ".orig.par"), true);
                File.Copy(DatapathFixExe, Game, true);
            }
            catch (Exception ex)
            {
                logger.LogError($"DatapathFix: {ex.Message}");
            }

            Thread.Sleep(1000);
        });

        public override Action<ILogger, PluginManagerType, CancellationToken> PostLaunchAction => new Action<ILogger, PluginManagerType, CancellationToken>((ILogger logger, PluginManagerType type, CancellationToken cancelToken) => { });

        internal static void ResetGameDirectory(ILogger logger)
        {
            try
            {
                string tmpPath = Path.Combine(App.FileSystem.BasePath, "tmp");
                if (File.Exists(tmpPath))
                    File.Delete(tmpPath);

                string origPar = Par.Replace(".par", ".orig.par");
                if (File.Exists(origPar))
                    File.Delete(origPar);

                string gameOld = Game.Replace(".exe", ".old");
                if (File.Exists(gameOld) && new FileInfo(gameOld).Length < 1000000)
                    File.Delete(gameOld);
            }
            catch (Exception ex)
            {
                logger.LogWarning($"DatapathFix: {ex.Message}");
            }

            try
            {
                if (File.Exists(Game.Replace(".exe", ".orig.exe")) && new FileInfo(Game).Length < 1000000)
                {
                    File.Delete(Game);
                    File.Move(Game.Replace(".exe", ".orig.exe"), Game);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"DatapathFix: {ex.Message}");
            }
        }
    }
}
