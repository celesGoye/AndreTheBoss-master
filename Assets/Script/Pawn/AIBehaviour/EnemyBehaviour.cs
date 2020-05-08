using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Enemy))]
public class EnemyBehaviour : MonoBehaviour
{
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public HexCell SeekPlayerUnitToAttack()
    {
        HexMap hexMap = GameObject.FindObjectOfType(typeof(HexMap)) as HexMap;
        hexMap.ProbeAttackTarget(enemy.currentCell);
        
    }

    public void DoAction()
    { 

    }
    */
}
