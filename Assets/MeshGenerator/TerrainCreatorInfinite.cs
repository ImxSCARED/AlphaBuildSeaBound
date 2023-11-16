using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===================================================================
    -----------------------------------------------------------------
   _____________________        
   \                   /
    \_____       _____/   _________               ____
          |     |        /   _____  \     _______[____]_______
          |     |       /   /     \__\    |       ____       |
          |     |       |   |       ___   |-------\()/-------|
          |     |       |   |      /  /   |_TC_____INFINITE__|
          |     |        \   \____/  /    ___________________________
          |_____|    ()   \_________/    /  TERRAIN CREATOR INFINITE |
    -----------------------------------------------------------------
 ===================================================================*/

[System.Serializable]

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(TerrainCreator))]

[DisallowMultipleComponent()]

[AddComponentMenu("TerrainCreator/Terrain Creator Infinite")]
public class TerrainCreatorInfinite : MonoBehaviour
{
    public enum PlayerMode
    {
        TAG = 0,
        MANUALLY,
    }

    const float m_ViewerThresholdForUpdatePattern = 25f;
    const float m_SqrViewerThresholdForUpdatePattern = m_ViewerThresholdForUpdatePattern * m_ViewerThresholdForUpdatePattern;

    public static float m_MaxViewDistance = 450;

    [Header("Player Related"), Tooltip("Choose how the player should be assigned. " +
        "TAG: Gets object with the Player tag,  MANUALLY: Drag n Drop the object.")]
    [SerializeField] PlayerMode m_Player = PlayerMode.MANUALLY;

    [SerializeField] Transform m_PlayerTransform;

    [Header("LOD Related"), Tooltip("Fill out for amount of LOD on meshes.")]
    [SerializeField] DetailILog[] m_DetailILog;

    Mesh m_mesh;
    [Header("World Material"), Tooltip("Apply a material that the Terrain Creator will use")]
    public Material m_WorldMaterial = null;
    static TerrainCreator m_TerrainCreator = null;

    public static Vector2 m_PlayerPos;
    Vector2 m_PlayerPosOld;

    int m_ChunkSize;
    int m_ChunksVisibleDist;
    static MeshCollider m_Collider;

    static List<TerrainChunk> m_LastChunks = new List<TerrainChunk>();
    Dictionary<Vector2, TerrainChunk> m_TerrainDictionary = new Dictionary<Vector2, TerrainChunk>();

    private void OnValidate()
    {
        if (m_PlayerTransform == null)
        {
            if (m_Player == PlayerMode.TAG)
            {
                GameObject asd = GameObject.FindGameObjectWithTag("Player");

                m_PlayerTransform = asd.transform;
            }
        }
    }

    private void Start()
    {
        m_TerrainCreator = FindObjectOfType<TerrainCreator>();
        m_WorldMaterial = GetComponent<Material>();

        if (m_TerrainCreator == null)
        {
            Debug.LogError("Terrain Creator Main Failed To Find. Please assign a TCM");
        }

        if (m_DetailILog.Length < 1)
        {
            Debug.LogError("Terrain Creator Infinite LOD detail log has not been filled out! One will be auto generated for run time.");

            int BaseDetail = 1;
            int AmountSeem = 200;

            m_DetailILog = new DetailILog[3];
            for (int i = 0; i < 3; i++)
            {
                m_DetailILog[i].m_DetailAmount = BaseDetail;
                m_DetailILog[i].m_AmountVisible = AmountSeem;

                BaseDetail++;
                AmountSeem += 200;
            }
        }

        m_MaxViewDistance = m_DetailILog[m_DetailILog.Length - 1].m_AmountVisible;
        m_ChunkSize = TerrainCreator.m_WorldSize - 1;
        m_ChunksVisibleDist = Mathf.RoundToInt(m_MaxViewDistance / m_ChunkSize);

        UpdateChunks();
    }

    private void Update()
    {
        m_PlayerPos = new Vector2(m_PlayerTransform.position.x, m_PlayerTransform.position.z) / m_TerrainCreator.GetWorldScale();

        if ((m_PlayerPosOld - m_PlayerPos).sqrMagnitude > m_SqrViewerThresholdForUpdatePattern)
        {
            m_PlayerPosOld = m_PlayerPos;
            UpdateChunks();
        }
    }

    void UpdateChunks()
    {
        for (int i = 0; i < m_LastChunks.Count; i++)
        {
            m_LastChunks[i].SetState(false);
        }

        m_LastChunks.Clear();
        //==================================================================

        int currentChunkX = Mathf.RoundToInt(m_PlayerPos.x / m_ChunkSize);
        int currentChunkY = Mathf.RoundToInt(m_PlayerPos.y / m_ChunkSize);

        for (int yOffset = -m_ChunksVisibleDist; yOffset <= m_ChunksVisibleDist; yOffset++)
        {
            for (int xOffset = -m_ChunksVisibleDist; xOffset <= m_ChunksVisibleDist; xOffset++)
            {
                Vector2 chunksViewedPosition = new Vector2(currentChunkX + xOffset, currentChunkY + yOffset);

                if (m_TerrainDictionary.ContainsKey(chunksViewedPosition))
                {
                    m_TerrainDictionary[chunksViewedPosition].UpdateChunks();
                }
                else
                {
                    m_TerrainDictionary.Add(chunksViewedPosition, new TerrainChunk(chunksViewedPosition, m_ChunkSize, gameObject.transform, m_WorldMaterial, m_DetailILog));
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject m_Mesh;
        Vector2 m_Position;
        Bounds m_Bound;

        MapData m_MapData;
        MeshRenderer m_Renderer;
        MeshFilter m_Filter;
        MeshCollider m_Collider;

        Material m_Material;
        DetailILog[] m_DetailILog;

        DetailMesh[] m_DetailMesh;
        MapData m_MapInfomation;
        bool m_HasGottenMapData;

        int m_PrevoiusDetailIndex = -1;

        public TerrainChunk(Vector2 _Pos, int _size, Transform _Parent, Material _Mat, DetailILog[] _detail)
        {

            m_Position = _Pos * _size;
            m_Bound = new Bounds(m_Position, Vector2.one * _size);
            Vector3 Position3D = new Vector3(m_Position.x, 0, m_Position.y);

            m_DetailILog = _detail;

            m_Mesh = new GameObject("TerrainCreatorInfinite");

            m_Mesh.AddComponent<MeshFilter>();
            m_Mesh.AddComponent<MeshRenderer>();
            m_Mesh.AddComponent<MeshCollider>();

            m_DetailMesh = new DetailMesh[m_DetailILog.Length];

            for (int i = 0; i < _detail.Length; i++)
            {
                m_DetailMesh[i] = new DetailMesh(m_DetailILog[i].m_DetailAmount, UpdateChunks);
            }

            SetState(false);

            m_Material = _Mat;
            m_Renderer = m_Mesh.GetComponent<MeshRenderer>();
            m_Filter = m_Mesh.GetComponent<MeshFilter>();
            m_Collider = m_Mesh.GetComponent<MeshCollider>();

            m_Mesh.transform.parent = _Parent;
            m_Mesh.transform.position = Position3D * m_TerrainCreator.GetWorldScale();
            m_Mesh.transform.localScale = Vector3.one * m_TerrainCreator.GetWorldScale();
            if (m_Renderer != null)
            {
                m_Renderer.material = m_Material;
            }
            else
            {
                Debug.LogError("Terrain Creator Infinite failed to find Renderer.");
            }

            m_TerrainCreator.RequestMapData(m_Position, OnWorldDataRecieved);
        }

        void OnWorldDataRecieved(MapData _world)
        {
            this.m_MapData = _world;
            m_HasGottenMapData = true;

            Texture2D texture = m_TerrainCreator.CreateColourMap(m_TerrainCreator.GetChunkSize(), m_TerrainCreator.GetChunkSize(), _world.m_ColourMap);
            m_Renderer.material.mainTexture = texture;

            UpdateChunks();
        }

        void OnWorldMeshReceived(MeshData data)
        {
            Mesh _mesh = data.Create();

            m_Filter.sharedMesh = _mesh;

            if (m_Filter.sharedMesh == null)
            {
                Debug.LogError("Failed To Assign Mesh To MeshFilter Component.");
            }
        }
        public void UpdateChunks()
        {
            if (!m_HasGottenMapData) return;

            if (m_DetailILog.Length < 1) { Debug.LogError("Terrain Creator Infinite LOD detail log has not been filled out!"); return; }

            float viewDistanceNear = Mathf.Sqrt(m_Bound.SqrDistance(m_PlayerPos));
            bool visible = viewDistanceNear <= m_MaxViewDistance;

            if (visible)
            {
                int DetailIndex = 0;

                for (int i = 0; i < m_DetailILog.Length - 1; i++)
                {
                    if (viewDistanceNear > m_DetailILog[i].m_AmountVisible)
                    {
                        DetailIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (DetailIndex != m_PrevoiusDetailIndex)
                {
                    DetailMesh mesh = m_DetailMesh[DetailIndex];
                    if (mesh.m_HasMeshData)
                    {
                        m_PrevoiusDetailIndex = DetailIndex;
                        m_Filter.sharedMesh = mesh.m_MeshTarget;
                        m_Collider.sharedMesh = mesh.m_MeshTarget;
                    }
                    else if (!mesh.m_IsRequested)
                    {
                        mesh.RequestMesh(m_MapData);
                    }
                }
                m_LastChunks.Add(this);
            }

            SetState(visible);
        }

        public void SetState(bool _state)
        {
            m_Mesh.SetActive(_state);
        }

        public bool IsActiveState()
        {
            return m_Mesh.activeSelf;
        }
    }

    class DetailMesh
    {
        public Mesh m_MeshTarget;
        public bool m_IsRequested;
        public bool m_HasMeshData;

        int m_DetailAmount;

        System.Action m_UpdateCallback;
        public DetailMesh(int _amount, System.Action _update)
        {
            this.m_DetailAmount = _amount;
            this.m_UpdateCallback = _update;
        }

        void OnMeshReceived(MeshData _data)
        {
            m_MeshTarget = _data.Create();
            m_HasMeshData = true;

            m_UpdateCallback();
        }
        public void RequestMesh(MapData _data)
        {
            m_IsRequested = true;
            m_TerrainCreator.RequestMesh(_data, m_DetailAmount, OnMeshReceived);
        }
    }

    [System.Serializable]
    public struct DetailILog
    {
        public int m_DetailAmount;
        public float m_AmountVisible;
    }
}
