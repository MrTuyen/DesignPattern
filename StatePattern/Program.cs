using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context();

            StartState startState = new StartState();
            startState.doAction(context);

            Console.WriteLine(context.getState().toString());

            StopState stopState = new StopState();
            stopState.doAction(context);

            Console.WriteLine(context.getState().toString());

            Console.ReadLine();
        }
    }

    public interface State
    {
        void doAction(Context context);

        String toString();
    }

    public class StartState : State {

       public void doAction(Context context) {
          Console.WriteLine("Player is in start state");
          context.setState(this);	
       }

       public String toString(){
          return "Start State";
       }
    }

    public class StopState : State {

       public void doAction(Context context) {
           Console.WriteLine("Player is in stop state");
           context.setState(this);	
       }

       public String toString(){
          return "Stop State";
       }
    }

    public class Context
    {
        private State state;

        public Context()
        {
            state = null;
        }

        public void setState(State state)
        {
            this.state = state;
        }

        public State getState()
        {
            return state;
        }
    }
}
