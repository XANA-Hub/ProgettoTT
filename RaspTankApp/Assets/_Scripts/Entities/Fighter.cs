using UnityEngine;

public class Fighter : MonoBehaviour {

    // Satistiche base
    [Header("Statistiche base del combattente")]
    public FighterData data;

    // Livello
    protected int level = -1;

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
    protected void InitializeStats() {

        // Trovo il bonus natura del fighter
        currentNatureBonus = GetNatureBonus(this.data.preferredNatureBonus);

        // Calcolo le varie stat del fighter in base al bonus natura scelto
        currentAttack = CalculateCurrentStatWithBonus(this.data.baseAttack, this.level, currentNatureBonus);
        currentDefense = CalculateCurrentStatWithBonus(this.data.baseDefense, this.level, currentNatureBonus);
        currentHP = CalculateCurrentStatWithBonus(this.data.baseHP, this.level, currentNatureBonus);
        currentSpeed = CalculateCurrentStatWithBonus(this.data.baseSpeed, this.level, currentNatureBonus);
        currentHeal = CalculateCurrentStatWithBonus(this.data.baseHeal, this.level, currentNatureBonus);

        // Imposto le stat massime
        maxCurrentAttack = currentAttack;
        maxCurrentDefense = currentDefense;
        maxCurrentHP = currentHP;
        maxCurrentSpeed = currentSpeed;
        maxCurrentHeal = currentHeal;

    }



    // Calcola le statistiche senza il bonus natura
    private int CalculateCurrentStatWithoutBonus(int baseStat, int level) {
        return baseStat * (1 + (level - 1) / 100);
    }

    // Calcola le statistiche con il bonus natura
    private int CalculateCurrentStatWithBonus(int baseStat, int level, NatureBonus chosenNatureBonus) {
        
        float natureBonusPercentage = this.data.natureBonusPercentage;

        int currentStat = CalculateCurrentStatWithoutBonus(baseStat, level);
        currentStat += (int)System.Math.Ceiling(currentStat * natureBonusPercentage); // Arrotonda per eccesso
        return currentStat;
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
        float randomValue = Random.value;

        // Se il valore casuale è inferiore alla probabilità del bonus preferito, scegli il bonus preferito
        if (randomValue < preferredNatureProbability) {
            return preferredNatureBonus;
        }
        else {
            // Altrimenti, scegli un bonus natura casuale diverso dal preferito e che non sia il RANDOM
            NatureBonus[] allNatureBonuses = (NatureBonus[])System.Enum.GetValues(typeof(NatureBonus));
            NatureBonus randomNatureBonus;

            do {
                randomNatureBonus = allNatureBonuses[Random.Range(0, allNatureBonuses.Length)];
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
        if(currentHP <= 0) 
            return true;
        else
            return false;
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

    public string getCurrentNature() {
        return System.Enum.GetName(typeof(NatureBonus), currentNatureBonus);
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
    public void SetCurrentAttack(int value)
    {
        currentAttack = value;
    }

    // Setter per currentDefense
    public void SetCurrentDefense(int value)
    {
        currentDefense = value;
    }

    // Setter per currentHP
    public void SetCurrentHP(int value)
    {
        currentHP = value;
    }

    // Setter per currentSpeed
    public void SetCurrentSpeed(int value)
    {
        currentSpeed = value;
    }

    // Setter per currentHeal
    public void SetCurrentHeal(int value)
    {
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