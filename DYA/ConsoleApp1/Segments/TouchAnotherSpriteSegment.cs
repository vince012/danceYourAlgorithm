using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class TouchAnotherSpriteSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (sB.handLeft.Position.Y < sB.hipLeft.Position.Y
                && sB.handRight.Position.Y < sB.hipRight.Position.Y

                && sB.handLeft.Position.Y < sB.elbowLeft.Position.Y
                && Math.Abs(sB.wristLeft.Position.Y - sB.elbowLeft.Position.Y) > 0.10

                && sB.handRight.Position.Y < sB.elbowRight.Position.Y
                && Math.Abs(sB.handRight.Position.Y - sB.elbowRight.Position.Y) > 0.10

                && sB.handLeft.Position.X > sB.elbowLeft.Position.X
                && Math.Abs(sB.handLeft.Position.X - sB.elbowLeft.Position.X) > 0.05

                && sB.handRight.Position.X < sB.elbowRight.Position.X
                && Math.Abs(sB.handRight.Position.X - sB.elbowRight.Position.X) > 0.05

                && sB.handLeft.Position.X > sB.kneeLeft.Position.X
                && sB.handLeft.Position.X < sB.kneeRight.Position.X

                && sB.handRight.Position.X < sB.kneeRight.Position.X
                && sB.handRight.Position.X > sB.kneeLeft.Position.X
                )
            {
                if (Math.Abs(sB.footLeft.Position.X - sB.footRight.Position.X) > 0.25)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
