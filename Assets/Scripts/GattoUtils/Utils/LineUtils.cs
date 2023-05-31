using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gatto.Utils
{
    public class LineUtils
    {

        public static float simplificationTolerance = 1f; // Tolerância de simplificação

        // Função que realiza a simplificação da linha
        public static List<Vector2> SimplifyLine(List<Vector2> points)
        {
            if (points == null || points.Count < 3)
                return points;

            List<Vector2> simplifiedPoints = new List<Vector2>();
            simplifiedPoints.Add(points[0]);

            SimplifyRecursive(points, 0, points.Count - 1, simplifiedPoints);

            simplifiedPoints.Add(points[points.Count - 1]);

            return simplifiedPoints;
        }

        private static void SimplifyRecursive(List<Vector2> points, int startIndex, int endIndex, List<Vector2> simplifiedPoints)
        {
            float maxDistance = 0f;
            int maxIndex = 0;

            for (int i = startIndex + 1; i < endIndex; i++)
            {
                float distance = PerpendicularDistance(points[i], points[startIndex], points[endIndex]);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = i;
                }
            }

            if (maxDistance > simplificationTolerance)
            {
                simplifiedPoints.Add(points[maxIndex]);

                SimplifyRecursive(points, startIndex, maxIndex, simplifiedPoints);
                SimplifyRecursive(points, maxIndex, endIndex, simplifiedPoints);
            }
        }

        private static float PerpendicularDistance(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            float lineLength = Vector2.Distance(lineStart, lineEnd);

            if (lineLength == 0f)
                return Vector2.Distance(point, lineStart);

            float t = ((point.x - lineStart.x) * (lineEnd.x - lineStart.x) + (point.y - lineStart.y) * (lineEnd.y - lineStart.y)) / (lineLength * lineLength);
            t = Mathf.Clamp01(t);

            Vector2 projection = new Vector2(lineStart.x + t * (lineEnd.x - lineStart.x), lineStart.y + t * (lineEnd.y - lineStart.y));

            return Vector2.Distance(point, projection);
        }
    }
}

