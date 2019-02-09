using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private int frameCounter = 0;
    private Material _doorMaterial;
    private bool CloseOpen = false; // false == open
    private int TilesX = 13;

    // Start is called before the first frame update
    void Start()
    {
        float size = 1f;
        Vector3[] vertices0 =
        {
            // right
            new Vector3(size, 0, 0),
            new Vector3(size, size, 0),
            new Vector3(size, size, size),
            new Vector3(size, 0, size),
        };

        Vector2[] uvs0 =
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
        };
        AddWall("door13_strip", vertices0, uvs0);

    }

    // Update is called once per frame
    void Update()
    {
        _doorMaterial.mainTextureScale = new Vector2(1.0f / TilesX, 1.0f / 1);
        StartCoroutine("PlayLoop", 0.10f);
    }

    IEnumerator PlayLoop(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (CloseOpen && frameCounter == 0)
        {
            CloseOpen = !CloseOpen;
        }

        if (!CloseOpen && frameCounter == TilesX - 1)
        {
            CloseOpen = !CloseOpen;
        }

        if (CloseOpen)
        {
            frameCounter--;
        }
        else
        {
            frameCounter++;
        }

        _doorMaterial.mainTextureOffset = new Vector2(1.0f / TilesX * frameCounter, 1.0f / 1);
        
        StopCoroutine("PlayLoop");
    }

    private void AddWall(string textureName, Vector3[] vertices, Vector2[] uvs)
    {
        int[] triangles =
        {
            0, 1, 2,
            2, 3, 0,
        };

        var door = new GameObject();
        Instantiate(door);

        var mesh = new Mesh();
        var meshFilter =
            (UnityEngine.MeshFilter)
            door.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

        // mesh renderer
        var meshRenderer =
            (UnityEngine.MeshRenderer)
            door.AddComponent(typeof(MeshRenderer));

        _doorMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        meshRenderer.materials = new Material[1];
        meshRenderer.materials[0] = _doorMaterial;

        var texture = Resources.Load<Texture2D>($"Textures/{textureName}");

        _doorMaterial.mainTexture = texture;

        meshRenderer.material = _doorMaterial;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
