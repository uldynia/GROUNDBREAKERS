using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;

public class GrapplingHook : PowerTool
{
    Rigidbody2D rb;
    Camera cam;
    public SyncList<Vector2> points = new();
    float duration = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderLines;
    }

    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderLines;
    }
    public override void OnRelease()
    {
        if (!isOwned) return;
        base.OnRelease();
        var hit = Physics2D.Raycast(transform.position, button.deltaNormalized, Mathf.Infinity, LayerMask.GetMask("Terrain"));
        if (hit.collider == null) return;
        points.Add(hit.point);
        duration = 2;
    }
    private void FixedUpdate()
    {
        if (!isOwned || points.Count == 0) return;
        rb.linearVelocity = Vector3.zero;
        Vector2 finalVector = new();
        duration -= Time.fixedDeltaTime;
        for (int i = 0; i < points.Count; i++)
        {
            finalVector += points[i];
        }
        finalVector /= points.Count;
        var diff = (finalVector - rb.position);
        rb.MovePosition(rb.position + diff.normalized * Time.fixedDeltaTime * 10 * points.Count);
        if(diff.sqrMagnitude < 1 || duration < 0) points.Clear();

    }

    public Material mat;
    void RenderLines(ScriptableRenderContext context, Camera camera)
    {
        GL.PushMatrix();
        GL.LoadOrtho();
        if (mat.SetPass(0))
        {
            Vector2 playerpos = camera.WorldToViewportPoint(rb.position);
            foreach (var point in points)
            {
                Vector2 grapplePos = camera.WorldToViewportPoint(point);
                GL.Begin(GL.LINES);
                GL.Color(Color.red);
                GL.Vertex3(playerpos.x, playerpos.y, 0);
                GL.Vertex3(grapplePos.x, grapplePos.y, 0);
                GL.End();
            }

        }
        GL.PopMatrix();

    }
}
