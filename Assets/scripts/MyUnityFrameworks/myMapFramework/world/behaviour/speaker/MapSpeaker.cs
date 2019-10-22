using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpeaker : MyBehaviour {
    [SerializeField] public MapBehaviour mBehaviour;

    /// <summary>
    /// 話かけられた時に返答できるか
    /// </summary>
    /// <returns>trueなら返答可能</returns>
    /// <param name="aCharacter">このbehaviourに話かけるキャラ</param>
    public virtual bool canReply(MapCharacter aCharacter) {
        return true;
    }
    /// <summary>
    /// 話かけた
    /// </summary>
    /// <param name="aCharacter">このbehaviourに話かけるキャラ</param>
    public virtual void speak(MapCharacter aCharacter, MapEventSystem aEventSystem) { }

    private void OnValidate() {
        //if (Application.isPlaying) return;
        mBehaviour = gameObject.transform.parent.GetComponent<MapBehaviour>();
    }
}
