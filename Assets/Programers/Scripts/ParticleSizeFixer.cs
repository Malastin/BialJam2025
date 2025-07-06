using UnityEngine;

public class ParticleSizeFixer : MonoBehaviour
{
    [SerializeField] private GameObject objactToGetScale;
    [SerializeField] private ParticleSystem[] particles;



    private void Start()
    {
        FixSizeOfParticles();
    }

    private void FixSizeOfParticles()
    {
        for (int a = 0; a < particles.Length; a++)
        {
            var particle = particles[a].shape;
            particle.scale = objactToGetScale.transform.localScale;
        }
    }
}
