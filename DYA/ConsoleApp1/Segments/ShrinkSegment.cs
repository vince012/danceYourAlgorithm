using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Segments
{
    class ShrinkSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (Math.Abs(sB.handLeft.Position.Y - sB.handRight.Position.Y) < 0.10 
                && Math.Abs(sB.handLeft.Position.X- sB.kneeLeft.Position.X) < 0.10
                && sB.handLeft.Position.Y > sB.kneeLeft.Position.Y
                && sB.handLeft.Position.Y < sB.hipLeft.Position.Y)
            {
                if (Math.Abs(sB.kneeLeft.Position.X - sB.kneeRight.Position.X) > 0.25)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
