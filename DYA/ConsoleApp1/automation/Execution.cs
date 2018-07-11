using ConsoleApp1.automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Execution
    {
        public List<State> state_list = new List<State>();

        public void AddState(string name)
        {
            if (!name.Equals(""))
            {
                if (state_list.Count == 0)
                {
                    state_list.Add(new State(name));
                }
                else
                {
                    state_list.Add(new State(name));
                    state_list[state_list.Count - 1].parent = state_list[state_list.Count - 2];
                    state_list[state_list.Count - 2].next = state_list[state_list.Count - 1];
                }
            }            
        }

        public void Accept(Visitor visitor)
        {
            foreach (State state in state_list)
            {
                state.Accept(visitor);
            }
        }
    }
}
