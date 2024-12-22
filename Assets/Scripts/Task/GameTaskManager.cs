using System.Collections;
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

        InitTasks();

        StartCoroutine(PeriodicallySaving());
    }

    private IEnumerator PeriodicallySaving()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(2);

        while (true)
        {
            yield return waitForSeconds;

            SaveTasks();
        }
    }

    private void InitTasks()
    {
        for (int i = 0; i < _tasks.Length; i++)
        {
            _tasks[i].Init();
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
        DataUtility.SaveAsync(GameConstants.SAVE_FILE_NAME, GameConstants.WEEKLY_TASKS, _tasks);
    }

    private BaseWeeklyTask GenerateTask()
    {
        BaseWeeklyTask task;

        TaskType randomTaskType = GetRandomEnumValue<TaskType>();

        if (randomTaskType == TaskType.Uncrew)
        {
            task = new UnscrewTask();

            task.RequirementValue = Random.Range(50, 300);
        }
        else
        {
            task = new UnscrewTask();

            task.RequirementValue = Random.Range(3, 8);
        }

        return task;
    }
}
