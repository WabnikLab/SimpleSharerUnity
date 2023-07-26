// Builds a Mesh containing a single triangle with uvs.
// Create arrays of vertices, uvs and triangles, and copy them into the mesh.

using UnityEngine;

public class Triangles : MonoBehaviour
{
    // Use this for initialization
    Mesh mesh;
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();


     int num = 3000000;
    Vector3[] vertices = new Vector3[num];
    int[] indices = new int[num];
         Vector3 A = new Vector3(0,0,0);
        Vector3 B = new Vector3(1,0,0);
        Vector3 C = new Vector3(0,1,0);
        int mult = 10;
        for(int i = 0; i <  num-2; i = i+3) {
          float randomx = Random.value*mult - (mult/2);  
          float randomy = Random.value*mult - (mult/2);  
          vertices[i] = new Vector3(0+randomx,0+randomy,20); 
          vertices[i+1] = new  Vector3(0.1f+randomx,0+randomy,20);
          vertices[i+2] = new  Vector3(0+randomx,0.1f+randomy,20);
          indices[i] =  i+2 ;
          indices[i+1] =   i+1;
          indices[i+2] =   i+0;
        }

        mesh.vertices = vertices;
        mesh.triangles =  indices;
    }


    Vector3 rotatePoint(Vector3 p, Vector3 c, float r) {
        p[0] = c[0] + (p[0]-c[0]) * Mathf.Cos(r) - (p[1]-c[1])*Mathf.Sin(r);
        p[1] = c[1] + (p[0]-c[0]) * Mathf.Sin(r) + (p[1]-c[1])*Mathf.Cos(r);
        return p;

    }

    void Update()
    {
        
        Vector3[] vertices = mesh.vertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i][0] += 0.001f;
        }
               mesh.vertices = vertices;
        
    }
}