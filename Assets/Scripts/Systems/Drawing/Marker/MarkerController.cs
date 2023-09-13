using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    public Camera playerCamera;

    //2d texture of the dot to painy
    public Texture2D dotTexture;
    public Material dotMaterial;
    private List<GameObject> dots = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If the player holds the left mouse button invokes a Raycast from the center of the screen
        // If the Raycast hits an object with the tag "Drawable" it will check the distance between the player and the object
        // If the distance is less than 5 units it will use the lin
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Drawable")
                {
                    if (Vector3.Distance(hit.collider.gameObject.transform.position, playerCamera.transform.position) < 2.5f)
                    {
                        Paint(hit);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // We merge all the dots into one mesh and clear the dots list
            MergeDots();
            ClearDots();
        }
    }

    void Paint(RaycastHit hit)
    {
        //Creates a new gameobject with a smallplane mesh and a material with the texture of the dot
        //Is important that the texture background is transparent and the dot is sticked to the wall, 0.1f distance from the surface
        GameObject paint = new GameObject();
        paint.AddComponent<MeshFilter>();
        paint.AddComponent<MeshRenderer>();
        paint.GetComponent<MeshFilter>().mesh = CreateMesh();
        paint.GetComponent<MeshRenderer>().material = dotMaterial;


        //paint.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Transparent"));
        //paint.GetComponent<MeshRenderer>().material.mainTexture = dotTexture;
        //Sets the position of the paint object to the position of the raycast hit
        paint.transform.position = hit.point;
        //Sets the rotation of the paint object to the rotation of the raycast hit
        paint.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        //Sets the scale of the paint object to 0.07 in all axis
        paint.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        //Sets the parent of the paint object to the object that was hit by the raycast
        paint.transform.parent = hit.collider.gameObject.transform;
        dots.Add(paint);
    }

    //Creates a mesh for the paint object
    Mesh CreateMesh()
    {
        Mesh m = new Mesh();
        //Sets the vertices of the mesh
        m.vertices = new Vector3[] { new Vector3(-1, 0, -1), new Vector3(-1, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, -1) };
        //Sets the UVs of the mesh
        m.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        //Sets the triangles of the mesh
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        //Recalculates the normals of the mesh
        m.RecalculateNormals();
        //Returns the mesh
        return m;
    }

    void MergeDots()
    {
        GameObject mergedDots = new GameObject();
        mergedDots.name = "Painting";
        List<MeshFilter> sourceMeshFilters = new List<MeshFilter>();
        MeshFilter targetMeshFilter;
        targetMeshFilter = mergedDots.AddComponent<MeshFilter>();
        mergedDots.AddComponent<MeshRenderer>();
        mergedDots.GetComponent<MeshRenderer>().material = dotMaterial;
        //mergedDots.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Transparent"));
        //mergedDots.GetComponent<MeshRenderer>().material.mainTexture = dotTexture;

             
        foreach (var dot in dots)
        {
            sourceMeshFilters.Add(dot.GetComponent<MeshFilter>());
        }
        var combine = new CombineInstance[sourceMeshFilters.Count];

        for (var i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        targetMeshFilter.mesh = mesh;
    }

    void ClearDots()
    {
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }
}
