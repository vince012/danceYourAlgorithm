using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Segments
{
    class ChangeSceneSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (Math.Abs(sB.handLeft.Position.Y - sB.handRight.Position.Y) < 0.10
                && sB.handRight.Position.Y > sB.head.Position.Y)
            {
                if (Math.Abs(sB.kneeLeft.Position.X - sB.kneeRight.Position.X) > 0.30
                    && Math.Abs(sB.handLeft.Position.X - sB.handRight.Position.X) > 0.60)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
