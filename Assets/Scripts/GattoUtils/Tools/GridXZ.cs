using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gatto
{
    namespace Tools
    {
    public class GridXZ<TGridObject>
    {
        private int width;
        private int height;
        private float cellSize;

        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int z;
        }


        private Vector3 originPosition;

        private TGridObject[,] gridArray;

        public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>,int,int,TGridObject> OnCreateGridObject = null, bool debug = false)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            this.originPosition = originPosition;

            gridArray = new TGridObject[width,height];

            for(int x = 0; x < gridArray.GetLength(0);x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    gridArray[x,z] = OnCreateGridObject(this,x,z);
                }
            }

            if(debug)
            {
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int z = 0; z < gridArray.GetLength(1); z++)
                    {
                        
                        Debug.DrawLine(GetWorldPosition(x,z), GetWorldPosition(x,z+1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x,z), GetWorldPosition(x+1,z), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.white, 100f);
            }
            
        }

        public int GetWidth()
        {
            return width;
        }
        public int GetHeight()
        {
            return height;
        }

        public bool CanPlace(int x, int y)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                return true;
            }
            return false;
        }
        public bool CanPlace(Vector3 worldPosition)
        {
            int x,z;
            GetXZ(worldPosition,out x,out z);

            return CanPlace(x,z);
        }

        public Vector3 GetWorldPosition(int x, int z, bool withOffset = false)
        {
            if(withOffset)
                return (((new Vector3(x,0f,z) * cellSize) + new Vector3(cellSize,cellSize,0) * .5f) + originPosition);
            
            return new Vector3(x,0f,z) * cellSize + originPosition;
        }

        public Vector3 GetPosition(Vector3 pos)
        {
            int x, z;
            GetXZ(pos, out x, out z);

            

            return GetWorldPosition(x,z,true);
        }

        public bool GetIsInGrid(int x, int z)
        {
            if(x >= 0 && z >= 0 && x < width && z < height)
            {
                return true;
            }
            return false;
        }

        public bool GetIsInGrid(Vector2 worldPosition)
        {
            int x;
            int z;
            GetXZ(worldPosition, out x, out z);

            return GetIsInGrid(x,z);
        }
        public void GetXZ(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x/cellSize);
            z = Mathf.FloorToInt((worldPosition - originPosition).z/cellSize);
        }


        public void TriggerGridObjectChanged(int x, int z)
        {

        }

        public void SetValue(int x, int z, TGridObject value)
        {
            if(x >= 0 && z >= 0 && x < width && z < height)
            {
                gridArray[x,z] = value;
                if(OnGridValueChanged != null) OnGridValueChanged(this,new OnGridValueChangedEventArgs{x = x, z = z});
            }
        }

        public void SetValue(Vector3 worldPosition, TGridObject value)
        {
            int x,z;
            GetXZ(worldPosition,out x,out z);

            SetValue(x,z, value);
        }

        public TGridObject GetValue(int x, int z)
        {
            if(x >= 0 && z >= 0 && x < width && z < height)
            {
                return gridArray[x,z];
            }else{
                return default(TGridObject);
            }
        }

        public TGridObject GetValue(Vector3 worldPosition)
        {
            int x,z;
            GetXZ(worldPosition, out x, out z);

            return GetValue(x,z);
        }

        public TGridObject GetValue(int x, int z, out bool couldReturn)
        {
            if(x >= 0 && z >= 0 && x < width && z < height)
            {
                couldReturn = true;
                return gridArray[x,z];
            }else{
                couldReturn = false;
                return default(TGridObject);
            }
        }
        public Vector2 GetGridCenter()
        {
            return new Vector3(width/2,height/2,0) * cellSize + originPosition;
        }
        public Vector2 GetRandomTilePos()
        {
            int x,z;
            x = UnityEngine.Random.Range(0, width);
            z = UnityEngine.Random.Range(0, height);

            return new Vector3(x,z,0) * cellSize + originPosition;
        }
    }
}
}

