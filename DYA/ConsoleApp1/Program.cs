using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        static Boolean _skeleton = true;
        static GestureController _mainGesture = new GestureController();
        
        static void Main(string[] args)
        {
            var sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();

            if (sensor != null)
            {
                sensor.SkeletonStream.Enable();
                sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;

                _mainGesture.GestureDetected += Gesture_Detected;

                try
                {
                    sensor.Start();
                }
                catch (IOException)
                {
                    sensor = null;
                }

                if (sensor != null)
                    Console.WriteLine("Sensor has just started ...");
                else
                    Console.WriteLine("No Kinect ready");
            }
            else
                Console.WriteLine("No sensor detected");

            Console.ReadKey();
        }

        static void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    Microsoft.Kinect.Skeleton[] skeletons = new Microsoft.Kinect.Skeleton[frame.SkeletonArrayLength];

                    frame.CopySkeletonDataTo(skeletons);

                    if (skeletons.Length > 0)
                    {
                        var user = skeletons.Where(u => u.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();

                        if (user != null)
                        {
                            if (!_skeleton) Console.WriteLine("Skeleton detected");
                            _skeleton = true;
                            _mainGesture.Update(user);
                        }
                        else
                        {
                            if (_skeleton) Console.WriteLine("No Skeleton detected");
                            _skeleton = false;
                        }
                    }
                }
            }
        }

        static void Gesture_Detected(object sender, GestureEventArgs e)
        {
            Console.WriteLine("Mouvement detecté, {0} {1}", e.Name, e.TrackingId);
        }
    }
}
