using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviourImageAnimator : MyBehaviour {
    public void jump(float aHeight,float aDuration){
        StartCoroutine(jumping(aHeight, aDuration));
    }
    private IEnumerator jumping(float aHeight,float aDuration){
        //経過時間
        float tTime = 0;
        //現在の高さ
        float tCurrentHeight = 0;
        while(true){
            tTime += Time.deltaTime;
            if (tTime > aDuration){
                positionY -= tCurrentHeight;
                yield break;
            }
            float tHeight = aHeight * Mathf.Sin(tTime / aDuration * Mathf.PI);
            positionY += tHeight - tCurrentHeight;
            tCurrentHeight = tHeight;
            yield return null;
        }
    }
}
