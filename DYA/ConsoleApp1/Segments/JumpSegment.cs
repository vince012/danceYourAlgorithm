using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class JumpSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (sB.handLeft.Position.Y > sB.head.Position.Y + 0.20 &&
                sB.handRight.Position.Y > sB.head.Position.Y + 0.20)
            {
                if ((sB.head.Position.X - sB.handLeft.Position.X) < 0.30 && (sB.head.Position.X - sB.handLeft.Position.X) > 0.10 &&
                    (sB.handRight.Position.X - sB.head.Position.X) < 0.30 && (sB.handRight.Position.X - sB.head.Position.X) > 0.10
                    && Math.Abs(sB.kneeLeft.Position.X - sB.kneeRight.Position.X) < 0.15)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
          
            return GesturePartResult.Fail;
        }
    }
}
