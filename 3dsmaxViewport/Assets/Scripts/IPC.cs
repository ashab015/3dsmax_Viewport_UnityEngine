using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using QuickPipes;


public class IPC : MonoBehaviour
{

    public string LastServerMessage;
    public MaxScene scene;
    QuickPipeServer pipeserver;

    // Use this for initialization
    void Start()
    {
        pipeserver = new QuickPipeServer();
        pipeserver.StartServer(2048);
      
    }     

    // Update is called once per frame
    void Update()
    {

        if (LastServerMessage != pipeserver.clientmessage)
        {
            LastServerMessage = pipeserver.clientmessage;
            string[] stringdata = LastServerMessage.Split('$');

            if (stringdata[0] == "NODEDATA")
            {
                scene.ClearScene();
                NODEDATA(stringdata);
            }
        }


    }

    public void OnApplicationQuit()
    {
        pipeserver.StopServer();
    }

    private string[] StringArrayClean(string[] str)
    {
        List<string> lst = str.OfType<string>().ToList();
        for (int x = 0; lst.Count > x; x++)
        {
            if (lst[x] == "" || lst[x] == null)
            {
                lst.RemoveAt(x);
            }
        }
        return lst.ToArray();
    }

    // Message functions from 3dsmax
    void NODEDATA(string[] data)
    {
        if (data.Length != 2)
            return;

        string[] nodedata = data[1].Split('%');

        for (int x = 1; nodedata.Length > x; x++)
        {
            string[] splitnodedata = nodedata[x].Split(';');
            string name = splitnodedata[0];
            string[] verts = splitnodedata[1].Split('|');
            string[] faces = splitnodedata[2].Split('|');
            string[] uv = splitnodedata[3].Split('|');
            string[] normals = splitnodedata[4].Split('|');
            string[] trans = splitnodedata[5].Split('|');
            verts = StringArrayClean(verts);
            faces = StringArrayClean(faces);
            uv = StringArrayClean(uv);
            normals = StringArrayClean(normals);
            trans = StringArrayClean(trans);

            List<Vector3> vertexs = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector3> normalval = new List<Vector3>();

            // verts
            for (int i = 2; verts.Length > i; i += 3)
            {
                Vector3 vert = new Vector3(Convert.ToSingle(verts[i - 2]), Convert.ToSingle(verts[i - 1]), Convert.ToSingle(verts[i - 0]));
                vertexs.Add(vert);
            }
            // faces
            for (int i = 0; faces.Length > i; i++)
            {
                indices.Add(Convert.ToInt32(faces[i]));
            }
            // uv
            List<Vector2> uvw = new List<Vector2>();
            for (int i=1; uv.Length > i; i+=2)
            {
                Vector2 vw = new Vector2(Convert.ToSingle(uv[i - 1]), Convert.ToSingle(uv[i - 0]));
                uvw.Add(vw);
            }
            // normals
            for (int i = 2; normals.Length > i; i += 3)
            {
                Vector3 norm = new Vector3(Convert.ToSingle(normals[i - 2]), Convert.ToSingle(normals[i - 1]), Convert.ToSingle(normals[i - 0]));
                normalval.Add(norm);
            }

            MaxNode mn = new MaxNode();
            mn.vertices = vertexs.ToArray();
            mn.triangles = indices.ToArray();
            mn.uv = uvw.ToArray();
            mn.normals = normalval.ToArray();
            mn.position = new Vector3(Convert.ToSingle(trans[0]), Convert.ToSingle(trans[1]), Convert.ToSingle(trans[2]));
            mn.name = name;
            scene.maxnodes.Add(mn);
        }


    }

    Vector3[] GenerateNormals(List<Vector3> points, List<int> triangles)
    {
        Vector3[] normals = new Vector3[points.Count];
        int[] counter = new int[points.Count];
        for (int x=0; points.Count > x; x++)
        {
            counter[x] = 0;
        }
        for (int x=2; triangles.Count > x; x+=3)
        {
            int i1 = triangles[x - 2];
            int i2 = triangles[x - 1];
            int i3 = triangles[x - 0];

            Vector3 normal = Vector3.Cross((points[i2] - points[i1]), (points[i3] - points[i1]));
            normal.Normalize();

            normals[i1] += normal;
            normals[i2] += normal;
            normals[i3] += normal;

            counter[i1] += 1;
            counter[i2] += 1;
            counter[i3] += 1;
        }
        for (int x = 0; points.Count > x; x++)
        {
            normals[x] = normals[x] / counter[x];
        }
        return normals;

    }


}
