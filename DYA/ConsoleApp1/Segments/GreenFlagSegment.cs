using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Segments
{
    class GreenFlagSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if(Math.Abs(sB.elbowRight.Position.Y - sB.neck.Position.Y) <= 0.10 &&
                Math.Abs(sB.elbowLeft.Position.Y - sB.neck.Position.Y) <= 0.10)
            {
                if (Math.Abs(sB.elbowRight.Position.X - sB.handRight.Position.X) <= 0.08 &&
                    Math.Abs(sB.elbowLeft.Position.X - sB.handLeft.Position.X) <= 0.08 &&
                    Math.Abs(sB.handLeft.Position.Y - sB.handRight.Position.Y) <= 0.08)
                {
                    return GesturePartResult.Success;
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Fail;
        }
    }
}
