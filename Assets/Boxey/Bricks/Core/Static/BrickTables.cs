using UnityEngine;

namespace Boxey.Bricks.Core {
    public static class BrickTables {
        public static readonly int[] CubicTriangles = {
            // Front
            0, 2, 1,
            1, 2, 3,
            // Back
            5, 7, 4,
            4, 7, 6,
            // Left
            8, 10, 9,
            9, 10, 11,
            // Right
            13, 15, 14,
            13, 14, 12,
            // Top
            16, 18, 17,
            17, 18, 19,
            // Bottom
            20, 21, 22,
            22, 21, 23
        };
        public static readonly Vector3[] CubicNormals = {
            // Front face
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
            // Back face
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
            // Left face
            Vector3.left,
            Vector3.left,
            Vector3.left,
            Vector3.left,
            // Right face
            Vector3.right,
            Vector3.right,
            Vector3.right,
            Vector3.right,
            // Top face
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            // Bottom face
            Vector3.down,
            Vector3.down,
            Vector3.down,
            Vector3.down
        };
    }
}