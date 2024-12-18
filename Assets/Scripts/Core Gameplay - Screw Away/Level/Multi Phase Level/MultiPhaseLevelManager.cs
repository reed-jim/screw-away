using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MultiPhaseLevelManager : MonoBehaviour
{
    private List<MultiPhaseScrew> _screws;
    private int _currentPhase;
    private int _totalScrew;
    private int _totalScrewLoosened;

    public static event Action<int> switchPhaseEvent;
    public static event Action<Dictionary<int, int>, float> updateUIEvent;

    private void Awake()
    {
        MultiPhaseScrew.manageScrewEvent += ManageScrew;
        BasicScrew.screwLoosenedEvent += OnScrewLoosened;

        _screws = new List<MultiPhaseScrew>();

        DelayUpdateUI();
    }

    private void OnDestroy()
    {
        MultiPhaseScrew.manageScrewEvent -= ManageScrew;
        BasicScrew.screwLoosenedEvent -= OnScrewLoosened;
    }

    private void ManageScrew(MultiPhaseScrew screw)
    {
        _screws.Add(screw);

        _totalScrew++;
    }

    private async void DelayUpdateUI()
    {
        await Task.Delay(1500);

        Dictionary<int, int> numberScrewByPhase = GetNumberScrewByPhase();

        updateUIEvent?.Invoke(numberScrewByPhase, 0);
    }

    private void OnScrewLoosened()
    {
        Dictionary<int, int> numberScrewByPhase = GetNumberScrewByPhase();

        if (numberScrewByPhase[_currentPhase] == 0)
        {
            _currentPhase++;

            switchPhaseEvent?.Invoke(_currentPhase);
        }

        _totalScrewLoosened++;

        updateUIEvent?.Invoke(numberScrewByPhase, (float)_totalScrewLoosened / _totalScrew);
    }

    private Dictionary<int, int> GetNumberScrewByPhase()
    {
        Dictionary<int, int> numberScrewByPhase = new Dictionary<int, int>();

        for (int i = 0; i < _screws.Count; i++)
        {
            int phase = _screws[i].Phase;

            if (!_screws[i].IsDone)
            {
                if (numberScrewByPhase.ContainsKey(phase))
                {
                    numberScrewByPhase[phase]++;
                }
                else
                {
                    numberScrewByPhase.Add(phase, 1);
                }
            }
            else
            {
                if (!numberScrewByPhase.ContainsKey(phase))
                {
                    numberScrewByPhase.Add(phase, 0);
                }
            }
        }

        return numberScrewByPhase;
    }
}
