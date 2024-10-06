using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr3
{
  // Задача
  public class Task
  {
    private string title;
    private string description;
    private string status;
    private string responsible;
    private DateTime dueDate;

    public Task(string title, string description, string responsible, DateTime dueDate)
    {
      this.title = title;
      this.description = description;
      this.responsible = responsible;
      this.dueDate = dueDate;
      this.status = "Не выполнено";
    }

    public void ChangeStatus(string newStatus)
    {
      this.status = newStatus;
    }

    public void DisplayTaskInfo()
    {
      Console.WriteLine($"Задача: {this.title}");
      Console.WriteLine($"Описание: {this.description}");
      Console.WriteLine($"Ответственный: {this.responsible}");
      Console.WriteLine($"Срок выполнения: {this.dueDate.ToShortDateString()}");
      Console.WriteLine($"Статус: {this.status}");
      Console.WriteLine();
    }

    public bool CheckResponsible(string responsible)
    {
      return this.responsible == responsible;
    }
  }

  // Проект
  public class Project
  {
    private string name;
    private List<Task> tasks;

    public Project(string name)
    {
      this.name = name;
      this.tasks = new List<Task>();
    }

    public void AddTask(Task task)
    {
      this.tasks.Add(task);
    }

    public void DisplayAllTasks()
    {
      Console.WriteLine($"Проект: {this.name}");
      foreach (var task in this.tasks)
      {
        task.DisplayTaskInfo();
      }
    }

    public void DisplayTasksByResponsible(string responsible)
    {
      Console.WriteLine($"Задачи для специалиста: {responsible}");
      foreach (var task in this.tasks)
      {
        if (task.CheckResponsible(responsible))
        {
          task.DisplayTaskInfo();
        }
      }
    }
  }

  // Отдел закупок
  public class ProcurementDepartment
  {
    private List<Project> projects;

    public ProcurementDepartment()
    {
      this.projects = new List<Project>();
    }

    public void AddProject(Project project)
    {
      this.projects.Add(project);
    }

    public void DisplayAllProjects()
    {
      foreach (var project in this.projects)
      {
        project.DisplayAllTasks();
      }
    }

    public void DisplayTasksForResponsible(string responsible)
    {
      foreach (var project in this.projects)
      {
        project.DisplayTasksByResponsible(responsible);
      }
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      ProcurementDepartment department = new ProcurementDepartment();

      // Добавляем примерный проект
      Project project1 = new Project("Закупка офисной техники");
      department.AddProject(project1);

      // Добавляем задачи в проект
      Task task1 = new Task("Закупка компьютеров", "Закупить 10 компьютеров", "Иванов Иван", new DateTime(2024, 10, 15));
      Task task2 = new Task("Закупка принтеров", "Закупить 5 принтеров", "Петров Петр", new DateTime(2024, 11, 20));

      project1.AddTask(task1);
      project1.AddTask(task2);

      // Отображение всех задач
      Console.WriteLine("Все задачи по проектам:");
      department.DisplayAllProjects();

      // Изменяем статус задачи
      task1.ChangeStatus("Выполнено");

      // Отображение задач по ответственному
      Console.WriteLine("Задачи для Иванова Ивана:");
      department.DisplayTasksForResponsible("Иванов Иван");

      // Пример добавления новой задачи
      Task task3 = new Task("Закупка бумаги", "Закупить бумагу для принтера", "Сидоров Сидор", new DateTime(2024, 12, 5));
      project1.AddTask(task3);

      // Отображение обновленного списка задач
      Console.WriteLine("Обновленный список всех задач:");
      department.DisplayAllProjects();
    }
  }
}
