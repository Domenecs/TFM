using UnityEngine;

public class FishingLineController : MonoBehaviour
{

    [Header("Line Settings")]
    public int SegmentCount = 20;
    public float SegmentLength = 0.1f;
    public int ConstraintIterations = 5;

    [Header("Transforms")]
    public Transform lineStart; // The rod tip
    public Transform lineEnd;   // The bobber (now moves based on Verlet)

    [Header("Visuals")]
    public LineRenderer lineRenderer;

    private LineParticle[] particles;



    [Header("Water Settings")]
    public float waterHeight = 0f;
    public bool stickToWater = true;

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

    // Struct to store particle data
    private struct LineParticle
    {
        public Vector3 position;
        public Vector3 oldPosition;
        public Vector3 acceleration;
    }

    private void InitializeLine()
    {
        particles = new LineParticle[SegmentCount];
        Vector3 direction = (lineEnd.position - lineStart.position).normalized;

        for (int i = 0; i < SegmentCount; i++)
        {
            Vector3 pos = lineStart.position + direction * SegmentLength * i;
            particles[i].position = pos;
            particles[i].oldPosition = pos;
            particles[i].acceleration = Vector3.zero;
        }
    }

    private void SimulateVerlet(float dt)
    {
        for (int i = 1; i < SegmentCount; i++)
        {
            // Skip the bobber if frozen
            if (i == SegmentCount - 1 && isBobberFrozen)
                continue;

            Vector3 temp = particles[i].position;
            Vector3 velocity = particles[i].position - particles[i].oldPosition;
            Vector3 gravity = Physics.gravity;

            particles[i].position += velocity + gravity * dt * dt;
            particles[i].oldPosition = temp;
        }

        // Keep the rod tip anchored
        particles[0].position = lineStart.position;
    }

    private void ApplyConstraints()
    {
        for (int iteration = 0; iteration < ConstraintIterations; iteration++)
        {
            for (int i = 0; i < SegmentCount - 1; i++)
            {
                LineParticle p1 = particles[i];
                LineParticle p2 = particles[i + 1];

                Vector3 delta = p2.position - p1.position;
                float dist = delta.magnitude;
                float diff = (dist - SegmentLength) / dist;

                Vector3 offset = delta * 0.5f * diff;

                // Don't move the anchored first point
                if (i != 0)
                    particles[i].position += offset;

                particles[i + 1].position -= offset;
            }
        }
    }

    private void UpdateBobberPosition()
    {
        if (lineEnd == null) return;

        Vector3 bobberPos = particles[SegmentCount - 1].position;

        // If it's below water, freeze
        if (!isBobberFrozen && bobberPos.y <= waterHeight)
        {
            isBobberFrozen = true;

            // Snap to water surface and freeze in place
            bobberPos.y = waterHeight;
            particles[SegmentCount - 1].position = bobberPos;
            particles[SegmentCount - 1].oldPosition = bobberPos;
        }

        // Unfreeze if pulled up (e.g., by the line)
        if (isBobberFrozen && bobberPos.y > waterHeight + 0.01f)
        {
            isBobberFrozen = false;
        }

        // Update the bobber's GameObject position
        lineEnd.position = particles[SegmentCount - 1].position;
        lineEnd.forward = Vector3.up;
    }


    private void DrawLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = SegmentCount;
            for (int i = 0; i < SegmentCount; i++)
            {
                lineRenderer.SetPosition(i, particles[i].position);
            }
        }
    }
}


