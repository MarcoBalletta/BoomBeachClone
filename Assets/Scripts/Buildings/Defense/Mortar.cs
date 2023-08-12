using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Mortar : Defense
{

    private Coroutine findingTargetCoroutine;

    protected new MortarData Data { get => data as MortarData; }

    //adds enemy to list of attackable enemies, but keeps checking if the enemy gets too close
    protected override void AddEnemyToList(Enemy enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Add(enemy);
            enemy.EventManager.onDead += TargetKilled;
            if (findingTargetCoroutine == null)
                findingTargetCoroutine = StartCoroutine(PeriodicallyUpdatesTargetList());
        }
    }

    //updates the enemies list based on the distance
    private void UpdateTargetsList()
    {
        if (targets.Count == 0) return;
        for(int i = 0; i< targets.Count; i++)
        {
            if (Vector3.Distance(transform.position, targets[i].transform.position) <= Data.minRange) 
            { 
                targets.Remove(targets[i]);
                targets.TrimExcess();
            }
        }
    }

    //periodically checks if the enemies are too close to the mortar
    private IEnumerator PeriodicallyUpdatesTargetList()
    {
        yield return new WaitForSeconds(0.1f);
        while(findingTargetCoroutine != null)
        {
            UpdateTargetsList();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
