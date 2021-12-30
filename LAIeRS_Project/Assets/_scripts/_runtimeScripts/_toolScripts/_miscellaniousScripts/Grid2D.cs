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
        
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        
        private int _cellWidth { get; }
        private int _cellHeight { get; }
        
        private readonly T[,] _items;
        
        public Grid2D(
            int gridWidth, int gridHeight, 
            int cellWidth, int cellHeight, 
            int gridPositionX, int gridPositionY, 
            Func<int, int, T> createGridItemAt = null)
        {
            _items = new T[gridWidth, gridHeight];
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            Height = gridHeight;
            Width = gridWidth;
            PositionX = gridPositionX;
            PositionY = gridPositionY;
            
            FillOutGrid(createGridItemAt);
        }
        
        // TODO: Add to visualizer class
        public void DrawGrid(Color color, float duration)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++) {
                    Vector2 cellBottomLeftCorner = GetPositionAtIndex(x, y);
                    Vector2 cellBottomRightCorner = GetPositionAtIndex(x + 1, y);
                    Vector2 cellTopLeftCorner = GetPositionAtIndex(x, y + 1);
                    Debug.DrawLine(cellBottomLeftCorner, cellBottomRightCorner, color, duration);
                    Debug.DrawLine(cellBottomLeftCorner, cellTopLeftCorner, color, duration); }

            Vector2 gridTopRightCorner = GetPositionAtIndex(Width, Height);
            Vector2 gridTopLeftCorner = GetPositionAtIndex(0, Height);
            Vector2 gridBottomRightCorner = GetPositionAtIndex(Width, 0);

            Debug.DrawLine(gridTopRightCorner, gridTopLeftCorner, color, duration);
            Debug.DrawLine(gridTopRightCorner, gridBottomRightCorner, color, duration);
        }
        
        // TODO: Checkout if function is needed
        // TODO: Check out if "int casting" does lead to unexpected results
        public void SetPosition(float gridPositionX, float gridPositionY)
        {
            PositionX = (int)gridPositionX;
            PositionY = (int)gridPositionY;
        }
        
        public void Foreach(Action<T> enumerationObject)
        {
            for (int y = 0; y < Height - 1; y++) {
                for (int x = 0; x < Width - 1; x++)
                {
                    T item = GetItemAtIndex(x, y);
                    
                    if (item != null)
                        enumerationObject(item);
                }
            }
        }
        
        public T GetItemAtIndex(int i, int j)
        {
            if (i >= 0 && i < Width &&
                j >= 0 && j < Height)
                return _items[i, j];
            
            return default;
        }
        
        public T GetItemAtPosition(float x, float y)
        {
            (int i, int j) indexTuple = GetIndexAtPosition(x, y);
            
            return GetItemAtIndex(indexTuple.i, indexTuple.j);
        }
        
        public void SetItemAtIndex(int i, int j, T item)
        {
            if (i >= 0 && i < Width &&
                j >= 0 && j < Height)
                _items[i, j] = item;
        }
        
        public void SetItemAtPosition(float x, float y, T item)
        {
            (int i, int j) indexTuple = GetIndexAtPosition(x, y);
            
            SetItemAtIndex(indexTuple.i, indexTuple.j, item);
        }
        
        public (int x, int y) GetIndexAtPosition(float x, float y)
        {
            x = Mathf.FloorToInt((x - PositionX) / _cellWidth);
            y = Mathf.FloorToInt((y - PositionY) / _cellHeight);

            return ((int)x, (int)y);
        }
        
        public Vector2 GetPositionAtIndex(int x, int y, bool centering = false)
        {
            (float x, float y) position = GetWorldPositionAtIndex(x, y);

            if (centering)
            {
                float halfCellWidth = (float) _cellWidth / 2;
                float halfCellHeight = (float) _cellHeight / 2;
                position.x += halfCellWidth;
                position.y += halfCellHeight;
            }
            
            return new Vector2(position.x, position.y);
        }
        
        private void FillOutGrid(Func<int, int, T> createGridItemAt)
        {
            if (createGridItemAt == null) return;
            
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++) 
                    _items[x, y] = createGridItemAt(x, y);
        }
        
        private (float x, float y) GetWorldPositionAtIndex(int i, int j)
        {
            return (i * _cellWidth + PositionX, j * _cellHeight + PositionY);
        }
    }
}
