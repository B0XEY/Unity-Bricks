using System.Collections.Generic;
using UnityEngine;

namespace Boxey.Bricks.Core {
    public class Brick {
        public GameObject BrickObject;

        private readonly Mesh m_brickMesh;
        private readonly MeshFilter m_brickFilter;
        private readonly MeshRenderer m_brickRenderer;
        private readonly MeshCollider m_brickCollider;
        
        public Brick(Vector3 brickStart, Vector3 brickEnd, float brickHeight) {
            BrickObject = new GameObject {
                name = "Brick: " + brickStart
            };
            m_brickMesh = new Mesh {
                name = "Brick Mesh"
            };
            m_brickFilter = BrickObject.AddComponent<MeshFilter>();
            m_brickRenderer = BrickObject.AddComponent<MeshRenderer>();
            m_brickCollider = BrickObject.AddComponent<MeshCollider>();
            ClearData();
            CreateMesh(brickEnd - brickStart, brickHeight);
        }
        private void CreateMesh(Vector3 brickEnd, float height) {
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
                0, 1, 2,
                2, 1, 3,
                // Back
                5, 4, 7,
                7, 4, 6,
                // Left
                4, 0, 6,
                6, 0, 2,
                // Right
                1, 5, 3,
                3, 5, 7,
                // Top
                2, 3, 6,
                6, 3, 7,
                // Bottom
                1, 0, 5,
                5, 0, 4
            };
            m_brickMesh.Clear();
            m_brickMesh.vertices = vertices;
            m_brickMesh.triangles = triangles;
            m_brickMesh.RecalculateNormals();
            m_brickFilter.sharedMesh = m_brickMesh;
            m_brickCollider.sharedMesh = m_brickMesh;
            m_brickRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }
        private void ClearData() {
            m_brickMesh.Clear();
        }
    }
}