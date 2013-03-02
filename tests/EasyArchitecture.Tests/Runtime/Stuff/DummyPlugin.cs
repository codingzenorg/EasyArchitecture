﻿using EasyArchitecture.Core;
using EasyArchitecture.Plugins;

namespace EasyArchitecture.Tests.Runtime.Stuff
{
    public class DummyPlugin : AbstractPlugin
    {
        protected override void ConfigurePlugin(ModuleAssemblies moduleAssemblies, PluginInspector pluginInspector)
        {
            pluginInspector.Log("Mensagem {0}","teste");
        }
    }
}
