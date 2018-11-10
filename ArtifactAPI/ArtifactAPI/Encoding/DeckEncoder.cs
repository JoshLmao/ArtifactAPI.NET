using ArtifactAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Encoding
{
    public class DeckEncoder
    {
        public static int CurrentVersion = 2;
        private static string EncodePrefix = "ADC";
        private static int MaxBytesForVarUint32 = 5;
        private static int HeaderSize = 3;

        public static string Encode(DecodedDeck deck)
        {
            if (deck == null)
                return null;

            byte[] bytes = EncodeBytes(deck);
            if (bytes == null)
                return null;

            string deckCode = EncodeBytesToString(bytes);
            return deckCode;
        }

        private static byte[] EncodeBytes(DecodedDeck deck)
        {
            if (deck == null || deck.Heroes == null || deck.Cards == null)
                return null;

            //Combine hero and normal cards and sort by id
            List<CardId> allCards = deck.Heroes.Select(x => (CardId)x).ToList();
            allCards.AddRange(deck.Cards.Select(x => (CardId)x));
            allCards = SortCardsById(allCards);

            int countHeroes = deck.Heroes.Count;
            byte[] bytes = new byte[256];
            
            //our version and hero count
            int intVersion = CurrentVersion << 4 | ExtractNBitsWithCarry(countHeroes, 3);
            byte version = (byte)intVersion;

            if (!AddByte(ref bytes, version))
                return null;
            
            //the checksum which will be updated at the end
            byte nDummyChecksum = 0;
            var nChecksumByte = bytes.Length;

            if (!AddByte(ref bytes, nDummyChecksum))
                return null;

            // write the name size
            int nameLen = 0;
            string name = "";
            if (!string.IsNullOrEmpty(deck.Name))
            {
                // replace strip_tags() with your own HTML santizer or escaper.
                name = Helper.StripTags(deck.Name);
                int trimLen = name.Length;

                while (trimLen > 63)
                {
                    int amountToTrim = (int)Math.Floor((trimLen - 63.0) / 4.0);
                    amountToTrim = (amountToTrim > 1) ? amountToTrim: 1;
                    name = name.Substring(0, name.Length - amountToTrim);
                    trimLen = name.Length;
                }

                nameLen = name.Length;
            }

            if (!AddByte(ref bytes, (byte)nameLen))
                return null;

            if (!AddRemainingNumberToBuffer(ref countHeroes, 3, bytes))
                return null;

            int unChecksum = 0;
            int prevCardId = 0;

            for (int unCurrHero = 0; unCurrHero < countHeroes; unCurrHero++ )
            {
                CardId card = allCards[unCurrHero];
                DecodedHero casted = (DecodedHero)card;

                if (casted.Turn == 0 )
                    return null;

                if (!AddCardToBuffer(casted.Turn, card.Id - prevCardId, bytes, unChecksum))
                    return null;

                prevCardId = casted.Id;
            }

            //reset our card offset
            prevCardId = 0;

            //now all of the cards
            for (int nCurrCard = countHeroes; nCurrCard < allCards.Count; nCurrCard++)
            {
                //see how many cards we can group together
                CardId card = allCards[nCurrCard];
                DecodedCard castedCard = (DecodedCard)card;

                if (castedCard.Count == 0 )
                    return null;

                if (castedCard.Id <= 0 )
                    return null;

                //record this set of cards, and advance
                if (!AddCardToBuffer(castedCard.Count, castedCard.Id - prevCardId, bytes, unChecksum))
                    return null;

                prevCardId = castedCard.Id;
            }

            // save off the pre string bytes for the checksum
            int preStringByteCount = bytes.Length;

            //write the string
            if(!string.IsNullOrEmpty(name))
            {
                byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
                foreach (byte nameByte in nameBytes)
                {
                    if (!AddByte(ref bytes, nameByte))
                        return null;
                }
            }

            int unFullChecksum = ComputeChecksum(ref bytes, preStringByteCount - HeaderSize);
            int unSmallChecksum = (unFullChecksum & 0x0FF );
            bytes[nChecksumByte] = (byte)unSmallChecksum;
            return bytes;
        }

        private static int ComputeChecksum(ref byte[] bytes, int unNumBytes)
        {
            int unChecksum = 0;
            for (int unAddCheck = HeaderSize; unAddCheck < unNumBytes + HeaderSize; unAddCheck++ )
            {
                var b= bytes[unAddCheck];
                unChecksum += b;
            }
            return unChecksum;
        }

        private static bool AddCardToBuffer(int unCount, int unValue, byte[] bytes, int unCheckSum)
        {
            //this shouldn't ever be the case
            if (unCount == 0)
                return false;

            int countBytesStart = bytes.Length;
            
            //determine our count. We can only store 2 bits, and we know the value is at least one, so we can encode values 1-5. However, we set both bits to indicate an 
            //extended count encoding
            byte knFirstByteMaxCount = 0x03;
            bool bExtendedCount = ( unCount - 1 ) >= knFirstByteMaxCount;
            //determine our first byte, which contains our count, a continue flag, and the first few bits of our value
            int unFirstByteCount = bExtendedCount ? knFirstByteMaxCount: /*( uint8 )*/( unCount - 1 );
            int unFirstByte = (unFirstByteCount << 6);
            unFirstByte |= ExtractNBitsWithCarry( unValue, 5);

            if (!AddByte(ref bytes, (byte)unFirstByte))
                return false;

            //now continue writing out the rest of the number with a carry flag
            if (!AddRemainingNumberToBuffer(ref unValue, 5, bytes))
                return false;

            //now if we overflowed on the count, encode the remaining count
            if (bExtendedCount)
            {
                if (!AddRemainingNumberToBuffer(ref unCount, 0, bytes))
                    return false;
            }

            int countBytesEnd = bytes.Length;
            if (countBytesEnd - countBytesStart > 11 )
            {
                //something went horribly wrong
                return false;
            }
            return true;
        }

        private static byte ExtractNBitsWithCarry(int value, int numBits)
        {
            int unLimitBit = 1 << numBits;
            int unResult = (value & ( unLimitBit - 1 ) );
            if (value >= unLimitBit)
            {
                unResult |= unLimitBit;
            }
            return (byte)unResult;
        }

        private static bool AddByte(ref byte[] bytes, byte b)
        {
            if (b > 255)
                return false;

            Array.Resize(ref bytes, bytes.Length + 1);
            bytes[bytes.GetUpperBound(0)] = b;

            return true;
        }

        private static bool AddRemainingNumberToBuffer(ref int unValue, int unAlreadyWrittenBits, byte[] bytes)
        {
            unValue = unValue >> unAlreadyWrittenBits;
            int unNumBytes = 0;
            while (unValue > 0)
            {
                var unNextByte = ExtractNBitsWithCarry(unValue, 7);
                unValue >>= 7;

                if (!AddByte(ref bytes, unNextByte))
                        return false;

                unNumBytes++;
            }
            return true;
        }

        private static string EncodeBytesToString(byte[] bytes)
        {
            int byteCount = bytes.Length;

            //if we have an empty buffer, just return
            if (byteCount == 0 )
                return null;

            //byte[] packed = pack("C*", bytes);
            string encoded = System.Text.Encoding.UTF8.GetString(bytes);
            string deck_string = EncodePrefix + encoded;

            deck_string = deck_string.Replace('-', '/');
            deck_string = deck_string.Replace('_', '=');
            string fixedString = deck_string;

            return fixedString;
        }

        private static List<CardId> SortCardsById(List<CardId> cards)
        {
            return null;
        }
    }
}
