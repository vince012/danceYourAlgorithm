using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using ConsoleApp1;
using System.Threading;
using CefSharp;
using System.IO.Compression;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private GestureController _mainGesture = new GestureController();
        private GestureController _mainGesture2 = new GestureController();
        readonly string IMAGE_OFF = "Off";
        readonly string PATH_PROJECT = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        private KinectSensor sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
        private List<string> _listGestures1 = new List<string>();
        private List<string> _listGestures2 = new List<string>();
        private List<int> listSkeletons = new List<int>();
        private bool fingerCount;
        private ColorImageFrame frame;

        private bool dialog = false;
        private List<String> listVar = new List<String>();

        Image<Bgr, Byte> currentFrame;

        public MainWindow()
        {
            if (sensor == null)
            {
                Console.WriteLine("Kinect not detected...");
                Thread.Sleep(2000);
                System.Environment.Exit(1);
            }

            InitializeComponent();
            this.Title = "DYA Algorithm :)";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            kinectViewer.FlippedHorizontally = false;
            kinectViewer.FlippedVertically = false;

            // Pattern Visitor tested on two executions (Execution) or sequences. The goal is
            // to read through the different statues to create an antomaton

            //Execution exec = new Execution();
            //exec.AddState("1");
            //exec.AddState("2");
            //exec.AddState("4");
            //StateVisitor v1 = new StateVisitor();
            //exec.Accept(v1);

            //Console.WriteLine("***************");

            //Execution exec2 = new Execution();
            //exec2.AddState("1");
            //exec2.AddState("3");
            //exec2.AddState("4");
            //StateVisitor v2 = new StateVisitor();
            //exec.Accept(v2);
        }

        /// <summary>
        /// Initializing Kinect
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Window Loaded");

            if (sensor != null)
            {
                sensor.ColorStream.Enable();
                sensor.DepthStream.Enable();
                sensor.SkeletonStream.Enable();

                sensor.ColorFrameReady += Sensor_ColorFrameReady;
                sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;

                _mainGesture.GestureDetected += Gesture_Detected;
                _mainGesture2.GestureDetected += Gesture_Detected;

                try
                {
                    sensor.Start();
                }
                catch (Exception)
                {
                    sensor = null;
                }

                if (sensor != null) Console.WriteLine("Sensor has just started ...");
                else Console.WriteLine("No Kinect ready");
            }
            else
            {
                Console.WriteLine("No sensor detected");
                System.Environment.Exit(1);
            }
        }

        public Mode Mode
        {
            get { return kinectViewer.FrameType;}
            set { kinectViewer.FrameType = value;}
        }

        Bitmap ImageToBitmap(ColorImageFrame Image)
        {
            byte[] pixeldata = new byte[Image.PixelDataLength];
            Image.CopyPixelDataTo(pixeldata);
            Bitmap bmap = new Bitmap(Image.Width, Image.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            BitmapData bmapdata = bmap.LockBits(
                new System.Drawing.Rectangle(0, 0, Image.Width, Image.Height),
                ImageLockMode.WriteOnly,
                bmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(pixeldata, 0, ptr, Image.PixelDataLength);
            bmap.UnlockBits(bmapdata);
            return bmap;
        }

        BitmapSource BitmapToSource(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        /// <summary>
        /// Shows the skeleton on screen
        /// </summary>
        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (Mode != Mode.Color) return;

            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    frame = colorFrame;
                    currentFrame = new Image<Bgr, byte>(ImageToBitmap(frame));

                    if (fingerCount && !dialog)
                    {
                        fingerCount = false;
                        this.dialog = true;
                        Thread th = new Thread(new ThreadStart(() =>
                        {
                            InputDialog inputDialog = new InputDialog("Insert variable :", "");
                            if (inputDialog.ShowDialog() == true)
                            {
                                listVar.Add(inputDialog.Answer);
                                this.dialog = false;
                            }
                        }));
                        th.SetApartmentState(ApartmentState.STA);
                        th.Start();
                    }
                    int left = 10;
                    int bottom = 80;
                    for (int i = 0; i < listVar.Count; i++)
                    {
                        String var = listVar.ElementAt<String>(i);
                        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 2d, 2d);
                        currentFrame.Draw(var.ToString(), ref font, new System.Drawing.Point(left, bottom), new Bgr(System.Drawing.Color.White));
                        bottom += 30;
                    }
                    kinectViewer.Update(BitmapToSource(currentFrame.ToBitmap()));
                }
            }
        }

        /// <summary>
        /// Resets the application in the initial state
        /// </summary>
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            listeGestes.Items.Clear();
            listeGestes2.Items.Clear();
            _listGestures1.Clear();
            _listGestures2.Clear();

            _mainGesture.setGreenFlag(false);
            _mainGesture2.setGreenFlag(false);
        }

        /// <summary>
        /// Saves the sequences of statues (2 children) then generate the Scratch project
        /// </summary>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            string[] res = new string[2];

            string res2 = "";
            string res1 = "";

            res[0] = res1;
            res[1] = res2;

            bool apparition_boucles = false;

            if (_listGestures1.Count > 2)
            {
                if (!_listGestures1[_listGestures1.Count - 1].Equals("End"))
                    _listGestures1.Add("End");

                for (int i = 1; i < _listGestures1.Count; i++)
                {
                    if (i == _listGestures1.Count - 1) res1 = res1 + _listGestures1[i];
                    else res1 = res1 + _listGestures1[i] + ",";
                }

                if (apparition_boucles)
                    res1 = generate_algorithm(res1);
                res[0] = res1;
                _listGestures1.Clear();
                listeGestes.Items.Clear();
            }

            if (_listGestures2.Count > 2)
            {
                if (!_listGestures2[_listGestures2.Count - 1].Equals("End"))
                    _listGestures2.Add("End");

                for (int i = 1; i < _listGestures2.Count; i++)
                {
                    if (i == _listGestures2.Count - 1) res2 = res2 + _listGestures2[i];
                    else res2 = res2 + _listGestures2[i] + ",";
                }

                if (apparition_boucles) res2 = generate_algorithm(res2);
                if (!res[0].Equals("")) res[1] = res2;
                else res[0] = res2;
                _listGestures2.Clear();
                listeGestes2.Items.Clear();
            }

            if (!res[0].Equals("") && !res[1].Equals(""))
                getDataFromJSAsync(res, 2);
            else if ((!res[0].Equals("") && res[1].Equals("")) || (res[0].Equals("") && !res[1].Equals("")))
                getDataFromJSAsync(res, 1);

            // Testing converting sequence to Scratch project  
            //res[0] = "GreenFlag,Jump,4,HandUp,Jump,3,5,End";
            ////res[1] = "GreenFlag,HandUp,End";
            //getDataFromJSAsync(res, 1);
        }

        /// <summary>
        /// Shows the red cross if the statue is not clearly detected
        /// </summary>
        private void Failed_Gesture()
        {
            string SourcePathNo = No.Source.ToString();
            string SourcePathYes = Yes.Source.ToString();
            if (!SourcePathYes.Contains(IMAGE_OFF))
            {
                SourcePathYes = SourcePathYes.Replace(".png", "Off.png");
                Yes.Source = new BitmapImage(new Uri(SourcePathYes));
            }
            if (SourcePathNo.Contains(IMAGE_OFF))
            {
                SourcePathNo = SourcePathNo.Replace(IMAGE_OFF, "");
                No.Source = new BitmapImage(new Uri(SourcePathNo));
            }
        }

        /// <summary>
        /// Shows the green validation symbol if the statue is correctly detected
        /// </summary>
        private void Success_Gesture()
        {
            string SourcePathNo = No.Source.ToString();
            string SourcePathYes = Yes.Source.ToString();

            if (SourcePathYes.Contains(IMAGE_OFF))
            {
                SourcePathYes = SourcePathYes.Replace(IMAGE_OFF, "");
                Yes.Source = new BitmapImage(new Uri(SourcePathYes));
                new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(150);
                    this.Dispatcher.Invoke(() =>
                    {
                        SourcePathYes = SourcePathYes.Replace(".png", "Off.png");
                        Yes.Source = new BitmapImage(new Uri(SourcePathYes));
                    });

                })).Start();
            }
            if (!SourcePathNo.Contains(IMAGE_OFF))
            {
                SourcePathNo = SourcePathNo.Replace(".png", "Off.png");
                No.Source = new BitmapImage(new Uri(SourcePathNo));
            }
        }

        /// <summary>
        /// Check if one or two children are on the scene (adding/clearing children's skeleton to the list, )
        /// then shows skeletons and check the statues 
        /// </summary>
        private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
                if (frame != null)
                {
                    kinectViewer.Clear();
                    var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);

                    if (skeletons.Count() == 0)
                    {
                        if (listSkeletons.Count != 0)
                        {
                            listSkeletons.Clear();                           
                            _mainGesture.setGreenFlag(false);
                            _mainGesture2.setGreenFlag(false);
                        }
                    }
                    else
                    {
                        if (skeletons.Count() == 2)
                        {
                            if (listSkeletons.Count == 2 && !recherche_skeleton(skeletons.Last().TrackingId))
                            {
                                if (listSkeletons.IndexOf(skeletons.First().TrackingId) == 1)
                                {
                                    listSkeletons[0] = 0;
                                    _listGestures1.Clear();
                                    listeGestes.Items.Clear();
                                }
                                if (listSkeletons.IndexOf(skeletons.First().TrackingId) == 0)
                                {
                                    listSkeletons.RemoveAt(1);
                                    _listGestures2.Clear();
                                    listeGestes2.Items.Clear();
                                }
                            }
                            else if (listSkeletons.Count == 2)                            
                                _mainGesture2.Update(skeletons.Last());
                            kinectViewer.DrawBody(skeletons.Last());
                        }
             
                        if (listSkeletons.Count == 0)
                        {
                            listeGestes.Items.Clear();
                            _listGestures1.Clear();
                            _mainGesture.setGreenFlag(false);
                            listSkeletons.Add(skeletons.First().TrackingId);
                            _listGestures1.Add(skeletons.First().TrackingId.ToString());
                        }
                        else if (listSkeletons.Count == 1)
                        {
                            if ((!recherche_skeleton(skeletons.Last().TrackingId)))
                            {
                                if (listSkeletons[0] == 0)
                                {
                                    _mainGesture.setGreenFlag(false);
                                    listSkeletons[0] = skeletons.Last().TrackingId;
                                    _listGestures1.Add(skeletons.Last().TrackingId.ToString());
                                }
                                else
                                {
                                    _mainGesture2.setGreenFlag(false);
                                    listSkeletons.Add(skeletons.Last().TrackingId);
                                    _listGestures2.Add(skeletons.Last().TrackingId.ToString());
                                }
                            }
                        }                      
                        if (skeletons.Count() == 1)
                        {
                            if (listSkeletons.IndexOf(skeletons.First().TrackingId) == 1)
                                _mainGesture2.Update(skeletons.First());
                            else                         
                                _mainGesture.Update(skeletons.First());
                        }
                        kinectViewer.DrawBody(skeletons.First());
                    }
                }
        }

        /// <summary>
        /// Checks if a skeleton detected by the camera is in the saved skeleton's list 
        /// </summary>
        public bool recherche_skeleton(int TrackingId)
        {
            bool res = false;
            foreach (int i in listSkeletons)
            {
                if (TrackingId == i)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        /// <summary>
        /// Adds the detected statue to the list depending on the number of children present on the scene 
        /// </summary>
        public void Gesture_Detected(object sender, GestureEventArgs e)
        {
            Success_Gesture();
            Console.WriteLine("Gesture detected, {0} {1}", e.Name, e.TrackingId);

            if (_listGestures1[0].Equals(e.TrackingId.ToString())) AddGesture(e, _listGestures1, listeGestes, _mainGesture);

            if (_listGestures2.Count != 0)
                if (_listGestures2[0].Equals(e.TrackingId.ToString())) AddGesture(e, _listGestures2, listeGestes2, _mainGesture2);
        }

        /// <summary>
        /// Adds the detected statue to the list depending on the number of children present on the scene 
        /// </summary>
        public void AddGesture(GestureEventArgs e, List<string> _listGestures, ListBox listeGestes, GestureController _mainGesture)
        {
            TextBlock txt = new TextBlock();

            txt.Text = e.TrackingId + ":" + e.Name;
            listeGestes.Items.Add(txt);
            listeGestes.SelectedIndex = listeGestes.Items.Count - 1;
            listeGestes.ScrollIntoView(listeGestes.SelectedItem);

            if ((e.Name.Equals("GreenFlag") && !_mainGesture.getGreenFlag()))
            {
                _mainGesture.setGreenFlag(true);
                _listGestures.Add(e.Name);
            }                                     
            else if (!e.Name.Equals("WaveRight") && _mainGesture.getGreenFlag())
            {
                if (e.Name.Equals("FingerCount"))
                {
                    Console.WriteLine("FingerCount");
                    fingerCount = true;
                }
                else if (e.Name.Equals("Stop"))
                {
                    _listGestures.Add("End");
                    _mainGesture.setGreenFlag(false);
                }
                else _listGestures.Add(e.Name);                   
            }
            else if (_mainGesture.getGreenFlag() && e.Name.Equals("WaveRight"))
            {
                if (listeGestes.Items.Count > 1) listeGestes.Items.RemoveAt(listeGestes.Items.Count - 1);
                if (_listGestures.Count > 2) _listGestures.RemoveAt(_listGestures.Count - 1);
            }
        }

        /// <summary>
        /// Setting up the elevation angle of Kinect
        /// </summary>
        private void btnSlide_Click(object sender, RoutedEventArgs e)
        {
            sensor.ElevationAngle = (int)slider.Value;
        }

        /// <summary>
        /// Settings to the embedded web browser
        /// </summary>
        private void ChromiumWebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "web\\index.html");
            this.myWeb.Address = path;
        }

        /// <summary>
        /// Deals with loops. Reveals a loop in a sequence.
        /// Ex : GreenFlag,Up,Left,Down,Up,Left,Down,Right,End --> GreenFlag,OpenRepeat,Up,Left,Down,2,CloseRepeat,Right,End
        /// </summary>
        public string generate_algorithm(string input)
        {
            string[] gestures = input.Split(',');
            List<string> mouv_seq_list = new List<string>();

            string actual;
            string next = "";
            string res = "";
            string next_value = "";

            bool loop = true;

            int cur_next = 0;
            int i = 0;
            int nb_loop = 1;
            int nb_suites = 1;
            int distance = 0;
            int prec_dist = 0;
            
            Regex regex = new Regex(@"\d+");
            Match match;

            if (gestures[0] != "GreenFlag" || gestures[gestures.Length - 1] != "End")
            {
                MessageBox.Show("Erreur format séquence : FlagGreen ou End non présent(s)");
                return "";
            }
            else
            {
                while (i <= gestures.Length - 1)
                {
                    actual = gestures[i];
                    match = regex.Match(actual);

                    if (match.Success) i++;
                    else
                    {
                        // recherche occurence suivante de act
                        if (i != gestures.Length - 1)
                        {
                            for (int j = i + 1; j < gestures.Length; j++)
                            {
                                if (gestures[j].Equals(actual))
                                {
                                    next = gestures[j];
                                    cur_next = j;
                                    break;
                                }
                            }
                        }
                        // si occurence trouvée
                        if (!next.Equals(""))
                        {
                            distance = cur_next - i;
                            next = "";
                            // si écart entre actuel et suivant < 2 alors peut-être boucle
                            if (distance > 1)
                            {
                                // un motif déjà validé mais le suivant n'est pas de la même longueur, on
                                // a plus de boucles
                                if (nb_loop > 1 && distance != prec_dist)
                                    loop = false;
                                // si on sort de la liste en testant la boucle alors c'est mort
                                if (cur_next + distance > gestures.Length || cur_next == gestures.Length - 1)
                                    loop = false;
                                else
                                {
                                    // vérif motif se répète ?
                                    for (int k = i + 1; k < i + distance; k++)
                                    {
                                        if (!gestures[k].Equals(gestures[k + distance]))
                                        {
                                            loop = false;
                                            break;
                                        }
                                    }
                                }
                                // si motif se répète, alors boucle validée
                                if (loop)
                                {
                                    // MessageBox.Show("boucle détectée");
                                    prec_dist = distance;
                                    if (cur_next + distance > gestures.Length - 1)
                                    {
                                        // on ajoute les items de la boucle dans une liste
                                        if (mouv_seq_list.Count == 0)
                                            for (int k = i; k < i + distance; k++)
                                                mouv_seq_list.Add(gestures[k]);
                                        nb_loop++;
                                        i = i + prec_dist;
                                    }
                                    else
                                    {
                                        if (nb_suites > 1)
                                        {
                                            res = res + "OpenRepeat," + next_value + "," + nb_suites + "," + "CloseRepeat" + ",";
                                            nb_suites = 1;
                                        }

                                        if (mouv_seq_list.Count == 0)
                                            for (int k = i; k < i + distance; k++)
                                                mouv_seq_list.Add(gestures[k]);
                                        i = i + prec_dist;
                                        nb_loop++;
                                    }
                                }// si pas de boucle validée 
                                else
                                {
                                    // si des items déjà validés, on affiche la boucle
                                    if (nb_loop > 1)
                                    {
                                        i = i + prec_dist;
                                        distance = 0;
                                        res = ajout_boucle_res(mouv_seq_list, nb_loop, res);
                                        mouv_seq_list.Clear();
                                        prec_dist = 0;
                                        nb_loop = 1;
                                    }

                                    else if (nb_suites > 1)
                                    {
                                        res = ajout_boucle_res(mouv_seq_list, nb_loop, res);
                                        i++;
                                        nb_suites = 1;
                                    }
                                    // sinon on affiche uniquement l'élément qui est tout seul et on passe au suivant
                                    else
                                    {
                                        res = res + actual + ",";
                                        i++;
                                    }
                                    loop = true;
                                }
                            }
                            // suite présente
                            else
                            {
                                // MessageBox.Show("suite détectée");
                                // passage d'une boucle à une suite
                                if (nb_loop > 1)
                                {
                                    res = ajout_boucle_res(mouv_seq_list, nb_loop, res);
                                    nb_loop = 1;
                                }

                                if (nb_suites == 1) next_value = actual;

                                match = regex.Match(gestures[i + 2]);
                                if (match.Success)
                                {
                                    if (nb_suites > 1) res = res + "OpenRepeat," + next_value + "," + nb_suites + "," + "CloseRepeat" + ",";
                                    else res = res + actual + ",";

                                    nb_suites = 1;
                                }
                                else nb_suites++;
                                i++;
                            }
                        }
                        // pas d'occurence de act trouvée
                        else
                        {
                            // on affiche la boucle si elle était présente
                            if (nb_loop > 1)
                            {
                                if (i + prec_dist <= gestures.Length)
                                {
                                    res = ajout_boucle_res(mouv_seq_list, nb_loop, res);
                                    nb_loop = 1;
                                    i = i + prec_dist;
                                }
                                else
                                {
                                    if (i < gestures.Length - 1)
                                        res = res + actual + ",";
                                    else
                                        if (i == gestures.Length - 1)
                                            res = res + actual;
                                    i++;
                                }
                            }
                            else if (nb_suites > 1)
                            {
                                if (i < gestures.Length - 1)
                                    res = res + "OpenRepeat" + "," + next_value + "," + nb_suites + "," + "CloseRepeat" + ",";
                                else if (i == gestures.Length - 1)
                                    res = res + "OpenRepeat" + "," + next_value + "," + nb_suites + "CloseRepeat";
                                nb_suites = 1;
                                i++;
                            }
                            // sinon on passe à l'item suivant
                            else
                            {
                                if (i < gestures.Length - 1)
                                {
                                    match = regex.Match(gestures[i + 1]);
                                    if (match.Success)
                                    {
                                        res = res + actual + "," + gestures[i + 1] + ",";
                                        i = i + 2;
                                    }
                                    else
                                    {                                  
                                        res = res + actual + ",";
                                        i++;
                                    }
                                }
                                else if (i == gestures.Length - 1)
                                {
                                    res = res + actual;
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// When loop detected, adds a string to the sequence
        /// </summary>
        public string ajout_boucle_res(List<string> l, int nb_boucles, string res)
        {
            string r = "";
            foreach (string s in l)
                r = r + s + ",";
            res = res + "OpenRepeat," + r + nb_boucles + "," + "CloseRepeat" + ",";
            return res;
        }

        /// <summary>
        /// Converts the sequence to a Scratch project
        /// </summary>
        public void getDataFromJSAsync(string[] input,int charact_numb)
        {
            string script = "";
            if (charact_numb == 2)
            {
                script = @"(function()
    				{
                        go(""" + input[0] + @""",""" + input[1] + @""");
	    				return document.getElementById('main').innerHTML;
    				})();";
            }
            else if (charact_numb == 1)
            {
                script = @"(function()
    				{
                        go(""" + input[0] + @""");
	    				return document.getElementById('main').innerHTML;
    				})();";
            }

            string returnData = "404";
            myWeb.EvaluateScriptAsync(script).ContinueWith(x =>
            {
                var response = x.Result;

                if (response.Success && response.Result != null)
                {
                    returnData = response.Result.ToString();
                    string dirName = "Scratch_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                    string path = PATH_PROJECT + dirName;
                    string fileName = "project.json";
                    Directory.CreateDirectory(path);
                    string sourcePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "projet");
                    string file = string.Empty;
                    string destFile = string.Empty;
                    if (System.IO.Directory.Exists(sourcePath))
                    {
                        if (!returnData.Equals("404"))
                        {
                            /* Copy files from projet to project */
                            string[] files = System.IO.Directory.GetFiles(sourcePath);

                            // Copy the files and overwrite destination files if they already exist. 
                            foreach (string s in files)
                            {
                                // Use static Path methods to extract only the file name from the path.
                                file = System.IO.Path.GetFileName(s);
                                destFile = System.IO.Path.Combine(path, file);
                                System.IO.File.Copy(s, destFile, true);
                            }
                            /* Create json files */
                            File.WriteAllText(path + "\\" + fileName, returnData);
                            string zipPath = path + "\\projet.sb3";
                            try
                            {
                                ZipFile.CreateFromDirectory(path, zipPath);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Exception dans ZipFile.CreateFromDirectory() mais on s'en fiche.");
                            }
                            string[] fileList = System.IO.Directory.GetFiles(path);
                            foreach (string f in fileList)
                            {
                                if (!f.Contains("projet.sb3"))
                                {
                                    Console.WriteLine(f + " will be deleted");
                                    System.IO.File.Delete(f);
                                }
                            }
                            Console.WriteLine("Project has been created successfully");                          
                        }
                        else
                            Console.WriteLine("Failed while tried to save json file");
                    }
                    else
                        Console.WriteLine("Source path does not exist!");
                }
            });
        }
    }    
}
