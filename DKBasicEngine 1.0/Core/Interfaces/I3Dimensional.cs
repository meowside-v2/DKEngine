namespace DKBasicEngine_1_0
{
    public interface _I3Dimensional
    {
        Dimensions Dimensions { get; set; }
        float width { get; }
        float height { get; }
        float depth { get; }

        Transform Transform { get; set; }
        float X { get; }
        float Y { get; }
        float Z { get; }

        Scale Scale { get; set; }
        float ScaleX { get; }
        float ScaleY { get; }
        float ScaleZ { get; }

        bool LockScaleRatio { get; set; }
    }
}
