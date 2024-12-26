using UnityEngine;

public class ScrewsDataManager : MonoBehaviour
{
    [SerializeField] private BaseScrew[] _screws;

    [SerializeField] private ScrewBoxManager screwBoxManager;

    [SerializeField] private IntVariable currentLevel;



    private void LoadLevelData()
    {
        ScrewData[] screwsData = DataUtility.Load($"{GameConstants.SCREWS_DATA}_{currentLevel.Value}", new ScrewData[_screws.Length]);

        for (int i = 0; i < screwsData.Length; i++)
        {
            BaseScrew screw = _screws[i];
            ScrewData screwData = screwsData[i];

            if (screwData.IsDestroyed)
            {
                screw.gameObject.SetActive(false);

                screw.IsDone = true;

                continue;
            }

            if (screwData.IsInScrewPort)
            {
                ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

                screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);

                // for (int j = 0; j < screwBoxManager.ScrewPorts.Count; j++)
                // {
                //     ScrewBoxSlot screwBoxSlot = screwBoxManager.ScrewPorts[j];

                //     if (!screwBoxSlot.IsFilled)
                //     {
                //         screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);

                //         break;
                //     }
                // }
            }

            if (screwData.IsDone)
            {
                ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

                screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);
            }
        }
    }

    private void SaveLevelData()
    {
        ScrewData[] screwsData = new ScrewData[_screws.Length];

        for (int i = 0; i < _screws.Length; i++)
        {
            screwsData[i] = new ScrewData();

            screwsData[i].IsDone = _screws[i].IsDone;

            if (!_screws[i].gameObject.activeSelf)
            {
                screwsData[i].IsDestroyed = true;
            }
        }

        DataUtility.SaveAsync($"{GameConstants.SCREWS_DATA}_{currentLevel.Value}", screwsData);
    }
}
