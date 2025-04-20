using UnityEngine;

public class FishingLineController : MonoBehaviour
{

    [Header("Line Settings")]
    [SerializeField]
    private int segmentCount = 20;
    [SerializeField]
    private float segmentLength = 0.1f;
    [SerializeField]
    private int constraintIterations = 5;
    [SerializeField]
    [Tooltip("Distance needed from the last particle of the line to the bobber for it to deatach from the water. More distance more force needed.")]
    private float unstickDistance = 0.01f;

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
    private bool isBobberFrozen = false;


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
        UpdateBobberPosition();
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
            // Skip the bobber if frozen
            if (i == segmentCount - 1 && isBobberFrozen)
                continue;

            Vector3 temp = particles[i].position;
            Vector3 velocity = particles[i].position - particles[i].oldPosition;
            Vector3 gravity = Physics.gravity;

            particles[i].position += velocity + gravity * dt * dt;
            particles[i].oldPosition = temp;
        }

        particles[0].position = lineStart.position;
    }
    

    public void FreezeBobber()
    {
        isBobberFrozen = true;
    }

    private void ApplyConstraints()
    {
        for (int iteration = 0; iteration < constraintIterations; iteration++)
        {
            for (int i = 0; i < particles.Length - 1; i++)
            {
                bool isLastSegment = i == particles.Length - 2;

                // Check if we need to unfreeze the bobber based on full rope stretch
                if (isBobberFrozen && isLastSegment)
                {
                    Vector3 tipToBobber = particles[particles.Length - 1].position - particles[0].position;
                    float currentLength = tipToBobber.magnitude;
                    float relaxedLength = segmentLength * (particles.Length - 1);
                    float maxAllowedLength = relaxedLength + unstickDistance;

                    if (currentLength > maxAllowedLength)
                    {
                        isBobberFrozen = false; // Rope is stretched too far — unfreeze
                    }
                }

                // Constraint resolution between particles[i] and particles[i + 1]
                LineParticle p1 = particles[i];
                LineParticle p2 = particles[i + 1];

                Vector3 deltaVec = p2.position - p1.position;
                float currentDist = deltaVec.magnitude;

                if (currentDist == 0f)
                    continue;

                float diff = (currentDist - segmentLength) / currentDist;
                Vector3 offset = deltaVec * 0.5f * diff;

                // Skip moving the first particle (attached to the rod tip)
                if (i != 0)
                    particles[i].position += offset;

                // Skip moving the bobber if it's still frozen
                bool isBobber = i + 1 == particles.Length - 1;

                if (!(isBobberFrozen && isBobber))
                    particles[i + 1].position -= offset;
            }

            // Always re-anchor the first point to the rod tip
            particles[0].position = lineStart.position;
        }
    }





    private void UpdateBobberPosition()
    {
        if (lineEnd == null) return;

        Vector3 bobberPos = particles[segmentCount - 1].position;

        // Set bobber position
        lineEnd.position = particles[segmentCount - 1].position;
        lineEnd.forward = Vector3.up;
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


