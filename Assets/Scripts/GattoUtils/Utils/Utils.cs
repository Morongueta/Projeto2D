using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gatto.Utils
{
    public class Utils 
    {
        public static GameObject GetClosestObject(Vector3 from, GameObject[] array)
        {
            if(array.Length <= 0) return null;

            GameObject result = array[0];
            float dist = Vector2.Distance(from, result.transform.position);

            for (int i = 0; i < array.Length; i++)
            {
                if(dist > Vector2.Distance(from, array[i].transform.position))
                {
                    result = array[i];
                    dist = Vector2.Distance(from, array[i].transform.position);
                }
            }

            return result;
        }
    }
}
