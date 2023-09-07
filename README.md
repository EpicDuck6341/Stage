NOLAI Project  

 

Gebruik maken van spraakherkenning om de uitspraak van gebruiker op te vangen. 

Gebruik maken van pose detectie om na te gaan of de gebruiker wijst/kijkt naar het goede voorwerp. 

Achterliggende AI/Geavanceerde Algoritme wordt gebruikt voor de spraakherkenning en bij de terugkoppeling/verbetering terug te geven aan de gebruiker. (Voorkeur ligt op AI) 

Gebruikers worden onthouden in een database door middel van een code of iets dergelijks. Hierdoor kunnen ze op een later tijdstip het scenario opstarten op het punt dat ze geëindigd zijn. 

Als gebruikers goedt of slecht scoren op een vraag wordt aan de hand daarvan hun taalniveau bijgesteld. Aan de hand van het taalniveau kan een situatie/scenario sneller of langzamer klaar zijn. 

Eventuele mogelijkheid om te communiceren met gebruikers in andere talen. 

Verschillende omgevingen/scenario's waarin gebruikers kunnen leren. 

Eventuele vrijheid van beweging voor gebruiker. Hierdoor kan het scenario niet altijd chronologisch is of volgens een vooraf gewild pad. 

Mogelijk lange scenario's maken die gesplitst worden in meerdere leer sessies van een aantal minuten. 

Onderzoeken wat het effect van VR is op gebruikers en hoe lang dit gebruikt kan worden per sessie. 

Een manier om voor de docent te zien wat de gebruiker heeft gezien/gedaan terwijl er ook rekening gehouden wordt met privacy wetten. 

Docent kan buitenaf live meekijken met de sessie van de gebruiker.  

Er moet van buitenaf dingen geregeld kunnen worden in het programma. 

Scenario met een realistische stijl en omgeving om een echte situatie zo precies mogelijk te representeren. 

Gebruik maken van 3DoF. Hierdoor zijn de gebruikers statisch en moet er gebruik gemaakt worden van bijvoorbeeld een controller om rond te bewegen. De scene zou mogelijk aangepast moeten worden zodat 3DoF niet voelt als een limiterende factor in het ontdekken van de omgeving.  

Eventueel kan de sessie onderbroken worden als de gebruiker opstaat of zich verplaatst. Dit kan werken om ongevallen te voorkomen en de immersie te breken indien er paniek ontstaat. 

 

 

 

 

 

Spraakherkenning 

 

Inleiding 

Bij het gebruik maken van Spraakherkenning zijn er twee voor de hand liggende mogelijkheden om dit te implementeren in een project, namelijk On-device (lokaal) of Online.  Hieronder zullen voor- en nadelen van beide opties genoemd worden.  

 

On-device 

	Voordelen 

Er is geen afhankelijkheid van internet en een online 3rd party die Spraakherkenning afhandelt. 

Meeste on-device Spraakherkenning is volledig gratis en open source. 

Bij open source software is het mogelijk om het gekozen model verder te finetunen om beter ingesteld te zijn op een soort gebruiker. 

Aangezien audiofiles nooit worden gedeeld met externe paartijen wordt de privacy van de gebruiker beter bewaakt. 

Omdat er geen internet gebruikt wordt, is er geen wachttijd voor het versturen en ontvangen van het audiobestand. 

Met open source software is er een mogelijkheid om mee te kijken in het proces van Spraakherkenning en hier eventuele handige data uit te halen. 

Er kan gekozen worden voor een specifiek model dat afgestemd is op de benodigdheden van het project. 

 	Nadelen 

Bij zeer accuraat model groot is er de vereiste van een redelijk krachtige machine. Een kleiner model kan gekozen worden ten koste van de accuraatheid van Spraakherkenning. 

Om de gebruiker niet lang te laten wachten moet de audio snel getranscribeerd worden, dit is afhankelijk van de snelheid van apparaat. 

Er moet meer tijd geïnvesteerd worden in het werkend krijgen van Spraakherkenning. 

Het moet zelf onderhouden worden, denk hierbij aan bijvoorbeeld nieuwe versies van de software. 

De opslag van het apparaat kan een limiterende factor zijn, alhoewel dit verholpen kan worden door bijvoorbeeld de audio te verwijderen naar een bepaalde tijd. 

 

Online-service  

Voordelen 

Online-services hebben veel data tot hun beschikking, hierdoor zullen de gebruikte modellen over het algemeen accurater zijn. 

Een online-service kan heel snel geïmplementeerd worden in het project. 

Er is geen sterke hardware vereist. 

Alle software en model updates worden afgehandeld door de leverancier. 

 	Nadelen 

Onlineservices brengen kosten met zich mee. 

De applicatie is compleet afhankelijk van internetverbinding en daarbij ook upload en download snelheid. 

Er is vaak geen toegang tot data anders dan de getranscribeerde tekst.  

Conclusie 

De keuze tussen welk van de twee Spraakherkenningen methodes geïmplementeerd moet worden is altijd afhankelijk van de eisen van het project. Wegens het werken met kinderen speelt privacy een grote rol, dit betekent dat er meer geleund zal worden naar de kant van on-device. 

Gebruik maken van on-device betekend wel dat er een redelijk krachtige machine aanwezig moet zijn wil er geen compromis gemaakt worden voor de accuraatheid van het gekozen model. Wel zal er geen toegevoegde vertraging zijn wegens het niet afhankelijk zijn van internet.  

Verder, als de voorkeur hierop ligt, kan het model gefinetuned worden om beter om te gaan met een bepaald vocabulaire. Bij dit proces komt veel tijd kijken, maar in ruil daarvoor krijg je een model dat is afgestemd om de behoefte van de klant. 

Alhoewel on-device implementatie wat uitdagingen met zich meebrengt zijn de voordelen, zoals kosten en privacy, het zeker waard om inspanning en moeite voor te leveren. 

 

Sources 

https://www.speechly.com/blog/on-device-vs-cloud-speech-recognition-comparing-privacy-cost-and-accuracy 

https://www.speechly.com/blog/when-to-run-speech-to-text-on-device-or-on-premise-vs-in-the-cloud 

https://vivoka.com/embedded-cloud-voice-technology 

 

 

Spraakherkenning opties 

On-device: 

Whisper: 

Voordelen: 

Whisper biedt veel verschillende modellen, in termen van grootte en dus ook accuraatheid.  

Het biedt opties om op low-level te werken met het programma en zo ook data te verkrijgen. 

De Word Error Rate (WER) van Nederlands light als een van de laagste in het model. 

Het wordt constant verbeterd en is gemakkelijk te op te schalen naar de vernieuwde versie. 

Er is geen restrictie op grootte van het bestand. 

Nadelen: 

Installeren kan een langdurig proces zijn. 

Het is niet even altijd snel op hele lange audio segmenten als bij online-services. 

  

Massively Multilingual Speech (MMS): 

  

Voordelen: 

Ondersteuning voor Meerdere Talen: MMS-modellen zijn ontworpen om meerdere talen te ondersteunen, wat het geschikt maakt voor internationale toepassingen. 

MMS is getraind om meer dan 1000 talen, en is daarmee zeer veelzijdig. 

MMS is het meest uitgebreide model dat er bestaat. 

Nadelen: 

MMS biedt niet veel alternatieve modellen zoals Whisper dat doet, dus zit standaard vast aan redelijk zware modellen. 

Aangezien het project een specifiek toepassing heeft kan het zijn dat MMS simpelweg overgekwalificeerd is en te veel meebrengt wat ten koste gaat van performance. 

Vaak zijn er niet veel verschillende sprekers gebruikt voor een taal wat kan leiden naar niet goed kunnen begrijpen van mensen met een andere uitspraak.  

  

 

 

 

 

 

Online-services : 

  

Google Cloud Speech-to-Text: 

  

Voordelen: 

Google Cloud Speech-to-Text biedt ondersteuning voor een breed scala aan talen en dialecten, waardoor het geschikt is voor internationale toepassingen. 

 De API ondersteunt streaming audio, wat nuttig kan zijn voor real-time toepassingen zoals live ondertiteling of spraak gestuurde commando's. 

Er kan eenvoudig gebruikmaken van andere Google Cloud-services om geavanceerde spraakherkenningsoplossingen te bouwen. 

Zeer klantvriendelijk met een simpele interface. 

 

Nadelen: 

Kosten: Het gebruik van Google Cloud-services kan kostbaar zijn, vooral als er grote hoeveelheden spraakgegevens verwerkt worden. 

Er is online vermeld dat het niet altijd even goede documentatie en weinig flexibiliteit. 

(Uit een paar persoonlijke tests is gebleken dat het niet altijd even soepel loopt bij Nederland, moet nog verder naar gekeken worden.) 

 

Microsoft Azure Cognitive Services - Speech Service:  

Voordelen: 

Azure's cloudservice kan een groot aantal verzoeken verwerken. 

Het integreert goed met andere Azure-services. 

Azure biedt gratis aan audio in de maand, daarna kost het geld. 

Nadelen: 

Azure's base model is vaak niet accuraat genoeg, dit zorgt ervoor dat het model nog verder getraind moet worden wat veel tijd gaat kosten. 

 

 

 

Sources: 

https://github.com/openai/whisper 

https://analyticsindiamag.com/openais-whisper-is-revolutionary-but-little-flawed/ 

https://about.fb.com/news/2023/05/ai-massively-multilingual-speech-technology/ 

https://huggingface.co/docs/transformers/model_doc/mms 

https://ai.meta.com/blog/multilingual-model-speech-recognition/#:~:text=A%20limitation%20of%20the%20Massively,often%20only%20a%20single%20speaker. 

https://www.g2.com/products/google-cloud-speech-to-text/reviews#survey-response-8547494 

https://medium.com/version-1/analysis-of-automatic-speech-recognition-tools-10696f131910 

 

 

--Whispp (Speech recognition) 

 

 

 
