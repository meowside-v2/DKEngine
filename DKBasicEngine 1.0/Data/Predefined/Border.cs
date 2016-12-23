namespace DKBasicEngine_1_0
{
    public class Border : GameObject
    {
        public Border(Scene ToParentToAdd, I3Dimensional Parent)
            :base(ToParentToAdd, Parent)
        {
            this.TypeName = "border";
        }
    }
}
