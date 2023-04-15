using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gatto
{
    namespace Tools
{
    public class Grid<TGridObject>
    {
        private int width;
        private int height;
        private float cellSize;

        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }


        private Vector3 originPosition;

        private TGridObject[,] gridArray;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>,int,int,TGridObject> OnCreateGridObject = null, bool debug = false)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            this.originPosition = originPosition;

            gridArray = new TGridObject[width,height];

            if(OnCreateGridObject != null)
                for(int x = 0; x < gridArray.GetLength(0);x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        gridArray[x,y] = OnCreateGridObject(this,x,y);
                    }
                }

            if(debug)
            {
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        
                        Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y), Color.white, 100f);
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
            int x,y;
            GetXY(worldPosition,out x,out y);

            return CanPlace(x,y);
        }

        public Vector3 GetWorldPosition(int x, int y, bool withOffset = false)
        {
            if(withOffset)
                return (((new Vector3(x,y) * cellSize) + new Vector3(cellSize,cellSize,0) * .5f) + originPosition);
            
            return new Vector3(x,y) * cellSize + originPosition;
        }

        public Vector3 GetPosition(Vector3 pos)
        {
            int x, y;
            GetXY(pos, out x, out y);

            

            return GetWorldPosition(x,y,true);
        }

        public bool GetIsInGrid(int x, int y)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                return true;
            }
            return false;
        }

        public bool GetIsInGrid(Vector2 worldPosition)
        {
            int x;
            int y;
            GetXY(worldPosition, out x, out y);

            return GetIsInGrid(x,y);
        }
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x/cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y/cellSize);
        }


        public void TriggerGridObjectChanged(int x, int y)
        {

        }

        public void SetValue(int x, int y, TGridObject value)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x,y] = value;
                if(OnGridValueChanged != null) OnGridValueChanged(this,new OnGridValueChangedEventArgs{x = x, y = y});
            }
        }

        public void SetValue(Vector3 worldPosition, TGridObject value)
        {
            int x,y;
            GetXY(worldPosition,out x,out y);

            SetValue(x,y, value);
        }

        public TGridObject GetValue(int x, int y)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x,y];
            }else{
                return default(TGridObject);
            }
        }

        public TGridObject GetValue(Vector3 worldPosition)
        {
            int x,y;
            GetXY(worldPosition, out x, out y);

            return GetValue(x,y);
        }

        public TGridObject GetValue(int x, int y, out bool couldReturn)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                couldReturn = true;
                return gridArray[x,y];
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
            int x,y;
            x = UnityEngine.Random.Range(0, width);
            y = UnityEngine.Random.Range(0, height);

            return new Vector3(x,y,0) * cellSize + originPosition;
        }
    }
}
}

