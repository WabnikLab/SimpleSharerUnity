using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Mathematics.LinearAlgebra;
using Common.Geometry.Shapes;
using Common.Unity.Drawing;
using Common.Unity.Mathematics;


public class Triangle {
    public Vector3d[] p;
    public int[] v;

    public Triangle(Vector3d A, Vector3d B, Vector3d C, int v1, int v2, int v3) {
        p = new Vector3d[3];
        v = new int[3];
        p[0] = A;
        p[1] = B;
        p[2] = C;
        v[0] = v1;
        v[1] = v2;
        v[2] = v3;
        
    }
}

public class Grid : MonoBehaviour
{
    private const int GRID_SIZE = 10;
    static int num = 1000000;
    Triangle[] triangles = new Triangle[num];

    // Start is called before the first frame update
    void Start()
    {
        Vector3d A = new Vector3d(0,0,0);
        Vector3d B = new Vector3d(1,0,0);
        Vector3d C = new Vector3d(0,1,0);
        int mult = 20;
        for(int i = 0; i <  num; i++) {
          double randomx = Random.value*mult - (mult/2);  
          double randomy = Random.value*mult - (mult/2);  
          triangles[i] = new Triangle(new Vector3d(0+randomx,0+randomy,0), 
                                      new  Vector3d(1+randomx,0+randomy,0), 
                                      new  Vector3d(0+randomx,1+randomy,0), 0, 1, 2);

        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i <  num; i++) {
                Triangle triangle = triangles[i];
                Vector3d c = (triangle.p[0] + triangle.p[1] + triangle.p[2]) / 3;
                triangle.p[0] = rotatePoint(triangle.p[0], c, 0.001f);
                triangle.p[1] = rotatePoint(triangle.p[1], c, 0.001f);
                triangle.p[2] = rotatePoint(triangle.p[2], c, 0.001f);
        }
    }


    Vector3d rotatePoint(Vector3d p, Vector3d c, float r) {
        p[0] = c[0] + (p[0]-c[0]) * Mathf.Cos(r) - (p[1]-c[1])*Mathf.Sin(r);
        p[1] = c[1] + (p[0]-c[0]) * Mathf.Sin(r) + (p[1]-c[1])*Mathf.Cos(r);
        return p;

    }

     private void OnRenderObject()
        {
                Camera camera = Camera.current;

                Vector3 min = new Vector3(-GRID_SIZE, 0, -GRID_SIZE);
                Vector3 max = new Vector3(GRID_SIZE, 0, GRID_SIZE);

                //DrawLines.DrawGrid(camera, Color.white, min, max, 1, transform.localToWorldMatrix);

                Matrix4x4d m = MathConverter.ToMatrix4x4d(transform.localToWorldMatrix);
                
                
                for(int i = 0; i <  num; i++) 
                    DrawLines.DrawVertices(LINE_MODE.TRIANGLES, camera, Color.red, triangles[i].p, triangles[i].v, m);

                //DrawLines.DrawBounds(camera, Color.green, StaticBounds, Matrix4x4d.Identity);

            
        }

}
