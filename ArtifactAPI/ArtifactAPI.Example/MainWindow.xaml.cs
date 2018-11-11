using ArtifactAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArtifactAPI.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArtifactClient m_client;

        public MainWindow()
        {
            InitializeComponent();

            c_exampleWindow.Loaded += OnViewLoaded;
            tb_DeckCode.TextChanged += OnDeckCodeChanged;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            m_client = new ArtifactClient();
        }

        private void OnDeckCodeChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            DecodedDeck decodedDeck = m_client.DecodeDeck(tb.Text);
            if(decodedDeck == null)
            {
                Console.WriteLine("Unable to get deck. DeckCode is invald");
                return;
            }
        }

        private void OnOpenOnWebsite(object sender, MouseButtonEventArgs e)
        {
            string deckCode = tb_DeckCode.Text;
            if (string.IsNullOrEmpty(deckCode))
                return;

            string urlPrefix = @"https://www.playartifact.com/d/";
            System.Diagnostics.Process.Start(urlPrefix + deckCode);
        }


        private void OnMouseEnterHyperlink(object sender, MouseEventArgs e) { Mouse.OverrideCursor = Cursors.Hand; }
        private void OnMouseLeaveHyperlink(object sender, MouseEventArgs e) { Mouse.OverrideCursor = null; }
    }
}
