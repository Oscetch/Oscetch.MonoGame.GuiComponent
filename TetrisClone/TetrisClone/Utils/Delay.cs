using Microsoft.Xna.Framework;
using System;

namespace TetrisClone.Utils
{
    public class Delay(double delayTime, bool invokeOnFirstCall = true)
    {
        private readonly bool _invokeOnFirstCall = invokeOnFirstCall;

        public double TargetTime { get; set; } = 0.0;
        public double DelayTime { get; set; } = delayTime;

        public long Loops { get; set; } = 0;

        public void Wait(GameTime gt, Action action)
        {
            if (!_invokeOnFirstCall && Loops == 0)
            {
                Reset(gt);
                Loops++;
            }

            if (TargetTime <= gt.TotalGameTime.TotalMilliseconds)
            {
                Reset(gt);
                Loops++;
                action?.Invoke();
            }
        }

        public bool Wait<T>(GameTime gt, Func<T> action, out T funcReturn)
        {
            funcReturn = default;
            if (!_invokeOnFirstCall && Loops == 0)
            {
                Reset(gt);
                Loops++;
            }

            if (TargetTime <= gt.TotalGameTime.TotalMilliseconds)
            {
                Reset(gt);
                Loops++;
                funcReturn = action.Invoke();
                return true;
            }

            return false;
        }

        public void Reset(GameTime gt)
        {
            TargetTime = gt.TotalGameTime.TotalMilliseconds + DelayTime;
        }
    }
}
