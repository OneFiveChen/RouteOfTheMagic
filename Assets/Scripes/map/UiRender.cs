using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RouteOfTheMagic
{
    public struct mapLine
    {
        public Vector2 x;
        public Vector2 y;
    }
    /// <summary>
    /// 绘制UI的类
    /// </summary>
    public class UiRender : MaskableGraphic
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        List<mapLine> line = new List<mapLine>();
        /// <summary>
        /// 增加绘制的线
        /// </summary>
        /// <param name="line">line结构体，线的两端，注意位置是像素位置</param>
        public void addLine(mapLine line)
        {
            this.line.Add(line);
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            foreach (var item in line)
            {
                vh.AddUIVertexQuad(GetQuad(item.x, item.y, Color.white));
            }

        }

        //通过两个端点绘制矩形
        private UIVertex[] GetQuad(Vector2 startPos, Vector2 endPos, Color color0, float lineWidth = 2.0f)
        {
            float dis = Vector2.Distance(startPos, endPos);
            float y = lineWidth * 0.5f * (endPos.x - startPos.x) / dis;
            float x = lineWidth * 0.5f * (endPos.y - startPos.y) / dis;
            if (y <= 0) y = -y;
            else x = -x;
            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = new Vector3(startPos.x + x, startPos.y + y);
            vertex[1].position = new Vector3(endPos.x + x, endPos.y + y);
            vertex[2].position = new Vector3(endPos.x - x, endPos.y - y);
            vertex[3].position = new Vector3(startPos.x - x, startPos.y - y);
            for (int i = 0; i < vertex.Length; i++) vertex[i].color = color0;
            return vertex;
        }

        //通过四个顶点绘制矩形
        private UIVertex[] GetQuad(Vector2 first, Vector2 second, Vector2 third, Vector2 four, Color color0)
        {
            UIVertex[] vertexs = new UIVertex[4];
            vertexs[0] = GetUIVertex(first, color0);
            vertexs[1] = GetUIVertex(second, color0);
            vertexs[2] = GetUIVertex(third, color0);
            vertexs[3] = GetUIVertex(four, color0);
            return vertexs;
        }

        //构造UIVertex
        private UIVertex GetUIVertex(Vector2 point, Color color0)
        {
            UIVertex vertex = new UIVertex
            {
                position = point,
                color = color0,
                uv0 = new Vector2(0, 0)
            };
            return vertex;
        }
    }
}
