using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Manager : MonoBehaviour
{
    public int m_n_Length_x;
    public int m_n_Length_z;

    private static Manager s_Manager = null;
    public static Manager g_Manager
    {
        get
        {
            if (s_Manager == null)
                return null;

            return s_Manager;
        }
    }

    // 원본
    public GameObject[,] m_ga_Cube;

    // 원본 1/2
    public Vector3[,] m_va_Cube_1d2;
    int[,] narytemp;

    public GameObject m_gCube;
    public Material[] m_gm_Depth;

    public GameObject m_GMesh;

    GameObject gameobject;

    private void Awake()
    {
        if (s_Manager == null)
        {
            s_Manager = this;
            //DontDestroyOnLoad(this.gameobject);
        }
        else
        {
            Destroy(this.gameobject);
        }

        InitialSet();
        Random.InitState(2024);
    }

    private void Start()
    {
        InitialSet_Map_Coordinate();
        InitialSet_Map_Coordinate_Compress();
        InitialSet_Map_Mesh();
        InitialSet_Map_Coordinate_Hide();
    }

    void Update()
    {
        // 맵 BSP 배열
        if (Input.GetKeyUp(KeyCode.F1))
        {
            for (int x = 0; x < m_n_Length_x; x++)
            {
                for (int z = 0; z < m_n_Length_z; z++)
                {
                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, 0, m_ga_Cube[x, z].gameObject.transform.position.z);
                    //m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material = m_gm_Depth[0];
                    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
                }
            }

            //BSP();
            //BSP(10, 40, 10, 40);
            //BSP(0, 50, 0, 50, 5);
            //BSP(0, 50, 0, 50, 1, 4, true);

            // 합 47
            //BSP_X(0, 200, 0, 15);
            //BSP_Z(0, 50, 0, 15);
            //BSP_X(0, 200, 0, 5);
            //BSP_X(0, 200, 0, 5);
            //BSP_Z(0, 50, 0, 7);

            //BSP_X(0, 75, 0, 10);
            //BSP_X(125, 200, 0, 10);
            //BSP_Z(0, 15, 0, 5);
            //BSP_Z(35, 50, 0, 5);

            //BSP_X(0, 100, 0, 10);
            //BSP_X(30, 100, 0, 10);
            //BSP_X(60, 100, 0, 10);
            //BSP_Z(0, 50, 0, 5);
            //BSP_Z(10, 40, 0, 5);
            //BSP_Z(20, 30, 0, 5);

            // 1
            //BSP_X(0, 100, 0, 8);
            //BSP_X(100, 200, 0, 8);
            //BSP_Z(0, 20, 0, 6);
            //BSP_Z(40, 50, 0, 6);
            //BSP_Z(20, 40, 0, 6);

            //BSP_X(0, 20, 0, 4);
            //BSP_X(0, 20, 0, 4);
            //BSP_X(0, 20, 0, 4);
            //BSP_Z(0, 10, 0, 3);
            //BSP_Z(0, 10, 0, 3);
            //BSP_Z(0, 10, 0, 3);

            //BSP_X(180, 200, 0, 4);
            //BSP_X(180, 200, 0, 4);
            //BSP_X(180, 200, 0, 4);
            //BSP_Z(40, 50, 0, 3);
            //BSP_Z(40, 50, 0, 3);
            //BSP_Z(40, 50, 0, 3);

            // 2
            //BSP_X(0, 50, 0, 8);
            //BSP_X(50, 200, 0, 8);
            //BSP_Z(0, 20, 0, 6);
            //BSP_Z(40, 50, 0, 6);
            //BSP_Z(20, 40, 0, 6);

            //BSP_X(0, 20, 0, 4);
            //BSP_X(0, 20, 0, 4);
            //BSP_X(0, 20, 0, 4);
            //BSP_X(0, 20, 0, 4);

            //BSP_X(100, 140, 0, 4);
            //BSP_X(100, 140, 0, 4);
            //BSP_X(100, 140, 0, 4);
            //BSP_X(100, 140, 0, 4);

            // 3
            //BSP_X(0, 20, 0, 5);
            //BSP_X(0, 10, 0, 3);
            //BSP_Z(0, 20, 0, 5);
            //BSP_Z(0, 10, 0, 3); 

            BSP_X(0, m_n_Length_x, 0, 5);
            //BSP_X(0, m_n_Length_x, 0, 3);
            BSP_Z(0, m_n_Length_z, 0, 5);
            //BSP_Z(0, m_n_Length_z, 0, 3);
        }
        // 강화 필터
        if (Input.GetKeyUp(KeyCode.F2))
        {
            Smooth_1(0, m_n_Length_x, 0, m_n_Length_z);
        }
        // 약화 필터
        if (Input.GetKeyUp(KeyCode.F3))
        {
            Smooth_2(0, m_n_Length_x, 0, m_n_Length_z);
        }
        // 맵 재생성(크기 초기화)
        if (Input.GetKeyUp(KeyCode.F5))
        {
            InitialSet();
        }
        // 활성화 / 비활성화
        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            for (int x = 0; x < m_n_Length_x; x++)
            {
                for (int z = 0; z < m_n_Length_z; z++)
                {
                    m_ga_Cube[x, z].gameObject.SetActive(!m_ga_Cube[x, z].gameObject.activeSelf);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Insert))
        {
            StreamWriter SW = new StreamWriter("./Assets/Map/MapInfo.csv");
            string str = string.Empty;

            str = m_n_Length_x + "," + m_n_Length_z + ",";
            SW.WriteLine(str);

            for (int x = 0; x < m_n_Length_x; x++)
            {
                for (int z = 0; z < m_n_Length_z; z++)
                {
                    str = string.Empty;

                    //str = x + ", " + z + ", " + m_ga_Cube[x, z].gameObject.transform.position.x + ", " + m_ga_Cube[x, z].gameObject.transform.position.y + ", " + m_ga_Cube[x, z].gameObject.transform.position.z + "\n";
                    str = m_ga_Cube[x, z].gameObject.transform.position.y.ToString() + ",";
                    if (z == m_n_Length_z - 1)
                        str += "\n";
                    SW.Write(str);
                }
            }
            SW.Close();
        }
        // 맵 데이터 로딩
        if (Input.GetKeyUp(KeyCode.Home))
        {
            StreamReader SR = new StreamReader("./Assets/Map/MapInfo.csv");
            string str = string.Empty;

            str = SR.ReadLine();
            var vary1 = str.Split(",");

            m_n_Length_x = int.Parse(vary1[0]);
            m_n_Length_z = int.Parse(vary1[1]);

            m_ga_Cube = new GameObject[m_n_Length_x, m_n_Length_z];
            InitialSet();

            int x = 0, z = 0;

            while (SR.EndOfStream == false)
            {
                str = SR.ReadLine();
                var vary2 = str.Split(','); // Debug.Log(vary2.Length);

                if (x == m_n_Length_x)
                    break;

                for (int i = 0; i < vary2.Length - 1; i++)
                {
                    //if (z < m_n_Length_z - 2)
                    {
                        int az = int.Parse(vary2[i]); // Debug.Log("[" + x + ", " + z + "]");
                        m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, az, m_ga_Cube[x, z].gameObject.transform.position.z);
                        float num = (int)az * 5 > 255 ? 255 : az * 5;
                        num = num < 0 ? 0 : num;
                        m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                        z++;
                    }
                }
                x++; z = 0;
            }

            SR.Close();
        }
        // 매쉬 생성 - 원본
        if (Input.GetKeyDown(KeyCode.End))
        {
            for (int x = 0; x < m_n_Length_x; x++)
            {
                for (int z = 0; z < m_n_Length_z; z++)
                {
                    if (x < m_n_Length_x - 1 && z < m_n_Length_z - 1)
                    {
                        gameobject = GameObject.Instantiate(m_GMesh);
                        gameobject.GetComponent<Create_Mesh_1>().Create_Mesh_1_1(0, new Vector3[3] { m_ga_Cube[x, z].gameObject.transform.position, m_ga_Cube[x, z + 1].transform.position, m_ga_Cube[x + 1, z + 1].transform.position });
                        gameobject = GameObject.Instantiate(m_GMesh);
                        gameobject.GetComponent<Create_Mesh_1>().Create_Mesh_1_1(1, new Vector3[3] { m_ga_Cube[x, z].gameObject.transform.position, m_ga_Cube[x + 1, z + 1].transform.position, m_ga_Cube[x + 1, z].transform.position });
                    }
                }
            }
        }
        // 필터 - 1/2 x, z 변화 없음
        // ■□■□
        // □□□□
        // ■□■□
        // □□□□
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            narytemp = new int[m_n_Length_x / 2, m_n_Length_z / 2];
            m_va_Cube_1d2 = new Vector3[m_n_Length_x / 2, m_n_Length_z / 2];

            int narytemp_x = 0, narytemp_z = 0;
            for (int x = 0; x < m_n_Length_x; x++)
            {
                for (int z = 0; z < m_n_Length_z; z++)
                {
                    if (x % 2 == 0 && z % 2 == 0)
                    if (narytemp_x < m_n_Length_x / 2 && narytemp_z < m_n_Length_z / 2)
                    {
                        narytemp[narytemp_x, narytemp_z] = (int)m_ga_Cube[x, z].gameObject.transform.position.y;
                        m_va_Cube_1d2[narytemp_x, narytemp_z] = new Vector3(m_ga_Cube[x, z].transform.position.x, m_ga_Cube[x, z].transform.position.y, m_ga_Cube[x, z].transform.position.z);
                        narytemp_z++;
                    }
                }
                if (x % 2 == 0)
                    narytemp_x++;
                narytemp_z = 0;
            }

            StreamWriter SW = new StreamWriter("./Assets/Map/MapInfo - 1d2.csv");
            string str = string.Empty;

            str = m_n_Length_x / 2 + "," + m_n_Length_z / 2 + ",";
            SW.WriteLine(str);
            for (int x = 0; x < m_n_Length_x / 2; x++)
            {
                for (int z = 0; z < m_n_Length_z / 2; z++)
                {
                    str += narytemp[x, z] + ",";
                }
                SW.WriteLine(str);
                str = string.Empty;
            }
            SW.Close();

        }
        // 필터 - 1/2 x, z 압축(평균)
        // ■■
        // ■■
        // 매쉬 생성 - 1/2
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            for (int x = 0; x < m_n_Length_x / 2; x++)
            {
                for (int z = 0; z < m_n_Length_z / 2; z++)
                {
                    if (x < (m_n_Length_x / 2) - 1 && z < (m_n_Length_z / 2) - 1)
                    {
                        gameobject = GameObject.Instantiate(m_GMesh);
                        gameobject.GetComponent<Create_Mesh_1>().Create_Mesh_1_1(0, new Vector3[3] { m_va_Cube_1d2[x, z], m_va_Cube_1d2[x, z + 1], m_va_Cube_1d2[x + 1, z + 1] });
                        gameobject = GameObject.Instantiate(m_GMesh);
                        gameobject.GetComponent<Create_Mesh_1>().Create_Mesh_1_1(1, new Vector3[3] { m_va_Cube_1d2[x, z], m_va_Cube_1d2[x + 1, z + 1], m_va_Cube_1d2[x + 1, z] });
                    }
                }
            }
        }
    }

    void InitialSet()
    {
        m_ga_Cube = new GameObject[m_n_Length_x, m_n_Length_z];

        float m_fMap_X_Offset = 0;
        float m_fMap_Y_Offset = 0;
        float m_fMap_Z_Offset = 0;

        for (int i = 0; i < GameObject.Find("Map Mesh GameObject Set").transform.childCount; i++)
        {
            Destroy(GameObject.Find("Map Mesh GameObject Set").transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameObject.Find("Coordinate GameObject Set").transform.childCount; i++)
        {
            Destroy(GameObject.Find("Coordinate GameObject Set").transform.GetChild(i).gameObject);
        }

        for (int x = 0; x < m_n_Length_x; x++)
        {
            m_fMap_Z_Offset = 0;
            for (int z = 0; z < m_n_Length_z; z++)
            {
                gameobject = GameObject.Instantiate(m_gCube);
                gameobject.name = "Cube[" + x + ", 0, " + z + "]";
                gameobject.transform.position = new Vector3(x + m_fMap_X_Offset, 0, z + (m_fMap_Z_Offset += 0.1f));
                gameobject.transform.SetParent(GameObject.Find("Coordinate GameObject Set").transform);
                m_ga_Cube[x, z] = gameobject;
            }
            m_fMap_X_Offset += 0.1f;
        }
    }

    void InitialSet_Map_Coordinate()
    {
        StreamReader SR = new StreamReader("./Assets/Map/MapInfo.csv");
        string str = string.Empty;

        str = SR.ReadLine();
        var vary1 = str.Split(",");

        m_n_Length_x = int.Parse(vary1[0]);
        m_n_Length_z = int.Parse(vary1[1]);

        m_ga_Cube = new GameObject[m_n_Length_x, m_n_Length_z];
        InitialSet();

        int x = 0, z = 0;

        while (SR.EndOfStream == false)
        {
            str = SR.ReadLine();
            var vary2 = str.Split(','); // Debug.Log(vary2.Length);

            if (x == m_n_Length_x)
                break;

            for (int i = 0; i < vary2.Length - 1; i++)
            {
                //if (z < m_n_Length_z - 2)
                {
                    int az = int.Parse(vary2[i]); // Debug.Log("[" + x + ", " + z + "]");
                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, az, m_ga_Cube[x, z].gameObject.transform.position.z);
                    float num = (int)az * 5 > 255 ? 255 : az * 5;
                    num = num < 0 ? 0 : num;
                    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                    z++;
                }
            }
            x++; z = 0;
        }

        SR.Close();
    }
    void InitialSet_Map_Coordinate_Compress()
    {
        narytemp = new int[m_n_Length_x / 2, m_n_Length_z / 2];
        m_va_Cube_1d2 = new Vector3[m_n_Length_x / 2, m_n_Length_z / 2];

        int narytemp_x = 0, narytemp_z = 0;
        for (int x = 0; x < m_n_Length_x; x++)
        {
            for (int z = 0; z < m_n_Length_z; z++)
            {
                if (x % 2 == 0 && z % 2 == 0)
                    if (narytemp_x < m_n_Length_x / 2 && narytemp_z < m_n_Length_z / 2)
                    {
                        narytemp[narytemp_x, narytemp_z] = (int)m_ga_Cube[x, z].gameObject.transform.position.y;
                        m_va_Cube_1d2[narytemp_x, narytemp_z] = new Vector3(m_ga_Cube[x, z].transform.position.x, m_ga_Cube[x, z].transform.position.y, m_ga_Cube[x, z].transform.position.z);
                        narytemp_z++;
                    }
            }
            if (x % 2 == 0)
                narytemp_x++;
            narytemp_z = 0;
        }

        StreamWriter SW = new StreamWriter("./Assets/Map/MapInfo - 1d2.csv");
        string str = string.Empty;

        str = m_n_Length_x / 2 + "," + m_n_Length_z / 2 + ",";
        SW.WriteLine(str);
        for (int x = 0; x < m_n_Length_x / 2; x++)
        {
            for (int z = 0; z < m_n_Length_z / 2; z++)
            {
                str += narytemp[x, z] + ",";
            }
            SW.WriteLine(str);
            str = string.Empty;
        }
        SW.Close();
    }
    void InitialSet_Map_Mesh()
    {

        for (int x = 0; x < m_n_Length_x / 2; x++)
        {
            for (int z = 0; z < m_n_Length_z / 2; z++)
            {
                if (x < (m_n_Length_x / 2) - 1 && z < (m_n_Length_z / 2) - 1)
                {
                    gameobject = GameObject.Instantiate(m_GMesh);
                    gameobject.GetComponent<Create_Mesh_1>().Create_Mesh_1_1(0, new Vector3[3] { m_va_Cube_1d2[x, z], m_va_Cube_1d2[x, z + 1], m_va_Cube_1d2[x + 1, z + 1] });
                    gameobject = GameObject.Instantiate(m_GMesh);
                    gameobject.GetComponent<Create_Mesh_1>().Create_Mesh_1_1(1, new Vector3[3] { m_va_Cube_1d2[x, z], m_va_Cube_1d2[x + 1, z + 1], m_va_Cube_1d2[x + 1, z] });
                }
            }
        }
    }
    void InitialSet_Map_Coordinate_Hide()
    {
        for (int x = 0; x < m_n_Length_x; x++)
        {
            for (int z = 0; z < m_n_Length_z; z++)
            {
                m_ga_Cube[x, z].gameObject.SetActive(!m_ga_Cube[x, z].gameObject.activeSelf);
            }
        }
    }

    void BSP()
    {
        int nRandomNum_X = Random.Range(1, 49);
        int nRandomNum_Z = Random.Range(1, 49);

        float m_fMap_X_Offset = 0;
        float m_fMap_Y_Offset = 0;
        float m_fMap_Z_Offset = 0;

        for (int x = 0; x < nRandomNum_X; x++)
        {
            m_fMap_Z_Offset = 0;
            for (int z = 0; z < 50; z++)
            {
                m_ga_Cube[x, z].gameObject.transform.position = new Vector3(x + m_fMap_X_Offset, 1, z + (m_fMap_Z_Offset += 0.1f));
            }
            m_fMap_X_Offset += 0.1f;
        }

        for (int x = nRandomNum_X + 1; x < 50; x++)
        {
            m_fMap_Z_Offset = 0;
            for (int z = 0; z < 50; z++)
            {
                m_ga_Cube[x, z].gameObject.transform.position = new Vector3(x + m_fMap_X_Offset, 1, z + (m_fMap_Z_Offset += 0.1f));
            }
            m_fMap_X_Offset += 0.1f;
        }
    }

    void BSP(int limit_min_x, int limit_max_x, int limit_min_z, int limit_max_z)
    {
        int nRandomNum_X = Random.Range(limit_min_x + 1, limit_max_x - 1);
        int nRandomNum_Z = Random.Range(limit_min_z + 1, limit_max_z - 1);

        float m_fMap_X_Offset = 0;
        float m_fMap_Y_Offset = 0;
        float m_fMap_Z_Offset = 0;

        for (int x = limit_min_x; x < nRandomNum_X; x++)
        {
            m_fMap_Z_Offset = 0;
            for (int z = limit_min_z; z < limit_max_z; z++)
            //for (int z = 0; z < 50; z++)
            {
                m_ga_Cube[x, z].gameObject.transform.position = new Vector3(x + m_fMap_X_Offset + 1, 1, z + (m_fMap_Z_Offset += 0.1f) + 1);
            }
            m_fMap_X_Offset += 0.1f;
        }

        for (int x = nRandomNum_X + 1; x < limit_max_x; x++)
        {
            m_fMap_Z_Offset = 0;
            for (int z = limit_min_z; z < limit_max_z; z++)
            //for (int z = 0; z < 50; z++)
            {
                m_ga_Cube[x, z].gameObject.transform.position = new Vector3(x + m_fMap_X_Offset + 1, 1, z + (m_fMap_Z_Offset += 0.1f) + 1);
            }
            m_fMap_X_Offset += 0.1f;
        }
    }

    // return 0 : end / return ~0 : process
    int BSP(int limit_min_x, int limit_max_x, int limit_min_z, int limit_max_z, int depth, int startingheight = 1)
    {
        if (depth == 0)
            return 0;
        else
        {
            int nRandomNum_X = Random.Range(limit_min_x + 1, limit_max_x - 1);
            int nRandomNum_Z = Random.Range(limit_min_z + 1, limit_max_z - 1);

            float m_fMap_X_Offset = 0;
            float m_fMap_Y_Offset = 0;
            float m_fMap_Z_Offset = 0;

            for (int x = limit_min_x; x < nRandomNum_X; x++)
            {
                m_fMap_Z_Offset = 0;
                //for (int z = limit_min_z; z < limit_max_z; z++)
                for (int z = 0; z < 50; z++)
                {
                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(x + m_fMap_X_Offset, startingheight, z + (m_fMap_Z_Offset += 0.1f));
                }
                m_fMap_X_Offset += 0.1f;
            }
            if (limit_max_x - nRandomNum_X <= 3) { }
            else
            {
                int nRandomNum_Min_X = Random.Range(limit_min_x + 1, nRandomNum_X - 1);
                if (nRandomNum_Min_X > 1)
                    BSP(limit_min_x, nRandomNum_Min_X, limit_min_z, limit_max_z, depth - 1, startingheight + 1);
                else { }
            }

            for (int x = nRandomNum_X + 1; x < limit_max_x; x++)
            {
                m_fMap_Z_Offset = 0;
                //for (int z = limit_min_z; z < limit_max_z; z++)
                for (int z = 0; z < 50; z++)
                {
                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(x + m_fMap_X_Offset, startingheight, z + (m_fMap_Z_Offset += 0.1f));
                }
                m_fMap_X_Offset += 0.1f;
            }
            if (nRandomNum_X - limit_max_x <= 46) { }
            else
            {
                int nRandomNum_Min_X = Random.Range(nRandomNum_X + 2, limit_max_x - 1);
                BSP(nRandomNum_X + 2, limit_max_x - 1, limit_min_z, limit_max_z, depth - 1, startingheight + 1);
            }

            return 1;
        }
    }

    void BSP(int limit_min_x, int limit_max_x, int limit_min_z, int limit_max_z, int depth_start, int depth, bool hv)
    {
        if (depth == 0)
            return;
        // x 2분할
        if (hv == true)
        {
            int nRandomNum_X = Random.Range(limit_min_x + 1, limit_max_x - 1);

            for (int x = limit_min_x; x < nRandomNum_X; x++)
            {
                //for (int z = limit_min_z; z < limit_max_z; z++)
                //for (int z = limit_min_z; z < limit_max_z; z++)
                for (int z = 0; z < 50; z++)
                {
                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y + 1, m_ga_Cube[x, z].gameObject.transform.position.z);
                    //Debug.Log("Cube[" + x + "," + z + "] : " + depth_start);
                    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material = m_gm_Depth[depth_start];
                }
            }
            for (int x = nRandomNum_X; x < limit_max_x; x ++)
            {
                for (int z = 0; z < 50; z++)
                {
                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y + 1, m_ga_Cube[x, z].gameObject.transform.position.z);
                    //Debug.Log("Cube[" + x + "," + z + "] : " + depth_start);
                    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material = m_gm_Depth[depth_start];
                }
            }

            if (nRandomNum_X - limit_min_x > limit_max_z - limit_min_z)
                BSP(limit_min_x, nRandomNum_X, limit_min_z, limit_max_z, depth_start + 1, depth - 1, true);
            //else
            //  BSP(limit_min_x, nRandomNum_X, limit_min_z, limit_max_z, depth_start + 1, depth, false);
            else
                BSP(limit_min_x, nRandomNum_X, limit_min_z, limit_max_z, depth_start + 1, depth - 1, true);

            if (limit_max_x - nRandomNum_X > limit_max_z - limit_min_z)
                BSP(nRandomNum_X, limit_max_x, limit_min_z, limit_max_z, depth_start + 1, depth - 1, true);
            //else
            //    BSP(nRandomNum_X, limit_max_x, limit_min_z, limit_max_z, depth_start + 1, depth, false);
            else
                BSP(nRandomNum_X, limit_max_x, limit_min_z, limit_max_z, depth_start + 1, depth - 1, true);
        }
        // y 2분할
        //else
        //{
        //    int nRandomNum_Z = Random.Range(limit_min_z + 1, limit_max_z - 1);

        //    for (int z = limit_min_z; z < nRandomNum_Z; z++)
        //    {
        //        for (int x = limit_min_x; z < limit_max_x; x++)
        //        {
        //            m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y + 1, m_ga_Cube[x, z].gameObject.transform.position.z);
        //            m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material = m_gm_Depth[depth_start];
        //        }
        //    }

        //    if (nRandomNum_Z - limit_min_z > limit_max_x - limit_min_x)
        //        BSP(limit_min_z, nRandomNum_Z, limit_min_x, limit_max_x, depth_start + 1, depth, false);
        //    else
        //        BSP(limit_min_z, nRandomNum_Z, limit_min_x, limit_max_x, depth_start + 1, depth, true);

        //    if (limit_max_z - nRandomNum_Z > limit_max_x - limit_min_x)
        //        BSP(nRandomNum_Z, limit_max_z, limit_min_x, limit_max_x, depth_start + 1, depth, false);
        //    else
        //        BSP(nRandomNum_Z, limit_max_z, limit_min_x, limit_max_x, depth_start + 1, depth, true);
        //}
    }

    void BSP_X(int limit_min_x, int limit_max_x, int depth_start, int depth)
    {
        if (depth > 0 && limit_min_x < limit_max_x)
        {
            int nRandom_X = Random.Range(limit_min_x + 1, limit_max_x - 1); // Debug.Log(nRandom_X);
            if (limit_min_x < nRandom_X)
            {
                for (int x = limit_min_x; x < nRandom_X; x++)
                {
                    for (int z = 0; z < m_n_Length_z; z++)
                    {
                        //if (Random.Range(0, 100) > 80)
                        {
                            m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y + 1, m_ga_Cube[x, z].gameObject.transform.position.z);
                            float num = (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5 > 255 ? 255 : (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5;
                            num = num < 0 ? 0 : num;
                            m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                        }
                        //else
                        //{
                        //    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y - 1, m_ga_Cube[x, z].gameObject.transform.position.z);
                        //    float num = (int)m_ga_Cube[x, z].gameObject.transform.position.y;
                        //    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                        //}
                    }
                }
                BSP_X(limit_min_x + 1, nRandom_X - 1, depth_start + 1, depth - 1);

                for (int x = nRandom_X; x < limit_max_x; x++)
                {
                    for (int z = 0; z < m_n_Length_z; z++)
                    {
                        //if (Random.Range(0, 100) > 80)
                        {
                            m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y + 2, m_ga_Cube[x, z].gameObject.transform.position.z);
                            float num = (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5 > 255 ? 255 : (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5;
                            num = num < 0 ? 0 : num;
                            m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                        }
                        //else
                        //{
                        //    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y - 1, m_ga_Cube[x, z].gameObject.transform.position.z);
                        //    float num = (int)m_ga_Cube[x, z].gameObject.transform.position.y;
                        //    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                        //}
                    }
                }
                BSP_X(nRandom_X, limit_max_x - 1, depth_start + 1, depth - 1);
            }
        }
    }
    void BSP_Z(int limit_min_z, int limit_max_z, int depth_start, int depth)
    {
        if (depth > 0 && limit_min_z < limit_max_z)
        {
            Debug.Log(limit_min_z + " / " + limit_max_z);
            int nRandom_Z = Random.Range(limit_min_z + 1, limit_max_z - 1);
            if (limit_min_z < nRandom_Z && nRandom_Z > 0)
            {
                for (int z = limit_min_z; z < nRandom_Z; z++)
                {
                    for (int x = 0; x < m_n_Length_x; x++)
                    {
                        m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y + 1, m_ga_Cube[x, z].gameObject.transform.position.z);
                        float num = (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5 > 255 ? 255 : (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5;
                        num = num < 0 ? 0 : num;
                        m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                    }
                }
                BSP_Z(limit_min_z + 1, nRandom_Z - 1, depth_start + 1, depth - 1);

                for (int z = nRandom_Z; z < limit_max_z; z++)
                {
                    for (int x = 0; x < m_n_Length_x; x++)
                    {
                        m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, m_ga_Cube[x, z].gameObject.transform.position.y + 2, m_ga_Cube[x, z].gameObject.transform.position.z);
                        float num = (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5 > 255 ? 255 : (int)m_ga_Cube[x, z].gameObject.transform.position.y * 5;
                        num = num < 0 ? 0 : num;
                        m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                    }
                }
                BSP_Z(nRandom_Z, limit_max_z - 1, depth_start + 1, depth - 1);
            }
        }
    }

    // 증가형
    void Smooth_1(int min_x, int max_x, int min_z, int max_z)
    {
        int numc1 = 0;
        for (int x = min_x; x < max_x; x++)
        {
            for (int z = min_z; z < max_z; z++)
            {
                if (x > min_x && x < max_x - 1 && z > min_z && z < max_z - 1)
                {
                    numc1 += (int)m_ga_Cube[x - 1, z - 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x, z - 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x + 1, z - 1].gameObject.transform.position.y;

                    numc1 += (int)m_ga_Cube[x - 1, z].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x, z].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x + 1, z].gameObject.transform.position.y;

                    numc1 += (int)m_ga_Cube[x - 1, z + 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x, z + 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x + 1, z + 1].gameObject.transform.position.y;

                    numc1 /= 9;

                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, numc1, m_ga_Cube[x, z].gameObject.transform.position.z);
                    float num = numc1 * 5 > 255 ? 255 : numc1 * 5;
                    num = num < 0 ? 0 : num;
                    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                }
            }
        }
    }

    // 감소형
    void Smooth_2(int min_x, int max_x, int min_z, int max_z)
    {
        int numc1 = 0;
        int numc2 = 0;
        for (int x = min_x; x < max_x; x++)
        {
            for (int z = min_z; z < max_z; z++)
            {
                if (x > min_x && x < max_x - 1 && z > min_z && z < max_z - 1)
                {
                    numc1 += (int)m_ga_Cube[x - 1, z - 1].gameObject.transform.position.y; numc2 = (int)m_ga_Cube[x - 1, z - 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x, z - 1].gameObject.transform.position.y;     numc2 = (int)m_ga_Cube[x, z - 1].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x, z - 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x + 1, z - 1].gameObject.transform.position.y; numc2 = (int)m_ga_Cube[x + 1, z - 1].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x + 1, z - 1].gameObject.transform.position.y;

                    numc1 += (int)m_ga_Cube[x - 1, z].gameObject.transform.position.y;     numc2 = (int)m_ga_Cube[x - 1, z].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x - 1, z].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x, z].gameObject.transform.position.y;         numc2 = (int)m_ga_Cube[x, z].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x, z].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x + 1, z].gameObject.transform.position.y;     numc2 = (int)m_ga_Cube[x + 1, z].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x + 1, z].gameObject.transform.position.y;

                    numc1 += (int)m_ga_Cube[x - 1, z + 1].gameObject.transform.position.y; numc2 = (int)m_ga_Cube[x - 1, z + 1].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x - 1, z + 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x, z + 1].gameObject.transform.position.y;     numc2 = (int)m_ga_Cube[x, z + 1].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x, z + 1].gameObject.transform.position.y;
                    numc1 += (int)m_ga_Cube[x + 1, z + 1].gameObject.transform.position.y; numc2 = (int)m_ga_Cube[x + 1, z + 1].gameObject.transform.position.y > numc2 ? numc2 : (int)m_ga_Cube[x + 1, z + 1].gameObject.transform.position.y;

                    numc1 /= 9;
                    numc1 = (numc1 + numc2) / 2;

                    m_ga_Cube[x, z].gameObject.transform.position = new Vector3(m_ga_Cube[x, z].gameObject.transform.position.x, numc1, m_ga_Cube[x, z].gameObject.transform.position.z);
                    float num = numc1 * 5 > 255 ? 255 : numc1 * 5;
                    num = num < 0 ? 0 : num;
                    m_ga_Cube[x, z].gameObject.GetComponent<MeshRenderer>().material.color = new Color(num / 255, num / 255, num / 255);
                }
            }
        }
    }
}
