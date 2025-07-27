﻿using PuttingTheTInTerraria.Content.Projectiles;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ModLoader;

namespace PuttingTheTInTerraria.Content.GlobalNPCs
{
	internal class DamageOverTime : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool exampleJavelinDebuff;

		public override void ResetEffects(NPC npc) {
			exampleJavelinDebuff = false;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (exampleJavelinDebuff) {
				if (npc.lifeRegen > 0) {
					npc.lifeRegen = 0;
				}
				// Count how many ExampleJavelinProjectile are attached to this npc.
				int exampleJavelinCount = 0;
				foreach (var p in Main.ActiveProjectiles) {
					if (p.type == ModContent.ProjectileType<Projectiles.Spike>() && p.ai[0] == 1f && p.ai[1] == npc.whoAmI) {
						exampleJavelinCount++;
					}
				}
				// Remember, lifeRegen affects the actual life loss, damage is just the text.
				// The logic shown here matches how vanilla debuffs stack in terms of damage numbers shown and actual life loss.
				npc.lifeRegen -= exampleJavelinCount * 2 * 3;
				if (damage < exampleJavelinCount * 3) {
					damage = exampleJavelinCount * 3;
				}
			}
		}

	}
}