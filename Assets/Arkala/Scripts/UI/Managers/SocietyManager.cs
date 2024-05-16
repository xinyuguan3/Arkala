using ClickNext.Scripts.UI.Managers.Interfaces;
using ClickNext.Scripts.UI.Managers.Templates;
using ClickNext.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ClickNext.Scripts.UI.Managers
{
    //管理所有的Meme
    public class SocietyManager : IManager
    {
        [SerializeField] Color[] colors;
        [SerializeField] GameObject NodePrefab;
        [SerializeField] GameObject LinePrefab;
        [SerializeField] Text ShowText;
        [SerializeField] Dictionary<Node, ImageNode> nodes = new Dictionary<Node, ImageNode>();
        [SerializeField] List<UILineRenderer> lines = new List<UILineRenderer>();
        [SerializeField] Transform Canvas;
        [SerializeField] ImageNode CurrentSelNode;
        bool isDrawLine = false;
        int num=0;
        private new void Awake()
        {
            base.Awake();
        }

        void Start(){
            gameObject.SetActive(false);
            InitNodes();
        }
        void Update(){
            UpdateLine();
            HighColorNodes();
            HighColorLines();
        }

        void InitNodes(){
            for(int i=0;i<5;i++){
                ImageNode node = Instantiate(NodePrefab, Canvas.Find("Nodes"), false).GetComponent<ImageNode>();
                // 每个Node上都有Button，创建时附加监听器，在选择Node时可以与其他Node建立连接
                node.Init($"Node{num + 1}", () => NodeSelectEvent(node));
                node.gameObject.name = $"Node{num + 1}";
                nodes.Add(node.ImgNode, node);
                num++;
            }
        }

        private void NodeSelectEvent(ImageNode imageNode)
        {
            if (imageNode == CurrentSelNode)
                return;
            if (isDrawLine&&CurrentSelNode!=null)
            {
                AddALine(imageNode.ImgNode, CurrentSelNode.ImgNode);
            }
            if (CurrentSelNode != null)
            {
                CurrentSelNode.GetComponent<Image>().color = Color.white;
                SetNormalNodes();
            }
            imageNode.GetComponent<Image>().color = Color.green;
            CurrentSelNode = imageNode;
            ShowText.text = imageNode.ImgNode.Name;
        }

        private void SetNormalNodes()
        {
            if (CurrentSelNode == null)
                return;
            foreach (var item in CurrentSelNode.ImgNode.Children)
            {
                nodes[item].GetComponent<Image>().color = Color.white;
            }
        }

        private void AddALine(Node node1, Node node2)
        {
            if (isHaveLine(node1, node2))
                return;
            Line line = new Line()
            {
                Node_1 = node1,
                Node_2 = node2,
            };
            UILineRenderer uILineRenderer = Instantiate(LinePrefab, Canvas.Find("Lines"), false).GetComponent<UILineRenderer>();
            uILineRenderer.line = line;
            uILineRenderer.gameObject.name = $"Node({node1.Name} with {node2.Name})";
            lines.Add(uILineRenderer);
            nodes[node1].AddNodeIntoChildren(node2);
            nodes[node2].AddNodeIntoChildren(node1);
        }

        //将控制的面板设置为相反的可见状态
        public override void OpenDialog(bool value)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            // base.OpenDialog(value);
        }

        private void UpdateLine()
        {
            if (lines is null || lines.Count == 0)
                return;
            Line line;
            foreach (var i in lines)
            {
                line = i.line;
                i.SetPositions(nodes[line.Node_1].transform.localPosition, nodes[line.Node_2].transform.localPosition);
            }
        }

        private void HighColorNodes()
    {

        if (CurrentSelNode == null)
        {
            foreach (var item in nodes)
            {
                item.Value.GetComponent<Image>().color = Color.white;
            }
            return;
        }
        foreach (var item in CurrentSelNode.ImgNode.Children)
        {
            nodes[item].GetComponent<Image>().color = Color.yellow;
        }
    }

    private void HighColorLines()
    {
        if (CurrentSelNode == null)
        {
            foreach (var item in lines)
            {
                item.color = Color.white;
            }
            return;
        }
        foreach (var item in lines)
        {
            if (item.line.Node_1 == CurrentSelNode.ImgNode || item.line.Node_2 == CurrentSelNode.ImgNode)
                item.color = Color.yellow;
            else
                item.color = Color.white;
        }
    }

        private bool isHaveLine(Node node1, Node node2)
        {
            if (nodes[node1].ImgNode.Children.Contains(node2))
                return true;
            return false;
        }
    }
}
