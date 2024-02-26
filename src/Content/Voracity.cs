using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Artifactor
{
    internal class Voracity : ArtifactBase<Voracity>
    {
        protected override string Token => "ITSSCHWER_VORACITY";
        protected override string Name => "Artifact of Voracity";
        protected override string Description => "Monsters can form greater numbers and spawn faster.";

        protected override Sprite EnabledIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/ElementalRings/texBuffElementalRingsReadyIcon.tif").WaitForCompletion();
        protected override Sprite DisabledIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/EliteLunar/texBuffAffixLunar.tif").WaitForCompletion();

        public Voracity() : base() {}

        private bool IsHost => UnityEngine.Networking.NetworkServer.active;

        protected override void OnEnabled()
        {
            if (!IsHost) return;

            On.RoR2.CombatDirector.Awake += CombatDirector_Awake;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;
        }

        protected override void OnDisabled()
        {
            On.RoR2.CombatDirector.Awake -= CombatDirector_Awake;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Run.onRunDestroyGlobal -= Run_onRunDestroyGlobal;
        }


        private static void CombatDirector_Awake(On.RoR2.CombatDirector.orig_Awake orig, CombatDirector self)
        {
            orig(self);
            self.creditMultiplier *= 1.25f;
            // Could make creditMultiplier and softTeamCharacterLimits configurable (host-dependent)?
        }

        private static void Run_onRunStartGlobal(Run _)
        {
            TeamIndex[] targets = { TeamIndex.Monster, TeamIndex.Void, TeamIndex.Lunar };
            foreach (TeamIndex team in targets) {
                softTeamCharacterLimits.Add(team, TeamCatalog.GetTeamDef(team).softCharacterLimit);
                TeamCatalog.GetTeamDef(team).softCharacterLimit *= 2;
            }
            Log.Debug($"{nameof(Voracity)}> Team character limits raised.");
        }

        private static void Run_onRunDestroyGlobal(Run _)
        {
            foreach (TeamIndex team in softTeamCharacterLimits.Keys) {
                TeamCatalog.GetTeamDef(team).softCharacterLimit = softTeamCharacterLimits[team];
            }
            softTeamCharacterLimits.Clear();
            Log.Debug($"{nameof(Voracity)}> Team character limits restored.");
        }

        private static readonly Dictionary<TeamIndex, int> softTeamCharacterLimits = [];
    }
}
