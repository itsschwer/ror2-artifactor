using BepInEx;

namespace Artifactor
{
    [BepInDependency(ServerSider.Plugin.GUID)]
    [BepInDependency(R2API.ContentManagement.R2APIContentManager.PluginGUID)]
    [BepInDependency(R2API.LanguageAPI.PluginGUID)]
    [R2API.Utils.NetworkCompatibility(R2API.Utils.CompatibilityLevel.EveryoneMustHaveMod, R2API.Utils.VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(GUID, Name, Version)]

    public sealed class Plugin : BaseUnityPlugin
    {
        public const string GUID = Author + "." + Name;
        public const string Author = "itsschwer";
        public const string Name = "Artifactor";
        public const string Version = "0.0.0";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Message")]
        private void Awake()
        {
            Log.Init(Logger);
            Content.Init();
            Log.Message("~awake.");
        }
    }
}
