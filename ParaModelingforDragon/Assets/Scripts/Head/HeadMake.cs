using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ***********************************************
 *
 * 頭部のモデルを生成する
 *
 * ***********************************************
 */
public class HeadMake : MonoBehaviour
{
    //頭のデータを取得
    HeadData headData;
    //ツールスクリプトを取得
    Tools toolscripts;
    //BaseParameterスクリプトを取得
    BaseParameter baseParameter;
    //BaseCrossSectionスクリプトを取得
    BaseCrossSection baseCrossSection;


    /* 断面 */
    //断面の頂点
    Vector3[] vertexArray;
    private List<Vector3> vertextList = new List<Vector3>();

    /* モデル */
    //モデルの中心点
    public List<Vector3> mainV = new List<Vector3>();
    //中心点から作る曲線
    public List<Vector3> mainVCurve = new List<Vector3>();
    //中心点から作る曲線の接ベクトル
    public List<Vector3> mainVCurve_TangentVec = new List<Vector3>();
    //モデル中心線上の断面の頂点リスト
    public List<Vector3> crossSection = new List<Vector3>();

    /* メッシュ */
    //生成するメッシュ
    private Mesh crossSectionMesh;
    //面を構成するインデックスリスト
    private List<int> meshTriangles = new List<int>();
    //初回メッシュを生成したかどうか
    private bool firstEnd = false;



    /*
     * *********************************************************************************************
     *
     * モデルの作成
     *
     * *********************************************************************************************
     */

    //断面の作成
    private void makeCrossSectionVertex()
    {
        //断面の頂点を作成
        for (int i = 0; i < 12; i++)
        {

        }

        //
        vertextList = new List<Vector3>(baseCrossSection.controlPoint);
        //断面生成完了
        baseCrossSection.createEnd = true;
    }

    //中心点から"モデル中心線"作る  12個
    private void MakeMainVCureve()
    {

        toolscripts.CatmullRomSplineCurve(mainV[0], mainV[0], mainV[1], mainV[2], 3, mainVCurve);
        toolscripts.CatmullRomSplineCurve(mainV[0], mainV[1], mainV[2], mainV[3], 3, mainVCurve);
        toolscripts.CatmullRomSplineCurve(mainV[1], mainV[2], mainV[3], mainV[4], 3, mainVCurve);
        toolscripts.CatmullRomSplineCurve(mainV[2], mainV[3], mainV[4], mainV[4], 3, mainVCurve);
    }

    //"モデル中心線"の接ベクトルを求める  12個
    private void GetTangent_MainVCureve()
    {
        toolscripts.CatmullRomSplineCurve_TangentVec(mainV[0], mainV[0], mainV[1], mainV[2], 3, mainVCurve_TangentVec);
        toolscripts.CatmullRomSplineCurve_TangentVec(mainV[0], mainV[1], mainV[2], mainV[3], 3, mainVCurve_TangentVec);
        toolscripts.CatmullRomSplineCurve_TangentVec(mainV[1], mainV[2], mainV[3], mainV[4], 3, mainVCurve_TangentVec);
        toolscripts.CatmullRomSplineCurve_TangentVec(mainV[2], mainV[3], mainV[4], mainV[4], 3, mainVCurve_TangentVec);
    }

    //断面を"モデル中心線"に移動し接ベクトル方向に向かせる
    private void GoToCureve()
    {
        //断面の法線
        Vector3 u = new Vector3(0.0f, 0.0f, 1.0f);

        //平行移動 Mtのリスト
        List<Vector3> Mt = new List<Vector3>();

        //曲線上の点の数だけMtを求める
        for (int i = 0; i < mainVCurve.Count; i++)
        {
            Vector3 mt = new Vector3(0f, 0f, 0f);
            mt = toolscripts.GetTranslation(mt, mainVCurve[i]);
            Mt.Add(mt);
        }

        //回転 Mrのリスト
        List<float[][]> Mr = new List<float[][]>();

        //曲線上の接ベクトルへの回転を求める
        for (int i = 0; i < mainVCurve.Count; i++)
        {
            //各軸の回転を求める
            float[][] mr_X = toolscripts.GetRotationX(u, mainVCurve_TangentVec[i] - Mt[i]);
            float[][] mr_Y = toolscripts.GetRotationY(u, mainVCurve_TangentVec[i] - Mt[i]);
            float[][] mr_Z = toolscripts.GetRotationZ(u, mainVCurve_TangentVec[i] - Mt[i]);

            //回転の合成
            float[][] mr_XY = toolscripts.GetMatrices(mr_X, mr_Y);
            float[][] mr_XYZ = toolscripts.GetMatrices(mr_XY, mr_Z);

            Mr.Add(mr_XYZ);

        }

        //断面の各点を移動させる
        for (int i = 0; i < mainVCurve.Count; i++)//12回
        {
            //各頂点の移動
            for (int j = 0; j < vertextList.Count; j++)
            {
                //元頂点
                Vector3 P = vertextList[j];
                //回転
                Vector3 P_mr = new Vector3(0f, 0f, 0f);
                float[][] mr = Mr[i];
                P_mr.x = P.x * mr[0][0] + P.y * mr[0][1] + P.z * mr[0][2];
                P_mr.y = P.x * mr[1][0] + P.y * mr[1][1] + P.z * mr[1][2];
                P_mr.z = P.x * mr[2][0] + P.y * mr[2][1] + P.z * mr[2][2];

                //平行移動
                Vector3 P_mt = P_mr + Mt[i];

                //移動後の頂点を格納
                crossSection.Add(P_mt);
            }
        }
    }


    /*
     * ********************
     *
     * メッシュの作成
     *
     * ********************
     */

    //メッシュに面を構成するインデックスリスト作成
    private void MakeFaceIndex()
    {
        //断面の個数
        int countCrossSection = mainVCurve.Count;

        //断面の頂点数
        int countVertex = baseCrossSection.controlPointSize;

        /*---側面(断面の個数分だけ生成)---*/
        for (int j = 0; j < countCrossSection - 1; j++)
        {
            int index = countVertex * j;
            //Debug.Log("断面の開始位置：" + index);
            //一周面を貼る
            for (int i = 0; i < countVertex; i++)
            {
                //最後の時
                if (i == 7)
                {
                    //一つ目
                    meshTriangles.Add(index + i);
                    meshTriangles.Add(index + i - 7);
                    meshTriangles.Add(index + i + 1);

                    //二つ目
                    meshTriangles.Add(index + i);
                    meshTriangles.Add(index + i + 1);
                    meshTriangles.Add(index + i + 8);

                }
                else
                {
                    //一つ目
                    meshTriangles.Add(index + i);
                    meshTriangles.Add(index + i + 1);
                    meshTriangles.Add(index + i + 9);

                    //二つ目
                    meshTriangles.Add(index + i);
                    meshTriangles.Add(index + i + 9);
                    meshTriangles.Add(index + i + 8);

                }
            }
        }
        /*---鼻先を面で閉じる(三角ポリゴン6枚で閉じる)---*/
        //最後の断面の頂点開始位置( (断面の個数 * 断面の頂点数)-断面の頂点数 )
        int lastCrossSectionPoint = (countCrossSection * countVertex) - countVertex;
        for (int i = 0; i < 6; i++)
        {
            meshTriangles.Add(lastCrossSectionPoint);
            meshTriangles.Add(lastCrossSectionPoint + i + 1);
            meshTriangles.Add(lastCrossSectionPoint + i + 2);
        }
    }

    //断面のメッシュの準備
    private Mesh CreateCrossSectionMeshs()
    {
        //メッシュ作成
        crossSectionMesh = new Mesh();

        /*
         * 頂点はリストのcrossSectionを使う
         */
        // 面を構成するインデックスリストを作成
        MakeFaceIndex();

        // メッシュに頂点を登録する
        crossSectionMesh.SetVertices(crossSection);
        // メッシュに面を構成するインデックスリストを登録する
        crossSectionMesh.SetTriangles(meshTriangles, 0);

        return crossSectionMesh;
    }

    //断面のメッシュの作成
    void CrossSection()
    {
        //メッシュを作る
        crossSectionMesh = CreateCrossSectionMeshs();
        // 作成したメッシュをメッシュフィルターに設定する
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = crossSectionMesh;
    }


    //モデルの作成(一度実行したら止める)
    void CreateModel()
    {
        //断面作成
        makeCrossSectionVertex();
        //曲線作成
        MakemainV();
        MakeMainVCureve();
        //曲線の接ベクトルを求める
        GetTangent_MainVCureve();
        //断面移動
        GoToCureve();
        //断面のメッシュ生成
        CrossSection();
    }

    /*
     * **********************************************************************
     *
     *
     * 確認・実行
     *
     *
     * **********************************************************************
     */

    void OnDrawGizmos()
    {
        //中心線
        Gizmos.color = Color.white;
        foreach (var point in mainVCurve)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
        Gizmos.color = Color.green;
        for (int i = 0; i < mainVCurve.Count; i++)
        {
            if (i < mainVCurve.Count - 1)
            {
                Gizmos.DrawLine(mainVCurve[i], mainVCurve[i + 1]);
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //頭のデータを取得
        headData = GameObject.Find("HeadData").GetComponent<HeadData>();
        mainV.AddRange(headData.HeadMainV);
        //ツールスクリプトの取得
        toolscripts = GameObject.Find("Tools").GetComponent<Tools>();
        //パラメータの取得
        baseParameter = GameObject.Find("BaseParameter").GetComponent<BaseParameter>();
        //断面を取得
        baseCrossSection = GameObject.Find("BaseCrossSection").GetComponent<BaseCrossSection>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * はじめの生成とその後の変化を確認して、モデルを変形
         */
        //はじめの生成
        if (baseCrossSection.createEnd == true && firstEnd == false)
        {
            //はじめのモデルを生成
            CreateModel();
            //完了を通知
            firstEnd = true;
        }
        //変形

    }
}
