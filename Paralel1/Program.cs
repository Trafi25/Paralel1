using System;
using System.Threading;

namespace Paralel1
{
    class Program
    {
        

     
        static void Main(string[] args)
        {
            Runable run = new Runable(5);
            run.setValuses(4, 5);
            Thread thread = new Thread(new ParameterizedThreadStart(run.Operation));
            run.Run();
            

        }
    
    }
}
