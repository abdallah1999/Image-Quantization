using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{

    public class MyGraph
    {

        double MSTW; //O(1)
        int no_V, no_E;  //O(1)
        PriorityQueue Nodes;  //O(1)
        Boolean[] Visited;   //O(1)
        Node[] N;    //O(1)
        double k; //O(1)
        public MaxPriorityQueue mxpQ = new MaxPriorityQueue(); //O(1)
        public int[] Parent; //O(1)
        public int[] tmp;  //O(1)
        public LinkedList<RGBPixel> data; //O(1)
        RGBPixel[,] ImageMatrix;  //O(1)
        public MyGraph(int v, int e, LinkedList<RGBPixel> data, RGBPixel[,] ImageMatrix,double K)
        {
            this.k = K;  //O(1)
            no_V = v;   //O(1)
            no_E = e;    //O(1)
            MSTW = 0;  //O(1)
            this.ImageMatrix = ImageMatrix;  //O(1)
            this.data = data;     //O(1)
            Visited = new Boolean[v];  //O(1)
            N = new Node[v];   //O(1)
            Parent = new int[no_V]; //O(1)
            tmp = new int[no_V];    //O(1)
            Nodes = new PriorityQueue(no_V + 1);  //O(1)
            N[0] = new Node(0, 0);     //O(1)
            Visited[0] = true;    //O(1)
            Parent[0] = -1;//O(1)   
            Nodes.Enqueue(N[0]);//O(1)


            //O(V Log(V))
            for (int i = 1; i < no_V; i++)  //V times
            {
                N[i] = new Node(i, double.MaxValue);//O(1)
                Visited[i] = false;//O(1)
                Parent[i] = -1;   //O(1)
                // Initialize key values to infinity 
                Nodes.Enqueue(N[i]); //O(Log(V))
            }
        }

        
        public void prim_MST(){     
            //(V Log V)+(V Log V) +(V^2)+(V^2 Log V)
            //(E Log V)
            while (Nodes.Count != 0)  //V
            {
                Node currentNode = Nodes.Dequeue();  //O(Log V)
                Visited[currentNode.Vertex] = true;//O(1)
                MSTW += N[currentNode.Vertex].Value;  //O(1)
                mxpQ.Enqueue(N[currentNode.Vertex]);   //O(Log(V))
                int count = 0;    //O(1)
                RGBPixel ValueOfVaritx = data.ElementAt(currentNode.Vertex); // O(V)
                foreach (RGBPixel element in data) //V
                {
                    N[count].Color = element;   //O(1)
                    if (currentNode.Vertex != count)  //O(1)
                    {
                        int x, z, y, res;   //O(1)
                        x = Math.Abs(ValueOfVaritx.red - element.red); //O(1)
                        y = Math.Abs(ValueOfVaritx.blue - element.blue);   //O(1)
                        z = Math.Abs(ValueOfVaritx.green - element.green); //O(1)
                        res = x * x + y * y + z * z;   //O(1)
                        AdjNode Node1 = new AdjNode(count, Math.Sqrt(res)); //O(1)
                        if (Visited[Node1.Des] == false) //O(1)
                        {
                            if (N[Node1.Des].Value > Node1.Weight)//O(1)
                            {
                                Nodes.ChangeKey(N[Node1.Des], Node1.Weight); //O(Log V)

                                Parent[Node1.Des] = currentNode.Vertex;  //O(1)
                            }
                        }                    
                    }
                    count++;  //O(1)
                }
                
            }
            Console.WriteLine("MST is : " + Math.Round(MSTW, 1)); //O(1)

            CreateNewColors(k);         //O(K D)     
        }

      
        public void CreateNewColors(double K) {
            int[] array = ExtractKclusters(K); //O(D)
            Dictionary<int, LinkedList<int>> cluster = SplitClusterstolist(array);           
            Hashtable FinalColor = new Hashtable(); //O(1)
            //O(K D)
            foreach (KeyValuePair<int, LinkedList<int>> item in cluster)  //O(K)
            {
                int red = 0, blue = 0, green = 0;  //O(1)

                foreach (int i in item.Value) { //O(D)
                    red = red + (int)(N[i].Color.red);//O(1)
                    blue = blue + (int)(N[i].Color.blue);//O(1)
                    green = green + (int)(N[i].Color.green);   //O(1)            
                }

                red = red / item.Value.Count; //O(1)
                blue = blue / item.Value.Count; //O(1)
                green = green / item.Value.Count; //O(1)
                RGBPixel C = new RGBPixel(); //O(1)
                C.red = (Byte)red;  //O(1)
                C.blue = (Byte)blue; //O(1)
                C.green = (Byte)green; //O(1)
                foreach (int i in item.Value)  //O(D)
                {
                    String key; //O(1)
                    String C1 = N[i].Color.red.ToString(); //O(1)
                    if (C1.Length == 1) { C1 = "00" + C1; } //O(1)
                    if (C1.Length == 2) { C1 = "0" + C1; } //O(1)

                    String C2 = N[i].Color.blue.ToString(); //O(1)
                    if (C2.Length == 1) { C2 = "00" + C2; } //O(1)
                    if (C2.Length == 2) { C2 = "0" + C2; }  //O(1)

                    String C3 = N[i].Color.green.ToString(); //O(1)
                    if (C3.Length == 1) { C3 = "00" + C3; } //O(1)
                    if (C3.Length == 2) { C3 = "0" + C3; } //O(1)

                    key = C1 + C2 + C3;  //O(1)
                    FinalColor[key] = C; //O(1)
                }
            }
            ChangeMatrixColor(FinalColor);
             //O(N^2)
           
            Console.WriteLine("#######################################################");
            MainForm.setmatrix(ImageMatrix); //O(1)     
        }
        public Dictionary<int, LinkedList<int>> SplitClusterstolist(int[] array)
        {

            Dictionary<int, LinkedList<int>> cluster = new Dictionary<int, LinkedList<int>>(); //O(1)

            for (int i = 0; i < no_V; i++)
            {  //O(V)

                if (!cluster.ContainsKey(array[i]))
                { //O(1)
                    cluster.Add(array[i], new LinkedList<int>()); //O(1)

                }
                cluster[array[i]].AddLast(i);      //O(1)         
            }

            return cluster;
        
        }
        public void ChangeMatrixColor(Hashtable FinalColor)
        {
            for (int i = 0; i < ImageMatrix.GetLength(0); i++)
            {

                for (int j = 0; j < ImageMatrix.GetLength(1); j++)
                {

                    String key;//O(1)
                    String C1 = ImageMatrix[i, j].red.ToString();//O(1)
                    if (C1.Length == 1) { C1 = "00" + C1; } //O(1)
                    if (C1.Length == 2) { C1 = "0" + C1; } //O(1)
                    String C2 = ImageMatrix[i, j].blue.ToString(); //O(1)
                    if (C2.Length == 1) { C2 = "00" + C2; } //O(1)
                    if (C2.Length == 2) { C2 = "0" + C2; } //O(1)
                    String C3 = ImageMatrix[i, j].green.ToString(); //O(1)
                    if (C3.Length == 1) { C3 = "00" + C3; } //O(1)
                    if (C3.Length == 2) { C3 = "0" + C3; } //O(1)
                    key = C1 + C2 + C3;

                    ImageMatrix[i, j] = (RGBPixel)FinalColor[key]; //O(1)
                }

            }
        
        
        }

        public int[] ExtractKclusters(double K) //(D+D) O(D)
        {

            //( K Log V)
            for (int i = 0; i < K - 1; i++) // K
            {
                int root = mxpQ.Peek().Vertex;//O(1)
                mxpQ.Dequeue();//O(Log V)
                Parent[root] = -1;//O(1)
            }

            for (int i = 0; i < Parent.Length; i++) //O(D)
            {
                if (Parent[i] == -1) //O(1)
                {
                    tmp[i] = i; //O(1)

                }
                else
                {
                    tmp[i] = find_parent(Parent[i]); //MAX O(D) ONE TIME 

                }
            }

            return tmp;//O(1)
        }

        public int find_parent(int par) //O(D)
        {
            if (Parent[par] == -1) //O(1)
            {
                return par;  //O(1)
            }
            return Parent[par] = find_parent(Parent[par]);//MAX O(D)
        }

    }
}
