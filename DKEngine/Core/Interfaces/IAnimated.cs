namespace DKEngine
{
    public interface IAnimated
    {
        /*AnimationLoop Settings { get; set; }
        int NumberOfPlays { get; set; }
        int AnimationState { get; set; }
        TimeSpan CurrentAnimationTime { get; }*/
    }

    public enum AnimationLoop
    {
        Once,
        Endless
    }
}