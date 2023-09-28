using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Malignant.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Malignant.Content.Items.Hell.MarsHell;
using System.Collections.Generic;
using System.Linq;
using Malignant.Common.Helper;

namespace Malignant.Content.Items.Holy.CrucifixConstructer
{
    
    public class Crucifix : ModProjectile
    {
        private bool shot = false;
        private Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.timeLeft = 220;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
            foreach (var proj in list)
            {
                if ((proj.IsHammer() && proj.active && proj.friendly && !proj.hostile))
                {
                    shot = true;
                    Projectile.active = false;
                }
            }
        }


        public override bool PreAI()
        {

            if (Projectile.ai[0] == 0)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            else
            {
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                int num996 = 15;
                bool flag52 = false;
                bool flag53 = false;
                Projectile.localAI[0] += 1f;
                if (Projectile.localAI[0] % 30f == 0f)
                    flag53 = true;

                int num997 = (int)Projectile.ai[1];
                if (Projectile.localAI[0] >= (float)(60 * num996))
                    flag52 = true;
                else if (num997 < 0 || num997 >= 200)
                    flag52 = true;
                else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage)
                {
                    Projectile.Center = Main.npc[num997].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[num997].gfxOffY;
                    if (flag53)
                    {
                        Main.npc[num997].HitEffect(0, 1.0);
                    }
                }
                else
                    flag52 = true;

                if (flag52)
                    Projectile.Kill();
            }
            return false;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = 1f;
            Projectile.ai[1] = (float)target.whoAmI;
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
            Projectile.netUpdate = true;
            Projectile.damage = 0;

            int num31 = 3;
            Point[] array2 = new Point[num31];
            int num32 = 0;

            for (int n = 0; n < 1000; n++)
            {
                if (n != Projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == Projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI)
                {
                    array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
                    if (num32 >= array2.Length)
                        break;
                }
            }

            if (num32 >= array2.Length)
            {
                int num33 = 0;
                for (int num34 = 1; num34 < array2.Length; num34++)
                {
                    if (array2[num34].Y < array2[num33].Y)
                        num33 = num34;
                }
                Main.projectile[array2[num33].X].Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(Main.rand.NextFloat(-15f, 15f))) * Main.rand.NextFloat(-0.25f, -0.35f);

                Dust.NewDustPerfect(Projectile.Center + new Vector2(0f, 28f), DustID.Gold, vel, 0, new Color(255, 255, 60) * 0.8f, 0.95f);

                Dust.NewDustPerfect(Projectile.Center, DustID.Gold, vel * 1.2f, 0, new Color(150, 80, 40), Main.rand.NextFloat(0.2f, 0.4f));
            }
            SoundEngine.PlaySound(SoundID.NPCHit4.WithPitchOffset(Main.rand.NextFloat(-0.1f, 0.1f)).WithVolumeScale(0.5f), Projectile.position);
        }
    }
}
