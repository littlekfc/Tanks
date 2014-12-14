using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.IO;

namespace Tanks.FogOfWar
{
    class FogOfWarManager : Singleton<FogOfWarManager>
    {
        public int m_MapWidth = 100;
        public int m_MapHeight = 100;
        public int m_GridWidth = 1;
        public int m_GridHeight = 1;
        private int m_GridXCnt;
        private int m_GridYCnt;

        public bool[][] m_Blocks;

        public float m_DestroyGridLiftTime = 3;

        public List<KeyValuePair<KeyValuePair<int, int>, float>> m_DestroyGrid;

        private int[][] m_VisibleCount;

        private Dictionary<TObject, List<KeyValuePair<int, int>>> m_ObjectsEyesight;

        public MeshRenderer m_FogTexture;

        private int[][] m_WeightCnt;

        public float m_UpdataFogTextureInterval = 0.3f;

        private float m_CurUpdataFogTextureTime;

        FogOfWarManager()
        {
            m_GridXCnt = m_MapWidth / m_GridWidth;
            m_GridYCnt = m_MapHeight / m_GridHeight;
            m_Blocks = new bool[m_GridXCnt][];
            for (int i = 0; i < m_GridXCnt; i++)
                m_Blocks[i] = new bool[m_GridYCnt];
            m_VisibleCount = new int[m_GridXCnt][];
            for (int i = 0; i < m_GridXCnt; i++)
            {
                m_VisibleCount[i] = new int[m_GridXCnt];
                for (int j = 0; j < m_GridYCnt; j++)
                    m_VisibleCount[i][j] = 0;
            }
            m_ObjectsEyesight = new Dictionary<TObject, List<KeyValuePair<int, int>>>();
            m_WeightCnt = new int[m_GridYCnt + 1][];
            for (int i = 0; i < m_GridXCnt + 1; i ++)
            {
                m_WeightCnt[i] = new int[m_GridYCnt + 1];
                for (int j = 0; j < m_GridYCnt + 1; j ++)
                {
                    m_WeightCnt[i][j] = 0;
                }
            }
            m_DestroyGrid = new List<KeyValuePair<KeyValuePair<int, int>, float>>();
            m_CurUpdataFogTextureTime = 0;
        }

        /// <summary>
        /// ReCalculate which map's grid is block
        /// </summary>
        /// <param name="blockFile"></param>
        private void ReCalBlock(StreamWriter blockFile)
        {
            int layer = LayerMask.NameToLayer("Wall");
            for (uint i = 0; i < m_GridXCnt; i++)
            {
                for (uint j = 0; j < m_GridYCnt; j++)
                {
                    float x = m_GridWidth * i + m_GridWidth / 2 - m_MapWidth / 2;
                    float z = m_GridHeight * j + m_GridHeight / 2 - m_MapHeight / 2;
                    m_Blocks[i][j] = Physics.Raycast(new Vector3(x, 1000, z), new Vector3(0, -1, 0), 1000, 1 << layer);
                    blockFile.Write(m_Blocks[i][j] ? 1 : 0);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_FogTexture.material.mainTexture = new Texture2D(m_GridXCnt, m_GridYCnt, TextureFormat.Alpha8, false, true);
            Color[] col = new Color[m_GridXCnt * m_GridYCnt];
            for (int i = 0; i < m_GridXCnt; i++)
                for (int j = 0; j < m_GridYCnt; j++)
                    col[(m_GridYCnt - 1 - j) * m_GridXCnt + m_GridXCnt - 1 - i] = new Color(0, 0, 0, 1);

            FileInfo blockFileInfo = new FileInfo("./Assets/Resources/misc/MapBlockFile.txt");
            if (!blockFileInfo.Exists)
            {
                StreamWriter sw = blockFileInfo.CreateText();
                ReCalBlock(sw);
                sw.Close();
                sw.Dispose();
            }
            else
            {
                StreamReader sr = blockFileInfo.OpenText();
                int curByte = 0;
                int curPos = 0;
                while ((curByte = sr.Read()) != -1)
                    m_Blocks[curPos / m_GridXCnt][(curPos++) % m_GridYCnt] = (curByte == (int)'1');
            }
        }
        
        public KeyValuePair<int, int> PosToGrid(Vector3 position)
        {
            int x = (int)((position.x + (float)m_MapWidth / 2.0f) / m_GridWidth);
            int y = (int)((position.z + (float)m_MapHeight / 2.0f) / m_GridHeight);
            return new KeyValuePair<int, int>(x, y);
        }

        List<KeyValuePair<int, int>> CalObjectEyesight(TObject obj)
        {
            List<KeyValuePair<int, int>> ret = new List<KeyValuePair<int,int>>();
            for (int i = 0; i < 360; i ++ )
            {
                double dx = Math.Cos((double)i * 0.01745/* (pi / 180.0) */);
                double dy = Math.Sin((double)i * 0.01745);
                KeyValuePair<int, int> grid = PosToGrid(obj.transform.position);
                double x = (double)grid.Key + 0.5f;
                double y = (double)grid.Value + 0.5f;
                for (int j = 0; j < obj.eyesightRange; j ++)
                {
                    if ((int)x >= m_GridXCnt || (int)x < 0 ||
                         (int)y >= m_GridYCnt || (int)y < 0)
                        break;
                    ret.Add(new KeyValuePair<int,int>((int)x, (int)y));
                    if (m_Blocks[(int)x][(int)y])
                        break;
                    x += dx;
                    y += dy;
                }
            }
            return ret;
        }

        public void PushMoveObject(TObject obj)
        {
            if (m_ObjectsEyesight.ContainsKey(obj))
            {
                List<KeyValuePair<int, int>> rmGrids = m_ObjectsEyesight[obj];
                foreach (KeyValuePair<int, int> p in rmGrids)
                {
                    if (p.Key < m_GridXCnt && p.Value < m_GridYCnt)
                    {
                        m_VisibleCount[p.Key][p.Value]--;
                    }
                }
                m_ObjectsEyesight.Remove(obj);
            }
            List<KeyValuePair<int, int>> addGrids = CalObjectEyesight(obj);
            m_ObjectsEyesight[obj] = addGrids;
            foreach (KeyValuePair<int, int> p in addGrids)
            {
                if (p.Key < m_GridXCnt && p.Value < m_GridYCnt)
                {
                    m_VisibleCount[p.Key][p.Value]++;
                }
            }
        }

        public void PushDestroyObject(TObject obj)
        {
            if (m_ObjectsEyesight.ContainsKey(obj))
            {
                List<KeyValuePair<int, int>> rmGrids = m_ObjectsEyesight[obj];
                foreach (KeyValuePair<int, int> p in rmGrids)
                {
                    if (p.Key < m_GridXCnt && p.Value < m_GridYCnt)
                    {
                        m_VisibleCount[p.Key][p.Value]--;
                    }
                    m_DestroyGrid.Add(new KeyValuePair<KeyValuePair<int, int>, float>(p, m_DestroyGridLiftTime));
                }
                m_ObjectsEyesight.Remove(obj);
            }

        }

        private void Update()
        {
            m_CurUpdataFogTextureTime += Time.deltaTime;
            if (m_CurUpdataFogTextureTime > m_UpdataFogTextureInterval)
            {
                m_CurUpdataFogTextureTime = 0;
                UpdateFogTexture();
            }
            for (int i = 0; i < m_DestroyGrid.Count;)
            {
                if (m_DestroyGrid[i].Value - Time.deltaTime > 0)
                {
                    m_DestroyGrid[i] = new KeyValuePair<KeyValuePair<int, int>, float>(m_DestroyGrid[i].Key, m_DestroyGrid[i].Value - Time.deltaTime);
                    i++;
                }
                else
                    m_DestroyGrid.RemoveAt(i);
            }

        }

        private void UpdateFogTexture()
        {
            Texture2D fog = m_FogTexture.material.mainTexture as Texture2D;
            Color[] col = new Color[fog.height * fog.width];
            for (int i = 0; i < fog.width; i ++)
            {
                for (int j = 0; j < fog.height; j ++)
                {
                    if ((m_WeightCnt[i][j] & 4) == 0)
                        m_WeightCnt[i][j] += 4;
                    if ((m_WeightCnt[i + 1][j] & 8) == 0)
                        m_WeightCnt[i + 1][j] += 8;
                    if ((m_WeightCnt[i][j + 1] & 1) == 0)
                        m_WeightCnt[i][j + 1] += 1;
                    if ((m_WeightCnt[i + 1][j + 1] & 2) == 0)
                        m_WeightCnt[i + 1][j + 1] += 2;
                    col[(fog.height - 1 - j) * fog.width + fog.width - 1 - i] =
                        m_VisibleCount[i * m_GridXCnt / fog.width][j * m_GridYCnt / fog.height] > 0 ? new Color(0, 0, 0, 0) : new Color(0, 0, 0, 1);
                }
            }
            foreach (KeyValuePair<KeyValuePair<int, int>, float> p in m_DestroyGrid)
            {
                col[(fog.height - 1 - p.Key.Key) * fog.width + fog.width - 1 - p.Key.Value].a -= p.Value / m_DestroyGridLiftTime;
                if (col[(fog.height - 1 - p.Key.Key) * fog.width + fog.width - 1 - p.Key.Value].a < 0.0f)
                    col[(fog.height - 1 - p.Key.Key) * fog.width + fog.width - 1 - p.Key.Value].a = 0.0f;
            }
            for (int i = 0; i < fog.width; i ++)
            {
                for (int j = 0; j < fog.height; j ++)
                {
                    float sum = 0;
                    for (int k = -5; k <= 5; k ++)
                    {
                        if (i + k >= 0 && i + k < fog.width)
                            sum += col[(fog.height - 1 - j) * fog.width + fog.width - 1 - (i + k)].a;
                        else
                            sum += 1;
                    }
                    col[i].a = sum / 11.0f;
                }
            }
            for (int i = 0; i < fog.width; i++)
            {
                for (int j = 0; j < fog.height; j++)
                {
                    float sum = 0;
                    for (int k = -5; k <= 5; k++)
                    {
                        if (j + k >= 0 && j + k < fog.height)
                            sum += col[(fog.height - 1 - (j + k)) * fog.width + fog.width - 1 - i].a;
                        else
                            sum += 1;
                    }
                    col[i].a = sum / 11.0f;
                }
            }
            fog.SetPixels(col);
            fog.Apply();
        }
    }
}
