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
          |     |       |   |      /  /   |_TC______NATURE___|
          |     |        \   \____/  /    _________________________
          |_____|    ()   \_________/    /  TERRAIN CREATOR NATURE|
    -----------------------------------------------------------------
 ===================================================================*/

[AddComponentMenu("TerrainCreator /Terrain Creator Nature")]
public class TerrainCreatorNature : MonoBehaviour
{
    enum PlayerSearch
    {
        TAG = 0,
        MANUALLY,
    }

    [Header("Player Related")]
    [Tooltip("Choose how the player should be assigned. " +
        "TAG: Gets object with the Player tag,  MANUALLY: Drag n Drop the object.")]
    [SerializeField] PlayerSearch m_SearchMode = PlayerSearch.MANUALLY;

    [Tooltip("The player position is need for water tracking to work.")]
    [SerializeField] Transform m_PlayerPos = null;

    [Header("Environment"), Space(), Tooltip("light source that will be treated as the sun.")]
    [SerializeField] Light m_Sun = null;

    [Tooltip("Pick the speed that the sun will rotate at. Low values are recommended.")]
    [SerializeField] Vector3 m_SunDirections = new Vector3(0.02f, 0, 0);

    [Tooltip("Select the object to use as the water.")]
    [SerializeField] Transform m_Water = null;

    //=============================
    // Private vars

    int m_TreeCount = 5;

    int m_RandomTreeSeed = 0;
    Vector3[] m_TreePositions;
    int m_CurrentTree = 0;

    GameObject[] m_Trees;
    GameObject m_TreeParent;

    bool m_TreesReady = false;

    Vector3 m_WaterPosition;
    Vector3 m_WaterScale;

    //==========================================================
    private void Start()
    {
        if (FindObjectOfType<TerrainCreatorInfinite>() != null)
        {
            m_WaterScale = new Vector3(60, 60, 60);
        }
        else
        {
            TerrainCreator temp = FindObjectOfType<TerrainCreator>();
            if (temp != null)
            {
                m_WaterScale = temp.transform.localScale;
            }
        }

        if (m_PlayerPos == null)
        {
            if (m_SearchMode == PlayerSearch.TAG)
            {
                GameObject asd = GameObject.FindGameObjectWithTag("Player");

                m_PlayerPos = asd.transform;
            }
            if (m_Water)
            {
                m_Water.localScale = m_WaterScale;
            }
        }
    }

    public void SetTreePosition(Vector3[] _treePos)
    {
        if (m_TreeCount < 1)
        {
            Debug.LogError("Terrain Creator Nature: Trees have not been paired! Pairing stage is being auto done.");
            BeginTreePairing();
        }
        else
        {
            m_TreePositions = _treePos;
        }

    }

    public void BeginTreePairing()
    {
        if (m_TreeParent == null)
        {
            Debug.LogError("Terrain Creator Nature: Trees don't have a parent.");
            m_TreesReady = false;
            return;
        }

        m_Trees = new GameObject[m_TreeCount];

        for (int i = 0; i < m_TreeCount; i++)
        {
            m_Trees[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            m_Trees[i].transform.SetParent(m_TreeParent.transform);
            m_TreesReady = true;
        }
    }

    public void SpawnTrees()
    {
        if (!m_TreesReady) return;

        for (int i = 0; i < m_TreePositions.Length; i++)
        {
            m_RandomTreeSeed = Random.Range(0, 10);

            if (m_RandomTreeSeed == 2 && i < m_Trees.Length)
            {
                m_Trees[m_CurrentTree].transform.position = m_TreePositions[i];
                m_TreePositions[m_CurrentTree].y += 15f;
                m_CurrentTree++;
            }
            else
            {
                break;
            }
        }
    }
    void UpdateSunRoute()
    {
        if (m_Sun == null)
            return;

        m_Sun.transform.Rotate(m_SunDirections);
    }

    private void FixedUpdate()
    {
        if (m_PlayerPos)
        {
            m_WaterPosition.x = m_PlayerPos.position.x;
            m_WaterPosition.z = m_PlayerPos.position.z;

            m_Water.position = m_WaterPosition;
        }
        UpdateSunRoute();
    }
}
