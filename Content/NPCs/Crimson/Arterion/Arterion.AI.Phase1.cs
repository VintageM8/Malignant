using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Malignant.Content.NPCs.Crimson.Arterion
{
    internal partial class Arterion
    {
        [StateMethod(AIState.Phase1)]
        private void DoPhase1()
        {
            if (!ValidTarget(NPC.target))
            {
                int newTarget;
                if ((newTarget = GetRandomTarget()) == -1)
                {
                    SetState(AIState.Fleeing);
                    return;
                }

                NPC.target = newTarget;
                NPC.netUpdate = true;
            }

            Player target = Main.player[NPC.target];
            float distanceToTarget = NPC.Center.DistanceSQ(target.Center);
            Vector2 directionToTarget = NPC.Center.DirectionTo(target.Center);
            if (distanceToTarget > MathF.Pow(250f, 2))
            {
                NPC.velocity += directionToTarget * 2f;
            }

            NPC.velocity *= 0.9f;
        }

        private void DoAttack1()
        {

        }
    }
}
