using System;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    public class WaveRightSegment1 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            
            if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ElbowRight].Position.Y)
            {
               
                if (skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ElbowRight].Position.X)
                {
                    return GesturePartResult.Success;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Undetermined;
            }

            // Hand dropped - no gesture fails
            return GesturePartResult.Fail;
        }
    }

    public class WaveRightSegment2 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            
            if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ElbowRight].Position.Y)
            {
                
                if (skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.ElbowRight].Position.X)
                {
                    return GesturePartResult.Success;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Undetermined;
            }

            // Hand dropped - no gesture fails
            return GesturePartResult.Fail;
        }
    }
}
