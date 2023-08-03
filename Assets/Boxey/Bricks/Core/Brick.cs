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
            var triangles = new int[36] {
                // Front
                0, 2, 1,
                1, 2, 3,
                // Back
                4, 6, 5,
                5, 6, 7,
                // Left
                8, 10, 9,
                9, 10, 11,
                // Right
                12, 14, 13,
                13, 14, 15,
                // Top
                16, 18, 17,
                17, 18, 19,
                // Bottom
                20, 22, 21,
                21, 22, 23
            };
            Vector3[] normals = new Vector3[24];
            // Assign the correct normals to each vertex
            for (int i = 0; i < 4; i++) {
                normals[i] = Vector3.forward;
                normals[i + 4] = Vector3.back;
                normals[i + 8] = Vector3.left;
                normals[i + 12] = Vector3.right;
                normals[i + 16] = Vector3.up;
                normals[i + 20] = Vector3.down;
            }
            m_brickMesh.Clear();
            m_brickMesh.vertices = vertices;
            m_brickMesh.triangles = triangles;
            m_brickMesh.RecalculateNormals();

            m_brickFilter.sharedMesh = m_brickMesh;
            m_brickCollider.sharedMesh = m_brickMesh;
            m_brickRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            m_brickRenderer.sharedMaterial.SetFloat("_Smoothness", 0);
            m_brickRenderer.sharedMaterial.color = Random.ColorHSV();
        }
        private void ClearData() {
            m_brickMesh.Clear();
        }
    }
}