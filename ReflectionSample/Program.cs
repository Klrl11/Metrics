using System.Reflection;

namespace ReflectionSample
{

    public class SampleClass
    {

        public int Prop1 { get; set; }

        public string Prop2 { get; set; }

        public void PrintValue(int a)
        {
            Console.WriteLine(a);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SampleClass sample1 = new SampleClass();
            SampleClass sample2 = new SampleClass();
            SampleClass sample3 = new SampleClass();


            Type t1 = typeof(SampleClass);
            Type t2 = sample2.GetType();

            PropertyInfo[] properties =  t1.GetProperties();
            MethodInfo[] methods =  t1.GetMethods();
            MethodInfo methodInfo =  t1.GetMethod("PrintValue");

            methodInfo.Invoke(sample2, new object[] { 35});

        }
    }
}