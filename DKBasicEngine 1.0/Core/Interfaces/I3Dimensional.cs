namespace DKBasicEngine_1_0
{
    public interface I3Dimensional
    {
        float width { get; set; }
        float height { get; set; }
        float depth { get; set; }

        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }

        float ScaleX { get; set; }
        float ScaleY { get; set; }
        float ScaleZ { get; set; }

        bool LockScaleRatio { get; set; }
    }
}
