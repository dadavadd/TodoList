# ✔TodoList Console App

A simple and interactive console-based Todo list application built with .NET and C#.

![Application Screenshot](https://github.com/user-attachments/assets/876f03bd-6a1a-41ed-91b1-ea09c086fb9d) 

## Description

This application allows users to manage their tasks from the command line. Users can add new tasks, mark them as completed, view the task list in a formatted table, and delete tasks. The data is persisted in a `todoConfig.json` file.

## Features

- **View Todos**: Displays all tasks in a table with ID, Title, Description, Completion Status, and timestamps.
- **Add Todo**: Add a new task with a title and description.
- **Mark as Completed**: Mark an existing task as completed.
- **Delete Todo**: Remove a task from the list.
- **Data Persistence**: Changes are automatically saved to `todoConfig.json` when the application exits.

## Built With

- **[.NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)** - The framework used to build the application.
- **[Spectre.Console](https://spectreconsole.net/)** - A library for creating beautiful, interactive console applications.
- **[xUnit](https://xunit.net/)** - The testing framework.

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

### Installation and Running

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/dadavadd/TodoList.git
    cd TodoList
    ```

2.  **Restore dependencies:**
    ```sh
    dotnet restore
    ```

3.  **Build the project:**
    ```sh
    dotnet build --configuration Release
    ```

4.  **Run the application:**
    ```sh
    dotnet run --project TodoList
    ```

## Usage

Once the application is running, you will be presented with a menu with the following options:
- `Add`: Add a new todo item.
- `Mark as completed`: Mark a todo as completed.
- `Delete`: Delete a todo.
- `Exit`: Exit the application and save changes.

Use the arrow keys to navigate the menu and `Enter` to select an option.

## Testing

To run the tests, execute the following command in the root directory of the project:

```sh
dotnet test
```
