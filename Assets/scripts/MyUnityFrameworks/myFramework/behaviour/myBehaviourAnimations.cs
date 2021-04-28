using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Reflection;

public partial class MyBehaviour : MonoBehaviour {
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <param name="delta">変化量</param>
    /// <param name="duration">変化時間</param>
    /// <param name="callback">変化終了時関数</param>
    public Coroutine scaleBy(Vector3 delta, float duration, Action callback = null) {
        return StartCoroutine(scaleDelta(delta, duration, callback));
    }
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <param name="delta">変化先</param>
    /// <param name="duration">変化時間</param>
    /// <param name="callback">変化終了時間数</param>
    public Coroutine scaleTo(Vector3 goal, float duration, Action callback = null) {
        return StartCoroutine(scaleDelta(goal - scale, duration, callback));
    }
    public Coroutine scaleTo(Vector2 goal, float duration, Action callback = null) {
        return StartCoroutine(scaleDelta(new Vector3(goal.x - scaleX, goal.y - scaleY, 0), duration, callback));
    }
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="delta">変化量</param>
    /// <param name="speed">変化速度(/s)</param>
    /// <param name="callback">変化終了時関数</param>
    public Coroutine scaleByWithSpeed(Vector3 delta, float speed, Action callback = null) {
        return StartCoroutine(scaleDelta(delta, delta.magnitude / speed, callback));
    }
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="goal">変化先</param>
    /// <param name="speed">変化速度(/s)</param>
    /// <param name="callback">変化終了時関数</param>
    public Coroutine scaleToWithSpeed(Vector3 goal, float speed, Action callback = null) {
        Vector3 tDelta = goal - scale;
        return StartCoroutine(scaleDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    public Coroutine scaleToWithSpeed(Vector2 goal, float speed, Action callback = null) {
        Vector2 tDelta = goal - scale2D;
        return StartCoroutine(scaleDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    private IEnumerator scaleDelta(Vector3 delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                scale += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            scale += delta * Time.deltaTime / duration;
            yield return null;
        }
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <param name="delta">移動量</param>
    /// <param name="duration">移動時間</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveBy(Vector3 delta, float duration, Action callback = null) {
        return StartCoroutine(moveDelta(delta, duration, callback));
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <param name="goal">移動先の座標(メソッド呼び出し時の座標からの相対座標分移動)</param>
    /// <param name="duration">移動時間</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveTo(Vector3 goal, float duration, Action callback = null) {
        return StartCoroutine(moveDelta(goal - position, duration, callback));
    }
    public Coroutine moveTo(Vector2 goal, float duration, Action callback = null) {
        return StartCoroutine(moveDelta(new Vector3(goal.x - positionX, goal.y - positionY, 0), duration, callback));
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="delta">移動量</param>
    /// <param name="speed">移動速度(/s)</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveByWithSpeed(Vector3 delta, float speed, Action callback = null) {
        return StartCoroutine(moveDelta(delta, delta.magnitude / speed, callback));
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="goal">目標地点</param>
    /// <param name="speed">移動速度(/s)</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveToWithSpeed(Vector3 goal, float speed, Action callback = null) {
        Vector3 tDelta = goal - position;
        return StartCoroutine(moveDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    public Coroutine moveToWithSpeed(Vector2 goal, float speed, Action callback = null) {
        Vector2 tDelta = goal - position2D;
        return StartCoroutine(moveDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    private IEnumerator moveDelta(Vector3 delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                position += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            position += delta * Time.deltaTime / duration;
            yield return null;
        }
    }
    /// <summary>
    /// Sinを利用した移動
    /// </summary>
    /// <param name="directionHorizontal">波の水平方向の移動距離</param>
    /// <param name="directionVertical">波の垂直方向の最大移動距離</param>
    /// <param name="startPi">移動開始時の座標のpi(PIはかけない)</param>
    /// <param name="goalPi">移動先の座標のpi(PIはかけない)</param>
    /// <param name="duration">移動時間</param>
    /// <param name="callback">終了時コールバック</param>
    /// <returns></returns>
    public Coroutine sinMove(Vector3 directionHorizontal, Vector3 directionVertical, float startPi, float goalPi, float duration, Action callback) {
        return StartCoroutine(sinMoveDelta(directionHorizontal, directionVertical, startPi, goalPi, duration, callback));
    }
    /// <summary>
    /// Sinを利用した移動
    /// </summary>
    /// <param name="directionHorizontal">波の水平方向の移動距離</param>
    /// <param name="directionVertical">波の垂直方向の最大移動距離</param>
    /// <param name="startPi">移動開始時の座標のpi(PIはかけない)</param>
    /// <param name="goalPi">移動先の座標のpi(PIはかけない)</param>
    /// <param name="horizontalSpeed">波の水平方向の移動速度</param>
    /// <param name="callback">終了時コールバック</param>
    /// <returns></returns>
    public Coroutine sinMoveWithSpeed(Vector3 directionHorizontal, Vector3 directionVertical, float startPi, float goalPi, float horizontalSpeed, Action callback) {
        return StartCoroutine(sinMoveDelta(directionHorizontal, directionVertical, startPi, goalPi, directionHorizontal.magnitude / horizontalSpeed, callback));
    }
    private IEnumerator sinMoveDelta(Vector3 directionHorizontal, Vector3 directionVertical, float startPi, float goalPi, float duration, Action callback) {
        float tElapsedTime = 0;
        Vector3 tCurrentY = Vector3.zero;
        Vector3 tY;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                //x方向
                position += directionHorizontal * (duration - tElapsedTime) / duration;
                //y方向
                tY = Mathf.Sin(goalPi * Mathf.PI) * directionVertical;
                position += tY - tCurrentY;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            //x方向
            position += directionHorizontal * Time.deltaTime / duration;
            //y方向
            tY = Mathf.Sin((startPi + tElapsedTime / duration * (goalPi - startPi)) * Mathf.PI) * directionVertical;
            position += tY - tCurrentY;
            tCurrentY = tY;
            yield return null;
        }
    }
    /// <summary>
    /// 回転させる
    /// </summary>
    /// <param name="delta">回転量</param>
    /// <param name="duration">回転時間</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateXBy(float delta, float duration, Action callback = null) {
        return StartCoroutine(rotateXDelta(delta, duration, callback));
    }
    /// <summary>
    /// 回転させる(オイラー角の仕様上バグあり)
    /// </summary>
    /// <param name="goal">回転後の回転量(メソッド呼び出し時の回転量からの相対回転量分移動)</param>
    /// <param name="duration">回転時間</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateXTo(float goal, float duration, Action callback = null) {
        return StartCoroutine(rotateXDelta(goal - rotateX, duration, callback));
    }
    /// <summary>
    /// 回転させる
    /// </summary>
    /// <param name="delta">回転量</param>
    /// <param name="speed">回転速度(/s)</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateXByWithSpeed(float delta, float speed, Action callback = null) {
        return StartCoroutine(rotateXDelta(delta, Mathf.Abs(delta / speed), callback));
    }
    /// <summary>
    /// 回転させる(オイラー角の仕様上バグあり)
    /// </summary>
    /// <param name="goal">回転後の回転量(メソッド呼び出し時の回転量からの相対回転量分移動)</param>
    /// <param name="speed">回転速度(/s)</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateXToWithSpeed(float goal, float speed, Action callback = null) {
        return StartCoroutine(rotateXDelta(goal - rotateX, Mathf.Abs((goal - rotateX) / speed), callback));
    }
    private IEnumerator rotateXDelta(float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                rotateX += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            rotateX += delta * Time.deltaTime / duration;
            yield return null;
        }

    }
    /// <summary>
    /// 回転させる
    /// </summary>
    /// <param name="delta">回転量</param>
    /// <param name="duration">回転時間</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateYBy(float delta, float duration, Action callback = null) {
        return StartCoroutine(rotateYDelta(delta, duration, callback));
    }
    /// <summary>
    /// 回転させる(オイラー角の仕様上バグあり)
    /// </summary>
    /// <param name="goal">回転後の回転量(メソッド呼び出し時の回転量からの相対回転量分移動)</param>
    /// <param name="duration">回転時間</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateYTo(float goal, float duration, Action callback = null) {
        return StartCoroutine(rotateYDelta(goal - rotateY, duration, callback));
    }
    /// <summary>
    /// 回転させる
    /// </summary>
    /// <param name="delta">回転量</param>
    /// <param name="speed">回転速度(/s)</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateYByWithSpeed(float delta, float speed, Action callback = null) {
        return StartCoroutine(rotateYDelta(delta, Mathf.Abs(delta / speed), callback));
    }
    /// <summary>
    /// 回転させる(オイラー角の仕様上バグあり)
    /// </summary>
    /// <param name="goal">回転後の回転量(メソッド呼び出し時の回転量からの相対回転量分移動)</param>
    /// <param name="speed">回転速度(/s)</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateYToWithSpeed(float goal, float speed, Action callback = null) {
        return StartCoroutine(rotateYDelta(goal - rotateY, Mathf.Abs((goal - rotateY) / speed), callback));
    }
    private IEnumerator rotateYDelta(float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                rotateY += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            rotateY += delta * Time.deltaTime / duration;
            yield return null;
        }

    }
    /// <summary>
    /// 回転させる
    /// </summary>
    /// <param name="delta">回転量</param>
    /// <param name="duration">回転時間</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateZBy(float delta, float duration, Action callback = null) {
        return StartCoroutine(rotateZDelta(delta, duration, callback));
    }
    /// <summary>
    /// 回転させる(オイラー角の仕様上バグあり)
    /// </summary>
    /// <param name="goal">回転後の回転量(メソッド呼び出し時の回転量からの相対回転量分移動)</param>
    /// <param name="duration">回転時間</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateZTo(float goal, float duration, Action callback = null) {
        return StartCoroutine(rotateZDelta(goal - rotateZ, duration, callback));
    }
    /// <summary>
    /// 回転させる
    /// </summary>
    /// <param name="delta">回転量</param>
    /// <param name="speed">回転速度(/s)</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateZByWithSpeed(float delta, float speed, Action callback = null) {
        return StartCoroutine(rotateZDelta(delta, Mathf.Abs(delta / speed), callback));
    }
    /// <summary>
    /// 回転させる(オイラー角の仕様上バグあり)
    /// </summary>
    /// <param name="goal">回転後の回転量(メソッド呼び出し時の回転量からの相対回転量分移動)</param>
    /// <param name="speed">回転速度(/s)</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateZToWithSpeed(float goal, float speed, Action callback = null) {
        return StartCoroutine(rotateZDelta(goal - rotateZ, Mathf.Abs((goal - rotateZ) / speed), callback));
    }
    private IEnumerator rotateZDelta(float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                rotateZ += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            rotateZ += delta * Time.deltaTime / duration;
            yield return null;
        }

    }
    /// <summary>
    /// 回転させ続ける
    /// </summary>
    /// <param name="speed">回転速度(度/s)</param>
    public Coroutine rotateForever(float speed) {
        return StartCoroutine(rotateForeverDelta(speed));
    }
    private IEnumerator rotateForeverDelta(float speed) {
        while (true) {
            rotateZ += speed * Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    /// 透明度を変化させる
    /// </summary>
    /// <param name="delta">変化量</param>
    /// <param name="duration">変化時間</param>
    /// <param name="callback">完了時コールバック</param>
    public void opacityBy(float delta, float duration, Action callback = null) {
        CallbackSystem system = new CallbackSystem();
        foreach (SpriteRenderer tC in GetComponentsInChildren<SpriteRenderer>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        foreach (TextMesh tC in GetComponentsInChildren<TextMesh>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        foreach (Image tC in GetComponentsInChildren<Image>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        foreach (Text tC in GetComponentsInChildren<Text>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        system.then(callback != null ? callback : () => { });
    }
    private IEnumerator opacityDelta(TextMesh obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        if (duration == 0) {
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta);
            if (callback != null) callback();
            yield break;
        }
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
    private IEnumerator opacityDelta(Image obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        if (duration == 0) {
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta);
            if (callback != null) callback();
            yield break;
        }
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
    private IEnumerator opacityDelta(Text obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        if (duration == 0) {
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta);
            if (callback != null) callback();
            yield break;
        }
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
    private IEnumerator opacityDelta(SpriteRenderer obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        if (duration == 0) {
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta);
            if (callback != null) callback();
            yield break;
        }
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
}
