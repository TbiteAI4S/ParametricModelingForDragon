using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControllerforControllPoint : MonoBehaviour
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

    /*---どの制御点か判別する値---*/
    int discriminationCP;

    //スライダーの値をテキストに反映する
    private void ChangeSliderText()
    {
        //スライダーの値を取得
        sliderValue = cpSlider.value;

        //スライダーの値をテキストに反映
        cpTexts.text = "現在の個数：" + sliderValue;
    }

    //どの部位の制御点なのか確認する
    //引数：スライダーの名前
    //戻り値：スライダーの判別用整数
    private int WhichControlPoint(string sliderName)
    {
        int cp = 0;

        switch (sliderName)
        {
            case "NeckControllPoint":
                cp = 0;
                break;

            case "TailControllPoint":
                cp = 1;
                break;

        }

        return cp;
    }

    //どの制御点を追加するのか確認し追加する
    private void AddWhichControlPoint(int discrimination)
    {
        switch (discrimination)
        {
            case 0:
                makeModelScript.AddTail();
                break;

            case 1:

                break;

        }
    }

    //どの制御点を削減するのか確認し追加する
    private void CutWhichControlPoint(int discrimination)
    {
        switch (discrimination)
        {
            case 0:
                makeModelScript.CutTail();
                break;

            case 1:

                break;

        }
    }

    //制御点の増減を確認する
    private void CheckSlider()
    {
        float nowValue = cpSlider.value;
        //saveより大きければ追加
        if(saveValue < nowValue)
        {
            //制御点を増やす
            AddWhichControlPoint(discriminationCP);
            //saveを更新
            saveValue = nowValue;

        }//saveより小さければ減少
        else if(saveValue > nowValue)
        {
            //制御点を減らす
            CutWhichControlPoint(discriminationCP);
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
        discriminationCP = WhichControlPoint(cpSlider.name);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSliderText();
        CheckSlider();
    }
}
