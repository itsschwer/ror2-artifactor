using RoR2;

namespace Artifactor
{
    public static class Content
    {
#if DISCONINUITY
        public static ArtifactDef Discontinuity { get; private set; }
#endif

        internal static void Init()
        {
#if DISCONINUITY
            Discontinuity = new Discontinuity().artifactDef;
#endif

            Plugin.Logger.LogMessage("~content initialized");
        }
    }
}
