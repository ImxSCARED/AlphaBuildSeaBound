using System.Collections;
using UnityEditor.Search;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource source;
    [SerializeField] private float defaultDuration = 0.0f;
    [SerializeField] CollisionLayer[] collisionLayers;

    [System.Serializable]
    public struct CollisionLayer
    {
        public int layer;
        public AudioClip clip;
        public Animator fxAnim;
        public string animKey;
        public float duration;
    }


    public void Spawn(Vector3 dir, float force)
    {
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collisionLayers.Length; i++)
        {
            if (collision.gameObject.layer == collisionLayers[i].layer)
            {
                if (collisionLayers[i].clip)
                {
                    source.PlayOneShot(collisionLayers[i].clip);
                }
                if (collisionLayers[i].fxAnim)
                {
                    collisionLayers[i].fxAnim.SetBool(collisionLayers[i].animKey, true);
                }

                StartCoroutine(Disable(collisionLayers[i].duration));
                return;
            }
        }
        StartCoroutine(Disable(defaultDuration));
    }

    private IEnumerator Disable(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
