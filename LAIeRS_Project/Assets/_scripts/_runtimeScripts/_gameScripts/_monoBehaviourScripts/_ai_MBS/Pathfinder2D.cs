using System.Collections.Generic;

using UnityEngine;

using LAIeRS.Miscellanious;
using LAIeRS.Settings;
using LAIeRS.Pathfinding;

namespace NKStudios.Pathfinding
{
    // TODO: Consider turning Monobehaviour into static class
    [DisallowMultipleComponent]
    public class Pathfinder2D : MonoBehaviour
    {
        [SerializeField] public GameSettings _gameSettings;
        
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 2;
        
        [Header("Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private LayerMask objectsToCollideWith;
        [SerializeField] private float pathfindExecutionIntervall = 1f;
        
        [Header("Monitoring")]
        [SerializeField] private float elapsedTime;

        private Transform _current => this.transform;
        private Rigidbody2D _rigidBody2D;
        
        public Grid2D<Node2D> nodeGrid2D;

        private List<Vector2> _currentPath;
        private int _currentIndex = 0;

        
        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            nodeGrid2D = new Grid2D<Node2D>(
                _gameSettings.RoomWidth, _gameSettings.RoomHeight, 
                1, 1, 
                -15, -5, 
                (x, y) => new Node2D(x, y));
            
            nodeGrid2D.DrawGrid(Color.black, 1000);

            nodeGrid2D.GetItemAtIndex(5, 0).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(5, 1).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(5, 2).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(5, 3).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(5, 4).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(5, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(5, 6).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(5, 7).IsWalkable = false;
            
            nodeGrid2D.GetItemAtIndex(4, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(3, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(2, 5).IsWalkable = false;
            
            nodeGrid2D.GetItemAtIndex(9, 13).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 12).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 11).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 9).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 8).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 7).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 6).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 4).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 3).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(9, 2).IsWalkable = false;
            
            nodeGrid2D.GetItemAtIndex(8, 3).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(7, 3).IsWalkable = false;
            
            nodeGrid2D.GetItemAtIndex(6, 7).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(8, 7).IsWalkable = false;
            
            nodeGrid2D.GetItemAtIndex(9, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(10, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(11, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(12, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(13, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(14, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(15, 5).IsWalkable = false;
            
            nodeGrid2D.GetItemAtIndex(21, 9).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 8).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 7).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 6).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 5).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 4).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 3).IsWalkable = false;

            nodeGrid2D.GetItemAtIndex(25, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(24, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(23, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(22, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(20, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(19, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(18, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(17, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(16, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(15, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(14, 10).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(13, 10).IsWalkable = false;
            
            nodeGrid2D.GetItemAtIndex(18, 9).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(18, 8).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(18, 7).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(18, 6).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(19, 6).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(20, 6).IsWalkable = false;
            nodeGrid2D.GetItemAtIndex(21, 6).IsWalkable = false;
            
            nodeGrid2D.Foreach(node =>
            {
                if (!node.IsWalkable)
                    Visualizer.DrawSquareAt(new Vector2(
                        nodeGrid2D.PositionX + node.X, nodeGrid2D.PositionY +  node.Y), 1, Color.red, 1000);
            });
            
            _currentPath = new List<Vector2>();
            elapsedTime = pathfindExecutionIntervall;
        }
        
        private void Update()
        {
            elapsedTime += Time.deltaTime;
            
            // TODO: Start idling when no target exists
            if (target == null) return;

            Vector2 currentPos = _current.position;
            Vector2 targetPos = target.position;
                
            Vector2 direction = (targetPos - currentPos).normalized;
            float distance = Vector2.Distance(currentPos, targetPos);
                
            RaycastHit2D hitInfo = Physics2D.Raycast(currentPos, direction, distance, objectsToCollideWith);
                
            if (_currentPath.Count > 0)
            {
                if (_currentIndex < _currentPath.Count)
                {
                    Vector2 nextGoToPosition = _currentPath[_currentIndex];
                        
                    if (Vector2.Distance(currentPos, nextGoToPosition) > GetComponent<CircleCollider2D>().radius)
                    {
                        Vector2 currentDirection = (nextGoToPosition - currentPos).normalized;
                        MoveTowards(currentDirection);
                    }
                    else _currentIndex++;
                }
                else
                {
                    ClearCurrentPath();
                    _rigidBody2D.velocity = Vector2.zero;
                }
            }
            
            if (elapsedTime >= pathfindExecutionIntervall)
            {
                elapsedTime = 0;
                
                List<Node2D> foundPath =
                    AStarPathfinder2D.FindPath(currentPos.x, currentPos.y, targetPos.x, targetPos.y, nodeGrid2D);
                    
                if (foundPath != null)
                {
                    ClearCurrentPath();
                        
                    foundPath.ForEach(node => _currentPath.Add(nodeGrid2D.GetPositionAtIndex(node.X, node.Y, true)));
                        
                    DrawPath(foundPath);
                }
            }
            
            // Make node un-/walkable
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                Node2D node = nodeGrid2D.GetItemAtPosition(mousePos.x, mousePos.y);
                
                node.IsWalkable = (!node.IsWalkable);
            }
        }
        
        private void ClearCurrentPath()
        {
            _currentPath.Clear();
            _currentIndex = 0;
        }
        
        private void DrawPath(List<Node2D> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                (int x, int y) indexTpl1 = (path[i].X, path[i].Y);
                (int x, int y) indexTpl2 = (path[i + 1].X, path[i + 1].Y);
                
                Debug.DrawLine(
                    nodeGrid2D.GetPositionAtIndex(indexTpl1.x, indexTpl1.y, true),
                    nodeGrid2D.GetPositionAtIndex(indexTpl2.x, indexTpl2.y, true),
                    Color.green, 4);
            }
        }
        
        // TODO: Consider moving to appropriate class
        private void MoveTowards(Vector2 direction)
        {
            // TODO: Get rid of magic number
            _rigidBody2D.velocity = direction * movementSpeed;
        }
    }
}
