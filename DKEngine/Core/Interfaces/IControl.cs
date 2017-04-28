namespace DKEngine
{
    public interface IControl
    {
        bool IsFocused { get; set; }
        int FocusElementID { get; }
    }
}
