using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public class Manager : MonoBehaviour
{
    public delegate void OnLevelUp();
    public event OnLevelUp UpLevel;
    public GameObject Pause;
    Graph MainGraph;
    public GameObject Player;
    [SerializeField] GameObject YouDied;

    public void Update()
    {
        ReadKeys();
    }

    void ReadKeys()
    {
        //
        //
        if (Input.GetButtonDown("Pause"))
        {
            if (Pause.active == false)
            {
                Service.SetGame(false);
                Pause.SetActive(true);
            }
            else
            {
                Service.SetGame(true);
                Pause.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        UpLevel += new OnLevelUp(LevelUpBots);
        Info.CharmLevel = 0;
        //test
        
        //endtest

        //garba


            //endgarbage

        Player = GameObject.Find("Hero");
        MainGraph = new Graph();
    }
    
    #region Методы обслуживания ботов
    /// <summary>
    /// Возвращает ближайшую доступную вершину к переданной
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    public Vector2 GetNearest(Vector2 coord)
    {
        Graph.Vertex v = MainGraph.GetNearest(coord);
        return new Vector2(v.x, v.y);
    }
    public Vector2 ChasePlayer(GameObject obj)
    {
        return ChasePoint(obj, Player.transform.position);
    }
    //TODO
    /// <summary>
    /// Возвращает первую вершину из списка последовательности вершин до определенной точки
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector2 ChasePoint(GameObject obj, Vector2 point)
    {
        List<Graph.Vertex> path = MainGraph.Dejikstra(MainGraph.GetNearest(obj.transform.position), MainGraph.GetNearest(point));
        float x, y;
        if (path.Count > 1)
        {
            x = path[1].x;
            y = path[1].y;
        }
        else
        {
            x = path[0].x;
            y = path[0].y;
        }
        return new Vector2(x, y);
    }

    /// <summary>
    /// Возвращает список вершин, которые находятся на одной плоскости с объектом
    /// </summary>
    /// <param name="startCoord">Координата объекта</param>
    /// <param name="range">Границы плоскости</param>
    /// <returns></returns>
    public List<Vector2> GetPlaneVertices(Vector2 startCoord, int range)
    {
        List<Vector2> plane = new List<Vector2>();
        Graph.Vertex startVertex = MainGraph.GetNearest(startCoord);
        foreach (Graph.Vertex v in MainGraph.GetPlaneVetices(startVertex, range))
        {
            plane.Add(new Vector2(v.x, v.y));
        }
        return plane;
    }
    #endregion

    #region LevelManagement
    public void OnDie()
    {
        YouDied.SetActive(true);
    }
    public void LevelUpBots()
    {
        Info.CharmLevel++;
        foreach (GameObject enm in FindObjectsOfType<GameObject>())
        {
            if (enm.GetComponent<ILevelable>() == null) continue;
            if(enm.GetComponent<ILevelable>().Level >= Info.CharmLevel)
            {
                enm.GetComponent<ILevelable>().SetVisability(true);
            }
        }
    }
    #endregion
}


public class Graph
{
    List<Vector2> tilesCoord = new List<Vector2>(); //координаты всех тайлей карты
    List<Vector2> accessibleTilesCoord = new List<Vector2>(); //коортинаты доступных для достижения тайлей карты
    public TileBase[] allTiles;
    BoundsInt bounds;
    List<Vector2> tiles;    
    List<Vertex> vertices = new List<Vertex>(); 
    List<Edge> edges = new List<Edge>(); 

    public Graph()
    {
        _initCoord();
        tiles = tilesCoord;
        foreach (Vector2 coord in accessibleTilesCoord)
        {
            vertices.Add(new Vertex(coord.x, coord.y + 1));
        }

        foreach (Vertex v1 in vertices)
        {
            foreach (Vertex v2 in vertices)
            {
                if (vertices.IndexOf(v2) > vertices.IndexOf(v1))
                {
                    CheckEdgeToAdd(v1, v2);
                }
            }
        }
    }
    //Graph(accessibleTilesCoord, tilesCoord);
    #region Операции над элементами графа
    Vertex GetHigher(Vertex v1, Vertex v2)
    {
        return v1.y > v2.y ? v1 : v2; 
    }
    bool IsHigher(Vertex v1, Vertex v2, int dif)
    {
        return v1.y == v2.y + dif ? true : false;
    }
    bool IsRight(Vertex v1, Vertex v2, int dif)
    {
        return v1.x == v2.x + dif ? true : false; 
    }
    bool IsUnobstructedX(Vertex v1, Vertex v2)
    {
        foreach (Vertex v in vertices)
        {
            if ((v.x > v1.x && v.x < v2.x) && (v.x < v1.x && v.x > v2.x))
                return false;
        }
        return true;
    }
    bool IsUnobstructedY(Vertex v1, Vertex v2)
    {
        foreach (Vector2 v in tiles)
        {
            if ((v.y > v1.y && v.y < v2.y && v.x == v2.x) || (v.y < v1.y && v.y > v2.y && v.x == v2.x))
                return false;
        }
        return true;
    }
    #endregion

    void _initCoord()
    {
        Tilemap tilemap = GameObject.Find("Grid").transform.GetChild(0).GetComponent<Tilemap>();
        bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3 coord = tilemap.layoutGrid.CellToWorld(new Vector3Int(x + bounds.min.x, y + bounds.min.y, 0));
                    tilesCoord.Add(new Vector2(coord.x, coord.y));
                }
            }
        }
        foreach (Vector2 tCoord in tilesCoord)
        {
            bool adding = true; // нужно ли добавлять эту вершину в список доступных вершин
            foreach (Vector2 tCoordToCompare in tilesCoord)
            {
                if (tCoord.y == tCoordToCompare.y - 1 && tCoord.x == tCoordToCompare.x)
                {
                    adding = false;
                    break;
                }

            }
            if (adding)
            {
                accessibleTilesCoord.Add(tCoord);
            }
        }

        
    }
    #region обслуживание юнитов
    public List<Vertex> GetPlaneVetices(Vertex v, int general)
    {
        return GetPlaneVetices(v, general, general);
    }
    public List<Vertex> GetPlaneVetices(Vertex v, int left, int right)
    {
        if(v == null)
            throw new System.NullReferenceException();
        List<Vertex> plane = new List<Vertex>();
        plane.Add(v);
        Vertex vertToCheck = v;
        while (left >= 0)
        {
            foreach (Vertex vert in GetHorizontalNeigbors(vertToCheck))
            {
                if (vert.x < vertToCheck.x)
                {
                    plane.Add(vert);
                    vertToCheck = vert;
                    break;
                }
            }
            left--;
        }
        vertToCheck = v;
        while (right > 0)
        {
            foreach (Vertex vert in GetHorizontalNeigbors(vertToCheck))
            {
                if (vert.x > vertToCheck.x)
                {
                    plane.Add(vert);
                    vertToCheck = vert;
                    break;
                }
            }
            right--;
        }
        foreach(Vertex vertex in plane)
        {
            //string line = "(" + vertex.x.ToString() + " , " + vertex.y.ToString() + ")"; 
            //Debug.Log(line);
        }
        return plane;
    }
    public List<Vertex> GetHorizontalNeigbors(Vertex v)
    {

        if (v == null)
            throw new System.NullReferenceException();

        List<Vertex> neighbors = new List<Vertex>();
        foreach (Vertex vert in Neigbors(v))
        {
            if (v.y == vert.y)
            {
                neighbors.Add(vert);
            }
        }
        return neighbors;
    }
    public List<Vertex> GetNeighbourVertices(Vertex v)
    {
        List<Vertex> neighbors = new List<Vertex>();

            foreach (Vertex vert in Neigbors(v))
            {
                if (Mathf.Abs(v.y - vert.y) <= 1)
                {
                    neighbors.Add(vert);
                }
            }
        return neighbors;
    }
    public Vertex GetNearest(Vector2 coord)
    {
        Vertex res = null;
        float minDif = 1000f;
        foreach (Vertex v in vertices)
        {
            if (Mathf.Sqrt((v.x + 0.5f - coord.x)*(v.x + 0.5f - coord.x) + (v.y - coord.y)*(v.y - coord.y)) < minDif)
            {
                res = v;
                minDif = Mathf.Sqrt((v.x + 0.5f - coord.x) * (v.x + 0.5f - coord.x) + (v.y - coord.y) * (v.y - coord.y));
            }
            
        }
        return res;
    }
    #endregion

    #region обработка графа
    void CheckEdgeToAdd(Vertex v1, Vertex v2)
    {
        if (Mathf.Abs(v1.x - v2.x) == 1 && v1.y == v2.y)
        {
            edges.Add(new Edge(v1, v2, 1));
        }
        if (IsHigher(v1, v2, 1))
        {
            if (IsRight(v2, v1, 1))
            {
                edges.Add(new Edge(v1, v2, 1, 1, 0));
            }
            if (IsRight(v1, v2, 1))
            {
                edges.Add(new Edge(v1, v2, 1, 0, 1));
            }
        }
        if (IsHigher(v2, v1, 1))
        {
            if (IsRight(v2, v1, 1))
            {
                edges.Add(new Edge(v1, v2, 1, 1, 0));
            }
            if (IsRight(v1, v2, 1))
            {
                edges.Add(new Edge(v1, v2, 1, 0, 1));
            }
        }
        if (GetHigher(v1, v2) == v1)
        {
            if (IsRight(v2, v1, 1) && IsUnobstructedY(v1, v2))
            {
                edges.Add(new Edge(v1, v2, 1));
            }

            if (IsRight(v1, v2, 1) && IsUnobstructedY(v1, v2))
            {
                edges.Add(new Edge(v1, v2, 1));
            }
        }
        if (GetHigher(v1, v2) == v2)
        {
            if (IsRight(v2, v1, 1) && IsUnobstructedY(v2, v1))
            {
                edges.Add(new Edge(v1, v2, 1, v2.y - v1.y, 0));
            }

            if (IsRight(v1, v2, 1) && IsUnobstructedY(v2, v1))
            {
                edges.Add(new Edge(v1, v2, 1, v2.y - v1.y, 0));
            }
        }
    }

    public Vertex FindVertex(Vector2 vector)
    {
        foreach (Vertex v in vertices)
        {
            if (v.x == vector.x && v.y == vector.y)
            {
                return v;
            }
        }
        return null;
    }

    public Vertex FindVertex(float x, float y)
    {
        foreach (Vertex v in vertices)
        {
            if(v.x == x && v.y == y)
            {
                return v;
            }
        }
        return null;

    }
    #endregion
    public class Edge
    {
        public Edge(Vertex v1, Vertex v2, int w)
        {
            this.v1 = v1;
            this.v2 = v2;
            jmptm_v1_v2 = 0;
            jmptm_v2_v1 = 0;
            weight = w;
        }

        public Edge(Vertex v1, Vertex v2, int w,  float jmptm_v1_v2, float jmptm_v2_v1)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.jmptm_v1_v2 = jmptm_v1_v2;
            this.jmptm_v2_v1 = jmptm_v2_v1;
            weight = w;
        }

        public Vertex v1;
        public Vertex v2;
        public float jmptm_v1_v2;
        public float jmptm_v2_v1;
        public int weight;
    }

    public class Vertex
    {
        public Vertex(float x, float y)
        {
            this.x = x;
            this.y = y;
            mark = 10000;
            prev = null;
            isChecked = false;
        }
        public bool isChecked;
        public float x;
        public float y;
        public int mark;
        public Vertex prev;

    }

    #region функции принта
    public void PrintGraph()
    {
        foreach (Vertex v in vertices)
        {
            //string line = "Vertex(" + v.x.ToString() + ", " + v.y.ToString() + ")";
            //Debug.Log(line);
            // MonoBehaviour.Instantiate(GameObject.Find("Circle"), new Vector3(v.x + 0.5f, v.y + 0.5f, 0), Quaternion.identity);
        }

        foreach (Edge e in edges)
        {
            //string line = "Edge (" + e.v1.x.ToString() + ", " + e.v1.y.ToString() + ")" + "(" + e.v2.x.ToString() + ", " + e.v2.y.ToString() + ")";
            //Debug.Log(line);
            Debug.DrawLine(new Vector3(e.v1.x + 0.5f, e.v1.y + 0.5f, 0), new Vector3(e.v2.x + 0.5f, e.v2.y + 0.5f, 0), Color.green, 1000f);
        }
    }
    public void PrintPath(int x1, int y1, int x2, int y2)
    {
        List<Vertex> path = Dejikstra(FindVertex(x1, y1), FindVertex(x2, y2));
        foreach (Vertex v in path)
        {
            //string line = "(" + v.x + ", " + v.y +")";
            //Debug.Log(line);
            if (path.IndexOf(v) != path.Count - 1)
            {
                Debug.DrawLine(new Vector3(v.x + 0.7f, v.y + 0.7f, 0), new Vector3(path[path.IndexOf(v) + 1].x + 0.7f, path[path.IndexOf(v) + 1].y + 0.7f, 0), Color.red, 1000);
            }
        }
    }
    #endregion
    
    public List<Vertex> Dejikstra(Vertex beginPnt, Vertex endPnt)
    {
        AnnulateVertices();
        Vertex beginPoint;
        beginPoint = beginPnt;
        beginPnt.mark = 0;
        Step(beginPoint);

        while (FindMin() != null && !isCompleted())
        {
            Vertex vertToCheck = FindMin();
            Step(vertToCheck);
        }        
        return GetPath(endPnt);
    }

    #region обслуживание алгоритма поиска
    void AnnulateVertices()
    {
        foreach (Vertex v in vertices)
        {
            v.isChecked = false;
            v.mark = 10000;
            v.prev = null;
        }
    }
    List<Vertex> GetPath(Vertex v)
    {
        if (v != null)
        {
            List<Vertex> path = new List<Vertex>();
            if (v.prev != null)
                path = GetPath(v.prev);
            path.Add(v);
            return path;
        }
        return null;
    }
    bool isCompleted()
    {
        foreach (Vertex v in vertices)
        {
            if (!v.isChecked)
                return false;
        }
        return true;
    }
    /// <summary>
    /// finding vertex with minimum mark
    /// </summary>
    /// <returns></returns>
    Vertex FindMin()
    {
        int min = 10000;
        Vertex minV = null;
        foreach (Vertex v in vertices)
        {
            if (v.mark < min && !v.isChecked)
            {
                min = v.mark;
                minV = v;
            }

        }
        return minV;
    }
    /// <summary>
    /// step of Dejikstra algorythm
    /// </summary>
    /// <param name="beginpoint"></param>
    void Step(Vertex beginpoint)
    {
        if (beginpoint != null)
        {
            foreach (Vertex nextp in Neigbors(beginpoint))
            {
                if (nextp.isChecked == false)//не отмечена
                {
                    int newmark = beginpoint.mark + GetMyEdge(nextp, beginpoint).weight;
                    if (nextp.mark > newmark)
                    {
                        nextp.mark = newmark;
                        nextp.prev = beginpoint;
                    }
                }
            }

            beginpoint.isChecked = true;//вычеркиваем
        }
    }
    List<Vertex> Neigbors(Vertex vert)
    {
        List<Vertex> neighbors = new List<Vertex>();
        foreach (Edge e in edges)
        {
            if (e.v1 == vert || e.v2 == vert)
            {
                if (e.v1 == vert)
                    neighbors.Add(e.v2);
                else
                    neighbors.Add(e.v1);
            }
        }
        return neighbors;
    }
    Edge GetMyEdge(Vertex vert1, Vertex vert2)
    {
        foreach (Edge e in edges)
        {
            if ((e.v1 == vert1 && e.v2 == vert2) || (e.v1 == vert2 && e.v2 == vert1))
            {
                return e;
            }
        }
        return null;

    }
    #endregion
}

