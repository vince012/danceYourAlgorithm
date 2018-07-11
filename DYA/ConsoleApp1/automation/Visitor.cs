using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.automation
{
    public abstract class Visitor
    {
        public abstract void VisitState(State state);
    }
}
