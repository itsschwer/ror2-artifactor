using RoR2;
using ServerSider;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Artifactor
{
    internal class Discontinuity : ArtifactBase<Discontinuity>
    {
        protected override string Token => "ITSSCHWER_DISCONTINUITY";
        protected override string Name => "Artifact of Discontinuity";
        protected override string Description => "<style=cWorldEvent>The <style=cHumanObjective>Rescue Ship</style> aligns with the planet..</style>";

        protected override Sprite EnabledIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texBuffElementalRingVoidReadyIcon.tif").WaitForCompletion();
        protected override Sprite DisabledIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texBuffElementalRingVoidCooldownIcon.tif").WaitForCompletion();

        public Discontinuity() : base()
        {
            ServerSider.Plugin.UnmanageHook(RescueShipLoopPortal.ManageHook);
            Run.onRunStartGlobal += ManageHook;
            Run.onRunDestroyGlobal += ManageHook;
            ManageHook();
        }

        private static void ManageHook(Run _ = null) => RescueShipLoopPortal.Rehook(
            UnityEngine.Networking.NetworkServer.active
         && RunArtifactManager.instance.IsArtifactEnabled(instance.artifactDef)
        );
    }
}
