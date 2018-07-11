using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class FingerCount : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (sB.head.Position.Z >= sB.handRight.Position.Z + 0.30)
            {
                if (Math.Abs(sB.head.Position.X - sB.handRight.Position.X) <= 0.15)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
