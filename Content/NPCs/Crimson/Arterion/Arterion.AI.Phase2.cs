using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malignant.Content.NPCs.Crimson.Arterion
{
    internal partial class Arterion
    {
        [StateMethod(AIState.Phase2)]
        private void DoPhase2()
        {
            if (!ValidTarget(NPC.target))
            {
                int newTarget;
                if ((newTarget = GetRandomTarget()) == -1)
                {
                    SetState(AIState.Fleeing);
                }

                NPC.target = newTarget;
            }
        }
    }
}
