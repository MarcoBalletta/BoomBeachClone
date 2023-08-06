using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Mortar : Defense
{

    private Coroutine findingTargetCoroutine;

    protected new MortarData Data { get => data as MortarData; }

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

    //protected override void RemoveEnemyFromList(Enemy enemy)
    //{
    //    if (!targets.Contains(enemy))
    //    {
    //        targets.Remove(enemy);
    //        targets.TrimExcess();
    //        enemy.EventManager.onDead -= TargetKilled;
    //        eventManager.onLostEnemy(enemy);
    //        if(targets.Count == 0)
    //        {
    //            StopCoroutine(findingTargetCoroutine);
    //            findingTargetCoroutine = null;
    //        }
    //    }
    //}

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
                //targets.RemoveAll(enemy => targets.Contains(targets[i]));
        }
    }

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
