using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Segments
{
    class HideSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (sB.handLeft.Position.Y > sB.neck.Position.Y
                && sB.handLeft.Position.Y < sB.head.Position.Y
                && sB.handLeft.Position.X > sB.shoulderLeft.Position.X
                && sB.handLeft.Position.X < sB.shoulderRight.Position.X
                && Math.Abs(sB.handLeft.Position.Y - sB.elbowLeft.Position.Y) < 0.10

                && sB.handRight.Position.Y > sB.neck.Position.Y
                && sB.handRight.Position.Y < sB.head.Position.Y
                && sB.handRight.Position.X > sB.shoulderLeft.Position.X
                && sB.handRight.Position.X < sB.shoulderRight.Position.X
                && Math.Abs(sB.handRight.Position.Y - sB.elbowRight.Position.Y) < 0.10
                )
            {
                if (Math.Abs(sB.kneeLeft.Position.X - sB.kneeRight.Position.X) < 0.20)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
