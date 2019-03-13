using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static partial class MyJson{
    //シリアライズして保存
    static public void serializeToFile(IDictionary data,string filePath,bool lineFeedCode=false){
        string tString = serialize(data, lineFeedCode);
        File.WriteAllText(filePath, tString);
    }
    //シリアライズ
    static public string serialize(IDictionary data,bool lineFeedCode=false){
        return new Serializer().serialize(data, lineFeedCode);
    }
    private class Serializer{
        private bool mLienFeedCode;
        public string serialize(IDictionary data,bool lineFeedCode){
            mLienFeedCode = lineFeedCode;
            string tOut;
            if (dictionaryToString(data, out tOut, true))
                return tOut;
            else
                return "";
        }
        //改行を入れる状態なら改行文字を返す
        private string newLineChar(bool aSecondFlag=true){
            if (mLienFeedCode && aSecondFlag)
                return "\n";
            return "";
        }
        //改行を入れる状態なら１文字取り消す
        private string backLine(string s,bool aSecondFlag=true){
            if (mLienFeedCode && aSecondFlag)
                return s.Remove(s.Length - 1);
            return s;
        }
        //１文字取り消す
        private string back(string s){
            return s.Remove(s.Length - 1);
        }
        //dictionaryをstringに
        private bool dictionaryToString(IDictionary aDic, out string oOut, bool aLineFeedCode){
            string tOut = "";
            //一つ以上要素を書き込めたか
            bool tWritten = false;
            //不正な要素があったか
            bool tContainsError = false;
            foreach (DictionaryEntry tEntry in aDic){
                //key書き出し
                string tKeyString;
                if (!toString(tEntry.Key, out tKeyString, aLineFeedCode)){
                    tContainsError = true;
                    continue;
                }
                //value書き出し
                string tValueString;
                if (!toString(tEntry.Value, out tValueString, aLineFeedCode)){
                    tContainsError = true;
                    continue;
                }
                //keyもvalueも書き込める
                tOut += tKeyString;
                tOut += ":";
                tOut += tValueString;
                tOut += ",";
                tOut += newLineChar(aLineFeedCode);
                tWritten = true;
            }
            if(!tWritten && tContainsError){//一つも要素がない かつ 不正な要素があった
                oOut = "";
                return false;
            }
            //一つ以上要素を書き込める
            if (tWritten){
                tOut = backLine(tOut, aLineFeedCode);//改行削除
                tOut = back(tOut);//コンマ削除
            }

            oOut = "{" + newLineChar(aLineFeedCode) + tOut + newLineChar(aLineFeedCode) + "}";
            return true;
        }
        //listをstringに
        private bool listToString(IList aList, out string oOut){
            string tOut = "";
            //一つ以上要素を書き込めたか
            bool tWritten = false;
            //不正な要素があったか
            bool tContainsError = false;
            foreach(object tObject in aList){
                string tElement;
                if (!toString(tObject, out tElement, false)){
                    tContainsError = true;
                    continue;
                }
                tOut += tElement;
                tOut += ",";
                tOut += newLineChar();
                tWritten = true;
            }
            if (!tWritten && tContainsError){//一つも要素がない かつ 不正な要素があった
                oOut = "";
                return false;
            }
            //一つ以上要素を書き込める
            if (tWritten){
                tOut = backLine(tOut);//改行削除
                tOut = back(tOut);//コンマ削除
            }

            oOut = "[" + newLineChar() + tOut + newLineChar() + "]";
            return true;
        }
        //型を見てStringにする
        private bool toString(object aObject,out string oOut,bool aSecondFlag){
            if(aObject is string){
                oOut = '"' + (string)aObject + '"';
                return true;
            }else if(aObject is float){
                oOut = ((float) aObject).ToString();
                return true;
            }else if(aObject is double){
                oOut = ((double)aObject).ToString();
                return true;
            }else if(aObject is int){
                oOut = ((int)aObject).ToString();
                return true;
            }else if(aObject is bool){
                oOut = ((bool)aObject).ToString();
                return true;
            }else if(aObject is IDictionary){
                return dictionaryToString((IDictionary)aObject,out oOut, aSecondFlag);
            }else if (aObject is IList){
                return listToString((IList)aObject, out oOut);
            }else if(aObject is Arg){
                return dictionaryToString(((Arg)aObject).dictionary, out oOut, aSecondFlag);
            }else{
                Debug.Log("MyJsonSerialize : サポートしていない型 「" + aObject.GetType() + "」");
                oOut = "";
                return false;
            }
        }
    }
}