using RoR2;

namespace Artifactor
{
    /* Based on KomradeSpectre's ItemModCreationBoilerplate
     * https://github.com/KomradeSpectre/ItemModCreationBoilerplate/blob/494fa6cb91cfa6f998990c27fad3bd514771c212/ItemModCreationBoilerplate/Artifact/ArtifactBase.cs
     */

    internal abstract class ArtifactBase<T> : ArtifactBase where T : ArtifactBase<T>
    {
#pragma warning disable IDE1006 // Naming rule violation: must begin with upper case character
        public static T instance { get; private set; }
#pragma warning restore IDE1006 // Naming rule violation: must begin with upper case character

        public ArtifactBase()
        {
            if (instance != null) throw new System.InvalidOperationException($"{nameof(ArtifactBase)} '{typeof(T).Name}' instance already exists.");

            instance = this as T;
        }
    }

    internal abstract class ArtifactBase
    {
        protected abstract string Token { get; }
        protected abstract string Name { get; }
        protected abstract string Description { get; }

        protected abstract UnityEngine.Sprite EnabledIcon { get; }
        protected abstract UnityEngine.Sprite DisabledIcon { get; }

        protected string CachedName => "ARTIFACT_" + Token;
        protected string NameToken => CachedName + "_NAME";
        protected string DescriptionToken => CachedName + "_DESCRIPTION";

        public ArtifactDef artifactDef;

        protected void Init()
        {
            CreateArtifactDef();
            CreateLanguageTokens();
            RunArtifactManager.onArtifactEnabledGlobal += OnArtifactEnabled;
            RunArtifactManager.onArtifactDisabledGlobal += OnArtifactDisabled;
        }

        protected ArtifactDef CreateArtifactDef()
        {
            artifactDef = UnityEngine.ScriptableObject.CreateInstance<ArtifactDef>();

            artifactDef.cachedName = CachedName;
            artifactDef.nameToken = NameToken;
            artifactDef.descriptionToken = DescriptionToken;

            artifactDef.smallIconSelectedSprite = EnabledIcon;
            artifactDef.smallIconDeselectedSprite = DisabledIcon;

            R2API.ContentAddition.AddArtifactDef(artifactDef);
            return artifactDef;
        }

        protected void CreateLanguageTokens()
        {
            R2API.LanguageAPI.Add(NameToken, Name);
            R2API.LanguageAPI.Add(DescriptionToken, Description);
        }

        protected void OnArtifactEnabled(RunArtifactManager _, ArtifactDef artifactDef)
        {
            if (artifactDef == this.artifactDef) OnEnabled();
        }
        protected void OnArtifactDisabled(RunArtifactManager _, ArtifactDef artifactDef)
        {
            if (artifactDef == this.artifactDef) OnDisabled();
        }

        protected abstract void OnEnabled();
        protected abstract void OnDisabled();
    }
}
