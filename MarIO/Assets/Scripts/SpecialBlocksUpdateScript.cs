using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Scripts
{
    public class SpecialBlocksUpdateScript : Script
    {
        public SpecialBlocksUpdateScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
        }

        protected override void Update()
        {
            while (Shared.AnimatedWorldReferences.SpecialActions.Count > 0)
            {
                Shared.AnimatedWorldReferences.SpecialActions.Pop().SpecialAction();
            }
        }
    }
}