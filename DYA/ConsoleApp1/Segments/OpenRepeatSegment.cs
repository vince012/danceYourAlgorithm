using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class OpenRepeatSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (Math.Abs(sB.footRight.Position.Y - sB.footLeft.Position.Y) < 0.15
                && sB.handLeft.Position.X  > sB.waist.Position.X
                && sB.handRight.Position.X  > sB.waist.Position.X)
                {
                    if (sB.handLeft.Position.Y > sB.head.Position.Y
                        && sB.handRight.Position.Y > sB.head.Position.Y
                        && Math.Abs(sB.head.Position.X - sB.waist.Position.X) > 0.10)
                    {
                        return GesturePartResult.Success;
                    }
                    return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
