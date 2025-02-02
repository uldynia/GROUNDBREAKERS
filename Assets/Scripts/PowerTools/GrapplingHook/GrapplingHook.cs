using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;

public class GrapplingHook : PowerTool
{
    Rigidbody2D rb;
    Camera cam;
    [System.Serializable]
    public class Point
    {
        public Vector2 point;
        public float duration = 2;
    }
    public SyncList<Point> points = new();
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

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
        Point point = new();
        point.point = hit.point;
        points.Add(point);
    }
    private void Update()
    {
        if (!isOwned || points.Count == 0) return;
        rb.linearVelocity = Vector3.zero;
        Vector2 finalVector = new();
        for (int i = 0; i < points.Count; i++)
        {
            points[i].duration -= Time.deltaTime;
            if (points[i].duration < 0) {
                points.RemoveAt(i);
            }
            else
            {
                finalVector += points[i].point;
            }
        }
        finalVector /= points.Count;
        var diff = (finalVector - rb.position);
        rb.position += diff.normalized * Time.deltaTime * 10;
        if(diff.sqrMagnitude < 1) points.Clear();

    }

    public Material mat;
    void RenderLines(ScriptableRenderContext context, Camera camera)
    {
        GL.PushMatrix();
        GL.LoadOrtho();
        if (mat.SetPass(0))
        {
            Vector2 playerpos = cam.WorldToViewportPoint(rb.position);
            foreach (var point in points)
            {
                Vector2 grapplePos = cam.WorldToViewportPoint(point.point);
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
