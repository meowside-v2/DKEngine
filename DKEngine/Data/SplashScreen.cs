using DKEngine.Core;
using DKEngine.Core.Components;

namespace DKEngine
{
    internal sealed class SplashScreen : GameObject
    {
        public SplashScreen()
        {
            this.TypeName = "splashScreen";
            this.InitNewComponent<Animator>();
        }

        public SplashScreen(GameObject Parent)
            : base(Parent)
        {
            this.TypeName = "splashScreen";
            this.InitNewComponent<Animator>();
        }

        protected override void Initialize()
        { }
    }
}