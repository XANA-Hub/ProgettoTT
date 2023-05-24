### Requisiti:
Il robot deve avere capacità di acquisizione e trasmissione video.


### Analisi del problema:
- Per la trasmissione video che tipo di protocollo usiamo? UDP
- Trasferiamo i dati attraverso la stessa socket che già abbiamo o ne creiamo una nuova? Ne creiamo una nuova
- Gestione della socket: 
	- come viene aperta? Se ne deve occupare il video handler ma la logica di basso livello deve essere nascosta. (invocazione di funzioni di RobotSocket a partire dal VideoActor)
	- come viene chiusa? Cosa succede se un client va in crash? Chi la chiude?
	Udp non riceve risposta, non è in grado autonomamente di rendersi conto che la connessione è stata chiusa.
- Encoding del flusso video?
- Gestione degli indirizzi? Embedded

### Progettazione
