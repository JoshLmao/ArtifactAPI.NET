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
            if (decodedDeck == null)
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

            List<GenericCard> sortedList = deck.Cards.OrderBy(x => x.ManaCost).ToList();

            //Set all other cards
            ic_genericCardsList.ItemsSource = sortedList;

            int totalGeneric = deck.Cards.Sum(x => x is GenericCard && x.Type.ToLower() != "item" ? ((GenericCard)x).Amount : 0);
            //Total cards are cards that aren't heroes or items
            t_totalCards.Text = $"{totalGeneric} CARDS";

            t_totalItems.Text = $"{deck.Cards.Sum(x => x.Type.ToLower() == "item" ? x.Amount : 0)} ITEMS";

            Enums.Colors colorOne = GetFirstColor(deck.Cards);
            Enums.Colors colorTwo = GetOtherColor(colorOne, deck.Cards);
            ManaDeckInfoDto deckManaInfo = new ManaDeckInfoDto()
            {
                OneManaCards = GetManaAmount(deck.Cards, 1),
                TwoManaCards = GetManaAmount(deck.Cards, 2),
                ThreeManaCards = GetManaAmount(deck.Cards, 3),
                FourManaCards = GetManaAmount(deck.Cards, 4),
                FiveManaCards = GetManaAmount(deck.Cards, 5),
                SixManaCards = GetManaAmount(deck.Cards, 6),
                SevenManaCards = GetManaAmount(deck.Cards, 7),
                EightPlusManaCards = deck.Cards.Sum(x => x.ManaCost >= 8 ? x.Amount : 0),

                ColorOneBrush = FactionColorToBrush(colorOne),
                ColorTwoBrush = FactionColorToBrush(colorTwo),
            };
            deckManaInfo.MaxManaCardCount = GetMaxMana(deckManaInfo.OneManaCards,
                                                    deckManaInfo.TwoManaCards,
                                                    deckManaInfo.ThreeManaCards,
                                                    deckManaInfo.FourManaCards,
                                                    deckManaInfo.FiveManaCards,
                                                    deckManaInfo.SixManaCards,
                                                    deckManaInfo.SevenManaCards,
                                                    deckManaInfo.EightPlusManaCards);

            ic_deckStats.ItemsSource = new List<ManaDeckInfoDto>() { deckManaInfo };
        }

        private int GetManaAmount(List<GenericCard> cards, int manaCostAmount)
        {
            return cards.Sum(x => x.ManaCost == manaCostAmount ? x.Amount : 0);
        }

        private Enums.Colors GetFirstColor(List<GenericCard> cards)
        {
            GenericCard card = cards.FirstOrDefault(x => x.IsBlack || x.IsBlue || x.IsGreen || x.IsRed);

            if (card == null)
                throw new NotImplementedException();

            if (card.IsBlack)
                return Enums.Colors.Black;
            else if (card.IsBlue)
                return Enums.Colors.Blue;
            else if (card.IsGreen)
                return Enums.Colors.Green;
            else if (card.IsRed)
                return Enums.Colors.Red;
            else
                throw new NotImplementedException();
        }

        private Enums.Colors GetOtherColor(Enums.Colors firstColor, List<GenericCard> cards)
        {
            var card = cards.FirstOrDefault(x => x.IsBlack && firstColor != Enums.Colors.Black
                                                || x.IsBlue && firstColor != Enums.Colors.Black
                                                || x.IsGreen && firstColor != Enums.Colors.Green
                                                || x.IsRed && firstColor != Enums.Colors.Red);

            if(card == null)
                throw new NotImplementedException();

            if (card.IsBlack)
                return Enums.Colors.Black;
            else if (card.IsBlue)
                return Enums.Colors.Blue;
            else if (card.IsGreen)
                return Enums.Colors.Green;
            else if (card.IsRed)
                return Enums.Colors.Red;
            else
                throw new NotImplementedException();
        }

        private SolidColorBrush FactionColorToBrush(Enums.Colors cardColor)
        {
            Converters.FactionColorEnumToColorConverter converter = new Converters.FactionColorEnumToColorConverter();
            return converter.Convert(cardColor, null, null, null) as SolidColorBrush;
        }

        private int GetMaxMana(params int[] manaCosts)
        {
            int highestInt = -1;
            for (int i = 0; i < manaCosts.Length; i++)
            {
                int current = manaCosts[i];
                if (current > highestInt)
                    highestInt = current;
            }
            return highestInt;
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

    public class ManaDeckInfoDto
    {
        public int OneManaCards { get; set; }
        public int TwoManaCards { get; set; }
        public int ThreeManaCards { get; set; }
        public int FourManaCards { get; set; }
        public int FiveManaCards { get; set; }
        public int SixManaCards { get; set; }
        public int SevenManaCards { get; set; }
        public int EightPlusManaCards { get; set; }

        public int MaxManaCardCount { get; set; }

        public int ColorOneTotalManaCount { get; set; }
        public int ColorTwoTotalManaCount { get; set; }

        public SolidColorBrush ColorOneBrush { get; set; }
        public SolidColorBrush ColorTwoBrush { get; set; }
    }
}
