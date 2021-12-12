// @author Code Monkey
// @source https://www.youtube.com/watch?v=waEsGu--9P8
//
// NOTE: Improved (Readability, Performance), added and enhanced functionality

using System;

using UnityEngine;

namespace LAIeRS.Miscellanious
{
    public class Grid2D<T>
    {
        public int Width { get; }
        public int Height { get; }
        
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        
        private int _cellWidth { get; }
        private int _cellHeight { get; }
        
        private readonly T[,] _items;
        
        public Grid2D(
            int gridWidth, int gridHeight, 
            int cellWidth, int cellHeight, 
            int gridPosX, int gridPosY, 
            Func<int, int, T> createGridItemAt = null)
        {
            _items = new T[gridWidth, gridHeight];
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            Height = gridHeight;
            Width = gridWidth;
            PosX = gridPosX;
            PosY = gridPosY;
            
            FillOutGrid(createGridItemAt);
        }
        
        public void DrawGrid(Color lineColor, float timeToShow)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++) {
                    Vector2 cellBottomLeftCorner = GetPosAtIndex(x, y);
                    Vector2 cellBottomRightCorner = GetPosAtIndex(x + 1, y);
                    Vector2 cellTopLeftCorner = GetPosAtIndex(x, y + 1);
                    Debug.DrawLine(cellBottomLeftCorner, cellBottomRightCorner, lineColor, timeToShow);
                    Debug.DrawLine(cellBottomLeftCorner, cellTopLeftCorner, lineColor, timeToShow); }

            Vector2 gridTopRightCorner = GetPosAtIndex(Width, Height);
            Vector2 gridTopLeftCorner = GetPosAtIndex(0, Height);
            Vector2 gridBottomRightCorner = GetPosAtIndex(Width, 0);

            Debug.DrawLine(gridTopRightCorner, gridTopLeftCorner, lineColor, timeToShow);
            Debug.DrawLine(gridTopRightCorner, gridBottomRightCorner, lineColor, timeToShow);
        }
        
        // TODO: Checkout if function is needed
        // TODO: Check out if "int casting" does lead to unexpected results
        public void UpdateGridPos(float x, float y)
        {
            PosX = (int)x;
            PosY = (int)y;
        }
        
        public void Foreach(Action<T> enumerationObject)
        {
            for (int y = 0; y < Height - 1; y++)
                for (int x = 0; x < Width - 1; x++)
                    enumerationObject(GetItemAtIndex(x, y));
        }
        
        public T GetItemAtIndex(int x, int y)
        {
            if (x >= 0 && x < Width &&
                y >= 0 && y < Height)
                return _items[x, y];
            
            return default;
        }
        
        public T GetItemAtPos(float x, float y)
        {
            (int x, int y) index = GetIndexAtPos(x, y);
            return GetItemAtIndex(index.x, index.y);
        }
        
        public void SetItemAtIndex(int x, int y, T item)
        {
            if (x >= 0 && x < Width &&
                y >= 0 && y < Height)
                _items[x, y] = item;
        }
        
        public void SetItemAtPos(float x, float y, T item)
        {
            var index = GetIndexAtPos(x, y);
            SetItemAtIndex(index.x, index.y, item);
        }
        
        public (int x, int y) GetIndexAtPos(float x, float y)
        {
            x = Mathf.FloorToInt((x - PosX) / _cellWidth);
            y = Mathf.FloorToInt((y - PosY) / _cellHeight);

            return ((int)x, (int)y);
        }
        
        public Vector2 GetPosAtIndex(int x, int y, bool centering = false)
        {
            var pos = GetWorldPositionAtIndex(x, y);

            if (centering)
            {
                float halfCellWidth = (float) _cellWidth / 2;
                float halfCellHeight = (float) _cellHeight / 2;
                pos.x += halfCellWidth;
                pos.y += halfCellHeight;
            }
            
            return new Vector2(pos.x, pos.y);
        }
        
        private void FillOutGrid(Func<int, int, T> createGridItemAt)
        {
            if (createGridItemAt == null) return;
            
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++) 
                    _items[x, y] = createGridItemAt(x, y);
        }
        
        private (float x, float y) GetWorldPositionAtIndex(int x, int y)
        {
            return (x * _cellWidth + PosX, y * _cellHeight + PosY);
        }
    }
}
