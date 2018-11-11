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

            tb_DeckName.Text = decodedDeck.Name;

            Deck deck = m_client.GetCardsFromDecodedDeck(decodedDeck);

            List<System.Windows.Controls.Image> heroImageHolders = new List<System.Windows.Controls.Image>()
            {
                img_HeroOne, img_HeroTwo, img_HeroThree, img_HeroFour, img_HeroFive
            };
            for (int i = 0; i < deck.Heroes.Count; i++)
            {
                int additional = 0;
                int turn = deck.Heroes[i].Turn;
                if (turn > 1)
                    turn += 2; //Add 2 because of heroImageHolders elements 0-2

                //If the hero turn is 1, but already added a hero, then increase additional counter
                while (heroImageHolders[turn - 1 + additional].Source != null)
                    additional++;

                heroImageHolders[turn - 1 + additional].Source = GetImageFromUrl(deck.Heroes[i].IngameImage.Default);
            }

            //Set all other cards
            ic_genericCardsList.ItemsSource = deck.Cards;
        }

        public static BitmapImage GetImageFromUrl(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.EndInit();
            return bitmap;
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
