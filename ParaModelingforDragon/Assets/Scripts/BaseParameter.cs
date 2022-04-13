using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParameter : MonoBehaviour
{
    /*
     * 断面のデータ
     */
    //全ての断面の中心と制御点との距離  mainVCurveの個数分必要
    public float[][] test_allControlPointDistance = new float[12][];
    //各断面の中心と制御点との距離 8個
    float[] test_controlPointDistance = new float[8];

    /*
     * スライダーを取得
     */
    //テスト用スライダー
    BaseSlider test;

    /*
     * スライダーの変化を確認
     */
    //テスト用
    bool testCheck = false;

    /*
     * スライダーの値
     */
    //テスト用
    float tesuValue = 1.0f;

    /*
     * 断面の形を決定
     */
    float[] testDecisionCsossSection(float[] array)
    {
        float[] returnArray = new float[8];

        for(int i=0; i < array.Length; i++)
        {
            returnArray[i] = array[i];
        }
        returnArray[2] = array[2] * 1.5f;
        returnArray[6] = array[6] * 0.5f;

        return returnArray;
    }

    /*
     * スライダー値から断面の変形を行う
     */
    void testChangeCsossSectionControlPoint(float parameter, float[][] arrayArray)
    {
        for(int i=0; i<12; i++)
        {
            float l = i;
            float k = parameter * (l * 0.1f);
            for(int j=0; j<8; j++)
            {
                arrayArray[i][j] = arrayArray[i][j] * k;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //スライダーを取得
        test = GameObject.Find("TestSlider").GetComponent<BaseSlider>();

        //断面の中心と制御点との距離の初期値は 1 とする
        for (int i = 0; i < 8; i++)
        {
            test_controlPointDistance[i] = 1.0f;
        }
        //全ての断面の中心と制御点との距離の配列に格納
        for (int i = 0; i < 12; i++)
        {
            test_allControlPointDistance[i] = testDecisionCsossSection(test_controlPointDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * スライダーの変化を確認し値を取得する
         * その後，スライダーの値から制御点の倍率を変化させる
         */
        //テスト用
        testCheck = test.change;
        if(testCheck == true)
        {
            tesuValue = test.nowValue;
            testChangeCsossSectionControlPoint(tesuValue, test_allControlPointDistance);
        }
    }
}
