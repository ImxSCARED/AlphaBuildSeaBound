using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/*===================================================================
    -----------------------------------------------------------------
   _____________________
   \                   /
    \_____       _____/   _________               ____
          |     |        /   _____  \     _______[____]_______
          |     |       /   /     \__\    |       ____       |
          |     |       |   |       ___   |-------\()/-------|
          |     |       |   |      /  /   |_TC_______________|
          |     |        \   \____/  /    ____________________
          |_____|    ()   \_________/    /  TERRAIN CREATOR  |
    -----------------------------------------------------------------
 ===================================================================*/

[System.Serializable]
/*
    BOOKMARKS - CLASSES & LOCATIONS.
 
    1) TerrainType Class
    2) MeshMaker Class
    3) MeshData Class
    4) Noise Creator Class
    5) Terrain Creator Class
 
 */

public class TerrainType
{
    [Tooltip("Give the Biome a name.")]
    public string m_Name = "Biome";

    [Tooltip("What height should the biome go up to.")]
    public float m_Height = 0;

    [Tooltip("Colour of the biome.")]
    public Color m_ColourBio = Color.magenta;
}
//==========================================================================
//          SIDE CLASSES FOR TERRAIN CREATOR
//==========================================================================
public static class MeshMaker
{
    public static MeshData CreateMesh(float[,] heightMap, float hillSize, AnimationCurve heightCurve, int _detailInMesh)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int MSI = (_detailInMesh == 0) ? 1 : _detailInMesh * 2;


        int VPL = (width - 1) / MSI + 1;
        MeshData m_Mesh = new MeshData(VPL);

        float m_TopLeftX = (width - 1) / -2f;
        float m_TopLeftZ = (height - 1) / 2f;

        AnimationCurve animationCurve = new AnimationCurve(heightCurve.keys);

        int m_VertexIndex = 0;

        for (int y = 0; y < height; y += MSI)
        {
            for (int x = 0; x < width; x += MSI)
            {
                float Z = heightMap[x, y];
                float HillSurface = animationCurve.Evaluate(Z);

                m_Mesh.m_Vectices[m_VertexIndex] = new Vector3(m_TopLeftX + x, HillSurface * hillSize, m_TopLeftZ - y); ;
                m_Mesh.m_Uvs[m_VertexIndex] = new Vector2((x - MSI) / (float)width, (y - MSI) / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    m_Mesh.AddTri(m_VertexIndex, m_VertexIndex + VPL + 1, m_VertexIndex + VPL);
                    m_Mesh.AddTri(m_VertexIndex + VPL + 1, m_VertexIndex, m_VertexIndex + 1);
                }
                m_VertexIndex++;
            }
        }
        return m_Mesh;
    }
}

//===============================================================

public class MeshData
{
    public Vector3[] m_Vectices;
    public int[] m_Triangles;
    public Vector2[] m_Uvs;

    Vector3[] boarderVert;
    int[] borderTri;

    int m_TriangleIndex = 0;
    int m_BorderIndex = 0;
    public MeshData(int vertsLine)
    {
        m_Vectices = new Vector3[vertsLine * vertsLine];
        m_Uvs = new Vector2[vertsLine * vertsLine];
        m_Triangles = new int[(vertsLine - 1) * (vertsLine - 1) * 6];

        boarderVert = new Vector3[vertsLine * 4 + 4];
        borderTri = new int[24 * vertsLine];
    }

    public void AddVert(Vector3 Pos, Vector2 uv, int vertexIndex)
    {
        if (vertexIndex < 0)
        {
            boarderVert[-vertexIndex - 1] = Pos;
        }
        else
        {
            m_Vectices[vertexIndex] = Pos;
            m_Uvs[vertexIndex] = uv;
        }
    }

    public void AddTri(int a, int b, int c)
    {
        if (a < 0 || b < 0 || c < 0)
        {
            borderTri[m_BorderIndex] = a;
            borderTri[m_BorderIndex + 1] = b;
            borderTri[m_BorderIndex + 2] = c;

            m_BorderIndex += 3;
        }
        else
        {
            m_Triangles[m_TriangleIndex] = a;
            m_Triangles[m_TriangleIndex + 1] = b;
            m_Triangles[m_TriangleIndex + 2] = c;

            m_TriangleIndex += 3;
        }
    }

    Vector3[] CalulateMeshNormals()
    {
        Vector3[] vertexNormals = new Vector3[m_Vectices.Length];
        int TriCount = m_Triangles.Length / 3;

        for (int i = 0; i < TriCount; i++)
        {
            int IndexTri = i * 3;
            int IndexVertA = m_Triangles[IndexTri];
            int IndexVertB = m_Triangles[IndexTri + 1];
            int IndexVertC = m_Triangles[IndexTri + 2];

            Vector3 TriNorm = SurfaceNormals(IndexVertA, IndexVertB, IndexVertC);
            vertexNormals[IndexVertA] += TriNorm;
            vertexNormals[IndexVertB] += TriNorm;
            vertexNormals[IndexVertC] += TriNorm;
        }

        int BoarderTri = borderTri.Length / 3;

        for (int i = 0; i < BoarderTri; i++)
        {
            int IndexTri = i * 3;
            int IndexVertA = borderTri[IndexTri];
            int IndexVertB = borderTri[IndexTri + 1];
            int IndexVertC = borderTri[IndexTri + 2];

            Vector3 TriNorm = SurfaceNormals(IndexVertA, IndexVertB, IndexVertC);
            if (IndexVertA >= 0)
            {
                vertexNormals[IndexVertA] += TriNorm;
            }
            if (IndexVertB >= 0)
            {
                vertexNormals[IndexVertB] += TriNorm;
            }
            if (IndexVertC >= 0)
            {
                vertexNormals[IndexVertC] += TriNorm;
            }
        }

        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }
        return vertexNormals;
    }

    Vector3 SurfaceNormals(int A, int b, int c)
    {
        Vector3 pointA = (A < 0) ? boarderVert[-A - 1] : m_Vectices[A];
        Vector3 pointB = (b < 0) ? boarderVert[-b - 1] : m_Vectices[b];
        Vector3 pointC = (c < 0) ? boarderVert[-c - 1] : m_Vectices[c];

        Vector3 AB = pointB - pointA;
        Vector3 AC = pointC - pointA;

        return Vector3.Cross(AB, AC).normalized;
    }

    public Mesh Create()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = m_Vectices;
        mesh.triangles = m_Triangles;
        mesh.uv = m_Uvs;

        mesh.normals = CalulateMeshNormals();

        return mesh;
    }
}
public static class NoiseCreator
{
    static float m_OffsetX;
    static float m_OffsetY;

    static int m_Spacer = 100000;

    public static float[,] CreateNewMap(int _width, int _height, int _seed, float _scale, int _octaves, float _persistance, float _lacunarity, Vector2 _Offsets, bool _UseMode)
    {
        float[,] newMap = new float[_width, _height];

        if (_scale <= 0)
        {
            _scale = 0.0001f;
        }

        float amp = 1;
        float freq = 1;
        float MaxPossibleHeight = 0;

        float Halfwidth = _width / 2f;
        float Halfheight = _height / 2f;

        float m_MaxHeight = float.MinValue;
        float m_MinHeight = float.MaxValue;

        System.Random rnd = new System.Random(_seed);

        Vector2[] OctOffsets = new Vector2[_octaves];

        for (int i = 0; i < _octaves; i++)
        {
            m_OffsetX = rnd.Next(-m_Spacer, m_Spacer) + _Offsets.x;
            m_OffsetY = rnd.Next(-m_Spacer, m_Spacer) - _Offsets.y;
            OctOffsets[i] = new Vector2(m_OffsetX, m_OffsetY);

            MaxPossibleHeight += amp;
            amp *= _persistance;
        }

        for (int Y = 0; Y < _height; Y++)
        {
            for (int X = 0; X < _width; X++)
            {
                amp = 1;
                freq = 1;

                float height = 0;

                for (int i = 0; i < _octaves; i++)
                {

                    float TempX = (X - Halfwidth + OctOffsets[i].x) / _scale * freq;
                    float TempY = (Y - Halfheight + OctOffsets[i].y) / _scale * freq;

                    float createdNoise = Mathf.PerlinNoise(TempX, TempY) * 2 - 1;

                    height += createdNoise * amp;
                    amp *= _persistance;
                    freq *= _lacunarity;
                }

                if (height > m_MaxHeight)
                {
                    m_MaxHeight = height;
                }
                else if (height < m_MinHeight)
                {
                    m_MinHeight = height;
                }

                newMap[X, Y] = height;
            }
        }

        for (int Y = 0; Y < _height; Y++)
        {
            for (int X = 0; X < _width; X++)
            {
                if (_UseMode)
                {
                    newMap[X, Y] = Mathf.InverseLerp(m_MinHeight, m_MaxHeight, newMap[X, Y]);
                }
                else
                {
                    float NormalizeHeight = (newMap[X, Y] + 1) / (2f * MaxPossibleHeight / 1.75f);
                    newMap[X, Y] = Mathf.Clamp(NormalizeHeight, 0, int.MaxValue);
                }
            }
        }
        return newMap;
    }
}

//===========================================================================

//                           MAIN SYSTEM 

//===========================================================================

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

[DisallowMultipleComponent()]

[AddComponentMenu("TerrainCreator/Terrain Creator Main")]
public class TerrainCreator : MonoBehaviour
{
    [Header("Terrain Settings")]

    int m_LengthX;
    public enum EditViewer
    {
        ON = 0,
        OFF,
    }

    public enum SeedSettings
    {
        RANDOM = 0,
        CUSTOM,
    }


    public const int m_WorldSize = 239;

    [Header("Terrain Creator General"), Space()]
    //=====================================================
    [Tooltip("generate a random seed at runtime. (NOTE: what ever seed you have will be overwritten.)")]
    [SerializeField] SeedSettings m_SeedMode = SeedSettings.CUSTOM;

    [Tooltip("Should the Terrain Creator update in the editor.")]
    [SerializeField] EditViewer m_AutoUpdate = EditViewer.OFF;
    public bool m_NormalizeWorld = false;

    [Tooltip("Adjust the world scale NOTE: Terrain Creator Infinite Required")]
    [SerializeField, Range(2f, 10f)] float m_WorldScale = 2f;

    [Header("Customise World"), Space()]

    [Tooltip("Seed for generating unique environments.")]
    [SerializeField, Space()] int m_WorldSeed = 0;

    [Tooltip("Chose the amount of Octaves in the noise.")]
    [SerializeField] int m_Octaves = 4;

    [Tooltip("Chose the amount of detail in pixels.")]
    [Range(0, 1)]
    [SerializeField] float m_persistance = 0.5f;

    [Tooltip("Chose the amount of Lacunarity.")]
    [SerializeField] float m_Lacunarity = 2;

    [Tooltip("Apply Offsets to the world.")]
    [SerializeField] Vector2 m_Offsets = Vector2.zero;

    //=====================================================
    [Space()]
    [Tooltip("Change the size of the hills when generated")]
    [SerializeField] float m_HillSize = 5f;

    [Tooltip("Adjust the shape of the surface & hills.")]
    [SerializeField] AnimationCurve m_HillShape;

    [Tooltip("Adjust the Resolution & Scale of the surface.")]
    [SerializeField] float m_Resolution = 10f;

    //==========================================
    // Environment

    [Tooltip("Assign & Adjust Biomes."), Space()]
    [SerializeField] TerrainType[] m_BiomeColours;

    public TerrainCreatorNature m_Nature;

    //==========================================
    // Non Exposed Vars

    float m_SunYPos;
    float[,] m_HeightMap;
    int[] m_Triangles;
    int m_DetailInMesh = 0;


    Vector3[] m_Vertices;
    Vector2[] m_Uvs;
    Color[] m_ColourMap;

    Texture2D m_TerrainTexture;
    Mesh m_MeshUsed;
    Renderer m_MeshRenderer = null;
    MeshFilter m_MeshFilter = null;

    //===========================================
    // BackUp Values

    int m_LengthXSave;
    int m_LengthZSave;

    float m_HillSizeSave;
    float m_RoughSave;

    float m_ResolutionSave;
    float m_LacunaritySave;
    int m_OctavesSave;
    float m_DetailAmountSave;

    Queue<WorldInfomation<MapData>> m_WorldQueue = new Queue<WorldInfomation<MapData>>();
    Queue<WorldInfomation<MeshData>> m_MeshQueue = new Queue<WorldInfomation<MeshData>>();

    //===========================================
    void Start()
    {
        if (gameObject.GetComponent<TerrainCreatorInfinite>() == null || gameObject.GetComponent<TerrainCreatorInfinite>().enabled == false)
        {
            m_NormalizeWorld = true;
            StartMeshCreator();
            GenerateWorld();
        }
        else if (gameObject.GetComponent<TerrainCreatorInfinite>() != null || gameObject.GetComponent<TerrainCreatorInfinite>().enabled == true)
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            m_NormalizeWorld = false;
        }
    }

    //===========================================
    public void StartMeshCreator()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshRenderer = GetComponent<Renderer>();

        if (m_SeedMode == SeedSettings.RANDOM)
        {
            RandomSeed();
        }
    }

    public int GetChunkSize()
    {
        return m_WorldSize;
    }

    private void Update()
    {
        if (m_WorldQueue.Count > 0)
        {
            for (int i = 0; i < m_WorldQueue.Count; i++)
            {
                lock (m_WorldQueue)
                {
                    WorldInfomation<MapData> result = m_WorldQueue.Dequeue();
                    result.callback(result.m_Parm);
                };
            }
        }
        if (m_MeshQueue.Count > 0)
        {
            for (int i = 0; i < m_MeshQueue.Count; i++)
            {
                lock (m_MeshQueue)
                {
                    WorldInfomation<MeshData> result = m_MeshQueue.Dequeue();
                    result.callback(result.m_Parm);
                }
            }
        }
    }

    //=========================================================
    //      THREADING DATA FOR TERRAIN CREATOR INFINITE
    //=========================================================
    public void RequestMapData(Vector2 centre, Action<MapData> callback)
    {
        ThreadStart TS = delegate
        {
            MapDataThread(centre, callback);
        };
        new Thread(TS).Start();

    }

    public void RequestMesh(MapData data, int _detail, Action<MeshData> callback)
    {
        ThreadStart TS = delegate
        {
            WorldMeshThread(data, _detail, callback);
        };
        new Thread(TS).Start();
    }

    void WorldMeshThread(MapData data, int _detail, Action<MeshData> callback)
    {
        MeshData mesh = MeshMaker.CreateMesh(data.m_HeightMap, m_HillSize, m_HillShape, _detail);
        lock (m_MeshQueue)
        {
            m_MeshQueue.Enqueue(new WorldInfomation<MeshData>(callback, mesh));
        }
    }

    void MapDataThread(Vector2 centre, Action<MapData> callback)
    {
        MapData data = InitializeMeshData(centre);
        lock (m_WorldQueue)
        {
            m_WorldQueue.Enqueue(new WorldInfomation<MapData>(callback, data));
        }
    }

    struct WorldInfomation<T>
    {
        public readonly Action<T> callback;
        public readonly T m_Parm;

        public WorldInfomation(Action<T> _callBack, T _parm)
        {
            this.callback = _callBack;
            this.m_Parm = _parm;
        }
    }

    //=========================================================
    MapData InitializeMeshData(Vector2 _worldCentre)
    {
        m_LengthX = m_WorldSize;

        //============================================================      TODO Rework 
        // Place vertices into an array for the mesh data

        float[,] noiseMap = NoiseCreator.CreateNewMap(m_WorldSize + 2, m_LengthX + 2, m_WorldSeed, m_Resolution, m_Octaves, m_persistance, m_Lacunarity, _worldCentre + m_Offsets, m_NormalizeWorld);

        Color[] colourMap = new Color[m_WorldSize * m_LengthX];

        for (int Z = 0; Z < m_WorldSize; Z++)
        {
            for (int X = 0; X < m_LengthX; X++)
            {
                float HeightCurrent = noiseMap[X, Z];

                for (int i = 0; i < m_BiomeColours.Length; i++)
                {
                    if (HeightCurrent >= m_BiomeColours[i].m_Height)
                    {
                        colourMap[Z * m_WorldSize + X] = m_BiomeColours[i].m_ColourBio;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colourMap);
    }

    private void OnValidate()
    {
        if (m_AutoUpdate == EditViewer.ON)
        {
            EditorMeshCreate();
        }
    }

    public void EditorMeshCreate()
    {
        StartMeshCreator();

        m_LengthX = m_WorldSize;

        //============================================================      TODO Rework 
        // Place vertices into an array for the mesh data

        m_HeightMap = NoiseCreator.CreateNewMap(m_WorldSize + 2, m_WorldSize + 2, m_WorldSeed, m_Resolution, m_Octaves, m_persistance, m_Lacunarity, m_Offsets, m_NormalizeWorld);

        m_ColourMap = new Color[m_WorldSize * m_LengthX];

        for (int Z = 0; Z < m_WorldSize; Z++)
        {
            for (int X = 0; X < m_LengthX; X++)
            {
                float HeightCurrent = m_HeightMap[X, Z];

                for (int i = 0; i < m_BiomeColours.Length; i++)
                {
                    if (HeightCurrent >= m_BiomeColours[i].m_Height)
                    {
                        m_ColourMap[Z * m_WorldSize + X] = m_BiomeColours[i].m_ColourBio;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        DrawMesh(MeshMaker.CreateMesh(m_HeightMap, m_HillSize, m_HillShape, m_DetailInMesh), CreateColourMap(m_WorldSize, m_LengthX, m_ColourMap));
    }

    //=====================================================
    // Render into game world

    public void RenderWorld(MapData m_map)
    {
        DrawMesh(MeshMaker.CreateMesh(m_map.m_HeightMap, m_HillSize, m_HillShape, m_DetailInMesh), CreateColourMap(m_WorldSize, m_LengthX, m_map.m_ColourMap));
    }

    //=====================================================
    // Create the Perlin map
    public Texture2D CreateHeightMap(float[,] _noiseMap)
    {
        int width = _noiseMap.GetLength(0);
        int height = _noiseMap.GetLength(1);

        m_ColourMap = new Color[width * height];
        for (int Z = 0; Z < height; Z++)
        {
            for (int X = 0; X < width; X++)
            {
                m_ColourMap[Z * width + X] = Color.Lerp(Color.black, Color.white, _noiseMap[X, Z]);
            }
        }

        return CreateColourMap(m_WorldSize, m_LengthX, m_ColourMap);
    }

    //========================================================
    // Draw the colour map onto the mesh
    void DrawTextures(Texture2D texture)
    {
        m_MeshRenderer.sharedMaterial.mainTexture = texture;
        m_MeshRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    //=======================================
    void DrawMesh(MeshData mesh, Texture2D texture)
    {
        Mesh CreatedMesh = mesh.Create();
        GetComponent<MeshCollider>().sharedMesh = CreatedMesh;
        if (m_MeshFilter)
        {
            m_MeshFilter.sharedMesh = CreatedMesh;
        }
        else
        {
            Debug.LogError("Terrain Creator failed to access MeshFilter.");
        }

        if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            m_MeshRenderer.sharedMaterial.mainTexture = texture;
        }
        else
        {
            Debug.LogError("Terrain Creator failed to access the MeshRenderer.");
        }
    }

    public void ClearData()
    {
        BackUpValues();

        m_Resolution = 0.1f;
        m_HillSize = 0;

        m_persistance = 0.5f;
        m_Resolution = 25f;
        m_Octaves = 5;

        m_Lacunarity = 2f;
        m_WorldScale = 2f;
    }

    void BackUpValues()
    {
        m_HillSizeSave = m_HillSize;
        m_RoughSave = m_Resolution;

        m_OctavesSave = m_Octaves;
        m_ResolutionSave = m_Resolution;
        m_LacunaritySave = m_Lacunarity;
        m_DetailAmountSave = m_persistance;

        EditorMeshCreate();
    }

    public void RestoreValues()
    {
        m_HillSize = m_HillSizeSave;
        m_Resolution = m_RoughSave;

        m_LengthX = m_LengthXSave;

        m_persistance = m_DetailAmountSave;
        m_Lacunarity = m_LacunaritySave;
        m_Resolution = m_ResolutionSave;
        m_Octaves = m_OctavesSave;

        EditorMeshCreate();
    }

    //===================================
    // TODO intergrate this into main function
    public Texture2D CreateColourMap(int _width, int _height, Color[] _map)
    {
        Texture2D texture = new Texture2D(_width, _height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.SetPixels(_map);
        texture.Apply();

        return texture;
    }
    TerrainType PickTerrain(float heightMap)
    {
        foreach (TerrainType terrains in m_BiomeColours)
        {
            if (heightMap < terrains.m_Height)
            {
                return terrains;
            }

        }
        return m_BiomeColours[m_BiomeColours.Length - 1];
    }

    //=============================================
    public void SetPersistance(float _amount)
    {
        m_persistance = _amount;
    }

    public void SetHillSize(float _amount)
    {
        m_HillSize = _amount;
    }

    public void SetResolution(float _amount)
    {
        m_Resolution = _amount;
    }

    public void RandomSeed()
    {
        m_WorldSeed = UnityEngine.Random.Range(12, 500000);
    }
    public void GenerateWorld()
    {
        EditorMeshCreate();
    }

    public float GetWorldScale()
    {
        return m_WorldScale;
    }

    //==============================================
}
public struct MapData
{
    public float[,] m_HeightMap;
    public Color[] m_ColourMap;

    public MapData(float[,] _height, Color[] _colourMap)
    {
        this.m_HeightMap = _height;
        this.m_ColourMap = _colourMap;
    }
}