

using PuttingTheTInTerraria.Content;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace PuttingTheTInTerraria.Content.NPCs.Bosses
{
	[AutoloadBossHead]
	// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
	internal class MassTHead : NPCs.WormHead
	{
		public override int BodyType => ModContent.NPCType<MassTBody>();

		public override int TailType => ModContent.NPCType<MassTTail>();

		public override void SetStaticDefaults()
		{


		}

		public override void SetDefaults()
		{
			// Head is 10 defense, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.aiStyle = -1;
			NPC.lifeMax = 700000;
			NPC.width = 120;
			NPC.height = 192;
			// These lines are only needed in the main body part.
			NPC.boss = true;
			NPC.npcSlots = 10f;

		}



		public override void Init()
		{
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 20;
			MaxSegmentLength = 25;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(NPCs.Worm worm)
		{
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 25f;
			worm.Acceleration = 2.045f;

		}

		private int attackCounter;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(attackCounter);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			attackCounter = reader.ReadInt32();
		}

		public override void AI()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (attackCounter > 0)
				{
					attackCounter--; // tick down the attack counter.
				}

				Player target = Main.player[NPC.target];
				// If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
				if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target.Center) < 200 && Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
				{
					Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
					direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

					int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ProjectileID.ShadowBeamHostile, 5, 0, Main.myPlayer);
					Main.projectile[projectile].timeLeft = 300;
					attackCounter = 500;
					NPC.netUpdate = true;
				}
			}
		}
	}

	internal class MassTBody : NPCs.WormBody
	{
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<MassTHead>();
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.aiStyle = -1;
			NPC.width = 120;
			NPC.height = 192;
			// Extra body parts should use the same Banner value as the main ModNPC.
			Banner = ModContent.NPCType<MassTHead>();
		}

		public override void Init() {
			MassTHead.CommonWormInit(this);
		}
	}

	internal class MassTTail : NPCs.WormTail
	{
		public override void SetStaticDefaults() {
	
			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<MassTHead>();
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;

			NPC.width = 120;
			NPC.height = 192;
		}

		public override void Init() {
			MassTHead.CommonWormInit(this);
		}
	}
}