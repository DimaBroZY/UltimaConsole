namespace BestConsole
{
    public interface IPlugin
    {
        string CommandName { get; }
        string Description { get; }
        void Run(string[] args);
    }
}
