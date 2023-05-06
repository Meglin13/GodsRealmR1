public interface ISingle
{
    static ISingle Instance { get; private set; }
    void Initialize();
}