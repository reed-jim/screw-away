using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeeklyTaskPopup : BasePopup
{
    [SerializeField] private RectTransform taskContainer;
    [SerializeField] private GameTaskManager gameTaskManager;

    protected override void MoreActionInAwake()
    {
        DelaySpawnTasks();
    }

    private async void DelaySpawnTasks()
    {
        await Task.Delay(2000);

        for (int i = 0; i < gameTaskManager.Tasks.Length; i++)
        {
            BaseWeeklyTask task = gameTaskManager.Tasks[i];

            TaskItemUI taskItemUI = ObjectPoolingEverything.GetFromPool<TaskItemUI>(GameConstants.TASK_ITEM_UI);

            taskItemUI.Container.SetParent(taskContainer);

            UIUtil.SetLocalPositionX(taskItemUI.Container, 0);
            UIUtil.SetLocalPositionY(taskItemUI.Container, 240 - i * 1.1f * taskItemUI.Container.sizeDelta.y);

            taskItemUI.SetProgress(task.CurrentValue, task.RequirementValue);
        }
    }
}
