using UnityEngine;

public static class HexHelper
{
    //outer array  - [0] = even rows [1] = odd rows
    //middle array - 6 directions around a hex
    //inner array  - x and y offset for that direction
    private static readonly int[][][] OddRDirectionDifferences = new int[][][]
    {
        //even Rows
        new int[][] { new int[] {1, 0}, new int[] {0, -1}, new int[] {-1, -1},
                      new int[] {-1, 0}, new int[] {-1, 1}, new int[] {0, 1} },
        //odd Rows
        new int[][] { new int[] {1, 0}, new int[] {1, -1}, new int[] {0, -1},
                      new int[] {-1, 0}, new int[] {0, 1}, new int[] {1, 1} }
    };
    public static Vector3 GetWorldPosition(int x, int z, float radius)
    {
        //horizontal distance between adjacent hex centers
        float width = Mathf.Sqrt(3) * radius;

        float height = 1.5f * radius;

        //if the row is odd then offset the x position by half of the idth
        float xOffset = (z % 2 == 1) ? (width * 0.5f) : 0;

        float xPos = (x * width) + xOffset;
        float zPos = z * height;

        return new Vector3(xPos, 0, zPos);
    }

    public static Vector2Int GetNeighbor(Vector2Int hex, int direction)
    {
        int parity = hex.y & 1;
        int[] diff = OddRDirectionDifferences[parity][direction % 6];
        return new Vector2Int(hex.x + diff[0], hex.y + diff[1]);
    }
}
