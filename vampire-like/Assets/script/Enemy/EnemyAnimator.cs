using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;
    public GameObject player;

    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {    
            am.SetBool("Move",true);
            SpriteDirectionChecker();
        }
        else
            am.SetBool("Move", false);
    }

    void SpriteDirectionChecker()
    {
        if (pm.moveDir.x<0)
            sr.flipX=true;
        else
            sr.flipX=false;
    }
}
