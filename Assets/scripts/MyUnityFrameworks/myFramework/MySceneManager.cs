using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class MySceneManager {
    static MySceneManager() {
        //新しくシーンを読み込んだ時
        SceneManager.sceneLoaded += (aScene, aMode) => {
            SceneData tData = findSceneData(aScene.name);
            //SceneDataにSceneを記憶
            tData.scene = aScene;
            //カメラノードのAudioListenerを消す
            if (tData.sceneOpentype == SceneOpenType.additive || tData.sceneOpentype == SceneOpenType.fade) {//additiveで開いた時のみ
                foreach (GameObject tObject in aScene.GetRootGameObjects()) {
                    AudioListener tAudioListener = tObject.GetComponent<AudioListener>();
                    if (tAudioListener != null) {
                        GameObject.Destroy(tAudioListener);
                        break;
                    }
                }
            }
            //開いた時のcallback
            if (tData.opened != null)
                tData.opened(aScene);
        };
        //シーンが閉じられた時
        SceneManager.sceneUnloaded += (aScene) => {
            string tName = aScene.name;
            for (int i = 0; i < mScenes.Count; i++) {
                SceneData tData = mScenes[i];
                if (tData.name != tName) continue;
                mScenes.RemoveAt(i);
                //閉じた時のcallback
                if (tData.closed != null)
                    tData.closed(tData.arg);
            }
        };
        //最初に開かれているシーン
        mScenes.Add(new SceneData(SceneManager.GetActiveScene().name, null, null, null, SceneOpenType.main));
    }
    public class SceneData {
        public SceneData(string aName, Arg aArg, Action<Scene> aOpened, Action<Arg> aClosed, SceneOpenType aSceneOpenType) {
            name = aName;
            arg = (aArg == null) ? new Arg() : aArg;
            opened = aOpened;
            closed = aClosed;
            pausedBehaviour = new List<MonoBehaviour>();
            sceneOpentype = aSceneOpenType;
        }
        public string name;//シーンの名前
        public Arg arg;//開いたシーンに渡す引数,閉じた時のcallbackに渡す引数(閉じるメソッドを読んだ時に上書きされる)
        public Action<Scene> opened;//シーンを開いた時のcallback
        public Action<Arg> closed;//シーンを閉じた時のcallback
        public Scene scene;//開いたシーン
        public List<MonoBehaviour> pausedBehaviour;//停止させたbehaviour
        public SceneOpenType sceneOpentype;//additiveでシーンを開いた
    }
    public enum SceneOpenType {
        main, additive, fade
    }
    public class FadeCallbackData {
        public Action fadeOutFinished;//画面を隠す演出完了時
        public Action nextSceneReady;//新しいシーンの準備完了時
        public Action fadeInFinished;//画面を見えるようにする演出完了時
    }
    static public FadeCallbackData fadeCallbacks;
    static private Action nextSceneReadyOfFade;
    static private Action fadeInFinishedOfFade;
    ///<summary>開いている全てのシーン</summary>
    static private List<SceneData> mScenes = new List<SceneData>();
    ///<summary>指定した名前のシーンのデータを探す</summary>
    static private SceneData findSceneData(string aName) {
        foreach (SceneData tData in mScenes) {
            if (tData.name == aName) {
                return tData;
            }
        }
        Debug.LogWarning("SceneManager:「" + aName + "」なんて名前のシーンはないよ");
        return null;
        //throw new KeyNotFoundException("SceneManager:「"+aName+"」なんて名前のシーンはないよ");
    }
    ///<summary>シーンを開く</summary>
    static public void openScene(string aName, Arg aArg = null, Action<Scene> aOpened = null, Action<Arg> aClosed = null) {
        SceneData tData = new SceneData(aName, aArg, aOpened, aClosed, SceneOpenType.additive);
        mScenes.Add(tData);
        //SceneManager.LoadSceneAsync(aName, LoadSceneMode.Additive);
        SceneManager.LoadScene(aName, LoadSceneMode.Additive);
    }
    ///<summary>シーンを閉じる</summary>
    static public void closeScene(string aName, Arg aArg = null, Action<Arg> aClosed = null) {
        for (int i = 0; i < mScenes.Count; i++) {
            SceneData tData = mScenes[i];
            if (tData.name != aName) continue;
            tData.arg = (aArg == null) ? new Arg() : aArg;
            if (aClosed != null) {
                if (tData.closed != null)
                    Debug.LogWarning("SceneManager:「" + aName + "」ってシーンを閉じた時のcallbak上書きしちゃった");
                tData.closed = aClosed;
            }
            if (SceneManager.sceneCount > 1) {
                SceneManager.UnloadSceneAsync(aName);
            } else {
                //開かれているシーンが一つのみ
                Action<Arg> tClosed = tData.closed;
                tData.closed = null;
                //シーンを閉じる前にコールバックを呼ぶ
                tClosed(aArg);
                SceneManager.UnloadSceneAsync(aName);
            }
            //SceneManager.UnloadSceneAsync(aName);
            //SceneManager.UnloadScene(aName);
            return;
        }
        throw new KeyNotFoundException("SceneManager:「" + aName + "」なんて名前のシーンはないから閉じれない");
    }
    ///<summary>シーン変更する</summary>
    static public void changeScene(string aName, Arg aArg = null, Action<Scene> aOpened = null, Action<Arg> aClosed = null) {
        SceneData tData = new SceneData(aName, aArg, aOpened, aClosed, SceneOpenType.main);
        mScenes.Clear();//シーンのデータを全て削除
        mScenes.Add(tData);
        SceneManager.LoadScene(aName);
    }
    /// <summary>フェードシーンを挟んでシーン変更</summary>
    static public void changeSceneWithFade(string aNextSceneName, string aFadeSceneName, Arg aToFadeAndNextSceneArg = null, Arg aToCurrentSceneArg = null, Action<Scene> aFadeSceneOpened = null, Action<Arg> aFadeSceneClosed = null, Action<Scene> aNextSceneOpened = null, Action<Arg> aNextSceneClosed = null) {
        SceneData tData = new SceneData(aFadeSceneName, aToFadeAndNextSceneArg, aFadeSceneOpened, aFadeSceneClosed, SceneOpenType.fade);
        List<SceneData> tScenes = new List<SceneData>(mScenes);
        mScenes.Add(tData);
        fadeCallbacks = new FadeCallbackData();
        fadeCallbacks.fadeOutFinished = () => {
            nextSceneReadyOfFade = fadeCallbacks.nextSceneReady;
            fadeCallbacks.nextSceneReady = () => {
                fadeInFinishedOfFade = fadeCallbacks.fadeInFinished;
                fadeCallbacks.fadeInFinished = () => {
                    closeScene(aFadeSceneName, null, null);
                    fadeCallbacks = null;
                    nextSceneReadyOfFade = null;
                    if (fadeInFinishedOfFade != null)
                        fadeInFinishedOfFade();
                    fadeInFinishedOfFade = null;
                };
                nextSceneReadyOfFade();
            };
            foreach (SceneData tSceneData in tScenes)
                closeScene(tSceneData.name, aToCurrentSceneArg, null);
            //nextSceneを開く
            SceneData tNextSceneData = new SceneData(aNextSceneName, aToFadeAndNextSceneArg, aNextSceneOpened, aNextSceneClosed, SceneOpenType.main);
            mScenes.Add(tNextSceneData);
            SceneManager.LoadScene(aNextSceneName, LoadSceneMode.Additive);
        };
        SceneManager.LoadScene(aFadeSceneName, LoadSceneMode.Additive);
    }
    ///<summary>引数を受け取る</summary>
    static public Arg getArg(string aName) {
        SceneData tData = findSceneData(aName);
        if (tData == null)
            return null;
        return tData.arg;
    }
    ///<summary>シーンを停止する</summary>
    static public void pauseScene(string aName) {
        SceneData tData = findSceneData(aName);
        foreach (GameObject tObject in tData.scene.GetRootGameObjects()) {
            foreach (MonoBehaviour tBehaviour in tObject.GetComponentsInChildren<MonoBehaviour>()) {
                if (tBehaviour.enabled == false) continue;
                tBehaviour.enabled = false;
                tData.pausedBehaviour.Add(tBehaviour);
            }
        }
    }
    ///<summary>シーンを再生する</summary>
    static public void playScene(string aName) {
        SceneData tData = findSceneData(aName);
        foreach (MonoBehaviour tBehaviour in tData.pausedBehaviour) {
            tBehaviour.enabled = true;
        }
        tData.pausedBehaviour.Clear();
    }
}