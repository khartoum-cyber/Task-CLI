using Task_CLI.Models;

namespace Task_CLI.Interfaces
{
    public interface ITaskService
    {
        List<string> GetAllHelpCommands();
        Task<int> AddNewTask(string description);
        Task<bool> UpdateTask(int id, string description);
        Task<bool> DeleteTask(int id);
        Task<List<CliTask>> ListAllTasks();
        Task<List<CliTask>> GetTaskByStatus(string status);
    }
}