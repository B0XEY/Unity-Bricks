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
            CreateMesh(brickEnd - brickStart);
        }
        private void CreateMesh(Vector3 brickEnd) {
            var start = Vector3.zero;
            var vertices = new Vector3[24] {
                // Front face
                new Vector3(start.x, start.y, start.z),
                new Vector3(brickEnd.x, start.y, start.z),
                new Vector3(start.x, brickEnd.y, start.z),
                new Vector3(brickEnd.x, brickEnd.y, start.z),
                // Back face
                new Vector3(start.x, start.y, brickEnd.z),
                new Vector3(brickEnd.x, start.y, brickEnd.z),
                new Vector3(start.x, brickEnd.y, brickEnd.z),
                new Vector3(brickEnd.x, brickEnd.y, brickEnd.z),
                // Left face
                new Vector3(start.x, start.y, start.z),
                new Vector3(start.x, brickEnd.y, start.z),
                new Vector3(start.x, start.y, brickEnd.z),
                new Vector3(start.x, brickEnd.y, brickEnd.z),
                // Right face
                new Vector3(brickEnd.x, start.y, start.z),
                new Vector3(brickEnd.x, brickEnd.y, start.z),
                new Vector3(brickEnd.x, start.y, brickEnd.z),
                new Vector3(brickEnd.x, brickEnd.y, brickEnd.z),
                // Top face
                new Vector3(start.x, brickEnd.y, start.z),
                new Vector3(brickEnd.x, brickEnd.y, start.z),
                new Vector3(start.x, brickEnd.y, brickEnd.z),
                new Vector3(brickEnd.x, brickEnd.y, brickEnd.z),
                // Bottom face
                new Vector3(start.x, start.y, start.z),
                new Vector3(brickEnd.x, start.y, start.z),
                new Vector3(start.x, start.y, brickEnd.z),
                new Vector3(brickEnd.x, start.y, brickEnd.z)
            };
            var triangles = BrickTables.CubicTriangles;
            var normals = BrickTables.CubicNormals;
            m_brickMesh.Clear();
            m_brickMesh.vertices = vertices;
            m_brickMesh.triangles = triangles;
            m_brickMesh.normals = normals;

            m_brickFilter.sharedMesh = m_brickMesh;
            m_brickCollider.sharedMesh = m_brickMesh;
            m_brickRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            m_brickRenderer.sharedMaterial.SetFloat("_Smoothness", 0);
            m_brickRenderer.sharedMaterial.color = Random.ColorHSV();
        }
    }
}