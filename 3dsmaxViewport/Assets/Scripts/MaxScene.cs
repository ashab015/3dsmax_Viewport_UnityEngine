using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaxNode
{
    public MaxNode()
    {
        name = "Ivalid";
        position = new Vector3(0,0,0);
        rotation = new Vector3(0,0,0);
        Created = false;
    }
    public bool Created;
    public string name;
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;
    public Vector3[] normals;

    public Mesh mesh;
    public Vector3 position;
    public Vector3 rotation;
    public GameObject go;
    public Material mat;
    public void Init()
    {
        if (Created == true)
            return;

        mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.normals = normals;
        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();

        go = new GameObject();
        go.name = name;
        go.transform.position = position;
        go.transform.eulerAngles = rotation;

        go.AddComponent<MeshFilter>();
        go.GetComponent<MeshFilter>().mesh = mesh;

        go.AddComponent<MeshRenderer>();
        go.AddComponent<MaxSceneNode>();
        go.AddComponent<MeshCollider>();
        go.GetComponent<MeshCollider>().sharedMesh = mesh;

        go.AddComponent<MaxSceneNode>();
        go.GetComponent<MaxSceneNode>().name = name;

        Material newMat = new Material(Shader.Find("Standard"));
        newMat.color = Color.grey;
        go.GetComponent<Renderer>().material = newMat;

        Created = true;
    }

}

public class MaxScene : MonoBehaviour {


    public List<MaxNode> maxnodes;
    public int SelectedNode = -1;
    public Material SelectedNodeMaterial;
    private bool running = false;

	// Use this for initialization
	void Start () {
        SelectedNode = -1;
        maxnodes = new List<MaxNode>();
        running = true;
    }

    // Update is called once per frame
    void Update()
    {

        // update new nodes
        for (int x = 0; maxnodes.Count > x; x++)
        {
            maxnodes[x].Init();
        }

        // select nodes
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0) == true)
        {
            if (hit.transform.gameObject.GetComponent<MaxSceneNode>() != null)
            {
                int index = GetNodeByName(hit.transform.gameObject.GetComponent<MaxSceneNode>().name);
                SelectedNode = index;
            }
        }

    }

    void OnPostRender()
    {
        SelectedNodeMaterial.SetPass(0);
        int selectednode = SelectedNode;
        if (selectednode == -1)
            return;
        MaxNode node = maxnodes[selectednode];

        GL.Begin(GL.LINES);

        for (int x=2; node.triangles.Length > x; x+=3)
        {
            int i1 = node.triangles[x-2];
            int i2 = node.triangles[x - 1];
            int i3 = node.triangles[x - 0];
            Vector3 pos1 = node.vertices[i1] + node.position;
            Vector3 pos2 = node.vertices[i2] + node.position;
            Vector3 pos3 = node.vertices[i3] + node.position;

            GL.Vertex3(pos1.x, pos1.y, pos1.z);
            GL.Vertex3(pos2.x, pos2.y, pos2.z);

            GL.Vertex3(pos2.x, pos2.y, pos2.z);
            GL.Vertex3(pos3.x, pos3.y, pos3.z);

            GL.Vertex3(pos1.x, pos1.y, pos1.z);
            GL.Vertex3(pos3.x, pos3.y, pos3.z);
        }

        GL.End();

    }

    void OnDrawGizmos()
    {
        if (running == false)
            return;
        SelectedNodeMaterial.SetPass(0);
        int selectednode = SelectedNode;
        if (selectednode == -1)
            return;
        MaxNode node = maxnodes[selectednode];

        GL.Begin(GL.LINES);

        for (int x = 2; node.triangles.Length > x; x += 3)
        {
            int i1 = node.triangles[x - 2];
            int i2 = node.triangles[x - 1];
            int i3 = node.triangles[x - 0];
            Vector3 pos1 = node.vertices[i1] + node.position;
            Vector3 pos2 = node.vertices[i2] + node.position;
            Vector3 pos3 = node.vertices[i3] + node.position;

            GL.Vertex3(pos1.x, pos1.y, pos1.z);
            GL.Vertex3(pos2.x, pos2.y, pos2.z);

            GL.Vertex3(pos2.x, pos2.y, pos2.z);
            GL.Vertex3(pos3.x, pos3.y, pos3.z);

            GL.Vertex3(pos1.x, pos1.y, pos1.z);
            GL.Vertex3(pos3.x, pos3.y, pos3.z);
        }

        GL.End();

    }

    public int GetNodeByName(string name)
    {
        int index = -1;

        for (int x=0; maxnodes.Count > x; x++)
        {
            if (maxnodes[x].name == name)
            {
                index = x;
            }
        }
        return index;
    }
    public void DeleteNode(int index)
    {
        GameObject.Destroy(maxnodes[index].go);
        maxnodes.RemoveAt(index);    
    }
    public void ClearScene()
    {

        // Delete Nodes
        for (int x = 0; maxnodes.Count > x; x++)
        {
            GameObject.Destroy(maxnodes[x].go);
            maxnodes.RemoveAt(x);
        }
    }
    

}
