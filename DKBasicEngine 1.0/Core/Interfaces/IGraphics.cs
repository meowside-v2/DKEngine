namespace DKBasicEngine_1_0
{
    public interface IGraphics
    {
        bool IsGUI { get; set; }
        bool HasShadow { get; set; }
        int AnimationState { get; set; }
        Material modelBase { get; set; }
        Material modelRastered { get; }
    }
}
