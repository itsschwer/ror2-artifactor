using RoR2;
using UnityEngine;

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

        protected abstract Sprite EnabledIcon { get; }
        protected abstract Sprite DisabledIcon { get; }

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




        internal static readonly Color EnabledColor = new Color(231 / 255f, 199 / 255f, 231 / 255f);
        internal static readonly Color DisabledColor = new Color(71 / 255f, 62 / 255f, 69 / 255f);

        internal static Sprite CreateIcon(Sprite src, Color tint)
        {
            Texture2D tex = DuplicateTexture(src.texture);
            Color[] cols = tex.GetPixels();
            for (int i = 0; i < cols.Length; i++) {
                cols[i].r *= tint.r;
                cols[i].g *= tint.g;
                cols[i].b *= tint.b;
            }
            tex.SetPixels(cols);
            tex.Apply(true, true);
            return Sprite.Create(tex, src.rect, src.pivot, src.pixelsPerUnit);
        }

        // Reference: https://stackoverflow.com/questions/44733841/how-to-make-texture2d-readable-via-script/44734346#44734346
        internal static Texture2D DuplicateTexture(Texture2D src)
        {
            RenderTexture rt = RenderTexture.GetTemporary(src.width, src.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(src, rt);
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(src.width, src.height);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}
