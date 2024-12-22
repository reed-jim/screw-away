using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

public class GameTaskManager : MonoBehaviour
{
    private BaseWeeklyTask[] _tasks;

    public BaseWeeklyTask[] Tasks
    {
        get => _tasks;
    }

    private void Awake()
    {
        _tasks = LoadTasks();

        PeriodicallySaving();
    }

    private async void PeriodicallySaving()
    {
        while (gameObject.activeSelf)
        {
            await Task.Delay(5000);

            SaveTasks();
        }
    }

    private BaseWeeklyTask[] LoadTasks()
    {
        BaseWeeklyTask[] tasks = DataUtility.Load(GameConstants.SAVE_FILE_NAME, GameConstants.WEEKLY_TASKS, new BaseWeeklyTask[2]);

        if (tasks[0] == null)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = GenerateTask();
            }
        }

        return tasks;
    }

    public void SaveTasks()
    {
        DataUtility.Load(GameConstants.SAVE_FILE_NAME, GameConstants.WEEKLY_TASKS, _tasks);
    }

    private BaseWeeklyTask GenerateTask()
    {
        BaseWeeklyTask task;

        TaskType randomTaskType = GetRandomEnumValue<TaskType>();

        if (randomTaskType == TaskType.Uncrew)
        {
            task = new UnscrewTask();

            task.SetRequirement(Random.Range(50, 300));
        }
        else
        {
            task = new CompleteLevelTask();

            task.SetRequirement(Random.Range(3, 8));
        }

        return task;
    }
}
