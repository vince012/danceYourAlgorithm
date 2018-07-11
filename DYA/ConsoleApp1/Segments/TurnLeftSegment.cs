using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class TurnLeftSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (sB.footRight.Position.Y > sB.footLeft.Position.Y + 0.10 &&
                sB.footLeft.Position.X + 0.15 < sB.footRight.Position.X &&
                sB.handRight.Position.Y < sB.waist.Position.Y)
            {
                if (sB.handLeft.Position.Y > sB.head.Position.Y &&
                    (sB.footRight.Position.X - sB.handLeft.Position.X) > 0.30)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
