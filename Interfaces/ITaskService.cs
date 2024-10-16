namespace Task_CLI.Interfaces
{
    public interface ITaskService
    {
        List<string> GetAllHelpCommands();
        Task<int> AddNewTask(string description);
        Task<bool> UpdateTask(int id, string description);
    }
}