# Task-CLI

Project Task URL : https://roadmap.sh/projects/task-tracker

This is .NET 8 console app for the task-tracker [challenge](https://roadmap.sh/projects/task-tracker) from [roadmap.sh](https://roadmap.sh/).

You can perform basic CRUD operations on tasks as well as filter on three different statuses : in-progress, todo and done.

## Features

+ Add new task
+ Update task
+ Delete task
+ List tasks
+ Change task status: in-progress, todo or done.
+ Display list of all commands
+ Clear console

## Installation
```
https://github.com/khartoum-cyber/Task-CLI.git
```
```
dotnet run
```

## HowTo's

After welcome message, start interacting with application using commands:

+ help: Displays a list of all available commands.
+ add [description]: Adds a new task with the provided description.
+ update [id] [new description]: Updates the task with the given ID.
+ delete [id]: Deletes the task with the given ID.
+ list: Lists all tasks.
+ list [status]: Lists tasks filtered by status ("todo", "in-progress", "done").
+ mark-todo [id]: Marks the task with the given ID as "To-Do".
+ mark-in-progress [id]: Marks the task with the given ID as "In-Progress".
+ mark-done [id]: Marks the task with the given ID as "Done".
+ clear: Clears the console.
+ exit: Exits the application.

```
add "Learn C#"
Task added successfully with Id : 1

add "Learn EF Core"
Task added successfully with Id : 2

list
Task Id          Description                         Status          Created Date    
1                Learn C#                            todo            22-10-2024
2                Learn EF Core                       todo            22-10-2024

mark-inprogress 2
Task status set successfully with Id : 2

list in-progress
Task Id          Description                         Status          Created Date   
2                Learn EF Core                       in-progress     22-10-2024

exit
```
