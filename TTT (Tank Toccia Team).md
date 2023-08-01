### Requisiti:
- Robot controllato remotamente tramite dispositivo esterno presente localmente.
- Il robot deve avere capacità di acquisizione e trasmissione video.
- Capacità di riconoscimento di oggetti specifici su richiesta dell'utente. La risposta dovrà essere restituita all'utente.
- Riconosciuto il nemico deve partire un minigioco.
- La pinza del robot mostrerà fisicamente la buona riuscita di un attacco.


### Analisi dei requisiti
- Presente localmente: il dispositivo di controllo sarà generalmente presente nella stessa stanza del robot o al più in una stanza adiacente.
- I dati video vengono ripresi mediante telecamera presente nel robot e devono essere trasmessi al dispositivo adibito al controllo per permettere all'utente di visualizzare cosa si trova di fronte al robot.
- In seguito all'invio di un comando specifico un'AI dovrà controllare se in quel momento di fronte al robot è presente uno di un set predeterminato di oggetti.
- Lato client, in seguito al riconoscimento andato a buon fine di uno degli oggetti sopra menzionati, sarà possibile per l'utente giocare ad un minigioco nel quale esso dovrà scontrarsi con un nemico diverso a seconda dell'oggetto riconosciuto. Durante il minigioco la pinza del robot dovrà azionarsi in seguito alla buona riuscita di un'azione del giocatore.


### Analisi del problema
- Quali oggetti verranno usati per il riconoscimento dei nemici?
- Quanto è grande il traffico dati della telecamera?
- in che modo verranno trasmessi i dati video?
- Comandi e Video vengono trasmessi in due stream diversi o nello stesso? (probabilmente nello stesso)
- Dopo la ricezione di un comando di controllo la AI da dove prende l'immagine? La invia core o chiede direttamente a telecamera?
- Chi gestisce i dati della game logic? Unit ha un supporto per database o li mettiamo embedded? Si potrebbe anche fare...
- Come funziona il gioco? Che dati usa? Cosa permette di fare all'utente? Qual è il goal del player?
- Come viene lanciato il gioco? Core che dopo la response dell'AI lancia un nuovo componente? O componente sempre attivo ma non interagibile finchè non arriva la response giusta? In ogni caso qualcuno deve far "partire il gioco" dopo la ricezione del messaggio (probabilmente ApplCore)


### Progettazione
- File di configurazione degli attori lato robot Config.py
- Protocollo di comunicazione: 
	"ID:00n;TYPE:StringaDiLunghezzaVariabile;BODY:Stringa di lunghezza variabile con spazi"
	- ID è un numero intero crescente a partire a 001 (000 è tenuto per i test)
	- TYPE può essere uno dei seguenti:
		- Movement
		- Robot_Arm
		- AI_Recognition
		- Start
		- Disconnect
		- Terminate
	- BODY normalmente ricade nei seguenti casi a seconda del TYPE
		- Movement:
			- Forward Start
			- Forward Stop
			- Backward Start
			- Backward Stop
			- Rotate_Left Start
			- Rotate_Left Stop
			- Rotate_Right Start
			- Rotate_Right Stop
		- Robot_Arm:
			- Up
			- Down
			- Grab
			- Release
		- AI_Recognition:
			- Identify_Current      (si limita a cercare ciò che è inquadrato al momento)
		- Start: will have address and body of client in the format 192.168.n.n:port.
		- Disconnect: will not have any body, represent the client stopping the connection
		- Terminate: will not have any body, is used for termination of the Actors

### Suddivisione Sprint
- [[Sprint 1]]: Console Remota e Robot funzionante con solo le funzionalità di base (movimento ruote e braccio)
- [[Sprint 2]]: introduzione stream video (e gioco?)
- [[Sprint 3]]: introduzione AI