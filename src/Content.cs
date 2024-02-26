using RoR2;

namespace Artifactor
{
    public static class Content
    {
        public static ArtifactDef Discontinuity;

        internal static void Init()
        {
            Discontinuity = new Discontinuity().artifactDef;

            Log.Message("~content initialized");
        }
    }
}
