Hoe gaan we een nft blockchain bouwen voor liedjes

ten eerste, wat zijn de use cases:

- als iemand hetzelfde liedje post, dan moet dat gedetecteerd worden 
-> ganse blockchain overlopen opnieuw voor te valideren of er overspending is gebeurd
- tis eig niet logisch maar je maakt een gecentraliseerde bot met dus 1 public private key dat de blockchain updatet, maar op zich kunnen andere mensen joinen
    ~ dit is meer voor een REST server en gecentraliseerd: de bot heeft credentials voor met een api te babbelen ofzo en iedereen heeft read rechten

- soorten transacties:
    - claim song -> elke public key kan dat doen? tis gedecentraliseerd
        ~ iedereen kan een assymetric key hebben, als ik liedje post in discord dan zou ik ook een signature kunnen posten dat het van mij is
        maar dit is niet gebruiksvriendelijk, iedereen wilt gewoon urls plakkenh
        -> beter: discord bot ownt alle liedjes om mee te beginnen, hij transfered die naar andere mensen -> dan heb je enkel public key nodig, iedereen moet er een registreren in discord
    - transfer claim -> public key van ontvanger, private key verzender -> discord bot is de initiele verzender dus niemand anders moet signatures maken
    - get dogecoin


- mensen registreren dus liedjes, hoe ga je dit op de blockchain steken:
-> iedereen kan eender wanneer de blockchain updaten, via proof of work enzo, zo zou je uiteindelijk wel een oplossing hebben
is es geen slimmere manier?
- wat als block is: previous hash, 1 song met signature, new hash
    - in het begin zal enkel de bot liedjes kunnen uitdelen want hij is owner van alle liedjes, hij kan dan inmemory block afwerken onmiddellijk en posten
        - niemand anders kan dan liedjes toevoegen want ze ontbreken de signature 
    - later: ik transfer liedje naar angelo, dus ik kan dan hetzelfde zeggen -> enkel ik maak block
    - zitten er daar gevaren achter? ik denk het niet, op zich kan maar 1 iemand owner tegelijk zijn
    voor en nadelen: op zich is er maar 1 liedje en is er geen gevaar voor double spending?



hoe bouwen:
- discord bot is een client die liedjes voor mensen registreert -> transer(song, mysign, user) -> dit versturen over het web evt
- kan iemand anders ook liedjes registeren voor de eerste keer? wel op zich mag iedereen dat doen ZOLANG het liedje niet eerder werd geregistreerd
-> serverlogica kan alles valideren? maar op zich is er geen 'server', enkel een gehoste blockchain toch?

je kan een sdk maken die anderen ook kunnen gebruiken ~ die bevat alle validatielogica enzo, vanwaar haalt die de chain, die moet je ergens registreren voor 'zijn' versie

hoe maak je een rating systeem om te zeggen dat songs goed zijn enzo of niet
- in naam van discord bot mss? die zegt dat die andere public key dat heeft geliked, dat is toch niet te vertrouwen want dan kan iedereen dat doen
- tenzij je ook transactie maakt: gedelegeerde trusted owners toevoegen, die mogen signen in naam van anderen 
* zo moet niemand sdk hebben maar is het wel niet veilig meer
* of discord bot die private keys en public keys bijhoudt, het genereert keys voor een gebruiker -> databank met public private key en owner





terug naar use cases:
- nu kunnen mensen dus liedjes hebben en die kunnen ook getransfered worden, er is ownership via de blockchain
- bot kan melding geven van wie liedjes zijn en error gooien
- zijn er nog dingen die je kan doen? rating systeem is dom, nee tis eig puur een nft ding zou ik zeggen, met ownership enzo, je kan ook een coin minten en zeggen dat coole liedjes veel coins hebben, een soort van scoresysteem voor uw blockchain is coole
- hoe implementeer je dat? vanwaar komen de coins, mss leuker dan positive coins is negative coins maar hoe maak je




ARCHITECTUUR:
- discord bot is een client voor meereder dingen:
  - het moet de blockchain kunnen aanspreken en updaten -> client logica voor dat te updaten en blocks te publishen
  - het moet users bijhouden van de server -> nood aan db die discordid koppelt aan pubkey
  - het is eigenlijk een laag bovenop de blockchain


lagen:
- het laagste is het model: songs en owners, owner is gewoon een public key op laagste niveau, song is een spotify id en ook naam enzo
- uw persistence laag is een blockchain, modellaag moet dit niet kennen
- je hebt operaties op de modellaag ~ applicatielaag: fetchen van songs en toevoegen ervan, het is aan de databanklaag om dit te implementeren?
  - evt ook get all songs by owner enzo
  - validatielogica hier toevoegen: song mag maar van 1 iemand zijn
  - genesisblock creation : speciaal record dat maar 1 keer bestaat: de owner van alle onbekende songs
  - noodzakelijk: unieke transactie: owner of all unknown songs ~ CreateGenesisBlock ofzo

dat is uw BLOCKCHAIN project, daarnaast heb je de discord bot:
- gebruikt de applicatielaag van blockchain voor toe te voegen / info te retrieven
- heeft een eigen modellaag voor ZIJN opslag: owners bevatten meer info, naast pubkey ook een discord id enzo
- heeft ook een eigen dblaag voor users van discord op te slaan voor linking aan blockchain
- moet ook een eigen applicatielaag hebben? of niet???
  - moet extra commandos hebben voor users te fetchen en up te daten, 
- ui laag: commandos roepen dan applicatielaag op


hoe hangen applicatielagen en storage enzo samen?
in applicatielaag injecteer je dbset voor aan databank te kunnen

* blockchain app applicatielaag -> injecteer 'custom dbset' met operaties RetrieveSong, TryAddSong -> eigen code moet dit implementeren
  * gebruik hiervoor geen ef maar wel het repository concept?
* bot: injecteer dbset voor userdb, deze applicatielaag moet requests kunnen uitvoeren van blockchainlaag -> interface injecteren?