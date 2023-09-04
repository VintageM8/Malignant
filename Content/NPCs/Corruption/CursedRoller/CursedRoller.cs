using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Corruption.CursedRoller
{
    public class CursedRoller : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0, -5f),
                PortraitPositionYOverride = -20f
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 60;
            NPC.defense = 24;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            // npc.noGravity = false;
            // npc.noTileCollide = false;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 26;
            AIType = NPCID.Unicorn;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement("It rolls and shoots fire....watch out"),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneOverworldHeight ? .075f : 0f;

        public bool firedSide1 = false;
        public int SideTime = 0;
        public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.05f;

            SideTime += 1;

            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(10f, 2f);
                Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y + 10), DustID.Corruption, speed);
                d.noGravity = true;
            }

            if (SideTime > 30)
            {
                if (firedSide1 == false)
                {
                    Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(8, 1.5f), ProjectileID.CursedFlameHostile, Main.rand.Next(10, 20), 5);
                    Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(-8, 1.5f), ProjectileID.CursedFlameHostile, Main.rand.Next(10, 20), 5);
                    firedSide1 = true;
                }

                if (SideTime >= 150)
                {
                    Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(8, -2f), ProjectileID.CursedFlameHostile, Main.rand.Next(10, 20), 5);
                    Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(-8, -2f), ProjectileID.CursedFlameHostile, Main.rand.Next(10, 20), 5);

                    SideTime = 0;
                    firedSide1 = false;
                }
            }
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            //Add Stuff here
        }
    }
}
