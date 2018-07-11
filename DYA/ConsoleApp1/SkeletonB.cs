using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
namespace ConsoleApp1
{
    public class SkeletonB
    {
        public Joint head;
        public Joint neck;
        public Joint shoulderLeft;
        public Joint shoulderRight;
        public Joint elbowLeft;
        public Joint elbowRight;
        public Joint wristLeft;
        public Joint wristRight;
        public Joint handLeft;
        public Joint handRight;
        public Joint spine;
        public Joint waist;
        public Joint hipLeft;
        public Joint hipRight;
        public Joint kneeLeft;
        public Joint kneeRight;
        public Joint ankleLeft;
        public Joint ankleRight;
        public Joint footLeft;
        public Joint footRight;

        public SkeletonB(Skeleton skeleton)
        {
            head = skeleton.Joints[JointType.Head];
            neck = skeleton.Joints[JointType.ShoulderCenter];
            shoulderLeft = skeleton.Joints[JointType.ShoulderLeft];
            shoulderRight = skeleton.Joints[JointType.ShoulderRight];
            elbowLeft = skeleton.Joints[JointType.ElbowLeft];
            elbowRight = skeleton.Joints[JointType.ElbowRight];
            wristLeft = skeleton.Joints[JointType.WristLeft];
            wristRight = skeleton.Joints[JointType.WristRight];
            handLeft = skeleton.Joints[JointType.HandLeft];
            handRight = skeleton.Joints[JointType.HandRight];
            spine = skeleton.Joints[JointType.Spine];
            waist = skeleton.Joints[JointType.HipCenter];
            hipLeft = skeleton.Joints[JointType.HipLeft];
            hipRight = skeleton.Joints[JointType.HipRight];
            kneeLeft = skeleton.Joints[JointType.KneeLeft];
            kneeRight = skeleton.Joints[JointType.KneeRight];
            ankleLeft = skeleton.Joints[JointType.AnkleLeft];
            ankleRight = skeleton.Joints[JointType.AnkleRight];
            footLeft = skeleton.Joints[JointType.FootLeft];
            footRight = skeleton.Joints[JointType.FootRight];
        }

        public static Double distance(Joint p1, Joint p2)
        {
            var x = p1.Position.X - p2.Position.X;
            var y = p1.Position.Y - p2.Position.Y;
            var z = p1.Position.Z - p2.Position.Z;

            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }

        /*public static Double distance(Joint p1, Joint p2, double supp)
        {
            var x = p1.Position.X - p2.Position.X + supp/2;
            var y = p1.Position.Y - p2.Position.Y;
            var z = p1.Position.Z - p2.Position.Z;

            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }*/
        
        public static Double distanceVertical(Joint p1, Joint p2)
        {
            //var x = p1.Position.X - p2.Position.X + suppX/2;
            var x = 0;
            var y = p1.Position.Y - p2.Position.Y;
            var z = p1.Position.Z - p2.Position.Z;

            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }

        public static Double distance(params Joint[] tab)
        {
            double height = 0;
            for(int i = 0; i < tab.Length - 1; i++)
            {
                height += distance(tab[i], tab[i + 1]);
            }
            return height;
        }

        public Double getHeight()
        {
            return distance(head, neck, spine, waist);
        }

        public Double getAllHeight()
        {
            double height = distance(head, neck, spine, waist);

            /*double hip = distance(hipRight, hipLeft);
            double knee = distance(kneeRight, kneeLeft);
            double ankle = distance(ankleRight, ankleLeft);*/

            height += distanceVertical(waist, hipRight);
            height += distanceVertical(hipRight, kneeRight);
            height += distanceVertical(kneeRight, ankleRight);
            height += distanceVertical(ankleRight, footRight);

            return height;
        }
    }
}
