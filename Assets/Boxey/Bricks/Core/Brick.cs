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
            position.y += 0.00001f;
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
            ClearData();
            CreateMesh(brickEnd - brickStart);
        }
        private void CreateMesh(Vector3 brickEnd) {
            var start = Vector3.zero;
            var vertices = new Vector3[8] {
                new (start.x, start.y, start.z),
                new (brickEnd.x, start.y, start.z),
                new (start.x, brickEnd.y, start.z),
                new (brickEnd.x, brickEnd.y, start.z),
                new (start.x, start.y, brickEnd.z),
                new (brickEnd.x, start.y, brickEnd.z),
                new (start.x, brickEnd.y, brickEnd.z),
                new (brickEnd.x, brickEnd.y, brickEnd.z)
            };
            var triangles = new int[36] {
                // Front
                0, 2, 1,
                2, 3, 1,
                // Back
                5, 7, 4,
                7, 6, 4,
                // Left
                4, 6, 0,
                6, 2, 0,
                // Right
                1, 3, 5,
                3, 7, 5,
                // Top
                2, 6, 3,
                6, 7, 3,
                // Bottom
                1, 5, 0,
                5, 4, 0
            };
            var normals = new Vector3[8] {
                Vector3.left,
                Vector3.right,
                Vector3.down,
                Vector3.up,
                Vector3.forward,
                Vector3.back,
                Vector3.forward,
                Vector3.back
            };

            m_brickMesh.Clear();
            m_brickMesh.vertices = vertices;
            m_brickMesh.triangles = triangles;
            m_brickMesh.normals = normals;

            m_brickFilter.sharedMesh = m_brickMesh;
            m_brickCollider.sharedMesh = m_brickMesh;
            m_brickRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            m_brickRenderer.sharedMaterial.color = Random.ColorHSV();
        }
        private void ClearData() {
            m_brickMesh.Clear();
        }
    }
}