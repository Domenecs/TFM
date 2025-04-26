using UnityEngine;

public class LineController : MonoBehaviour
{
    [Header("Line Settings")]
    [SerializeField] private int segmentCount = 20; // Represents number of segments
    [SerializeField] private float segmentLength = 0.1f;
    [SerializeField] private int constraintIterations = 5;
    [SerializeField] private float customGravity = -9.81f;

    [Header("Transforms")]
    [SerializeField] private Transform lineStart;
    [SerializeField] private Transform lineEnd;

    [Header("Visuals")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private MeshRenderer hookMesh;
    


    private LineParticle[] particles;

    private struct LineParticle
    {
        public Vector3 position;
        public Vector3 oldPosition;
        public Vector3 acceleration;
    }

    void Start()
    {
        InitializeLine();
    }

    void FixedUpdate()
    {
        float dt = Mathf.Min(Time.fixedDeltaTime, 0.05f); // Prevent instability on large deltaTime
        SimulateVerlet(dt);
        ApplyConstraints();
    }

    void LateUpdate()
    {
        DrawLine();
    }

    private void InitializeLine()
    {
        // FIXED: +1 particle because segmentCount defines the number of *segments*, not particles
        particles = new LineParticle[segmentCount + 1];

        Vector3 direction = (lineEnd.position - lineStart.position).normalized;

        for (int i = 0; i < particles.Length; i++) // FIXED: loop through all particles
        {
            Vector3 pos = lineStart.position + direction * (segmentLength * i);
            particles[i].position = pos;
            particles[i].oldPosition = pos;
            particles[i].acceleration = Vector3.zero;
        }
    }

    private void SimulateVerlet(float dt)
    {
        for (int i = 1; i < particles.Length - 1; i++) // Keep first and last particles fixed
        {
            Vector3 temp = particles[i].position;
            Vector3 velocity = particles[i].position - particles[i].oldPosition;
            Vector3 gravity = new Vector3(0, customGravity, 0);

            particles[i].position += velocity + gravity * dt * dt;
            particles[i].oldPosition = temp;
        }

        // Fix anchor positions
        particles[0].position = lineStart.position;
        particles[particles.Length - 1].position = lineEnd.position;
    }

    private void ApplyConstraints()
    {
        for (int iteration = 0; iteration < constraintIterations; iteration++)
        {
            for (int i = 0; i < particles.Length - 1; i++)
            {
                LineParticle p1 = particles[i];
                LineParticle p2 = particles[i + 1];

                Vector3 deltaVec = p2.position - p1.position;
                float currentDist = deltaVec.magnitude;

                if (currentDist == 0f) continue;

                float diff = (currentDist - segmentLength) / currentDist;
                Vector3 offset = deltaVec * 0.5f * diff;

                // FIXED: Apply half of the correction to both ends, unless they’re anchored
                if (i != 0)
                    particles[i].position += offset;
                if (i + 1 != particles.Length - 1)
                    particles[i + 1].position -= offset;
            }

            // Re-anchor ends
            particles[0].position = lineStart.position;
            particles[particles.Length - 1].position = lineEnd.position;
        }
    }

    private void DrawLine()
    {
        if (lineRenderer != null)
        {
            // FIXED: lineRenderer needs segmentCount + 1 points
            lineRenderer.positionCount = particles.Length;
            for (int i = 0; i < particles.Length; i++)
            {
                lineRenderer.SetPosition(i, particles[i].position);
            }
        }
    }


    public void ToggleVisibility()
    {
        lineRenderer.enabled = !lineRenderer.enabled;   
        hookMesh.enabled = !hookMesh.enabled;
    }
}


