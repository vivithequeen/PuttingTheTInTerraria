

using PuttingTheTInTerraria.Content;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using PuttingTheTInTerraria.Content.Items.Consumables;
using Terraria.GameContent.ItemDropRules;
using System;
using PuttingTheTInTerraria.Content.Projectiles;

namespace PuttingTheTInTerraria.Content.NPCs.Bosses
{
	[AutoloadBossHead]
	// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
	internal class MassTHead : NPCs.WormHead
	{
		public int AttackTimer = 0;
		Vector2 targetPosition;
		public override int BodyType => ModContent.NPCType<MassTBody>();

		public override int TailType => ModContent.NPCType<MassTTail>();

		public override void SetStaticDefaults()
		{


		}

		public override void SetDefaults()
		{
			// Head is 10 defense, body 20, tail 30.
			NPC.CloneDefaults(NPCID.WyvernHead);
			NPC.aiStyle = -1;
			NPC.lifeMax = 700000;
			NPC.width = 120;
			NPC.height = 192;
			// These lines are only needed in the main body part.
			NPC.boss = true;
			NPC.npcSlots = 10f;
			NPC.damage = 30;

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
			worm.MoveSpeed = 45f;
			worm.Acceleration = 4.045f;

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

				if (AttackTimer == 1)
				{
					targetPosition = Main.player[NPC.target].Center;
				}
				if (AttackTimer >= 2)
				{

					if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{

						var source = NPC.GetSource_FromAI();
						Vector2 position = NPC.Center;

						Vector2 direction = targetPosition - position;
						direction.Normalize();



						float projSpeed = 10f;

						int type = ModContent.ProjectileType<MassTSpike>();


						int damage = NPC.damage;

						Projectile.NewProjectile(source, position, direction * projSpeed, type, 200, 0f, Main.myPlayer);


					}
					AttackTimer = 0;

				}
				else
				{
					AttackTimer += 1;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MassTTresureBag>()));
		}
	}

	internal class MassTBody : NPCs.WormBody
	{
		public int AttackTimer = 0;
		Vector2 targetPosition;
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<MassTHead>();
		}

		public override void SetDefaults()
		{
			Random rnd = new Random();
			AttackTimer = (int)(rnd.NextDouble() * 20);
			NPC.CloneDefaults(NPCID.WyvernBody);
			NPC.aiStyle = -1;
			NPC.width = 120;
			NPC.height = 192;
			// Extra body parts should use the same Banner value as the main ModNPC.
			NPC.damage = 25;
		}
		public override void AI()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{

				if (AttackTimer == 230)
				{
					targetPosition = Main.player[NPC.target].Center;
				}
				if (AttackTimer >= 240)
				{

					if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{

						var source = NPC.GetSource_FromAI();
						Vector2 position = NPC.Center;

						Vector2 direction = targetPosition - position;
						direction.Normalize();



						float projSpeed = 10f;

						int type = ModContent.ProjectileType<MassTSpike>();


						int damage = NPC.damage;

						Projectile.NewProjectile(source, position, direction * projSpeed, type, 200, 0f, Main.myPlayer);


					}
					AttackTimer = 0;

				}
				else
				{
					AttackTimer += 1;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MassTTresureBag>()));
		}

		public override void Init()
		{
			MassTHead.CommonWormInit(this);
		}
	}

	internal class MassTTail : NPCs.WormTail
	{
		public override void SetStaticDefaults()
		{

			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<MassTHead>();
		}

		public override void SetDefaults()
		{
			NPC.damage = 20;
			NPC.CloneDefaults(NPCID.WyvernTail);
			NPC.aiStyle = -1;

			NPC.width = 120;
			NPC.height = 192;
		}

		public override void Init()
		{
			MassTHead.CommonWormInit(this);
		}
	}
}