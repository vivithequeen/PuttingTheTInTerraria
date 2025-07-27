using PuttingTheTInTerraria.Content.Buffs;
using PuttingTheTInTerraria.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PuttingTheTInTerraria.Content.Items
{
	public class ExampleWhip : ModItem
	{
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ExampleWhipDebuff.TagDamage);

		public override void SetDefaults() {
            // This method quickly sets the whip's properties.
            // Mouse over to see its parameters.
			Item.DefaultToWhip(ModContent.ProjectileType<ExampleWhipProjectile>(), 1500, 20, 17);
			Item.rare = ItemRarityID.Red;
			Item.channel = true;
		}

		// Makes the whip receive melee prefixes
		public override bool MeleePrefix() {
			return true;
		}
	}
}