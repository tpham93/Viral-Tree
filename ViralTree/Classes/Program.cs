using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine(createExp(10));
            
            Logger.Create(Logger.ELoggerType.Console);

            TestClass.testAll();

            using (Game g = new Game())
                g.Run();
            
            Logger.Close();
             
        }

        private static Random random = new Random();

        private static string createExp(int max)
        {
            return createIfExp(1, max);
        }

        private static string getCommandOrIf(int d, int m)
        {
            if (random.NextDouble() <= 0.5)
                return getCommand(d + 1, m);

            else
                return createIfExp(d + 1, m);
        }

        private static string getSensorOrOperator(int d, int m)
        {
            if (random.NextDouble() <= 0.5)
                return getSensor(d + 1, m);

            else
                return getOperator(d + 1, m);
        }

        private static string getCommand(int d, int m)
        {
            double rand = random.NextDouble();

            if (rand <= 0.25)
                return "north";

            else if (rand <= 0.5)
                return "east";

            else if (rand <= 0.75)
                return "south";

            else
                return "west";
        }

        private static string getSensor(int d, int m)
        {
            double rand = random.NextDouble();
            double delta = 1.0 / 8.0;

            for (int i = 1; i <= 8; i++)
            {
                if (rand <= delta * i)
                    return "s" + (i - 1);
            }

            return "Error";
            
        }

        private static string getOperator(int d, int m)
        {
            double rand = random.NextDouble();

            if (rand <= 0.33)
                return "(or " + getSensorOrOperator(d + 1, m) + ", " + getSensorOrOperator(d, m) + ")";

            else if(rand <= 0.66)
                return "(and " + getSensorOrOperator(d + 1, m) + ", " + getSensorOrOperator(d, m) + ")";

            else 
                return "(not " + getSensorOrOperator(d + 1, m) + ")" ;
        }

        private static string createIfExp(int d, int m)
        {
            String tmp = "";
            for (int i = 0; i < d; i++)
                tmp += "    ";
            

            if (d == m)
                return getCommand(d, m);

            else
                return "(if \n" + tmp + getSensorOrOperator(d + 1, m) + ", \n" + tmp  + getCommandOrIf(d + 1, m) + ", \n" + tmp + getCommandOrIf(d + 1, m) + ")";
        }
    }
}
