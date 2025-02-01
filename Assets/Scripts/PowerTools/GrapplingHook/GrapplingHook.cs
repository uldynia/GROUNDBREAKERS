using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : PowerTool
{
    Rigidbody2D rb;
    [System.Serializable]
    public class Point
    {
        public Vector2 point;
        public float duration = 2;
    }
    public List<Point> points = new();
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnRelease()
    {
        base.OnRelease();

        var hit = Physics2D.Raycast(transform.position, button.deltaNormalized, Mathf.Infinity, LayerMask.GetMask("Terrain"));
        if (hit.collider == null) return;
        Point point = new();
        point.point = (hit.point - rb.position);
        points.Add(point);
    }
    private void Update()
    {
        if (points.Count == 0) return;
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
        rb.position += finalVector.normalized * Time.deltaTime * 10;

    }


}
