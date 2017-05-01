using DKEngine;

namespace DKEngine_Tester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Engine.Init();

            Engine.ChangeScene<Test>();
        }
    }
}