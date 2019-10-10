using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchManager
{
    void SearchForMatches();
    void CreateMatch();
    void RefreshMatches();
    void ClearMatches();
}
