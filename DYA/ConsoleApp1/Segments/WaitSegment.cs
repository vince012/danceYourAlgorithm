using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class WaitSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (sB.handLeft.Position.Y < sB.neck.Position.Y
                && sB.handRight.Position.Y < sB.neck.Position.Y

                && sB.handLeft.Position.Y < sB.elbowLeft.Position.Y
                && Math.Abs(sB.wristLeft.Position.Y - sB.elbowLeft.Position.Y) > 0.10

                && sB.handRight.Position.Y < sB.elbowRight.Position.Y
                && Math.Abs(sB.handRight.Position.Y - sB.elbowRight.Position.Y) > 0.10

                && sB.handLeft.Position.X > sB.elbowLeft.Position.X
                && Math.Abs(sB.handLeft.Position.X - sB.elbowLeft.Position.X) > 0.05

                && sB.handRight.Position.X < sB.elbowRight.Position.X
                && Math.Abs(sB.handRight.Position.X - sB.elbowRight.Position.X) > 0.05

                && sB.handLeft.Position.X < sB.hipLeft.Position.X
                && Math.Abs(sB.handLeft.Position.X - sB.hipLeft.Position.X) < 0.20

                && sB.handRight.Position.X > sB.hipRight.Position.X
                && Math.Abs(sB.handRight.Position.X - sB.hipRight.Position.X) < 0.20
                )
            {
                if (Math.Abs(sB.kneeLeft.Position.X - sB.kneeRight.Position.X) < 0.15
                    && Math.Abs(sB.kneeLeft.Position.Y - sB.kneeRight.Position.Y) < 0.10
                    )
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
