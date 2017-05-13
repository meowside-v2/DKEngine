using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System.Diagnostics;

namespace MarIO.Assets.Scripts
{
    public class DeathZoneScript : Script
    {
        public DeathZoneScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            Debug.WriteLine($"{e.Parent}");

            if (e.Parent is AnimatedObject)
            {
                ((AnimatedObject)e.Parent).IsDestroyed = true;
            }
        }

        protected override void Start()
        { }

        protected override void Update()
        { }
    }
}