using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FishingData;

public class SpinningLineController : MonoBehaviour
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


    private bool isInWater = false;


    [SerializeField]
    [Range(0, 1)]
    private float velocityDamping = 0.98f;


    [Header("ObligatoryLineRendererPoints")]
    [SerializeField]
    private Transform[] guidePoints;

    [Header("ThrowingMechanic")]
    [SerializeField] private float castExtendDuration = 1.2f;
    [SerializeField] private float totalExtendAmount = 1.5f; // Total a extender en ese tiempo



    void Start()
    {
        InitializeLine();
    }

    public void ReleaseCasting(Vector3 force)
    {
        int last = particles.Length - 1;
        particles[last].oldPosition = particles[last].position - force * Time.fixedDeltaTime;

        //Gradually extend the line.
        StartCoroutine(GraduallyExtendLine());
    }

    public IEnumerator GraduallyExtendLine()
    {
        float elapsed = 0f;
        float interval = 0.02f; // frecuencia de los pasos
        int steps = Mathf.CeilToInt(castExtendDuration / interval);
        float stepAmount = totalExtendAmount / steps;

        while (elapsed < castExtendDuration)
        {
            ChangeLineLength(stepAmount);
            elapsed += interval;
            yield return new WaitForSeconds(interval);
        }
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

    public void OnEnterWater()
    {
        isInWater = true;

    }

    public void OnExitWater()
    {
        ResetVelocities();
        isInWater = false;

    }

    public void ChangeLineLength(float amount)
    {
        
        segmentLength += amount;

        if(segmentLength < 0.005)
        {
            segmentLength = 0.005f;
        }
    }
    private void InitializeLine()
    {
        particles = new LineParticle[segmentCount];

        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 pos;
            if (i < guidePoints.Length)
            {
                //Anchor fixed points.
                pos = guidePoints[i].position;
            }
            else
            {
                //Non fixed points
                Vector3 dir = (lineEnd.position - guidePoints[guidePoints.Length - 1].position).normalized;
                pos = guidePoints[guidePoints.Length - 1].position + dir * segmentLength * (i - guidePoints.Length);
            }

            particles[i].position = pos;
            particles[i].oldPosition = pos;
            particles[i].acceleration = Vector3.zero;
        }
    }



    private void SimulateVerlet(float dt)
    {
        for (int i = guidePoints.Length; i < segmentCount; i++)
        {
            // Si está en agua y esta es una de las últimas N partículas, mantener fijas
            if (isInWater && i >= segmentCount - 10)
            {
                // Fijar posición como si fuera un guide point
                continue;
            }

            Vector3 temp = particles[i].position;
            Vector3 velocity = (particles[i].position - particles[i].oldPosition) * velocityDamping;
            Vector3 gravity = new Vector3(0, customGravity, 0);

            particles[i].position += velocity + gravity * dt * dt;
            particles[i].oldPosition = temp;
        }

        // Reanclar puntos fijos (guide points)
        for (int i = 0; i < guidePoints.Length; i++)
        {
            particles[i].position = guidePoints[i].position;
        }

        // Fijar posición de partículas flotando en el agua
        if (isInWater)
        {
            for (int i = segmentCount - 3; i < segmentCount; i++)
            {
                particles[i].position = particles[i].position; // opcionalmente podrías fijarla a una posición registrada al entrar al agua
            }
        }
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

                // Solo ajustar si no son guías fijas
                if (i >= guidePoints.Length)
                    particles[i].position += offset;

                if (i + 1 >= guidePoints.Length)
                    particles[i + 1].position -= offset;
            }

            // Re-anclar puntos fijos cada iteración
            for (int i = 0; i < guidePoints.Length; i++)
            {
                particles[i].position = guidePoints[i].position;
            }
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



    public void ResetVelocities()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].oldPosition = particles[i].position;
        }
    }


    public void ToggleLineEnd()
    {
        lineEnd.gameObject.SetActive(!lineEnd.gameObject.activeInHierarchy);
    }

}


