using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.automation
{
    public class State : Element
    {
        public State next;
        public State parent;
        public string name;

        public State(string name)
        {
            this.name = name;
            this.next = null;
            this.parent = null;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.VisitState(this);
        }

    }
}
