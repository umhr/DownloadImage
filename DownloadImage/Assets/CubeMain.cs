using UnityEngine;
using System;
using System.IO;
using System.Net;

public class CubeMain : MonoBehaviour {

    private GameObject containerCube;
    // Use this for initialization
    void Start () {

        // 動きの中心となる、Cubeを作る。
        containerCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        containerCube.GetComponent<Renderer>().material.color = new Color(1, 0, 0);

        // 非表示の場合は、BoxColliderとMeshRendererはfalseにする。
        //cube0.GetComponent<BoxCollider>().enabled = false;
        //cube0.GetComponent<MeshRenderer>().enabled = false;

        int n = 51;
        for (int i = 0; i < n; i++)
        {
            float tx = (float)(10 * Math.Cos(Math.PI * 2 * ((float)i / n)));
            float tz = (float)(10 * Math.Sin(Math.PI * 2 * ((float)i / n)));
            string url = "images/image" + i + ".png";
            url = "http://192.168.0.30/" + url;
            setQuad(url, tx, 0, tz);
        }
        
    }

    private void setQuad(string url, float tx, float ty, float tz)
    {
        // Quadを作って、Textureを設定して、親にくっつける。
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = new Vector3(tx, ty, tz);
        quad.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        quad.GetComponent<Renderer>().material.mainTexture = ReadTexture(url, 512, 512);
        quad.transform.parent = containerCube.transform;
    }

    // http上から取得
    // 参考　http://www.moonmile.net/blog/archives/3157
    private byte[] ReadHttpFile(string path)
    {
        return new WebClient().DownloadData(path);
    }

    // ローカルのファイルを取得
    // 参考　http://macomu.sakura.ne.jp/blog/?p=55
    private byte[] ReadLocalFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);
        bin.Close();
        return values;
    }

    // ファイルを読み込み、Textureにして返す。
    Texture ReadTexture(string path, int width, int height)
    {
        byte[] readBinary;
        if (path.Substring(0, 4) == "http")
        {
            // httpから始まる場合は、web上にあるとみなす。
            readBinary = ReadHttpFile(path);
        }
        else
        {
            readBinary = ReadLocalFile(path);
        }

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(readBinary);
        return texture;
    }


    // Update is called once per frame
    void Update () {
        if(containerCube != null) {
            containerCube.transform.Rotate(0.0f, -0.2f, 0.0f);
        }
    }
}
