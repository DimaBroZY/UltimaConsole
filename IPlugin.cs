namespace BestConsole
{
    public interface IPlugin
    {
        string CommandName { get; }
        void Run(string[] args);
    }
}
