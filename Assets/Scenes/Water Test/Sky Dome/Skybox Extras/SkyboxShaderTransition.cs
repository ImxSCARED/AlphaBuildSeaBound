using UnityEngine;

enum SkyDomeMode
{
    Cycle = 0,
    Day,
    Night,
}

public class SkyboxShaderTransition : MonoBehaviour
{
    [Header("general")]
    [SerializeField] SkyDomeMode m_DomeMode = SkyDomeMode.Cycle;

    //===================================== Sky Transition General
    Material m_SkyBoxMaterial;
    bool m_LerpState = true;
    float m_SkyTransitionIndex = 1;
    float m_TransistionRate = 0.05f;

    //===================================== Sun General
    float m_SunPosition = -0.4f;
    bool m_UpdateSunAllowed = false;

     Transform m_SunLight; 

    void Start()
    {
        m_SkyBoxMaterial = GetComponent<MeshRenderer>().material;

        if (m_DomeMode == SkyDomeMode.Day)
            m_SkyBoxMaterial.SetFloat("_Transition", 0);
        else if (m_DomeMode == SkyDomeMode.Night)
            m_SkyBoxMaterial.SetFloat("_Transition", 1);
    }

    void Update()
    {
        if (m_DomeMode == SkyDomeMode.Cycle)
        {
            if (m_SkyTransitionIndex >= 0.9f)
                m_LerpState = false;
            else if (m_SkyTransitionIndex <= 0.1f)
                m_LerpState = true;

            if (m_LerpState)
            {
                m_SkyTransitionIndex = Mathf.Lerp(m_SkyTransitionIndex, 1, (m_SkyBoxMaterial.GetFloat("_CycleSpeed") + m_TransistionRate) * Time.deltaTime);
            }
            else
            {
                m_SkyTransitionIndex = Mathf.Lerp(m_SkyTransitionIndex, 0, (m_SkyBoxMaterial.GetFloat("_CycleSpeed") + m_TransistionRate) * Time.deltaTime);
            }

            m_SkyBoxMaterial.SetFloat("_Transition", m_SkyTransitionIndex);

            //HasSunReachedEnd();
            //UpdateSunPosition();

            m_SunPosition = -m_SunLight.rotation.x;
            m_SkyBoxMaterial.SetVector("_TilingOffset", new Vector4(m_SunPosition, 0, 0, 0));
        }
    }

    void UpdateSunPosition()
    {
        if (!m_UpdateSunAllowed)
            return;
        m_SunPosition += 1 * m_SkyBoxMaterial.GetFloat("_CycleSpeed") * Time.deltaTime;
    }

    // TODO: Come back to this I cant be bothered rn
    void HasSunReachedEnd()
    {
        if (m_SunPosition > 1)
        {
            m_SunPosition = -0.1f;
        }

    }
}
