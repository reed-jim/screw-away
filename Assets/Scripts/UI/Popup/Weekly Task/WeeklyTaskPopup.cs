using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyTaskPopup : BasePopup
{
    [SerializeField] private RectTransform taskContainer;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private GameTaskManager gameTaskManager;

    protected override void MoreActionInAwake()
    {
        DelaySpawnTasks();
    }

    private async void DelaySpawnTasks()
    {
        await Task.Delay(2000);

        int currentTotalScore = 0;
        int totalScore = 0;

        for (int i = 0; i < gameTaskManager.Tasks.Length; i++)
        {
            BaseWeeklyTask taskData = gameTaskManager.Tasks[i];

            TaskItemUI taskItemUI = ObjectPoolingEverything.GetFromPool<TaskItemUI>(GameConstants.TASK_ITEM_UI);

            taskItemUI.Container.SetParent(taskContainer);

            UIUtil.SetLocalPositionX(taskItemUI.Container, 0);
            UIUtil.SetLocalPositionY(taskItemUI.Container, 240 - i * 1.1f * taskItemUI.Container.sizeDelta.y);

            taskItemUI.Setup(taskData);

            if (taskData.IsDone)
            {
                currentTotalScore += taskData.Reward;
            }

            totalScore += taskData.Reward;
        }

        progressBarFill.fillAmount = (float)currentTotalScore / totalScore;
    }
}
