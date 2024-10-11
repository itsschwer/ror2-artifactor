#if DISCONINUITY
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
            Init();
            ServerSider.Plugin.Tweaks.managedTweaks.Remove(ServerSider.Plugin.Tweaks.RescueShipLoopPortal);
            ServerSider.Plugin.Tweaks.RescueShipLoopPortal.Disable();
        }

        protected override void OnEnabled()
        {
            if (UnityEngine.Networking.NetworkServer.active) {
                ServerSider.Plugin.Tweaks.RescueShipLoopPortal.Enable();
            }
        }
        protected override void OnDisabled() => ServerSider.Plugin.Tweaks.RescueShipLoopPortal.Disable();
    }
}
#endif