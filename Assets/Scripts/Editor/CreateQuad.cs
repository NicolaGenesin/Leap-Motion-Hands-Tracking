using UnityEngine;
using UnityEditor;
using System.Collections;

public static class CreateQuad
{
    #region Mesh properties
    
    static private string meshFilename = "Assets/Art/Meshes/SimpleQuad.asset";

    static private Vector3[] vertices = {
        new Vector3(-0.5f,-0.5f),
        new Vector3( 0.5f,-0.5f),
        new Vector3(-0.5f, 0.5f),
        new Vector3( 0.5f, 0.5f)
        };

    static private Vector2[] uvs = {
        new Vector2(0,0),
        new Vector2(1,0),
        new Vector2(0,1),
        new Vector2(1,1)
        };

    static private int[] triangles = { 2, 1, 0, 1, 2, 3 };

    #endregion Mesh Properties
    

    static Mesh GetOrCreateMesh()
    {
        // Look for existing quad mesh asset
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(meshFilename, typeof(Mesh));
        
        // Create new quad mesh if there is no existing mesh
        if (!mesh)
        {
            // Create new quad mesh using the script's static properties
            mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            // Create and save quad mesh asset
            AssetDatabase.CreateAsset(mesh, meshFilename);
            AssetDatabase.SaveAssets();
        }

        return mesh;
    }

    [MenuItem("Component/Mesh/Quad")]
    static void AddQuadMesh()
    {
        // Determine if can & should alter the GameObjects' mesh components
        bool editMesh = true;

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            // Do ANY of the selected objects have mesh components?
            editMesh = !(gameObject.GetComponent<MeshFilter>());
            if (!editMesh) break;
        }

        if (!editMesh &&
            EditorUtility.DisplayDialog(
                "Edit MeshFilter?",
                "Warning! This action will edit any existing MeshFilter " +
                    "on each of the selected objects. Do you want to continue?",
                "Continue",
                "Cancel"))
            editMesh = true;

        // Add / Edit the mesh
        if (editMesh)
        {
            MeshFilter meshFilter;

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                // Get or add mesh filter
                meshFilter = gameObject.GetComponent<MeshFilter>();
                if (!meshFilter)
                    meshFilter = gameObject.AddComponent<MeshFilter>();

                // add mesh missing mesh renderer
                if (!gameObject.GetComponent<MeshRenderer>())
                    gameObject.AddComponent<MeshRenderer>();

                // Get quad mesh
                Mesh mesh = GetOrCreateMesh();

                // Assign new mesh to mesh filter
                meshFilter.mesh = mesh;
                mesh.RecalculateBounds();
            }
        }
        
    }

    [MenuItem("Component/Mesh/Quad", true)]
    static bool ValidateAddQuadMesh()
    {
        return Selection.activeGameObject != null;
    }
}