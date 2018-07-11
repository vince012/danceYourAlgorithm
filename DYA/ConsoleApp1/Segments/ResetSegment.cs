using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Segments
{
    class ResetSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (Math.Abs(sB.handLeft.Position.X - sB.elbowLeft.Position.X) < 0.10
                && Math.Abs(sB.handRight.Position.Y - sB.elbowLeft.Position.Y) < 0.10
                && sB.handRight.Position.X > sB.kneeRight.Position.X
                && sB.handRight.Position.Y > sB.spine.Position.Y
                && sB.handLeft.Position.Y > sB.elbowLeft.Position.Y)
            {
                if (Math.Abs(sB.handRight.Position.Y - sB.elbowLeft.Position.Y) < 0.05)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
