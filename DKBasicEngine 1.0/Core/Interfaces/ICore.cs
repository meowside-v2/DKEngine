namespace DKBasicEngine_1_0
{
    public interface ICore
    {
        EmptyGameObject Parent { get; }
        
        void Start();
        void Update();
        void Destroy();
    }
}
