using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class HandBottomSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (sB.head.Position.Z < sB.waist.Position.Z && 
                sB.handLeft.Position.Y + 0.25 < sB.waist.Position.Y)
            {
                if (Math.Abs(sB.handLeft.Position.X - sB.kneeLeft.Position.X) < 0.10
                    && Math.Abs(sB.kneeLeft.Position.X - sB.handRight.Position.X) < 0.20)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            else if (sB.head.Position.Z < sB.waist.Position.Z && 
                sB.handRight.Position.Y + 0.25 < sB.waist.Position.Y )
            {
                if (Math.Abs(sB.handRight.Position.X - sB.kneeRight.Position.X) < 0.10
                    && Math.Abs(sB.kneeLeft.Position.X - sB.handRight.Position.X) < 0.20)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
