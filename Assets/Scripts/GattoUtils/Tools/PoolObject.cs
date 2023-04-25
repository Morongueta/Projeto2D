using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gatto;

namespace Gatto.Utils
{
    public class PoolObject
    {
        public static List<PooledObject> poolObjects = new List<PooledObject>();

        private static void SetupPool()
        {
            GameObject[] objs = Resources.LoadAll<GameObject>("PoolObjects/");
            
            if(poolObjects.Count != objs.Length)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    PooledObject pool = new PooledObject();
                    pool.objectName = objs[i].name;
                    pool.objectReference = objs[i];

                    poolObjects.Add(pool);
                }
            }
        }

        public static GameObject Instantiate(string objectName, Vector3 position, Quaternion rotation)
        {
            SetupPool();
            PooledObject pool = FindObject(objectName);

            if(pool == null)
            {
                Debug.LogError("Pool not found: " + objectName);
                return null;
            }
            
            GameObject obj = pool.GetObject();

            obj.transform.position = position;
            obj.transform.rotation = rotation;

            return obj;
        }

        public static void Destroy(GameObject obj, float timed = 0f)
        {
            PooledObject pool = FindObject(obj.name.Replace("(Clone)", ""));

            if(pool == null)
            {
                Debug.LogError("Pool not found in Destroy");
                return;
            }

            PeriodTimer.Timer(timed, ()=>{
                GameObject poolObj = pool.DestroyObject(obj);
                if(poolObj != null)
                {
                    poolObj.transform.position = new Vector3(10000, 10000, 0);
                }else{
                    Debug.LogError("Object not found: " + obj.name + " In pool: " + pool.objectName);
                }
    
            });
        }

        private static PooledObject FindObject(string objectName)
        {
            PooledObject result = null;

            for (int i = 0; i < poolObjects.Count; i++)
            {
                if(poolObjects[i].objectName.ToLower() == objectName.ToLower())
                {
                    result = poolObjects[i];
                    break;
                }
            }

            return result;
        }

        private static PooledObject FindObject(GameObject objectRef)
        {
            PooledObject result = null;

            for (int i = 0; i < poolObjects.Count; i++)
            {
                if(poolObjects[i].objectReference == objectRef)
                {
                    result = poolObjects[i];
                    break;
                }
            }

            return result;
        }
    }



    public class PooledObject
    {
        public string objectName;
        public GameObject objectReference;
        public List<GameObject> freeToUse = new List<GameObject>();
        public List<GameObject> inUse = new List<GameObject>();

        public GameObject GetObject()
        {
            GameObject result = null;

            CleanPool();

            if(freeToUse.Count <= 0)
            {
                AddObjects(9);
            }

            result = freeToUse[0];
            freeToUse.Remove(result);
            inUse.Add(result);
            result.SetActive(true);
            return result;
        }

        public void CleanPool()
        {
            for (int i = 0; i < freeToUse.Count;)
            {
                if(freeToUse[i] == null)
                {
                    freeToUse.RemoveAt(i);
                }else{
                    i++;
                }
            }

            for (int i = 0; i < inUse.Count;)
            {
                if(inUse[i] == null)
                {
                    inUse.RemoveAt(i);
                }else{
                    i++;
                }
            }
        }

        public GameObject DestroyObject(GameObject obj)
        {
            if(!inUse.Contains(obj))
            {
                return null;
            }
            inUse.Remove(obj);
            freeToUse.Add(obj);
            obj.SetActive(false);
            return obj;
        }

        public void AddObjects(int amount)
        {
            int index = 0;
            
            while (index < amount)
            {
                GameObject newInstance = MonoBehaviour.Instantiate(objectReference, new Vector2(10000,10000), Quaternion.identity);
                freeToUse.Add(newInstance);
                newInstance.SetActive(false);
                index++;
            }
        }
    }
}
