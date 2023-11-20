//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CoyoteTime : MonoBehaviour
//{
//    public float coyoteTimeLimit;
//    public bool coyoteEnable;

//    PlayerCharacter playerCharacerScr;

//    BoxCollider2D bc;

//    Coroutine c_RCoyote;

//    private void Awake()
//    {
//        bc = GetComponent<BoxCollider2D>();
//        playerCharacerScr= GetComponent<PlayerCharacter>();
//    }

//    public void OnTriggerExit2D(Collider2D collision)
//    {
//        c_RCoyote = StartCoroutine(C_CoyoteTimerUpdate());
//    }

//    public void OnTriggerEnter2D(Collider2D collision)
//    {
//        coyoteEnable = true;
//        if(c_RCoyote != null)
//        {
//            StopCoroutine(c_RCoyote);
//        }

//        if(playerCharacerScr.isJumping == false)
//        {
//            bc.sharedMaterial.friction = 10f;
//            Debug.Log("Coyote Box friction is 10");
//        }
//    }

//    IEnumerator C_CoyoteTimerUpdate()
//    {
//        bc.sharedMaterial.friction = 0;
//        Debug.Log("Coyote Box friction is 0");
//        yield return new WaitForSeconds(coyoteTimeLimit);
//        coyoteEnable = false;
//    }
//}
