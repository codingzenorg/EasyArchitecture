using System.Collections.Generic;
using System.Linq;
using EasyArchitecture.Runtime;
using EasyArchitecture.Runtime.Plugin;
using EasyArchitecture.Translation.Plugin.Contracts;

namespace EasyArchitecture.Translation.Plugin.BultIn
{
    internal class TranslatorPlugin : AbstractPlugin, ITranslatorPlugin
    {
        private readonly List<TypeMap> _mappedTypes = new List<TypeMap>();

        protected override void ConfigurePlugin(ModuleAssemblies moduleAssemblies, PluginInspector pluginInspector)
        {
            var assembly = moduleAssemblies.InfrastructureAssembly;
            foreach (var mapRule in from tipo in assembly.GetExportedTypes()
                                      where tipo.BaseType == typeof(MapRule)
                                      select tipo.Assembly.CreateInstance(tipo.FullName))
            {
                AddMapRule((MapRule) mapRule);
                pluginInspector.Log("Adding MapRule for {0}", mapRule.GetType().Name);
            }
        }

        public ITranslator GetInstance()
        {
            return new Translator(_mappedTypes);
        }

        private void AddMapRule(MapRule mapRule)
        {
            _mappedTypes.AddRange(mapRule.GetMapRules());
        }
    }
}
