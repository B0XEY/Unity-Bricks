using UnityEngine;

namespace Boxey.Bricks.Core {
    public class Brick {
        public GameObject BrickObject;

        private readonly Mesh m_brickMesh;
        private readonly MeshFilter m_brickFilter;
        private readonly MeshRenderer m_brickRenderer;
        private readonly MeshCollider m_brickCollider;
        
        public Brick(Vector3 brickStart, Vector3 brickEnd) {
            var position = brickStart;
            position.y += 0.001f;
            BrickObject = new GameObject {
                name = "Brick: " + brickStart,
                transform = {
                    position = position
                }
            };
            m_brickMesh = new Mesh {
                name = "Brick Mesh"
            };
            m_brickFilter = BrickObject.AddComponent<MeshFilter>();
            m_brickRenderer = BrickObject.AddComponent<MeshRenderer>();
            m_brickCollider = BrickObject.AddComponent<MeshCollider>();
            m_brickCollider.convex = true;
            CreateMesh(brickEnd - brickStart);
        }
        private void CreateMesh(Vector3 brickEnd) {
            var start = Vector3.zero;
            var end = brickEnd;
            var vertices = new Vector3[24] {
                // Front face
                new Vector3(start.x, start.y, start.z),
                new Vector3(end.x, start.y, start.z),
                new Vector3(start.x, end.y, start.z),
                new Vector3(end.x, end.y, start.z),
                // Back face
                new Vector3(start.x, start.y, end.z),
                new Vector3(end.x, start.y, end.z),
                new Vector3(start.x, end.y, end.z),
                new Vector3(end.x, end.y, end.z),
                // Left face
                new Vector3(start.x, start.y, start.z),
                new Vector3(start.x, end.y, start.z),
                new Vector3(start.x, start.y, end.z),
                new Vector3(start.x, end.y, end.z),
                // Right face
                new Vector3(end.x, start.y, start.z),
                new Vector3(end.x, end.y, start.z),
                new Vector3(end.x, start.y, end.z),
                new Vector3(end.x, end.y, end.z),
                // Top face
                new Vector3(start.x, end.y, start.z),
                new Vector3(end.x, end.y, start.z),
                new Vector3(start.x, end.y, end.z),
                new Vector3(end.x, end.y, end.z),
                // Bottom face
                new Vector3(start.x, start.y, start.z),
                new Vector3(end.x, start.y, start.z),
                new Vector3(start.x, start.y, end.z),
                new Vector3(end.x, start.y, end.z)
            };
            var triangles = BrickTables.CubicTriangles;
            var normals = BrickTables.CubicNormals;
            m_brickMesh.Clear();
            m_brickMesh.vertices = vertices;
            m_brickMesh.triangles = triangles;
            m_brickMesh.normals = normals;

            m_brickFilter.sharedMesh = m_brickMesh;
            m_brickCollider.sharedMesh = m_brickMesh;
            m_brickRenderer.sharedMaterial = new Material(Shader.Find("Shader Graphs/Brick"));
            m_brickRenderer.sharedMaterial.SetColor("_color", Random.ColorHSV());
        }
    }
}