using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClass
{
    class Adder
    {
        private int storedVal;
        
        public Adder()
        {
            storedVal = 0;
        }

        public Adder(int val)
        {
            storedVal = val;
        }

        public int Add(int val)
        {
            int result = storedVal + val;
            Console.WriteLine(result);
            return result;
        }
    }
}
