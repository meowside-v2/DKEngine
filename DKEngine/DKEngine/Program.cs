using DKEngine;

namespace DKEngine_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Init();
            
            Engine.ChangeScene<Test>();
        }
    }
}
