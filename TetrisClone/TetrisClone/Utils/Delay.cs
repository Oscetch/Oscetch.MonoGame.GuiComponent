using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisClone.Utils
{
    public class Delay
    {
        private bool _invokeOnFirstCall;

        public double TargetTime { get; set; } = 0.0;
        public double DelayTime { get; set; } = 0.0;

        public long Loops { get; set; } = 0;

        public Delay(double delayTime, bool invokeOnFirstCall = true)
        {
            DelayTime = delayTime;
            _invokeOnFirstCall = invokeOnFirstCall;
        }

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
