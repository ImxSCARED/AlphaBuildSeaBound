//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///*===================================================================
//    -----------------------------------------------------------------
//   _____________________
//   \                   /
//    \_____       _____/   _________               ____
//          |     |        /   _____  \     _______[____]_______
//          |     |       /   /     \__\    |       ____       |
//          |     |       |   |       ___   |-------\()/-------|
//          |     |       |   |      /  /   |__________________|
//          |     |        \   \____/  /    ____________________
//          |_____|    ()   \_________/    /  TERRAIN CREATOR  |
//    -----------------------------------------------------------------
// ===================================================================*/

//[System.Serializable]
//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(MeshCollider))]

//[DisallowMultipleComponent()]

//public class TerrainType
//{
//    public string m_Name;
//    public float m_Height;
//    public Color m_ColourBio;
//}

//public static class NoiseCreator
//{
//    static Vector2[] m_OctOffsets;
//    static float m_MaxHeight = float.MinValue;
//    static float m_MinHeight = float.MaxValue;

//    static float m_OffsetX;
//    static float m_OffsetY;

//    static int m_Spacer = 100000;

//    public static float[,] CreateNewMap(int _width, int _height, int _seed, float _scale, int _octaves, float _persistance, float _lacunarity, Vector2 _Offsets)
//    {
//        float[,] newMap = new float[_width, _height];

//        if (_scale <= 0)
//        {
//            _scale = 0.5f;
//        }

//        float Halfwidth = _width / 2f;
//        float Halfheight = _height / 2f;

//        System.Random rnd = new System.Random(_seed);

//        m_OctOffsets = new Vector2[_octaves];
//        for (int i = 0; i < _octaves; i++)
//        {
//            m_OffsetX = rnd.Next(-m_Spacer, m_Spacer) + _Offsets.x;
//            m_OffsetY = rnd.Next(-m_Spacer, m_Spacer) + _Offsets.y;
//            m_OctOffsets[i] = new Vector2(m_OffsetX, m_OffsetY);
//        }

//        for (int Y = 0; Y < _height; Y++)
//        {
//            for (int X = 0; X < _width; X++)
//            {

//                float amp = 1;
//                float freq = 1;

//                float height = 0;

//                for (int i = 0; i < _octaves; i++)
//                {

//                    float TempX = (X - Halfwidth) / _scale * freq + m_OctOffsets[i].x;
//                    float TempY = (Y - Halfheight) / _scale * freq + m_OctOffsets[i].y;

//                    float createdNoise = Mathf.PerlinNoise(TempX, TempY) * 2 - 1;

//                    height += createdNoise * amp;
//                    amp *= _persistance;
//                    freq *= _lacunarity;
//                }

//                if (height > m_MaxHeight)
//                {
//                    m_MaxHeight = height;
//                }
//                else if (height < m_MinHeight)
//                {
//                    m_MinHeight = height;
//                }

//                newMap[X, Y] = height;
//            }
//        }

//        for (int Y = 0; Y < _height; Y++)
//        {
//            for (int X = 0; X < _width; X++)
//            {
//                newMap[X, Y] = Mathf.InverseLerp(m_MinHeight, m_MaxHeight, newMap[X, Y]);
//            }
//        }
//        return newMap;
//    }
//}

////===========================================================================

////                           MAIN SYSTEM 

////===========================================================================

//public class TerrainCreator : MonoBehaviour
//{
//    [Header("Terrain Settings")]
//    [Range(2, 500)]
//    [Tooltip("Length of the Terrain")]
//    [SerializeField] int m_LengthX = 20;

//    [Range(2, 500)]
//    [Tooltip("Width of the Terrain")]
//    [SerializeField] int m_LengthZ = 20;

//    //=====================================================
//    [Tooltip("Seed for generating unique environments.")]
//    [SerializeField] int m_WorldSeed = 4;

//    [Tooltip("Chose the amount of Octaves in the noise.")]
//    [SerializeField] int m_Octaves = 4;

//    [Tooltip("Chose the persistance to affect noise.")]
//    [Range(0, 1)]
//    [SerializeField] float m_Persistance = 0.5f;

//    [Tooltip("Chose the amount of Lacunarity.")]
//    [SerializeField] float m_Lacunarity = 2;

//    [Tooltip("Apply Offsets to the world.")]
//    [SerializeField] Vector2 m_Offsets;

//    //=====================================================
//    [Space()]
//    [Tooltip("Change the size of the hills when generated")]
//    [SerializeField] float m_HillSize = 5f;

//    [Tooltip("Adjust the Terrain surface")]
//    [SerializeField] AnimationCurve m_Height;

//    [Range(0.1f, 1f), Tooltip("Adjust the roughness of the surface.")]
//    [SerializeField] float m_Roughness = 0.3f;

//    //==========================================
//    // Environment

//    [Header("Environment"), Space(), Tooltip("light source that will be treated as the sun.")]
//    [SerializeField] Light m_Sun = null;

//    [Tooltip("Pick the speed that the sun will rotate at.")]
//    [SerializeField] float m_SunSpeed;

//    [Tooltip("Chose the colours & heights of biomes"), Space()]
//    [SerializeField] TerrainType[] m_Terrains;

//    //==========================================
//    // Gizmos In Editor

//    [Header("Gizmos"), Tooltip("Toggle drawing points in Editor")]
//    [SerializeField] bool m_DrawGizmos = true;

//    //===========================================
//    // Non Exposed Vars
//    Mesh m_MeshUsed;
//    float m_SunYPos;

//    Texture2D m_TerrainTexture;
//    float[,] m_HeightMap;

//    Vector3[] m_Vertices;
//    int[] m_Triangles;

//    //===========================================
//    // BackUp Values

//    int m_LengthXSave;
//    int m_LengthZSave;

//    float m_HillSizeSave;
//    float m_RoughSave;

//    bool m_SunRotation = false;

//    //===========================================
//    void Start()
//    {
//        StartMeshCreator();
//    }

//    //===========================================

//    private void StartMeshCreator()
//    {
//        m_MeshUsed = new Mesh();

//        GetComponent<MeshFilter>().mesh = m_MeshUsed;
//        m_MeshUsed.name = "Terrain Creator Mesh";


//        InitializeMeshData();
//    }

//    private void Update()
//    {
//        UpdateSunRoute();
//    }

//    void InitializeMeshData()
//    {
//        //============================================================      TODO Rework 
//        // Place vertices into an array for the mesh data

//        m_Vertices = new Vector3[(m_LengthX + 1) * (m_LengthZ + 1)];


//        float Xoffset = -gameObject.transform.position.x;
//        float Zoffset = -gameObject.transform.position.z;

//        float[,] NoiseMap = NoiseCreator.CreateNewMap(m_LengthZ, m_LengthX, m_WorldSeed, m_Roughness, m_Octaves, m_Persistance, m_Lacunarity, m_Offsets);

//        for (int T = 0, Z = 0; Z <= m_LengthZ; Z++)
//        {
//            for (int X = 0; X <= m_LengthX; X++)
//            {
//                float TempZ = (Z + Zoffset) / m_Roughness;
//                float TempX = (X + Xoffset) / m_Roughness;

//                float Y = Mathf.PerlinNoise(TempX, TempZ);

//                m_Vertices[T] = new Vector3(TempX, m_Height.Evaluate(Y) * m_HillSize, TempZ);
//                T++;
//            }
//        }

//        //============================================================
//        // Draw the triangles for each point.

//        m_Triangles = new int[m_LengthX * m_LengthZ * 6];

//        int Vertices = 0;
//        int Triangles = 0;

//        for (int Z = 0; Z < m_LengthZ; Z++)
//        {
//            for (int X = 0; X < m_LengthX; X++)
//            {

//                m_Triangles[Triangles + 0] = Vertices + 0;
//                m_Triangles[Triangles + 1] = Vertices + m_LengthX + 1;
//                m_Triangles[Triangles + 2] = Vertices + 1;
//                m_Triangles[Triangles + 3] = Vertices + 1;
//                m_Triangles[Triangles + 4] = Vertices + m_LengthX + 1;
//                m_Triangles[Triangles + 5] = Vertices + m_LengthX + 2;

//                Vertices++;
//                Triangles += 6;
//            }
//            Vertices++;
//        }
//        UpdateMeshData();
//    }

//    //=======================================================================
//    // This function is unused for now but will be intergrated into the Initalise Function

//    float[,] CreateNoise(int _LengthZ, int _LengthX, float _Roughness)
//    {
//        float[,] NoiseMesh = new float[_LengthZ, _LengthX];


//        for (int Z = 0; Z < _LengthZ; Z++)
//        {
//            for (int X = 0; X < _LengthX; X++)
//            {
//                float TempZ = Z / _Roughness;
//                float TempX = X / _Roughness;

//                float GenNoise = Mathf.PerlinNoise(TempX, TempZ);

//                NoiseMesh[Z, X] = GenNoise;
//            }
//        }

//        return NoiseMesh;
//    }

//    private void UpdateMeshNormals()
//    {
//        m_MeshUsed.RecalculateBounds();
//        m_MeshUsed.RecalculateNormals();
//        m_MeshUsed.RecalculateTangents();
//    }

//    private void UpdateMeshData()
//    {

//        m_MeshUsed.Clear();
//        m_MeshUsed.vertices = m_Vertices;
//        m_MeshUsed.triangles = m_Triangles;
//        UpdateMeshNormals();

//        gameObject.GetComponent<MeshCollider>().sharedMesh = m_MeshUsed;
//    }

//    //========================================================
//    public void ClearData()
//    {
//        BackUpValues();

//        m_LengthX = 2;
//        m_LengthZ = 2;

//        m_Roughness = 0.1f;
//        m_HillSize = 0;

//        InitializeMeshData();
//        UpdateMeshData();
//    }


//    //========================================================
//    // Draw the editor stuff for the system
//    private void OnDrawGizmos()
//    {
//        if (m_Vertices == null)
//            return;
//        UpdateGizmos();
//    }

//    void UpdateGizmos()
//    {
//        if (m_DrawGizmos)
//        {
//            for (int i = 0; i < m_Vertices.Length; i++)
//            {
//                Gizmos.DrawSphere(m_Vertices[i], .1f);
//            }
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (m_MeshUsed == null)
//        {
//            StartMeshCreator();
//        }

//        InitializeMeshData();
//        UpdateGizmos();
//    }

//    void BackUpValues()
//    {
//        m_LengthXSave = m_LengthX;
//        m_LengthZSave = m_LengthZ;

//        m_HillSizeSave = m_HillSize;
//        m_RoughSave = m_Roughness;
//    }

//    public void RestoreValues()
//    {
//        m_HillSize = m_HillSizeSave;
//        m_Roughness = m_RoughSave;

//        m_LengthX = m_LengthXSave;
//        m_LengthZ = m_LengthZSave;

//        InitializeMeshData();
//    }

//    void UpdateSunRoute()
//    {
//        if (m_Sun == null)
//            return;

//        float SunIntensity = m_Sun.intensity;

//        if (m_SunRotation)
//        {
//            SunIntensity -= 0.005f * Time.deltaTime;

//            if (m_Sun.transform.rotation.x < 0)
//            {
//                m_SunRotation = false;
//            }
//        }
//        else
//        {
//            SunIntensity += 0.005f * Time.deltaTime;
//            if (m_Sun.transform.rotation.x > 180)
//            {
//                m_SunRotation = true;
//            }
//        }

//        m_Sun.transform.Rotate(m_SunSpeed * Time.deltaTime, 0, 0);
//        m_Sun.intensity = SunIntensity;
//    }


//    //===================================
//    // TODO intergrate this into main function
//    void CreateTerrainTexture(float[,] heightMap)
//    {
//        int depth = m_HeightMap.GetLength(0);
//        int width = m_HeightMap.GetLength(1);

//        Color[] colourMap = new Color[depth * width];

//        for (int Z = 0; Z < depth; Z++)
//        {
//            for (int X = 0; X < width; X++)
//            {
//                int Index = Z * width + X;

//                float height = heightMap[Z, X];
//                TerrainType terrain = PickTerrain(height);

//                colourMap[Index] = terrain.m_ColourBio;
//            }
//        }

//        m_TerrainTexture = new Texture2D(depth, width);
//        m_TerrainTexture.wrapMode = TextureWrapMode.Clamp;

//        m_TerrainTexture.SetPixels(colourMap);
//        m_TerrainTexture.Apply();
//    }

//    TerrainType PickTerrain(float heightMap)
//    {
//        foreach (TerrainType terrains in m_Terrains)
//        {
//            if (heightMap < terrains.m_Height)
//            {
//                return terrains;
//            }

//        }
//        return m_Terrains[m_Terrains.Length - 1];
//    }

//}

//===================================================================
// EDITOR BACKUP
//===================================================================


//public override void OnInspectorGUI()
//{
//    TerrainCreator terrainCreator = (TerrainCreator)target;


//    DrawDefaultInspector();



//    EditorGUILayout.Space();

//    if (GUILayout.Button("Create")) //Clear the data in the terrain system.
//    {
//        terrainCreator.InitializeMeshData();
//    }

//    EditorGUILayout.Space();

//    if (GUILayout.Button("Clear Data")) //Clear the data in the terrain system.
//    {
//        terrainCreator.ClearData();
//    }

//    EditorGUILayout.Space();

//    if (GUILayout.Button("Restore")) // Restore all the values that were cleared.
//    {
//        terrainCreator.RestoreValues();
//    }
//}
