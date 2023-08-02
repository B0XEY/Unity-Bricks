using System.Collections.Generic;
using UnityEngine;

namespace Boxey.Bricks.Core {
    public class Brick {
        public GameObject BrickObject;
        
        private List<Vector3> m_brickVerts;
        private List<int> m_brickTriangles;


        private Mesh m_brickMesh;
        private MeshFilter m_brickFilter;
        private MeshRenderer m_brickRenderer;
        private MeshCollider m_brickCollider;
        
        public Brick(Vector3 brickLocation, Vector2 brickStart, Vector2 brickEnd, float brickHeight) {
            CreateData(brickEnd - brickStart, brickHeight);
            //Create mesh and gameObject
        }


        private void CreateData(Vector2 end, float height) {
            var positions = new Vector3[8];
            for (int i = 0; i < 4; i++) {
                var x = (i % 2 == 0) ? 0 : end.x;
                var z = (i < 2) ? 0 : end.y;
                var y = (i < 4) ? 0 : height;

                positions[i] = new Vector3(x, y, z);
                positions[i + 4] = new Vector3(x, y, z);
            }

            foreach (var vert in positions) {
                m_brickVerts.Add(vert);
                m_brickTriangles.Add(m_brickVerts.Count - 1);
            }
        }
    }
}