using System;
using Microsoft.Kinect;

namespace ConsoleApp1.Segments
{
    public class WaveLeftSegment1 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            
            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ElbowLeft].Position.Y)
            {
               
                if (skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.ElbowLeft].Position.X)
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

    public class WaveLeftSegment2 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            
            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ElbowLeft].Position.Y)
            {
                
                if (skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ElbowLeft].Position.X)
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
