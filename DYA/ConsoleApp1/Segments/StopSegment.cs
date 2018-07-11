using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
namespace ConsoleApp1.Segments
{
    class StopSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (Math.Abs(sB.shoulderLeft.Position.Y - sB.neck.Position.Y) <= 0.10 &&
                Math.Abs(sB.shoulderRight.Position.Y - sB.neck.Position.Y) <= 0.10)
            {
                if (Math.Abs(sB.elbowRight.Position.X - sB.shoulderRight.Position.X) <= 0.06 &&
                    Math.Abs(sB.elbowLeft.Position.X - sB.shoulderLeft.Position.X) <= 0.06 &&
                    Math.Abs(sB.handLeft.Position.Y - sB.handRight.Position.Y) <= 0.05 &&
                    Math.Abs(sB.handLeft.Position.X - sB.handRight.Position.X) <= 0.08
                    && Math.Abs(sB.kneeLeft.Position.X - sB.kneeRight.Position.X) < 0.10)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
