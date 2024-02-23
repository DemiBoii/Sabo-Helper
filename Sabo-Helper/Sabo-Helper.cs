using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.IoC;
using System.IO;
using System.Reflection;

namespace Sabo-Helper
{
    public class Sabo-Helper : IDalamudPlugin
    {
        public string Name => "Sabo-Helper";

        private const string CommandName = "/fchelper";

        private Configuration Configuration { get; init; }
        private Sabo-HelperUI UI { get; init; }

        public Sabo-Helper([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            DalamudContainer.Initialize(pluginInterface);

            this.Configuration = DalamudContainer.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(DalamudContainer.PluginInterface);
            
            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"goat.png");
            var goatImage = DalamudContainer.PluginInterface.UiBuilder.LoadImage(imagePath);
            this.UI = new Sabo-HelperUI(this.Configuration, goatImage);
            
            DalamudContainer.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Configure Free Company Helper"
            });

            DalamudContainer.PluginInterface.UiBuilder.Draw += DrawUI;
            DalamudContainer.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.UI.Dispose();

            DalamudContainer.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            this.UI.Visible = true;
        }

        private void DrawUI()
        {
            this.UI.Draw();
        }

        private void DrawConfigUI()
        {
            this.UI.SettingsVisible = true;
        }
    }
}
