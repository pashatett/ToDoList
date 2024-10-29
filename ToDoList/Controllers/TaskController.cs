using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Service.Interfaces;

namespace ToDoList.Controllers;

public class TaskController : Controller
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Progress()
    {
        var tasks = await _taskService.GetAllTasks();
        return View(tasks); // Передаем задачи в представление
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskViewModel model)
    {
        var response = await _taskService.Create(model);
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsDone(int id)
    {
        var task = await _taskService.GetTaskById(id); // Предполагается, что метод существует
        if (task == null)
        {
            return NotFound(new { description = "Задача не найдена" });
        }

        task.IsDone = true; // Обновляем статус задачи
        await _taskService.Update(task); // Метод обновления задачи в сервисе

        return Ok(new { description = "Задача завершена" });
    }


}