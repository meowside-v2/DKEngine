namespace DKBasicEngine_1_0
{
    public interface IGraphics
    {
        bool IsGUI { get; set; }
        bool HasShadow { get; set; }
        Material Model { get; set; }
        Animator Animator { get; set; }
    }
}
