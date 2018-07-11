using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
namespace ConsoleApp1.Segments
{
    class ShowSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if (Math.Abs(sB.handLeft.Position.Y - sB.elbowLeft.Position.Y) < 0.10
                && Math.Abs(sB.elbowLeft.Position.Y - sB.shoulderLeft.Position.Y) < 0.10
                && Math.Abs(sB.handRight.Position.Y - sB.elbowRight.Position.Y) < 0.10
                && Math.Abs(sB.elbowRight.Position.Y - sB.shoulderRight.Position.Y) < 0.10
                && sB.handLeft.Position.X < sB.kneeLeft.Position.X
                && sB.handRight.Position.X > sB.kneeRight.Position.X)
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
