namespace DKBasicEngine_1_0
{
    public class Border : GameObject
    {
        public Border()
        {
            /*this.Animator.Animations.Add("default", new AnimationNode("default", Database.GetGameObjectMaterial("border")));
            this.Animator.Play("default");*/
            this.TypeName = "border";
        }

        public Border(GameObject Parent)
            : base(Parent)
        {
            /*this.Animator.Animations.Add("default", new AnimationNode("default", Database.GetGameObjectMaterial("border")));
            this.Animator.Play("default");*/
            this.TypeName = "border";
        }
    }
}
