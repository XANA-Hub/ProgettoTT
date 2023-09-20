using UnityEngine;

public class Fighter : MonoBehaviour {

    // Satistiche base
    [Header("Statistiche base del combattente")]
    public FighterData data;

    // Livello
    protected int level = 1;

    // Statistiche attuali
    protected int currentAttack;
    protected int currentDefense;
    protected int currentHP;
    protected int currentSpeed;
    protected int currentHeal;


    // Statistiche massime
    protected int maxCurrentAttack;
    protected int maxCurrentDefense;
    protected int maxCurrentHP;
    protected int maxCurrentSpeed;
    protected int maxCurrentHeal;

    protected NatureBonus currentNatureBonus;

    // Metodo per calcolare le statistiche effettive in base al livello
    // TODOOOOO
    //
    protected void InitializeStats() {

        // Trovo il bonus natura del fighter
        currentNatureBonus = GetNatureBonus(data.preferredNatureBonus);

        // Calcolo le varie stat del fighter in base al bonus natura scelto
        currentAttack = CalculateCurrentStatWithoutBonus(data.baseAttack, level);
        currentDefense = CalculateCurrentStatWithoutBonus(data.baseDefense, level);
        currentHP = CalculateCurrentStatWithoutBonus(data.baseHP, level);
        currentSpeed = CalculateCurrentStatWithoutBonus(data.baseSpeed, level);
        currentHeal = CalculateCurrentStatWithoutBonus(data.baseHeal, level);

        // Aggiungo il nature bonus
        switch(currentNatureBonus) {
            case NatureBonus.HP:
                currentHP += Mathf.RoundToInt(currentHP * data.natureBonusPercentage); 
                break;
            case NatureBonus.ATTACK:
                currentAttack += Mathf.RoundToInt(currentAttack * data.natureBonusPercentage);
                break;
            case NatureBonus.DEFENSE:
                currentDefense += Mathf.RoundToInt(currentDefense * data.natureBonusPercentage); 
                break;
            case NatureBonus.SPEED:
                currentSpeed += Mathf.RoundToInt(currentSpeed * data.natureBonusPercentage);
                break;
            case NatureBonus.HEAL:
                currentHeal += Mathf.RoundToInt(currentHeal * data.natureBonusPercentage);
                break;
        }

        // Imposto le stat massime
        maxCurrentAttack = currentAttack;
        maxCurrentDefense = currentDefense;
        maxCurrentHP = currentHP;
        maxCurrentSpeed = currentSpeed;
        maxCurrentHeal = currentHeal;

    }

    // Calcola le statistiche senza il bonus natura
    private int CalculateCurrentStatWithoutBonus(int baseStat, int level) {
        float stat = (float)baseStat * (1 + ((float)(level - 1) / 100f));
        return Mathf.RoundToInt(stat); // Arrotonda il risultato all'intero più vicino
    }

    // Permette di dare il NatureBonus in base a quello preferito
    private NatureBonus GetNatureBonus(NatureBonus preferredNatureBonus) {
        
        // Restituisco un Bonus casuale se non è stato specificato il preferito
        if (preferredNatureBonus == NatureBonus.RANDOM) {
            return GetRandomNatureBonus();
        }

        // Probabilità di scegliere il bonus natura preferito (es. 70%)
        float preferredNatureProbability = this.data.preferredNatureProbability;

        // Genera un numero casuale tra 0 e 1
        float randomValue = Random.Range(0f, 1f);

        // Se il valore casuale è inferiore alla probabilità del bonus preferito, scegli il bonus preferito
        if (randomValue < preferredNatureProbability) {
            return preferredNatureBonus;
        }
        else {

            // Continuo a generare finché non viene scelto un valore diverso da "RANDOM" e da quello PREFERITO
            NatureBonus randomNatureBonus;
            
            do {
                randomNatureBonus = (NatureBonus)Random.Range(0, System.Enum.GetValues(typeof(NatureBonus)).Length);
            } while (randomNatureBonus == preferredNatureBonus || randomNatureBonus == NatureBonus.RANDOM);
            
            return randomNatureBonus;

        }
    }

    
    // Da il Nature Bonus a caso, viene usato soltanto nel caso in cui il fighter ha come valore natura preferito "RANDOM"
    private NatureBonus GetRandomNatureBonus() {

        NatureBonus randomNatureBonus;

        // Continuo a generare finché non viene scelto un valore diverso da "RANDOM"
        do {
            randomNatureBonus = (NatureBonus)Random.Range(0, System.Enum.GetValues(typeof(NatureBonus)).Length);
        } while (randomNatureBonus == NatureBonus.RANDOM);
        
        return randomNatureBonus;

    }


    // Metodo per subire danno
    public bool takeDamage(int dmgAmount) {

        currentHP -= dmgAmount;

        // Morto
        if(currentHP <= 0) {
            currentHP = 0;
            return true;
        }
        else {
            return false;
        }
    }

    public void Heal(int healAmount) {

        // Se gli HP sono maggiori di quelli massimi attuali
        if(currentHP+healAmount > maxCurrentHP) { 
            currentHP = maxCurrentHP;
        }else{
            currentHP += healAmount;
        }

    }

    //
    // Getters stat correnti
    //

    // Metodo per ottenere il livello del combattente
    public int getLevel() {
        return this.level;
    }

    // Metodo per ottenere l'attacco corrente del combattente
    public int getCurrentAttack() {
        return this.currentAttack;
    }

    // Metodo per ottenere la difesa corrente del combattente
    public int getCurrentDefense() {
        return this.currentDefense;
    }

    // Metodo per ottenere gli HP correnti del combattente
    public int getCurrentHP() {
        return this.currentHP;
    }

    public int getCurrentSpeed() {
        return this.currentSpeed;
    }

    public int getCurrentHeal() {
        return this.currentHeal;
    }

    public string getCurrentNatureBonusAsString() {
        return System.Enum.GetName(typeof(NatureBonus), currentNatureBonus);
    }
    public NatureBonus getCurrentNatureBonus() {
        return this.currentNatureBonus;
    }


    //
    // Getters stat massime
    //

    // Metodo per ottenere l'attacco corrente del combattente
    public int getMaxCurrentAttack() {
        return this.maxCurrentAttack;
    }

    // Metodo per ottenere la difesa corrente del combattente
    public int getMaxCurrentDefense() {
        return this.maxCurrentDefense;
    }

    // Metodo per ottenere gli HP correnti del combattente
    public int getMaxCurrentHP() {
        return this.maxCurrentHP;
    }

    public int getMaxCurrentSpeed() {
        return this.maxCurrentSpeed;
    }


    //
    // Setters current stats
    //

    // Setter per currentAttack
    public void SetCurrentAttack(int value) {
        currentAttack = value;
    }

    // Setter per currentDefense
    public void SetCurrentDefense(int value) {
        currentDefense = value;
    }

    // Setter per currentHP
    public void SetCurrentHP(int value) {
        currentHP = value;
    }

    // Setter per currentSpeed
    public void SetCurrentSpeed(int value) {
        currentSpeed = value;
    }

    // Setter per currentHeal
    public void SetCurrentHeal(int value) {
        currentHeal = value;
    }

    //
    // Setter stat max
    //

    // Setter per maxCurrentAttack
    public void SetMaxCurrentAttack(int value)
    {
        maxCurrentAttack = value;
    }

    // Setter per maxCurrentDefense
    public void SetMaxCurrentDefense(int value)
    {
        maxCurrentDefense = value;
    }

    // Setter per maxCurrentHP
    public void SetMaxCurrentHP(int value)
    {
        maxCurrentHP = value;
    }

    // Setter per maxCurrentSpeed
    public void SetMaxCurrentSpeed(int value)
    {
        maxCurrentSpeed = value;
    }

    // Setter per maxCurrentHeal
    public void SetMaxCurrentHeal(int value)
    {
        maxCurrentHeal = value;
    }

 

}