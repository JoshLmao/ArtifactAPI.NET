using ArtifactAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI
{
    /// <summary>
    /// C# version of Valve's DeckDecoder script wrote in PHP (https://github.com/ValveSoftware/ArtifactDeckCode/blob/master/PHP/deck_decoder.php)
    /// </summary>
    public class DeckDecoder
    {
        const int CURRENT_VERSION = 2;
        const string DECK_PREFIX = "ADC";

        /// <summary>
        /// Decodes a base 64 string and returns the deck decoded
        /// </summary>
        /// <param name="deckCode">The base64 encoded string</param>
        /// <returns>A decoded deck containing deck name, card id's and positions</returns>
        public static DecodedDeck Decode(string deckCode)
        {
            byte[] deckBytes = DeckDecodeString(deckCode);
            if (deckBytes == null)
                return null;

            DecodedDeck deck = ParseDeck(deckCode, deckBytes);
            return deck;
        }

        private static byte[] DeckDecodeString(string deckCode)
        {
            string codePrefix = deckCode.Substring(0, DECK_PREFIX.Length);
            if (codePrefix != DECK_PREFIX)
                return null;

            string noPrefix = deckCode.Substring(DECK_PREFIX.Length, deckCode.Length - DECK_PREFIX.Length);

            noPrefix = noPrefix.Replace('-', '/');
            noPrefix = noPrefix.Replace('_', '=');
            byte[] data = Convert.FromBase64String(noPrefix);

            string decodedString = Encoding.UTF8.GetString(data);

            return data;
        }

        private static DecodedDeck ParseDeck(string deckCode, byte[] deckBytes)
        {
            var nCurrentByteIndex = 0;
            var nTotalBytes = deckBytes.Length;

            var nVersionAndHeroes = deckBytes[nCurrentByteIndex++];
            var version = nVersionAndHeroes >> 4;

            if (CURRENT_VERSION != version && version != 1)
                return null;

            byte checksum = deckBytes[nCurrentByteIndex++];

            int nStringLength = 0;
            if(version > 1)
            {
                nStringLength = deckBytes[nCurrentByteIndex++];
            }
            int nTotalCardBytes = nTotalBytes - nStringLength;

            int computedChecksum = 0;
            for(int i = nCurrentByteIndex; i < nTotalCardBytes; i++)
            {
                computedChecksum += deckBytes[i];
            }

            var masked = (computedChecksum & 0xFF);
            if (checksum != masked)
                return null;

            //read in our hero count (part of the bits are in the version, but we can overflow bits here
            var nNumHeroes = 0;
            if (!ReadVarEncodedUint32((int)nVersionAndHeroes, 3, deckBytes, nCurrentByteIndex, nTotalCardBytes, ref nNumHeroes))
                return null;

            //now read in the heroes
            var nPrevCardBase = 0;
            List<DecodedHero> heroes = new List<DecodedHero>();
            for (int nCurrHero = 0; nCurrHero < nNumHeroes; nCurrHero++)
            {
                var nHeroTurn = 0;
                var nHeroCardID = 0;
                if (!ReadSerializedCard( deckBytes, ref nCurrentByteIndex, nTotalCardBytes, nPrevCardBase, ref nHeroTurn, ref nHeroCardID))
                {
                    return null;
                }

                DecodedHero currentHero = new DecodedHero()
                {
                    Id = nHeroCardID,
                    Turn = nHeroTurn,
                };

                heroes.Add(currentHero);
            }

            List<DecodedCard> cards = new List<DecodedCard>();
            nPrevCardBase = 0;
            while ( nCurrentByteIndex <= nTotalCardBytes )
            {
                int nCardCount = 0;
                int nCardID = 0;
                if (!ReadSerializedCard( deckBytes, ref nCurrentByteIndex, nTotalBytes, nPrevCardBase, ref nCardCount, ref nCardID))
                    return null;

                cards.Add(new DecodedCard()
                {
                    Id = nCardID,
                    Count = nCardCount,
                });
            }

            string name = "";
            if (nCurrentByteIndex <= nTotalBytes)
            {
                byte[] bytes = deckBytes.Skip(deckBytes.Length - nStringLength).ToArray();
                name = System.Text.Encoding.UTF8.GetString(bytes);

                // replace strip_tags with an HTML sanitizer or escaper as needed.
                System.Text.RegularExpressions.Regex regHtml = new System.Text.RegularExpressions.Regex("<[^>]*>");
                string s = regHtml.Replace(name, "");
            }

            return new DecodedDeck()
            {
                Heroes = heroes,
                Cards = cards,
                Name = name,
            };
        }

        private static bool ReadSerializedCard(byte[] data, ref int indexStart, int indexEnd, int nPrevCardBase, ref int nOutCount, ref int nOutCardID)
        {
            //end of the memory block?
            if (indexStart > indexEnd)
                return false;

            //header contains the count (2 bits), a continue flag, and 5 bits of offset data. If we have 11 for the count bits we have the count
            //encoded after the offset
            var nHeader = data[indexStart++];
            var bHasExtendedCount = (( nHeader >> 6 ) == 0x03 );
            //read in the delta, which has 5 bits in the header, then additional bytes while the value is set
            var nCardDelta = 0;

            if (!ReadVarEncodedUint32( nHeader, 5, data, indexStart, indexEnd, ref nCardDelta))
                return false;
            
             nOutCardID = nPrevCardBase + nCardDelta;
            //now parse the count if we have an extended count
            if (bHasExtendedCount)
            {
                if (!ReadVarEncodedUint32(0, 0, data, indexStart, indexEnd, ref nOutCount))
                    return false;
            }
            else
            {
                //the count is just the upper two bits + 1 (since we don't encode zero)
                nOutCount = ( nHeader >> 6 ) +1;
            }
            //update our previous card before we do the remap, since it was encoded without the remap
            nPrevCardBase = nOutCardID;
            return true;
        }

        private static bool ReadVarEncodedUint32(int nBaseValue, int nBaseBits, byte[] data, int indexStart, int indexEnd, ref int outValue)
        {
            outValue = 0;

            int deltaShift = 0;
            if ((nBaseBits == 0 ) || ReadBitsChunk(nBaseValue, nBaseBits, deltaShift, ref outValue))
            {
                deltaShift += nBaseBits;

                while (true)
                {
                    //do we have more room?
                    if (indexStart > indexEnd )
                        return false;

                    //read the bits from this next byte and see if we are done
                    var nNextByte = data[indexStart++];
                    if (!ReadBitsChunk( nNextByte, 7, deltaShift, ref outValue))
                        break;

                    deltaShift += 7;
                }
            }
            return true;
        }

        private static bool ReadBitsChunk(int nChunk, int nNumBits, int nCurrShift,ref int nOutBits)
        {
            var nContinueBit = (1 << nNumBits);
            int nNewBits = nChunk & (nContinueBit - 1 );
            nOutBits |= ( nNewBits << nCurrShift );

            return (nChunk & nContinueBit ) != 0;
        }
    }
}
