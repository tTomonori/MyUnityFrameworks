using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFlag {
    private Dictionary<string, object> mFlags;
    public MapFlag(){
        mFlags = new Dictionary<string, object>();
    }
    private void setFlag(string aFlagName,object aValue){
        string[] tDir = aFlagName.Split('/');
        if(tDir.Length==1){
            mFlags[aFlagName] = aValue;
            return;
        }
        if(!mFlags.ContainsKey(tDir[0]) || !(mFlags[tDir[0]] is MapFlag)){
            mFlags[tDir[0]] = new MapFlag();
        }
        ((MapFlag)mFlags[tDir[0]]).setFlag(aFlagName.Substring(tDir[0].Length+1), aValue);
    }
    //<summary>フラグセット</summary>
    public void set(string aFlagName, bool aValue){
        setFlag(aFlagName, aValue);
    }
    //<summary>フラグセット</summary>
    public void set(string aFlagName,int aValue){
        setFlag(aFlagName, aValue);
    }
    //<summary>フラグ削除</summary>
    public void delete(string aFlagName){
        string[] tDir = aFlagName.Split('/');
        if(tDir.Length==1){
            mFlags.Remove(aFlagName);
            return;
        }
        if (!mFlags.ContainsKey(tDir[0])) return;
        ((MapFlag)mFlags[tDir[0]]).delete(aFlagName.Substring(tDir[0].Length+1));
    }
    //<summary>フラグ取得</summary>
    public T get<T>(string aFlagName){
        string[] tDir = aFlagName.Split('/');
        if(tDir.Length==1){
            return (T)mFlags[aFlagName];
        }
        return ((MapFlag)mFlags[tDir[0]]).get<T>(aFlagName.Substring(tDir[0].Length+1));
    }
}
