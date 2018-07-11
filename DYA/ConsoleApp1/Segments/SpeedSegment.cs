using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    class SpeedSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);

            if (sB.footRight.Position.Z < sB.footLeft.Position.Z
                && Math.Abs(sB.footLeft.Position.Z - sB.footRight.Position.Z) > 0.30
                && Math.Abs(sB.footLeft.Position.X - sB.footRight.Position.X) < 0.20
                && Math.Abs(sB.footLeft.Position.Y - sB.footRight.Position.Y) < 0.10

                && sB.handRight.Position.Z < sB.handLeft.Position.Z
                && Math.Abs(sB.handLeft.Position.Z - sB.handRight.Position.Z) > 0.60
                && sB.handLeft.Position.X < sB.neck.Position.X
                && sB.handRight.Position.X > sB.neck.Position.X
                && Math.Abs(sB.handLeft.Position.X - sB.handRight.Position.X) < 0.60

                && sB.handLeft.Position.Y < sB.neck.Position.Y
                && sB.handRight.Position.Y > sB.neck.Position.Y)
            {
                if (Math.Abs(sB.footLeft.Position.Z - sB.footRight.Position.Z) > 0.40
                    && Math.Abs(sB.handLeft.Position.Z - sB.handRight.Position.Z) > 0.90)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
