using RoR2;

namespace Artifactor
{
    public static class Content
    {
        public static ArtifactDef Discontinuity;
        public static ArtifactDef Voracity;

        internal static void Init()
        {
            Discontinuity = new Discontinuity().artifactDef;
            Voracity = new Voracity().artifactDef;

            Plugin.Logger.LogMessage("~content initialized");
        }
    }
}
