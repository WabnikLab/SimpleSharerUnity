// Builds a Mesh containing a single triangle with uvs.
// Create arrays of vertices, uvs and triangles, and copy them into the mesh.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Collections;

public class GPUTriangle : MonoBehaviour
{

	[SerializeField]
	ComputeShader computeShader;
    [SerializeField]
	Material material;
	[SerializeField]
   bool useGPU;

    // Use this for initialization
    public Mesh mesh;

    [SerializeField]
    int num = 30 ;
		private Bounds bounds;


    ComputeBuffer positionsBuffer;


		private ComputeBuffer argsBuffer;
    private ComputeBuffer positionBuffer;


    int kernel;
    uint threadGroupSize;
int threadGroups;

private ComputeBuffer GetArgsBuffer(uint count)
		{
			uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
			args[0] = (uint)mesh.GetIndexCount(0);
			args[1] = (uint)count;
			args[2] = (uint)mesh.GetIndexStart(0);
			args[3] = (uint)mesh.GetBaseVertex(0);
			args[4] = 0;

			ComputeBuffer buffer = new ComputeBuffer(args.Length, sizeof(uint), ComputeBufferType.IndirectArguments);
			buffer.SetData(args);
			return buffer;
		}

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        Build_Triangle();
				bounds = new Bounds(Vector3.zero, new Vector3(100000, 100000, 100000));

        argsBuffer = GetArgsBuffer((uint)num);
        positionBuffer = new ComputeBuffer(num, 4*4);
        Vector4[] positions = new Vector4[num];
        for (int i = 0; i < num; i++) {
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            float distance = Random.Range(10.0f, 10.0f);
            float height = Random.Range(-20f, 20f);
            float size = Random.Range(0.05f, 0.25f);
            positions[i] = new Vector4(Mathf.Sin(angle) * distance, height, Mathf.Cos(angle) * distance, size);
        }
        positionBuffer.SetData(positions);
        material.SetBuffer("positionBuffer", positionBuffer);


    }


  void OnDestroy () {
		argsBuffer.Dispose();
    positionBuffer.Dispose();
	}


Vector3 rotatePoint(Vector3 p, Vector3 c, float r) {
        p[0] = c[0] + (p[0]-c[0]) * Mathf.Cos(r) - (p[1]-c[1])*Mathf.Sin(r);
        p[1] = c[1] + (p[0]-c[0]) * Mathf.Sin(r) + (p[1]-c[1])*Mathf.Cos(r);
        return p;

    }



void UpdateFunctionOnCPU()
    {        
        Vector3[] vertices = mesh.vertices;
        for(int i = 0; i <  num-2; i = i+3) {
   
          vertices[i][0] += 0.01f;
          vertices[i+1][0] += 0.01f;
          vertices[i+2][0] += 0.01f;
        }
        mesh.vertices = vertices;
        
    }

	void UpdateFunctionOnGPU () {    

        computeShader.Dispatch(kernel, threadGroups, 1, 1);        
        Vector3[] vertices = new Vector3[num];
        positionsBuffer.GetData(vertices);
        mesh.vertices = vertices;
        
	}


	void Update () {

        /*
        Vector4[] positions = new Vector4[num];
        for (int i = 0; i < num; i++) {
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            float distance = Random.Range(1.0f, 10.0f);
            float height = Random.Range(-2.0f, 2.0f);
            float size = Random.Range(0.05f, 0.25f);
            positions[i] = new Vector4(Mathf.Sin(angle) * distance, height, Mathf.Cos(angle) * distance, size);
        }
        positionBuffer.SetData(positions);*/
				Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
	/*
    if(!useGPU)
      UpdateFunctionOnCPU();
    else  
		  UpdateFunctionOnGPU();
      */
	}

	protected void Build_Triangle()
		{
			mesh = new Mesh();
			mesh.Clear();
			Vector3[] vertices = new Vector3[3]
			{
				new Vector3(0, 0, 0),
				new Vector3(0, 1, 0),
				new Vector3(1, 1, 0),
			
			};

			Vector3[] normals = new Vector3[3]
			{
				Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized,
				Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized,
				Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized,
			
			};

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.triangles = Enumerable.Range(0, vertices.Length).ToArray();

			mesh.uv2 = new Vector2[3]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
			
			};

			mesh.uv = new Vector2[3]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
			
			};

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;

		}

   

}