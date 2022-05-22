﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControllerforNeckBend : MonoBehaviour
{
    /*---モデルを操作するスクリプト---*/
    MakeModel makeModelScript;

    /*---スライダー---*/
    Slider bendSlider;

    /*---テキスト---*/
    public Text bendTexts;

    /*---スライダーの値---*/
    //現在の値
    float sliderValue;
    //保存した値
    float saveValue;

    //スライダーの値をテキストに反映する
    private void ChangeSliderText()
    {
        //スライダーの値を取得
        sliderValue = bendSlider.value;

        //スライダーの値をテキストに反映
        bendTexts.text = "曲がり係数：" + sliderValue;
    }

    //制御点の増減を確認する
    private void CheckSlider()
    {
        float nowValue = bendSlider.value;
        //saveより大きい曲がり
        if(saveValue < nowValue)
        {
            //制御点を増やす
            makeModelScript.BendNeck(nowValue);
            //saveを更新
            saveValue = nowValue;

        }//saveより小さい曲がり
        else if(saveValue > nowValue)
        {
            //制御点を減らす
            makeModelScript.BendNeck(nowValue);
            //saveを更新
            saveValue = nowValue;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        makeModelScript = GameObject.Find("MakeModel").GetComponent<MakeModel>();
        bendSlider = this.GetComponent<Slider>();
        saveValue = bendSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSliderText();
        CheckSlider();
    }
}
