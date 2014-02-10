using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Cognascence
{

    public partial class Page2 : PhoneApplicationPage
    {
        DispatcherTimer timer = new DispatcherTimer();
        int session_duration_in_seconds;
        int image_number;
        int sets_of_images_completed = 0;

        public Page2()
        {
            InitializeComponent();
            
            //Set the subtitle
            var session_mode = PhoneApplicationService.Current.State["session_mode"].ToString();

            if (session_mode == "mood")
            {
                TextFieldSessionMode.Text = "Choose the happy face…";    
            }

            if (session_mode == "eating")
            {
                TextFieldSessionMode.Text = "Choose the healthiest food…";
            }

            //Set the clock
            var session_duration = PhoneApplicationService.Current.State["session_duration"].ToString();
            double session_duration_numeric = Convert.ToDouble(session_duration);
            session_duration_numeric = Math.Round(session_duration_numeric);
            session_duration_in_seconds = (int)session_duration_numeric * 60;
            ProgressBarSessionDuration.Maximum = session_duration_in_seconds;

            //Timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick +=
                delegate(object s, EventArgs args)
                {
                    session_duration_in_seconds--;
                    TextFieldSessionCount.Text = session_duration_in_seconds.ToString() + " sec.";
                    ProgressBarSessionDuration.Value = ProgressBarSessionDuration.Maximum - session_duration_in_seconds;

                    //Handle timer stop
                    if (session_duration_in_seconds == 0)
                    {
                        timer.Stop();
                        //TODO: fill in duration and score
                        MessageBoxResult messageBoxEnd = MessageBox.Show("Your session has ended.\n\nDuration: \nScore: ", "Time's up…", MessageBoxButton.OKCancel);
                        NavigationService.Navigate(new Uri("/Pivot_Session_All_About.xaml?page=2", UriKind.Relative));
                    }

                };
            timer.Start();

            Set_Up_Images();

        }

        //If the user tries to exit during a session, double check the back action
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult messageBoxPrematureEnd = MessageBox.Show("Are you sure you want end your session?", "Wait a minute…", MessageBoxButton.OKCancel);
            if (messageBoxPrematureEnd != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                timer.Stop();
            }
        }

        public void Set_Up_Images()
        {
            //Load images
            //Assuming we are just doing faces for now
            Random random = new Random();
            image_number = random.Next(1, 25);
            //Pick a happy face
            String happy_face = "Assets/faces/happy/" + image_number.ToString() + "s.png";
            //Pick 4 sad faces of which one will be replaced by a happy face
            image_number = random.Next(1, 25);
            String sad_face_1 = "Assets/faces/sad/" + image_number.ToString() + "f.png";
            image_number = random.Next(1, 25);
            String sad_face_2 = "Assets/faces/sad/" + image_number.ToString() + "f.png";
            image_number = random.Next(1, 25);
            String sad_face_3 = "Assets/faces/sad/" + image_number.ToString() + "f.png";
            image_number = random.Next(1, 25);
            String sad_face_4 = "Assets/faces/sad/" + image_number.ToString() + "f.png";

            //Plug in the four sad faces
            SessionImage1.Source = new BitmapImage(new Uri(sad_face_1, UriKind.RelativeOrAbsolute));
            SessionImage2.Source = new BitmapImage(new Uri(sad_face_2, UriKind.RelativeOrAbsolute));
            SessionImage3.Source = new BitmapImage(new Uri(sad_face_3, UriKind.RelativeOrAbsolute));
            SessionImage4.Source = new BitmapImage(new Uri(sad_face_4, UriKind.RelativeOrAbsolute));

            //Pick a number for the happy face
            int random_happy_face_pos = random.Next(1, 4);
            PhoneApplicationService.Current.State["position_image_positive"] = random_happy_face_pos;

            //Replace one of the saf faces with a happy face
            if (random_happy_face_pos == 1)
            {
                SessionImage1.Source = new BitmapImage(new Uri(happy_face, UriKind.RelativeOrAbsolute));
            }
            else if (random_happy_face_pos == 2)
            {
                SessionImage2.Source = new BitmapImage(new Uri(happy_face, UriKind.RelativeOrAbsolute));  
            }
            else if (random_happy_face_pos == 3)
            {
                SessionImage3.Source = new BitmapImage(new Uri(happy_face, UriKind.RelativeOrAbsolute));
            }
            else
            {
                SessionImage4.Source = new BitmapImage(new Uri(happy_face, UriKind.RelativeOrAbsolute));
            }

            //System.Diagnostics.Debug.WriteLine(SessionImage1.Source.ToString());
        }

        private void Verify_Image_Status(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var image_clicked_on = (Image)sender;
            String image_positive = "SessionImage" + PhoneApplicationService.Current.State["position_image_positive"].ToString();
            String image_clicked_on_name = image_clicked_on.Name.ToString();
            if (image_positive == image_clicked_on_name)
            {
                //MessageBoxResult successImage = MessageBox.Show("Good boy...…\nClicked on:" + image_clicked_on_name + "\nPositive:" + image_positive, "Success…", MessageBoxButton.OKCancel);
            }
            else
            {
                //MessageBoxResult failImage = MessageBox.Show("Bad boy...…\nClicked on:" + image_clicked_on_name + "\nPositive:" + image_positive, "Fail", MessageBoxButton.OKCancel);
            }
            sets_of_images_completed++;
            //TODO: database: Time,duration,sets seen, sets success
            Set_Up_Images();
        }

    }
}