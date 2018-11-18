using ArtifactAPI.Enums;
using ArtifactAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArtifactAPI.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArtifactClient m_client;

        public Language SetLanguage { get { return m_currentLanguage; } }

        private Language m_currentLanguage = Enums.Language.English;

        public MainWindow()
        {
            InitializeComponent();

            c_exampleWindow.Loaded += OnViewLoaded;
            tb_DeckCode.TextChanged += OnDeckCodeChanged;
            c_exampleWindow.DataContext = this;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            m_client = new ArtifactClient();
            g_Loading.Visibility = Visibility.Collapsed;

            //Add all languages to Language ComboBox
            var allLanguages = Enum.GetValues(typeof(Language));
            foreach(Language language in allLanguages)
                cb_language.Items.Add(language);

            cb_language.SelectedIndex = 0;
            m_currentLanguage = (Language)allLanguages.GetValue(0);
        }

        private async void OnDeckCodeChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            await Update(tb.Text);
        }

        /// <summary>
        /// Updated the whole UI using a deck code
        /// </summary>
        /// <param name="deckCode"></param>
        /// <returns></returns>
        private async Task Update(string deckCode)
        {
            //Decode the string into a deck. See if it's valid
            DecodedDeck decodedDeck = m_client.DecodeDeck(deckCode);
            if (decodedDeck == null)
            {
                Console.WriteLine("Unable to get deck. DeckCode is invald");
                return;
            }

            g_Loading.Visibility = Visibility.Visible;

            /*Reset all UI holder*/
            if (img_HeroOne.Source != null)
                img_HeroOne.Source = null;
            if (img_HeroTwo.Source != null)
                img_HeroTwo.Source = null;
            if (img_HeroThree.Source != null)
                img_HeroThree.Source = null;
            if (img_HeroFour.Source != null)
                img_HeroFour.Source = null;
            if (img_HeroFive.Source != null)
                img_HeroFive.Source = null;

            SetFocusedCard(-1, ArtType.Ingame, Enums.Language.English);
            t_totalCards.Text = null;
            t_totalItems.Text = null;
            t_TCSpell.Text = null;
            t_TCCreep.Text = null;
            t_TCImprovement.Text = null;
            t_TIarmor.Text = null;
            t_TIweapon.Text = null;
            t_TIhealth.Text = null;
            t_TIconsumable.Text = null;
            /*End of resetting UI holders*/

            //Set the deck name title
            tb_DeckName.Text = decodedDeck.Name;

            //Decode the deck to the complete deck
            Deck deck = await m_client.GetCardsFromDecodedDeckAsync(decodedDeck);

            //Populate the hero UI
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

                System.Windows.Controls.Image img = heroImageHolders[turn - 1 + additional];
                img.Source = GetImageFromUrl(deck.Heroes[i].IngameImage.Default);
                img.DataContext = deck.Heroes[i]; //Set context for click event finding card art
            }

            //Sort cards by mana cost & set UI
            List<GenericCard> sortedList = deck.Cards.OrderBy(x => x.ManaCost).ToList();
            ic_genericCardsList.ItemsSource = sortedList;

            //Total cards excludes items, hence the separate total items UI
            int totalGeneric = deck.Cards.Sum(x => x is GenericCard && x.Type == CardType.Item ? ((GenericCard)x).Count : 0);
            t_totalCards.Text = $"{totalGeneric} CARDS";
            t_totalItems.Text = $"{deck.Cards.Sum(x => x.Type == CardType.Item ? x.Count : 0)} ITEMS";

            //Find out both colors of deck and get stats for each color
            CardColor colorOne = GetOtherColor(deck.Cards, CardColor.None);
            CardColor colorTwo = GetOtherColor(deck.Cards, colorOne | CardColor.None);
            ManaDeckInfoDto deckManaInfo = new ManaDeckInfoDto()
            {
                OneManaCards = GetManaAmount(deck.Cards, 1),
                TwoManaCards = GetManaAmount(deck.Cards, 2),
                ThreeManaCards = GetManaAmount(deck.Cards, 3),
                FourManaCards = GetManaAmount(deck.Cards, 4),
                FiveManaCards = GetManaAmount(deck.Cards, 5),
                SixManaCards = GetManaAmount(deck.Cards, 6),
                SevenManaCards = GetManaAmount(deck.Cards, 7),
                EightPlusManaCards = deck.Cards.Sum(x => x.ManaCost >= 8 ? x.Count : 0),

                ColorOneBrush = FactionColorToBrush(colorOne),
                ColorTwoBrush = FactionColorToBrush(colorTwo),
                ColorOneTotalCardCount = deck.Cards.Sum(x => x.FactionColor == colorOne ? x.Count : 0),
                ColorTwoTotalCardCount = deck.Cards.Sum(x => x.FactionColor == colorTwo ? x.Count : 0),
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

            //Set relevant stat information for deck
            t_TCSpell.Text = deck.Cards.Sum(x => x.Type == CardType.Spell ? x.Count : 0).ToString();
            t_TCCreep.Text = deck.Cards.Sum(x => x.Type == CardType.Creep ? x.Count : 0).ToString();
            t_TCImprovement.Text = deck.Cards.Sum(x => (x.Type == CardType.Improvement) ? x.Count : 0).ToString();

            t_TIarmor.Text = deck.Cards.Sum(x => x.Type == CardType.Item && x.SubType == CardType.Armor ? x.Count : 0).ToString();
            t_TIweapon.Text = deck.Cards.Sum(x => x.Type == CardType.Item && x.SubType == CardType.Weapon ? x.Count : 0).ToString();
            t_TIhealth.Text = deck.Cards.Sum(x => x.Type == CardType.Item && x.SubType == CardType.Accessory ? x.Count : 0).ToString();
            t_TIconsumable.Text = deck.Cards.Sum(x => x.Type == CardType.Item && x.SubType == CardType.Consumable ? x.Count : 0).ToString();

            g_Loading.Visibility = Visibility.Collapsed;
        }

        private int GetManaAmount(List<GenericCard> cards, int manaCostAmount)
        {
            return cards.Sum(x => x.ManaCost == manaCostAmount ? x.Count : 0);
        }

        private CardColor GetOtherColor(List<GenericCard> cards, CardColor colors)
        {
            Card card = cards.FirstOrDefault(x => x.FactionColor != colors);

            if(card == null)
                throw new NotImplementedException();
            return card.FactionColor;
        }

        private SolidColorBrush FactionColorToBrush(CardColor cardColor)
        {
            Converters.CardColorToBrushConverter converter = new Converters.CardColorToBrushConverter();
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
            if (string.IsNullOrEmpty(url))
                return null;

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

        private async void OnClickedCard(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement conv = sender as FrameworkElement;
            if (conv == null)
                return;

            Card gCard = conv.DataContext as Card;
            if (gCard == null)
                return;

            await SetFocusedCard(gCard.Id, ArtType.Large, m_currentLanguage);
        }

        private async Task SetFocusedCard(int cardId, ArtType artType, Language language)
        {
            string artUrl = await m_client.GetCardArtUrlAsync(cardId, artType, language);
            if (string.IsNullOrEmpty(artUrl))
            {
                img_lastClickedCard.Source = null;
            }
            else
            {
                img_lastClickedCard.Source = GetImageFromUrl(artUrl);
            }
        }

        private void OnMouseEnterHyperlink(object sender, MouseEventArgs e) { Mouse.OverrideCursor = Cursors.Hand; }
        private void OnMouseLeaveHyperlink(object sender, MouseEventArgs e) { Mouse.OverrideCursor = null; }

        private async void OnLanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int newIndex = (sender as ComboBox).SelectedIndex;
            m_currentLanguage = (Language)newIndex;
            await Update(tb_DeckCode.Text);

            //ToDo: reselect card with new selected language
        }
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

        public int ColorOneTotalCardCount { get; set; }
        public int ColorTwoTotalCardCount { get; set; }

        public SolidColorBrush ColorOneBrush { get; set; }
        public SolidColorBrush ColorTwoBrush { get; set; }
    }
}
