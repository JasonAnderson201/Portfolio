using System.Collections.Generic;
using UnityEngine;

public class ActionPointUI : MonoBehaviour
{
    [SerializeField] private Transform actionPointParent;
    [SerializeField] private ActionPoint actionPointPrefab;

    private List<ActionPoint> activePoints = new List<ActionPoint>();

    private Character ActiveCharacter { get { return BattleManager.instance.activeCharacter; } }

    private void Start()
    {
        
    }

    public void UpdateUI()
    {
        if(activePoints.Count > 0)
        {
            for(int i = 0; i < activePoints.Count; i++)
            {
                Destroy(activePoints[i].gameObject);
            }

            activePoints.Clear();
        }

        for(int i = 0; i < ActiveCharacter.actionPoints; i++)
        {
            ActionPoint newPoint = Instantiate(actionPointPrefab, actionPointParent);

            activePoints.Add(newPoint);
        }
    }
}
