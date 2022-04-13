using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlider : MonoBehaviour
{
    //テスト用のスライダー
    Slider parameterSlider;

    //現在のスライダーの値
    public float nowValue;

    //値の変化を通知
    public bool change = false;

    //値の更新を確認して通知
    private float checkChange(float value, float now)
    {
        //スライダーの値を小数第二位で切り捨て
        float floorNow = now * 100;
        floorNow = Mathf.Floor(floorNow) / 100;

        //値が変化していたら更新・通知
        if (value > floorNow)
        {
            value = floorNow;
            change = true;
        }
        else if (value < floorNow)
        {
            value = floorNow;
            change = true;
        }
        else
        {
            change = false;
        }

        return value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //パラメータの取得
        parameterSlider = GetComponent<Slider>();

        //最大値，最小値，現在値の設定
        float max = 1f;
        parameterSlider.maxValue = max;

        float min = -1f;
        parameterSlider.minValue = min;

        float start = 0f;
        parameterSlider.value = start;

        //現在値の取得
        nowValue = parameterSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        float now = parameterSlider.value;

        //値の更新を確認
        nowValue = checkChange(nowValue, now);
    }
}
