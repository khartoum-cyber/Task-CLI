﻿using System.Text.Json;
using Task_CLI.Enums;
using Task_CLI.Interfaces;
using Task_CLI.Models;

namespace Task_CLI.Services
{
    public class TaskService : ITaskService
    {
        private static readonly string FileName = "cli_task_data.json";

        private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

        public Task<int> AddNewTask(string description)
        {
            try
            {
                var appTasks = new List<CliTask>();
                var task = new CliTask
                {
                    Id = GetTaskId(),
                    Description = description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    TaskStatus = Status.ToDo
                };

                var fileCreatedSuccessfully = CreateFileIfNotExist();

                if (fileCreatedSuccessfully)
                {
                    string tasksFromJsonFileString = File.ReadAllText(FilePath);
                    if (!string.IsNullOrEmpty(tasksFromJsonFileString))
                    {
                        appTasks = JsonSerializer.Deserialize<List<CliTask>>(tasksFromJsonFileString);
                    }

                    appTasks?.Add(task);
                    var updatedAppTasks = JsonSerializer.Serialize(appTasks ?? new List<CliTask>());
                    File.WriteAllText(FilePath, updatedAppTasks);
                    return Task.FromResult(task.Id);
                }

                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Task addition failed. Error - " + ex.Message);
                return Task.FromResult(0);
            }
        }

        public Task<bool> UpdateTask(int id, string description)
        {
            if (!File.Exists(FilePath))
            {
                return Task.FromResult(false);
            }

            var tasksFromJson = GetTasksFromJson();

            if (tasksFromJson.Result.Count > 0)
            {
                var taskToBeUpdated = tasksFromJson.Result
                    .SingleOrDefault(x => x.Id == id);

                if (taskToBeUpdated != null)
                {
                    var updatedTask = new CliTask
                    {
                        Id = id,
                        Description = description,
                        CreatedAt = taskToBeUpdated.CreatedAt,
                        UpdatedAt = DateTime.UtcNow,
                        TaskStatus = taskToBeUpdated.TaskStatus
                    };

                    tasksFromJson.Result.Remove(taskToBeUpdated);
                    tasksFromJson.Result.Add(updatedTask);
                    UpdateJsonFile(tasksFromJson);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
        public Task<bool> DeleteTask(int id)
        {
            if (!File.Exists(FilePath))
            {
                return Task.FromResult(false);
            }

            var tasksFromJson = GetTasksFromJson();

            if (tasksFromJson.Result.Count > 0)
            {
                var taskToBeDeleted = tasksFromJson.Result
                    .SingleOrDefault(x => x.Id == id);

                if (taskToBeDeleted != null)
                {
                    tasksFromJson.Result.Remove(taskToBeDeleted);
                    UpdateJsonFile(tasksFromJson);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
        public Task<List<CliTask>> ListAllTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return Task.FromResult(new List<CliTask>());
                }

                string jsonString = File.ReadAllText(FilePath);

                if (!string.IsNullOrEmpty(jsonString))
                {
                    List<CliTask> tasks = JsonSerializer.Deserialize<List<CliTask>>(jsonString);
                    return Task.FromResult(tasks);
                }

                return Task.FromResult(new List<CliTask>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<bool> SetTaskStatus(string status, int id)
        {
            if (!File.Exists(FilePath))
            {
                return Task.FromResult(false);
            }

            var tasksFromJson = GetTasksFromJson();

            if (tasksFromJson.Result.Count > 0)
            {
                var statusToBeUpdated = tasksFromJson.Result
                    .SingleOrDefault(x => x.Id == id);

                if (statusToBeUpdated != null)
                {
                    var updatedTask = new CliTask
                    {
                        Id = id,
                        Description = statusToBeUpdated.Description,
                        CreatedAt = statusToBeUpdated.CreatedAt,
                        UpdatedAt = DateTime.UtcNow,
                        TaskStatus = GetStatusToDisplay(status)
                    };

                    tasksFromJson.Result.Remove(statusToBeUpdated);
                    tasksFromJson.Result.Add(updatedTask);
                    UpdateJsonFile(tasksFromJson);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
        public Task<List<CliTask>> GetTaskByStatus(string status)
        {
            
            
            if (!File.Exists(FilePath))
            {
                return Task.FromResult(new List<CliTask>());
            }

            string jsonString = File.ReadAllText(FilePath);

            if (!string.IsNullOrEmpty(jsonString))
            {
                var tasks = JsonSerializer.Deserialize<List<CliTask>>(jsonString);
                var statusToCheck = ChangeStatus(status);
                return Task.FromResult(tasks?.Where(x => x.TaskStatus == statusToCheck).ToList() ??
                                       new List<CliTask>());
            }

            return Task.FromResult(new List<CliTask>());
        }

        private Status GetStatusToDisplay(string status) => status switch
        {
            "mark-in-progress" => Status.InProgress,
            "mark-done" => Status.Done,
            "mark-todo" => Status.ToDo,
            _ => Status.ToDo
        };

        private Status ChangeStatus(string status) => status switch
        {
            "in-progress" => Status.InProgress,
            "done" => Status.Done,
            "todo" => Status.ToDo
        };

        private static void UpdateJsonFile(Task<List<CliTask>> tasksFromJson)
        {
            string updatedAppTasks = JsonSerializer.Serialize(tasksFromJson.Result);
            File.WriteAllText(FilePath, updatedAppTasks);
        }

        private static Task<List<CliTask>> GetTasksFromJson()
        {
            string tasksFromJsonFileString = File.ReadAllText(FilePath);

            if (!string.IsNullOrEmpty(tasksFromJsonFileString))
            {
                return Task.FromResult(JsonSerializer.Deserialize<List<CliTask>>(tasksFromJsonFileString));
            }

            return Task.FromResult(new List<CliTask>());
        }

        private int GetTaskId()
        {
            if (!File.Exists(FilePath))
            {
                return 1;
            }

            string tasksFromJsonFileString = File.ReadAllText(FilePath);

            if (!string.IsNullOrEmpty(tasksFromJsonFileString))
            {
                var appTasks = JsonSerializer.Deserialize<List<CliTask>>(tasksFromJsonFileString);
                if (appTasks != null && appTasks.Count > 0)
                {
                    return appTasks.OrderBy(x => x.Id).Last().Id + 1;
                }
            }
            return 1;
        }

        public List<string> GetAllHelpCommands()
        {
            return new List<string>
            {
                "add \"Task Description\" - To add a new task, type add with task description",
                "update \"Task Id\" \"Task Description\" - To update a task, type update with task id and task description",
                "delete \"Task Id\" - To delete a task, type delete with task id",
                "mark-in-progress \"Task Id\" - To mark a task to in progress, type mark-in-progress with task id",
                "mark-done \"Task Id\" - To mark a task to done, type mark-done with task id",
                "list - To list all task with its current status",
                "list done - To list all task with done status",
                "list todo  - To list all task with todo status",
                "list in-progress  - To list all task with in-progress status",
                "exit - To exit from app",
                "clear - To clear console window"
            };
        }

        private static bool CreateFileIfNotExist()
        {
            try
            {
                // Check if the file exists
                if (!File.Exists(FilePath))
                {
                    // Create the file if it does not exist
                    using (FileStream fs = File.Create(FilePath))
                    {
                        Console.WriteLine($"File {FileName} created successfully.");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File {FileName} creation failed. Error - " + ex.Message);
                return false;
            }
        }

    }
}
