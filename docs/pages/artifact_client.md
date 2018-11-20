---
layout: default
---

#### **Quick Navigate:**

* [Encoding](#Encoding)
* [Decoding](#Decoding)

# Artifact Client

Below lists all methods. Please note, only the synchronous methods are listed below. Async methods will do exactly the same (but asynchronous)

### GetAllCards()

> Used to get all cards currently available in Artifact
>
> Returns: **List\<Card\>** cards
<br/>
> A list of cards

```csharp
//Get all cards from the API
List<Card> allCards = client.GetAllCards();
```

### GetCardSet(_string_ cardSetId)

> Gets a specific set of cards from the API. Currently only supports the codes "00" & "01"
> 
> Returns: **CardSet** cardSet 
<br/>
> The card set object which contains all cards of the set, version & set info <br/> Can return null

```csharp
//Get the 1st set of cards from the API
CardSet cardSet = client.GetCardSet("00");
```

### GetCard(_int_ cardId)

> Gets a specific card by it's Id
> 
> Returns: **Card** card
<br/>
> The required card with stats, art work urls, etc <br/> Can return null

```csharp
//Get the Ventriloquy card from it's id
CardSet cardSet = client.GetCard(10418);
```

### GetCard(_string_ cardName)

> Gets a specific card by it's name. Casing is ignored by this method, just in case
> 
> Returns: **Card** cardSet 
<br/>
> The card set object which contains all cards of the set, version & set info <br/> Can return null

```csharp
//Get the Ventriloquy card from it's name
CardSet cardSet = client.GetCard("venTRIiloquy");
```

### GetCardsFromDecodedDeck(_DecodedDeck_ decodedDeck)

> Converts the _DecodedDeck_ object to a _Deck_ object with all card details
> 
> Returns: **Deck** deck
<br/>
> The _Deck_ object with all card details and info <br/> Can return null

```csharp
//Decodes the deck code and get's the deck with info, card art and stats
DecodedDeck decodedDeck = client.DecodeDeck("ADCJQUQI30zuwEYg2ABeF1Bu94BmWIBTEkLtAKlAZakAYmHh0JsdWUvUmVkIEV4YW1wbGU_");
Deck deck = client.GetCard(decodedDeck);
```

### GetCardArtUrl(_int_ cardId, _ArtType_ artType, _Language_ language = Language.English)

> Gets the wanted url of the type of card art from the card id, including the correct language
> 
> Returns: **string** url
<br/>
> The url of the card art <br/> Can return null or the English version of the card it hasn't been translated

```csharp
// Gets the Simplified Chinese card art for Ventriloquy card from it's id
string artUrl = client.GetCardArtUrl(10418, ArtType.Large, Language.ChineseSimplified);
```

### GetCardArtUrl(_string_ cardName, _ArtType_ artType, _Language_ language = Language.English)

> Gets the wanted url of the type of card art from the card name, including the correct language
> 
> Returns: **string** url
<br/>
> The url of the card art <br/> Can return null or the English version of the card it hasn't been translated

```csharp
// Gets the Simplified Chinese card art for Ventriloquy card from it's name
string artUrl = client.GetCardArtUrl("Ventriloquy", ArtType.Large, Language.ChineseSimplified);
```

* * *

# Encoding

### EncodeDeck(_DecodedDeck_ decodedDeck)

> Encodes a deck from a built DecodedDeck object.
>
> Returns: **_string** encodedDeckString
> The encoded string of the deck. Can be viewed on playartifact.com/d/**{encodedDeckString}**
> Can return null

```csharp
//Create a new DecodedDeck, populate and encode it
DecodedDeck decodedDeck = new DecodedDeck();
/* Populate decodedDeck with card id's and amounts/turns */
string encodedString = client.EncodeDeck(decodedDeck);
```

### EncodeDeck(_Deck_ deck)

> Encodes a deck from a manually built Deck object
>
> Returns: **string** encodedDeckString
<br/>
> The encoded string of the deck. Can be viewed on playartifact.com/d/**{encodedDeckString}** <br/> Can return null

```csharp
//Create a new deck, populate and encode it
Deck myDeck = new Deck();
/* Populate Deck object with cards */
string encodedString = client.EncodeDeck(myDeck);
```

* * *

# Decoding

#### DecodeDeck(string encodedDeckString)

> Decodes a deck from an encoded string.
>
> Returns: **DecodedDeck** decodedDeck
<br/>
> The DecodeDeck object containing deck name, all cards & items <br/> Can return null

```csharp
//Create a new deck, populate and encode it
string encodedDeckCode = "ADCJQUQI30zuwEYg2ABeF1Bu94BmWIBTEkLtAKlAZakAYmHh0JsdWUvUmVkIEV4YW1wbGU_";
DecodedDeck decodedDeck = client.DecodeDeck(encodedDeckCode);
```

* * *

[back](./../)
