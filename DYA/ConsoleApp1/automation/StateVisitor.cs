using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.automation
{
    public class StateVisitor : Visitor
    {
        public override void VisitState(State state)
        {
            // opération de traitement
            Console.WriteLine("{0} visited by {1}",state.name, this.GetType().Name);
        }
    }
}
