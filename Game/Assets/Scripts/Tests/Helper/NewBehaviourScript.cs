using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class WaitForFrames : CustomYieldInstruction
    {
        private int targetFrameCount;

        public WaitForFrames(int numberOfFrames) {
            targetFrameCount = Time.frameCount + numberOfFrames;
        }

        public override bool keepWaiting {
            get {
                return Time.frameCount < targetFrameCount;
            }
        }
    }
}