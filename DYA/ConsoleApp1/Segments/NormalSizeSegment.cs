using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Segments
{
    class NormalSizeSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (Math.Abs(sB.handLeft.Position.Y - sB.elbowLeft.Position.Y) < 0.05
                && Math.Abs(sB.handRight.Position.Y - sB.elbowRight.Position.Y) < 0.05
                && Math.Abs(sB.elbowLeft.Position.Y - sB.elbowRight.Position.Y) < 0.05
                && sB.elbowLeft.Position.Y < sB.shoulderLeft.Position.Y)
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
