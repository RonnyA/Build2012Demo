using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SignalRChat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HubConnection _connection;
        private IHubProxy _proxy;
        private SynchronizationContext _syncContext;

        public MainPage()
        {
            this.InitializeComponent();

            _syncContext = SynchronizationContext.Current;
            Messages = new ObservableCollection<Message>();

            messages.ItemsSource = Messages;
        }

        public ObservableCollection<Message> Messages { get; set; }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void connect_Click(object sender, RoutedEventArgs e)
        {
            if (_connection == null)
            {
                _connection = new HubConnection("http://localhost:2553/");
                _proxy = _connection.CreateHubProxy("Chat");

                _proxy.On<string>("send", message =>
                {
                    _syncContext.Post(_ => Messages.Add(new Message(message)), null);
                });

                try
                {
                    await _connection.Start();
                }
                catch(Exception ex)
                {
                    Messages.Add(new Message(ex.Message));
                }
            }
        }

        private async void send_Click(object sender, RoutedEventArgs e)
        {
            if (_proxy != null)
            {
                await _proxy.Invoke("send", "Windows8: " + msg.Text);
            }
        }
    }

    public class Message
    {
        public Message(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
