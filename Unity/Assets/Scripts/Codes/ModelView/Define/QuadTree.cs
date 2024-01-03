using System.Collections.Generic;
using ET;
using UnityEngine;
using ET;

namespace Model
{
    //四叉树算法
    public class QuadTree
    {
        private Node root;
        private int maxDepth = 3;

        private Dictionary<IQuadTreeUnit, Node[]> _unitPos = new Dictionary<IQuadTreeUnit,Node[]>();
        
        public QuadTree(Rect rect)
        {
            root = new Node(1, rect, null, this);
        }

        public void Update(IQuadTreeUnit unit)
        {
            var list = ListComponent<Node>.Create();;
            var r = unit.GetRect();
            if (_unitPos.ContainsKey(unit))
            {
                var oldNodes = _unitPos[unit];
                foreach (var oldNode in oldNodes)
                {
                    oldNode.Remove(unit);
                }
                root.Add(unit, r, list);
            }
            else
            {
                root.Add(unit, r, list);
            }

            _unitPos[unit] = list.ToArray();
            list.Dispose();
        }

        public bool IsOverlaps(Rect rect)
        {
            return root.IsOverlaps(rect);
        }

        public IQuadTreeUnit[] Overlaps(Rect rect)
        {
            var list = ListComponent<IQuadTreeUnit>.Create();
            root.Find(rect, list);
            var res = list.ToArray();
            list.Dispose();
            return res;
        }

        public IQuadTreeUnit Contain(Vector2 point)
        {
            return root.Contain(point);
        }

        public void Remove(IQuadTreeUnit unit)
        {
            if (_unitPos.ContainsKey(unit))
            {
                var oldNodes = _unitPos[unit];
                foreach (var oldNode in oldNodes)
                {
                    oldNode.Remove(unit);
                }

                _unitPos.Remove(unit);
            }
        }
        
        public void OnDrawGizmos()
        {
            root.OnDrawGizmos();
        }
        
        
        public class Node
        {
            public int Id;
            private int _depth;
            private Rect _rect;
            private QuadTree _tree;
            private Node _parent;
            private Node[] _children;
            private UnOrderList<IQuadTreeUnit> _dataList = new UnOrderList<IQuadTreeUnit>();
            
            public Node(int depth, Rect r, Node p, QuadTree belongTree)
            {
                this._rect = r;
                this._tree = belongTree;
                this._parent = p;
                this._depth = depth;
                if (depth <= belongTree.maxDepth)
                {
                    _children = new Node[4];
                    for (int i = 0; i < 4; i++)
                    {
                        var xSymbol = (i == 1 || i == 2) ? 0.5f : 0;
                        var ySymbol = (i == 0 || i == 1) ? 0f : 0.5f;
                        var childRect = new Rect(_rect.position +
                            new Vector2(_rect.width * xSymbol, _rect.height * ySymbol),
                            new Vector2(_rect.width / 2, _rect.height / 2)
                            );
                        _children[i] = new Node(depth + 1, childRect, this, _tree);
                    }
                }
            }

            public void Add(IQuadTreeUnit unit, Rect r, List<Node> result)
            {
                if (_rect.Overlaps(r))
                {
                    if (_children == null)
                    {
                        _dataList.Add(unit);
                        result.Add(this);
                    }
                    else
                    {
                        for (int i = 0; i < _children.Length; i++)
                        {
                            var node = _children[i];
                            node.Add(unit, r, result);
                        }
                    }
                }
            }

            public bool IsOverlaps(Rect r)
            {
                if (_rect.Overlaps(r))
                {
                    if (_children == null)
                    {
                        foreach (var unit in _dataList)
                        {
                            if (r.Overlaps(unit.GetRect()))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var node in _children)
                        {
                            if (node.IsOverlaps(r))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            
            public void Find(Rect r, List<IQuadTreeUnit> result)
            {
                if (_rect.Overlaps(r))
                {
                    if (_children == null)
                    {
                        foreach (var unit in _dataList)
                        {
                            if (r.Overlaps(unit.GetRect()))
                            {
                                result.Add(unit);
                            }
                        }
                    }
                    else
                    {
                        foreach (var node in _children)
                        {
                            node.Find(r, result);
                        }
                    }
                }
            }

            public IQuadTreeUnit Contain(Vector2 point)
            {
                if (_rect.Contains(point))
                {
                    if (_children == null)
                    {
                        foreach (var unit in _dataList)
                        {
                            if (unit.GetRect().Contains(point))
                                return unit;
                        }
                    }
                    else
                    {
                        foreach (var node in _children)
                        {
                            var res = node.Contain(point);
                            if (res != null)
                                return res;
                        }
                    }
                }

                return null;
            }

            public void Remove(IQuadTreeUnit unit)
            {
                _dataList.Remove(unit);
            }

            public void OnDrawGizmos()
            {
                var c = Color.white;
                switch (_depth)
                {
                    case 1:
                        c = Color.white;
                        break;
                    case 2:
                        c = Color.cyan;
                        break;
                    case 3:
                        c = Color.magenta;
                        break;
                    case 4:
                        c = Color.yellow;
                        break;
                }

                c.a = 1f;
                
                if (_children != null)
                {
                    foreach (var node in _children)
                    {
                        node.OnDrawGizmos();
                    }
                }
                GizmosHelper.DrawRect(_rect, c);

                if (_dataList.Count > 0)
                {
                    GizmosHelper.Draw2DCube(_rect, new Color(0, 1, 0, 0.5f));
                }
            }

        }
        
        
        public interface IQuadTreeUnit
        {
            Rect GetRect();
        }
        
    }
    
}