using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class HandRightSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (sB.handRight.Position.X - 0.20 > sB.footRight.Position.X &&
                sB.handRight.Position.X - 0.60 > sB.neck.Position.X &&
                sB.handLeft.Position.Y < sB.waist.Position.Y)
            {
                if (Math.Abs(sB.handRight.Position.Y - sB.neck.Position.Y) < 0.10)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
