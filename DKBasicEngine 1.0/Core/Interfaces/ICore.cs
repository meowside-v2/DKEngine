namespace DKBasicEngine_1_0
{
    public interface ICore
    {
        I3Dimensional Parent { get; }

        void Render();
        void Start();
        void Update();
        void Destroy();
    }
}
