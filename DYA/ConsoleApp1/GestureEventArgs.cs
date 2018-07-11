using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class GestureEventArgs : EventArgs
    {
        public string Name {get; private set;}

        public int TrackingId {get; private set;}

        public GestureEventArgs(string name, int trackingId)
        {
            Name = name;
            TrackingId = trackingId;
        }
    }
}
