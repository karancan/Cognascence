using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Cognascence
{
    public partial class Pivot_Session_All_About : PhoneApplicationPage
    {
        public Pivot_Session_All_About()
        {
            InitializeComponent();
            Thread.Sleep(3000);
        }

        private void Image_Tap_Email(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Coganscence query";
            emailComposeTask.To = "karan-91@outlook.com";

            emailComposeTask.Show();
        }

        private void Image_Tap_Facebook(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri("https://www.facebook.com/karanhkhiani", UriKind.Absolute);

            webBrowserTask.Show();
        }

        private void Image_Tap_Linkedin(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri("http://www.linkedin.com/profile/view?id=311679347", UriKind.Absolute);

            webBrowserTask.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //First check if one of the modes was selected
            var mode_mood_state = RadioSessionModeMood.IsChecked;
            var mode_eating_state = RadioSessionModeEating.IsChecked;
            if (mode_mood_state == false && mode_eating_state == false)
            {
                //TODO: Handle multiple themes for social images
                MessageBoxResult messageBoxError = MessageBox.Show("You must pick a behavior type…", "Cannot proceed!", MessageBoxButton.OK);
                return;
            }

            //Then pass the duration to the appropriate page
            PhoneApplicationService.Current.State["session_duration"] = SliderSessionDuration.Value;
            

            if (mode_eating_state == true)
            {
                PhoneApplicationService.Current.State["session_mode"] = "eating";
            }

            if (mode_mood_state == true)
            {
                PhoneApplicationService.Current.State["session_mode"] = "mood";
            }

            NavigationService.Navigate(new Uri("/PageSession.xaml", UriKind.Relative));

        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("page"))
            {
                String page = NavigationContext.QueryString["page"];
                if (page == "2")
                {
                    Pivot_New_All_About.SelectedItem = Pivot_Item_All;

                }
            }
        }


    }
}