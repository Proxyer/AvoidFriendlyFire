﻿using System.Collections.Generic;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace AvoidFriendlyFire.Patches
{
    [HarmonyPatch(typeof(GizmoGridDrawer), "DrawGizmoGrid")]
    public class GizmoGridDrawer_DrawGizmoGrid_Patch
    {
        public static bool Prefix(ref IEnumerable<Gizmo> gizmos)
        {
            if (!Main.Instance.IsModEnabled())
                return true;

            var extendedDataStore = Main.Instance.GetExtendedDataStorage();
            var pawn = Main.GetSelectedDraftedPawn();
            if (!extendedDataStore.canTrackPawn(pawn))
                return true;

            if (!FireCalculations.HasValidWeapon(pawn))
                return true;

            var pawnData = extendedDataStore.GetExtendedDataFor(pawn);

            var gizmoList = gizmos.ToList();
            var ourGizmo = new Command_Toggle
            {
                defaultLabel = "Avoid Friendly Fire",
                icon = Resources.FriendlyFireIcon,
                isActive = () => pawnData.AvoidFriendlyFire,
                toggleAction = () => pawnData.AvoidFriendlyFire = !pawnData.AvoidFriendlyFire
            };
            gizmoList.Add(ourGizmo);
            gizmos = gizmoList;

            return true;
        }
    }
}