### Requisiti:
- Robot controllato remotamente tramite dispositivo esterno presente localmente.

### Analisi dei Requisiti
__Presente localmente__: il dispositivo di controllo sarà generalmente presente nella stessa stanza del robot o al più in una stanza adiacente.
__Robot__: [[Tutorial.pdf|Documentation]]

### Analisi del Problema
- Connessione:
	La connessione deve essere tale da permettere ai due dispositivi di scambiarsi messaggi contenenti i comandi. I due dispositivi saranno presenti nello stesso luogo non abbiamo bisogno di connessione remota attraverso la rete.
- Codice disomogeneo:
	Il robot sfrutta delle librerie python, il controller può essere scritto in qualunque linguaggio e con qualsiasi supporto, necessario per la connessione trovare un modo Code-Independent di scambiare i messaggi.
- Che tipo di messaggi si scambiano?
	Ci interessa avere risposta ai messaggi inviati?
- Protocollo di scambio messaggi:
	I messaggi scambiati tra i due dovranno condividere uno stesso protocollo di codifica dell'informazione. Messaggi malfomati verrano scartati automaticamente.

### Progettazione

![[Sprint1Schema.png]]

L'architettura del robot è composta da una serie di Attori, un file di supporto Config.py avrà i riferimenti a tutti gli attori del contesto.
```python
#this file contains the reference of all the actors of the system 
#default value is None, main will change it at start

actorCore_ref = None
actorArmControl_ref = None
actorMovementControl_ref = None
```
Per la creazione di un attore in Python ci appoggiamo alla libreria [pykka](https://pykka.readthedocs.io/en/stable/quickstart/).

I messaggi scambiati saranno di tipo Fire and Forget, non ci interessa avere una risposta dal robot a seguito dell'invio di un comando.

Per comunicare con il robot dobbiamo scrivere le nostre librerie:
```python
#lib per il movimento delle ruote
```

```python
#libreria per il movimento del braccio
```

Per la connessione possiamo accontentarci di una socket tcp attraverso la quale verrano scambiati i messaggi caratterizzati dal il seguente formato:
`ID:00n;TYPE:StringDiLunghezzaVariabile;BODY:String di lunghezza variabile e spazi`
	- ID è un numero intero crescente a partire a 001 (000 è tenuto per i test)
	- TYPE può essere uno dei seguenti:
		- Movement
		- Robot_Arm
		- Start
		- Stop
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
		- Start: will not have any body
		- Stop: will not have any body

Console: Tenendo in considerazione il progetto finale, già da questo primo Sprint modelliamo la console tramite Unity.




