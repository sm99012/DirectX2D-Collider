using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Mesh_1 : MonoBehaviour
{
    public Material[] m_gm_Ground;

    Vector3[] vVertices = new Vector3[3];
    Vector2[] vUvs = new Vector2[3];
    int[] nTriangles = new int[3];

    // num_ground == 0 : бс бс бс жв num_ground == 1 : бр бр бс
    //                   бс бс бр жв                   бр бс бс
    //                   бс бр бр жв                   бс бс бс
    public void Create_Mesh_1_1(int num_ground, Vector3[] v_vertices)
    {
        Mesh mesh = new Mesh();

        if (num_ground == 0)
        {
            vUvs[0] = new Vector2(0f, 0f);
            vUvs[1] = new Vector2(0f, 1f);
            vUvs[2] = new Vector2(1f, 1f);

            nTriangles[0] = 0;
            nTriangles[1] = 1;
            nTriangles[2] = 2;
        }
        else
        {
            vUvs[0] = new Vector2(0f, 0f);
            vUvs[1] = new Vector2(1f, 1f);
            vUvs[2] = new Vector2(1f, 0f);

            nTriangles[0] = 0;
            nTriangles[1] = 1;
            nTriangles[2] = 2;
        }

        //mesh.vertices = vVertices;
        mesh.vertices = v_vertices;
        mesh.uv = vUvs;
        mesh.triangles = nTriangles;
        mesh.RecalculateNormals();

        //this.gameObject.GetComponent<MeshRenderer>().material = m_gm_Ground[num_ground];
        this.gameObject.GetComponent<MeshRenderer>().material = m_gm_Ground[1];
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh;

        //try
        //{
        //    this.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        //}
        //catch(Exception e)
        //{
        //    Debug.Log(e);
        //}
        //this.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        //this.gameObject.GetComponent<MeshCollider>().convex = true;
        //Debug.Log(this.gameObject.GetComponent<MeshCollider>().bounds);

        this.gameObject.name = "Map Mesh(" + v_vertices[0].x + "," + v_vertices[0].y + ", " + v_vertices[0].z + ")";
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        this.gameObject.transform.SetParent(GameObject.Find("Map Mesh GameObject Set").transform);

        Destroy(this.GetComponent<MeshCollider>());
        this.gameObject.AddComponent<MeshCollider>();
    }
}
