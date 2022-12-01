using System;
using System.Collections;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common
{
    public class Coroutine
    {
        public readonly IEnumerator Enumerator;

        public WaitFor currentWaitFor;
        public bool Active { get; private set; }

        public Coroutine(IEnumerator enumerator)
        {
            Enumerator = enumerator;
            Active = true;
        }

        public void Update()
        {
            if (Enumerator is null)
            {
                Stop();
                return;
            }

            object current = Enumerator.Current;

            if (current is bool cBool && !cBool)
            {
                Active = false;
            }
            else if (current is null)
            {
                MoveNext();
            }
            else if (current is WaitFor waitForO)
            {
                if (currentWaitFor is not null) currentWaitFor.WaitFrames--;
                else currentWaitFor = WaitFor.Frames(waitForO.WaitFrames - 1);

                if (currentWaitFor.WaitFrames <= 0) MoveNext();
            }
        }

        public void MoveNext()
        {
            currentWaitFor = null;
            bool finished = !Enumerator.MoveNext();

            if (finished) Active = false;
        }

        public void Stop()
        {
            Active = false;
        }
    }

    public class WaitFor
    {
        public int WaitFrames { get; set; }

        public static WaitFor Frames(int frames)
        {
            return new WaitFor() { WaitFrames = frames };
        }
    }
}
