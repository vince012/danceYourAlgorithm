using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class TurnRightSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (sB.footLeft.Position.Y > sB.footRight.Position.Y + 0.10 &&
                sB.footLeft.Position.X + 0.15 < sB.footRight.Position.X &&
                sB.handLeft.Position.Y < sB.waist.Position.Y)
            {
                if (sB.handRight.Position.Y > sB.head.Position.Y &&
                    (sB.handRight.Position.X - sB.footRight.Position.X) > 0.30)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
