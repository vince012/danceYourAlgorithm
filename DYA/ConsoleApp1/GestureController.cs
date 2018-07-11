using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Segments;

namespace ConsoleApp1
{
    public class GestureController
    {
        private static int NB_FRAME = 25;
        private List<Gesture> _gestures = new List<Gesture>();
        private Boolean greenFlag = false;

        public event EventHandler<GestureEventArgs> GestureDetected;

        /// <summary>
        /// List of statues
        /// </summary>
        private enum Statues
        {
            GreenFlag,
            HandUp,
            HandRight,
            HandLeft,
            HandBottom,
            Reset,
            Jump,
            TurnRight,
            TurnLeft,
            Stop,
            OpenRepeat,
            CloseRepeat,
            Grow,
            Shrink,
            ChangeScene,
            NormalSize,
            Show,
            Hide,
            Speed,
            Wait,
            TouchAnotherSprite,
            WaveRight,
            FingerCount
        };

        public GestureController()
        {
            foreach (Statues name in Enum.GetValues(typeof(Statues)))
            {
                AddStatue(name);
            }
        }

        public void Update(Skeleton skeleton)
        {
            foreach (Gesture gesture in _gestures)
            {
                if (greenFlag && gesture.getName().Equals("GreenFlag")) continue;
                else gesture.Update(skeleton);
            }
        }

        private void AddStatue(Statues name)
        {
            IGestureSegment[] segments = null;
            switch (name)
            {
                case Statues.HandUp:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new HandUpSegment();
                break;

                case Statues.HandRight:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new HandRightSegment();
                break;

                case Statues.HandLeft:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new HandLeftSegment();
                break;

                case Statues.HandBottom:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new HandBottomSegment();
                break;

                case Statues.Reset:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new ResetSegment();
                break;

                case Statues.Jump:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new JumpSegment();
                break;

                case Statues.TurnRight:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new TurnRightSegment();
                break;

                case Statues.TurnLeft:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new TurnLeftSegment();
                break;

                case Statues.OpenRepeat:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new OpenRepeatSegment();
                break;

                case Statues.CloseRepeat:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new CloseRepeatSegment();
                break;

                case Statues.Stop:
                    segments = new IGestureSegment[15];
                    for (int i = 0; i < 15; i++)
                        segments[i] = new StopSegment();
                break;

                case Statues.GreenFlag:
                    segments = new IGestureSegment[15];
                    for (int i = 0; i < 15; i++)
                        segments[i] = new GreenFlagSegment();
                break;

                case Statues.Grow:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new GrowSegment();
                 break;

                case Statues.Shrink:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new ShrinkSegment();
                break;

                case Statues.ChangeScene:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new ChangeSceneSegment();
                break;

                case Statues.NormalSize:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new NormalSizeSegment();
                break;

                case Statues.Show:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new ShowSegment();
                break;

                case Statues.Hide:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new HideSegment();
                break;

                case Statues.Speed:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new SpeedSegment();
                break;

                case Statues.Wait:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new WaitSegment();
                break;

                case Statues.TouchAnotherSprite:
                    segments = new IGestureSegment[NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new TouchAnotherSpriteSegment();
                break;

                case Statues.WaveRight:
                    segments = new IGestureSegment[2*NB_FRAME];
                    for (int i = 0; i < NB_FRAME; i++)
                        segments[i] = new WaveRightSegment1();
                    for (int i = NB_FRAME; i < 2*NB_FRAME; i++)
                        segments[i] = new WaveRightSegment2();
                break;

                case Statues.FingerCount:
                    segments = new IGestureSegment[20];
                    for (int i = 0; i < 20; i++)
                        segments[i] = new FingerCount();
                break;

                default:
                break;
            }

            Gesture gesture = new Gesture(name.ToString(), segments);

            gesture.GestureDetected += OnGestureDetected;

            _gestures.Add(gesture);
        }

        public void AddGesture(string name, IGestureSegment[] segments)
        {
            Gesture gesture = new Gesture(name, segments);
            gesture.GestureDetected += OnGestureDetected;
        }

        private void OnGestureDetected(object sender, GestureEventArgs e)
        {
            if (GestureDetected != null) GestureDetected(this, e);

            foreach (Gesture gesture in _gestures)
                gesture.Reset();
        }

        public void setGreenFlag(Boolean statut)
        {
            this.greenFlag = statut;
        }

        public Boolean getGreenFlag()
        {
            return this.greenFlag;
        }

    }
}
