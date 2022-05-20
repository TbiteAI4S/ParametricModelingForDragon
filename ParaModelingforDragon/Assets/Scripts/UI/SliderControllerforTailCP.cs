using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControllerforTailCP : MonoBehaviour
{
    /*---モデルを操作するスクリプト---*/
    MakeModel makeModelScript;

    /*---スライダー---*/
    Slider cpSlider;

    /*---テキスト---*/
    public Text cpTexts;

    /*---スライダーの値---*/
    //現在の値
    float sliderValue;
    //保存した値
    float saveValue;

    //スライダーの値をテキストに反映する
    private void ChangeSliderText()
    {
        //スライダーの値を取得
        sliderValue = cpSlider.value;

        //スライダーの値をテキストに反映
        cpTexts.text = "現在の個数：" + sliderValue;
    }

    //制御点の増減を確認する
    private void CheckSlider()
    {
        float nowValue = cpSlider.value;
        //saveより大きければ追加
        if(saveValue < nowValue)
        {
            //制御点を増やす
            makeModelScript.AddTail();
            //saveを更新
            saveValue = nowValue;

        }//saveより小さければ減少
        else if(saveValue > nowValue)
        {
            //制御点を減らす
            makeModelScript.CutTail();
            //saveを更新
            saveValue = nowValue;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        makeModelScript = GameObject.Find("MakeModel").GetComponent<MakeModel>();
        cpSlider = this.GetComponent<Slider>();
        saveValue = cpSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSliderText();
        CheckSlider();
    }
}
