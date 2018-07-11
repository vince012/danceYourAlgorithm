using System;
using Microsoft.Kinect;

namespace ConsoleApp1
{
    public class Gesture
    {
        readonly int WINDOW_SIZE = 50; // Max frame per second
        readonly int MAX_PAUSE_COUNT = 10;
        int _currentSegment = 0;
        int _frameCount = 0;
        int _pausedFrameCount = 10;
        
        IGestureSegment[] _segments;

        private static string oldGesture = "null";
        string _name;

        Boolean _paused = false;
        
        public event EventHandler<GestureEventArgs> GestureDetected;       

        public Gesture(String name, IGestureSegment[] segments)
        {
            _name = name;
            _segments = segments;
        }

        public void Update(Skeleton skeleton)
        {
            if (_paused)
            {
                if (_frameCount == _pausedFrameCount) _paused = false;
                _frameCount++;
            }

            GesturePartResult result = _segments[_currentSegment].Update(skeleton);

            if (result == GesturePartResult.Success)
            {
                if (_currentSegment + 1 < _segments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                    _pausedFrameCount = MAX_PAUSE_COUNT;
                    _paused = true;
                }
                else
                {
                    if (GestureDetected != null)
                    {
                        var sB = new SkeletonB(skeleton);
                        var height = sB.getHeight();
                        var fullHeight = sB.getAllHeight();

                        if (height > 0.40)
                        {
                            if (Gesture.oldGesture.Equals(_name)) return;
                            Console.WriteLine("Gesture detected with height: {0}, oldGesture : {1}", height, Gesture.oldGesture);
                            GestureDetected(this, new GestureEventArgs(_name, skeleton.TrackingId));
                            Gesture.oldGesture = _name;
                            Console.WriteLine(Gesture.oldGesture);

                        }
                        else
                            Console.WriteLine("ERROR: Gesture detected with height: {0}", height);
                        Reset();
                    }
                    else
                    {
                        Console.WriteLine("Unknown gesture");
                        Gesture.oldGesture = "null";
                    }
                }
            }
            else if (result == GesturePartResult.Fail || _frameCount == WINDOW_SIZE)
            {
                if(_frameCount == WINDOW_SIZE)
                {
                    Console.WriteLine("frameCount out OU Gesture Failed, seg = {0}, frame = {1}", _currentSegment, _frameCount);
                    Gesture.oldGesture = "null";
                }
                if (_frameCount >= WINDOW_SIZE/2)
                    Gesture.oldGesture = "null";
                Reset();              
            }
            else
            {
                _frameCount++;
                _pausedFrameCount = MAX_PAUSE_COUNT / 2;
                _paused = true;
            }
        }

        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
        }

        public string getName()
        {
            return this._name;
        }
    }
}
