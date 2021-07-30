using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Collections;
using System.Diagnostics;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public static RGBPixel[,] ImageMatrix; //O(1)
        static double K;                       //O(1)
        static Stopwatch stopwatch;            //O(1)

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }        
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();

            stopwatch = Stopwatch.StartNew();//O(1)
            System.Threading.Thread.Sleep(500);//O(1)

        }
        public static void setmatrix(RGBPixel[,] I) {

            ImageMatrix = I;  //O(1)
            stopwatch.Stop(); //O(1)
            Console.WriteLine("Time is : "+  stopwatch.Elapsed);//O(1)
          
        
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {

            K = double.Parse(txtGaussSigma.Text);
            GetConstractGrph(ImageMatrix, ImageOperations.GetHeight(ImageMatrix), ImageOperations.GetWidth(ImageMatrix)); 
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
           
        }
        
        
        //O(N^2)
        private static LinkedList<RGBPixel> GetDestinctColor(RGBPixel[,] ImageMatrix, int h, int w)
        {
            LinkedList<RGBPixel> list_DC = new LinkedList<RGBPixel>();  //O(1)
            Hashtable my_hashtable1 = new Hashtable(); //O(1)

            //O(n^2)
            for (int i = 0; i < h; i++) {  // n times
                for (int j = 0; j < w; j++) { // n times

                    String key;//O(1)
                    String C1 = ImageMatrix[i, j].red.ToString(); //O(1)
                    if (C1.Length == 1) { C1 = "00" + C1; }    //O(1)
                    if (C1.Length == 2) { C1 = "0" + C1; }    //O(1)
                    String C2 = ImageMatrix[i, j].blue.ToString();  //O(1)
                    if (C2.Length == 1) { C2 = "00" + C2; } //O(1)
                    if (C2.Length == 2) { C2 = "0" + C2; }         //O(1)         
                    String C3 = ImageMatrix[i, j].green.ToString(); //O(1)
                    if (C3.Length == 1) { C3 = "00" + C3; } //O(1)
                    if (C3.Length == 2) { C3 = "0" + C3; }  //O(1)

                    key = C1 + C2 + C3;  //O(1)

                    if (!my_hashtable1.ContainsKey(key)) //O(1)
                    {
                        my_hashtable1[key] = 0; //O(1)
                        list_DC.AddLast(ImageMatrix[i, j]); //O(1)

                    }
                }
            
            }
            Console.WriteLine("DC is : " + list_DC.Count);  //O(1)

            return list_DC;
        }


        public static void GetConstractGrph(RGBPixel[,] matrix, int h, int w)
        {
            // this is LinkedList of destinct color 
            LinkedList<RGBPixel> data = GetDestinctColor(matrix, h, w); //O(N^2)       
   
            int k = 0, V_s = 0, E_s = 0; //O(1)

            V_s = data.Count(); // Number of vertices in graph  O(1)
            E_s = ((data.Count() * data.Count()) - data.Count()) / 2; // Number of edges in graph  O(1)

            MyGraph graph = new MyGraph(V_s, E_s, data, ImageMatrix, K); //O(V Log(V))


            graph.prim_MST();           //(E Log V)
        }


       
       
    }
}