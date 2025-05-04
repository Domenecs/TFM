using UnityEngine;

public class LineController : MonoBehaviour
{

    [Header("Line Settings")]
    [SerializeField]
    private int segmentCount = 20;
    [SerializeField]
    private float segmentLength = 0.1f;
    [SerializeField]
    private int constraintIterations = 5;

    [Tooltip("Used to make the end feel heavier")]
    [SerializeField]
    private float customGravity = -9.81f;

    [Header("Transforms")]
    [SerializeField]
    private Transform lineStart;
    [SerializeField]
    private Transform lineEnd; 

    [Header("Visuals")]
    [SerializeField]
    private LineRenderer lineRenderer;

    private LineParticle[] particles;

    [SerializeField]
    [Range(0, 1)]
    private float velocityDamping = 0.5f; // Try something like 0.9 to 0.98


    void Start()
    {
        InitializeLine();
    }

    void FixedUpdate()
    {
        SimulateVerlet(Time.fixedDeltaTime);
        ApplyConstraints();
    }

    void LateUpdate()
    {
        UpdateLineEndPosition();
        DrawLine();
    }

    // Struct to store the particles that compose the line renderer data
    private struct LineParticle
    {
        public Vector3 position;
        public Vector3 oldPosition;
        public Vector3 acceleration;
    }

    private void InitializeLine()
    {
        particles = new LineParticle[segmentCount];
        Vector3 direction = (lineEnd.position - lineStart.position).normalized;

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 pos = lineStart.position + direction * (segmentLength * i); 
            particles[i].position = pos;
            particles[i].oldPosition = pos;
            particles[i].acceleration = Vector3.zero;
        }
    }


    private void SimulateVerlet(float dt)
    {
        for (int i = 1; i < segmentCount; i++)
        {
            Vector3 temp = particles[i].position;
            Vector3 velocity = (particles[i].position - particles[i].oldPosition) * velocityDamping;
            Vector3 gravity = new Vector3(0, customGravity, 0);

            particles[i].position += velocity + gravity * dt * dt;
            particles[i].oldPosition = temp;
        }

        // Only re-anchor the first particle
        particles[0].position = lineStart.position;
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

                if (currentDist == 0f)
                    continue;

                float diff = (currentDist - segmentLength) / currentDist;
                Vector3 offset = deltaVec * 0.5f * diff;

                // Move particles unless they are locked
                if (i != 0)
                    particles[i].position += offset;

                particles[i + 1].position -= offset;
            }

            // Always re-anchor the first point
            particles[0].position = lineStart.position;
        }
    }





    private void UpdateLineEndPosition()
    {
        if (lineEnd == null) return;


        // Set bobber position
        lineEnd.position = particles[segmentCount - 1].position;
        lineEnd.up = Vector3.up;

    }



    private void DrawLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = segmentCount;
            for (int i = 0; i < segmentCount; i++)
            {
                lineRenderer.SetPosition(i, particles[i].position);
            }
        }
    }
}


