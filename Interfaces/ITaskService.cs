namespace Task_CLI.Interfaces
{
    public interface ITaskService
    {
        List<string> GetAllHelpCommands();
        Task<int> AddNewTask(string description);
    }
}