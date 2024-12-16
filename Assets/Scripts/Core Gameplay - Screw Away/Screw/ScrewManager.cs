using System.Collections.Generic;
using UnityEngine;

public class ScrewManager : MonoBehaviour
{
    [SerializeField] private List<BaseScrew> _screws;

    void Awake()
    {
        BaseScrew.addScrewToListEvent += AddScrew;

        _screws = new List<BaseScrew>();
    }

    void OnDestroy()
    {
        BaseScrew.addScrewToListEvent -= AddScrew;
    }

    private void AddScrew(BaseScrew screw)
    {
        _screws.Add(screw);
    }
}
