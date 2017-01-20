namespace DKBasicEngine_1_0
{
    public class Border : GameObject
    {
        public Border()
        {
            this.TypeName = "border";
        }

        public Border(GameObject Parent)
            : base(Parent)
        {
            this.TypeName = "border";
        }
    }
}
