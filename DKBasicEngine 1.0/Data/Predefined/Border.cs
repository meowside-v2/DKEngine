namespace DKBasicEngine_1_0
{
    public class Border : GameObject
    {
        public Border(Scene ToParentToAdd)
            :base(ToParentToAdd)
        {
            this.TypeName = "border";
        }

        public Border(EmptyGameObject Parent)
            : base(Parent)
        {
            this.TypeName = "border";
        }
    }
}
