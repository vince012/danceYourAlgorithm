using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Segments
{
    class HandUpSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            SkeletonB sB = new SkeletonB(skeleton);
            if(Math.Abs(sB.footRight.Position.Y - sB.footLeft.Position.Y) < 0.08)
            {
                if (sB.handLeft.Position.Y > sB.head.Position.Y + 0.15 &&
                    sB.handRight.Position.Y < sB.waist.Position.Y)
                {
                    if((sB.handLeft.Position.X - sB.head.Position.X) < 0.30)
                    {
                        return GesturePartResult.Success;
                    }
                    return GesturePartResult.Undetermined;
                }
                else if (sB.handRight.Position.Y > sB.head.Position.Y + 0.15 &&
                        sB.handLeft.Position.Y < sB.waist.Position.Y)
                {
                    if((sB.head.Position.X - sB.handRight.Position.X) < 0.30){
                        return GesturePartResult.Success;
                    }
                    return GesturePartResult.Undetermined;
                }
            }
            return GesturePartResult.Fail;
        }
    }
}
