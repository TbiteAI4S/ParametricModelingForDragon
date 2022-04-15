using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * ************************************
 *
 * モデルの断面
 *
 * ************************************
 */
public class BaseCrossSection : MonoBehaviour
{
    //ツールスクリプトを取得
    Tools tools;
    //パラメータスクリプトを取得
    BaseParameter parameter;

    /* 変数*/
    //断面生成が完了したかどうか
    public bool createEnd = false;
    //制御点の個数
    public int controlPointSize;
    //曲線分割数
    public int divisionsControlPoint = 2;
    public int divisionsCrossSection = 3;
    //頂点の個数
    public int crossSectionPointSize;

    /* 断面データ */
    //断面の制御点リスト
    public Vector3[] controlPoint;
    //制御点から生成される頂点リスト
    public Vector3[] crossSectionPoint;

    //test
    Vector3[] a;
    Vector3[] b;

    /* パラメータ */
    //断面の中心と制御点との距離  12個の断面，制御点は8個
    float[][] controlPointDistance;
    //各断面の中心と制御点との距離 8個
    float[] controlPointDistanceEach;

    /*
     * **************************************************************
     *
     * 断面の作成
     *
     * 断面の制御点順番
     *
     *     2
     *   3   1
     * 4       0
     *   5   7
     *     6
     *
     * 変形は左右対称に行う
     *
     * **************************************************************
     */

    //制御点の生成
    private Vector3[] CreateControlPoint()
    {
        //制御点を生成するための元
        Vector3 generationPint1 = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 generationPint2 = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 generationPint3 = new Vector3(-1.0f, 0.0f, 0.0f);
        Vector3 generationPint4 = new Vector3(0.0f, -1.0f, 0.0f);

        //生成した点を一時格納するリスト
        List<Vector3> pointList = new List<Vector3>();


        //左上の制御点
        tools.CatmullRomSplineCurve(generationPint4, generationPint1, generationPint2, generationPint3, divisionsControlPoint, pointList);
        //右上の制御点
        tools.CatmullRomSplineCurve(generationPint1, generationPint2, generationPint3, generationPint4, divisionsControlPoint, pointList);
        //右下の制御点
        tools.CatmullRomSplineCurve(generationPint2, generationPint3, generationPint4, generationPint1, divisionsControlPoint, pointList);
        //左下の制御点
        tools.CatmullRomSplineCurve(generationPint3, generationPint4, generationPint1, generationPint2, divisionsControlPoint, pointList);

        //配列としてリストを返す
        Vector3[] pointArray = pointList.ToArray();

        return pointArray;
    }

    /*
     * 御点から断面の頂点を生成
     * 引数
     * array:制御点のList
    */
    private Vector3[] CreateCrossSectionPoints(Vector3[] array)
    {
        //制御点のリストを取得する
        List<Vector3> controlList = new List<Vector3>(array);
        int controlListSize = controlList.Count;

        //生成したリスト
        List<Vector3> createList = new List<Vector3>();

        /*---制御点リストから断面の頂点を作る---*/
        for (int i = 0; i <controlListSize; i++)
        {
            int p1 = i - 1;
            int p2 = i;
            int p3 = i + 1;
            int p4 = i + 2;

            //p1，p3，p4が配列外に出たとき修正する
            if(p1 < 0)
            {
                p1 += 8;
            }
            if(p3 >= controlListSize)
            {
                p3 = p3 - 8;
            }
            if(p4 >= controlListSize)
            {
                p4 = p4 - 8;
            }

            tools.CatmullRomSplineCurve(controlList[p1], controlList[p2], controlList[p3], controlList[p4], divisionsCrossSection, createList);
        }

        //配列としてリストを返す
        Vector3[] createArray = createList.ToArray();

        return createArray;
    }

    /*
     * 断面の生成
     * 引数
     * parameterArray:パラメータの配列
     */
    public Vector3[] MakeCrossSection(float[] parameterArray)
    {
        //制御点の生成
        controlPoint = CreateControlPoint();
        //制御点の個数を取得
        controlPointSize = controlPoint.Length;

        //制御点にパラメータを適用
        for (int i = 0; i < controlPointSize; i++)
        {
            controlPoint[i] = controlPoint[i] * parameterArray[i];
            Debug.Log("parameterArray[" + i + "]" + parameterArray[i]);
        }

        //制御点から断面頂点を生成
        crossSectionPoint = CreateCrossSectionPoints(controlPoint);
        //断面頂点の個数を取得
        crossSectionPointSize = crossSectionPoint.Length;

        return crossSectionPoint;
    }
    /*
     * **************************************************************
     *
     * 断面の変更
     *
     * **************************************************************
     */

    /*
     * 制御点の変更
     * 引数
     * nowControlPoint:現在の制御点リスト
     * parameterArray:各断面の中心と制御点との距離
     */
    public Vector3[] ChangeControlPoint(Vector3[] nowControlPoint, float[] parameterArray)
    {
        //現在の制御点のリストを取得する
        Vector3[] controlList = nowControlPoint;
        int controlListSize = controlList.Length;

        //パラメータから変形された制御点と中心とのきょり倍率を取得
        float[] magnification = parameterArray;

        //制御点に倍率を適応
        for(int i = 0; i < controlListSize; i++)
        {
            Vector3 point = controlList[i] * magnification[i];
            controlList[i] = point;
        }

        return controlList;
    }


    // Start is called before the first frame update
    void Start()
    {
        //ツールスクリプトの取得
        tools = GameObject.Find("Tools").GetComponent<Tools>();
        //パラメータスクリプトを取得
        parameter = GameObject.Find("BaseParameter").GetComponent<BaseParameter>();

        //パラメータの配列を取得
        controlPointDistance = parameter.test_allControlPointDistance;

        //テスト用の断面を生成
        a = MakeCrossSection(controlPointDistance[0]);
        MakeMesh();
        foreach (var point in a)
        {
            //Debug.Log(point);
        }
    }

    int z = 1;
    // Update is called once per frame
    void Update()
    {
        //パラメータが変化したとき
        bool checkparameter = parameter.makeParameterEnd;
        if (checkparameter == true)
        {
            z += 1;
            //パラメータの配列を取得
            controlPointDistance = parameter.test_allControlPointDistance;
            //断面の再生性
            a = MakeCrossSection(controlPointDistance[0]);
            //makeParameterEndをリセット
            parameter.makeParameterEnd = false;
            foreach (var point in a)
            {
                //Debug.Log(z+"change"+point);
            }
            MakeMesh();
        }
    }



    /*
     * 確認
     */
    //生成するメッシュ
    private Mesh mesh;
    //面を構成するインデックスリスト
    private List<int> triangles = new List<int>();
    private Mesh CreateCrossSectionMeshs()
    {
        //メッシュ作成
        mesh = new Mesh();

        // 面を構成するインデックスリストを作成
        for (int i = 0; i < 20; i++)
        {
            triangles.Add(0);
            triangles.Add(0 + i + 2);
            triangles.Add(0 + i + 1);
        }

        // メッシュに頂点を登録する
        mesh.SetVertices(crossSectionPoint);
        // メッシュに面を構成するインデックスリストを登録する
        mesh.SetTriangles(triangles, 0);

        return mesh;
    }
    void MakeMesh()
    {
        //メッシュを作る
        mesh = CreateCrossSectionMeshs();
        // 作成したメッシュをメッシュフィルターに設定する
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }


    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        foreach (var point in crossSectionPoint)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }

    }
}
