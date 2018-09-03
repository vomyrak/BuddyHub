using System.Reflection;
using System;
using System.IO;
using System.Dynamic;

namespace Test {
    public class Programme
    {
        public static void Main(string[] args)
        {
            var dll = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\TestClass.dll");
            Type type = dll.GetType(dll.GetName().Name + ".Adder");
            dynamic adder = Activator.CreateInstance(type, new object[] { 1 });
            dynamic result = type.GetMethod("Add").Invoke(adder, new object[] { 4 });
            Console.ReadKey();
        }
    }
}
