using RoR2;

namespace Artifactor
{
    public static class Content
    {
#if DISCONINUITY
        public static ArtifactDef Discontinuity { get; private set; }
#endif
        public static ArtifactDef Voracity;

        internal static void Init()
        {
#if DISCONINUITY
            Discontinuity = new Discontinuity().artifactDef;
#endif
            Voracity = new Voracity().artifactDef;

            Plugin.Logger.LogMessage("~content initialized");
        }
    }
}
