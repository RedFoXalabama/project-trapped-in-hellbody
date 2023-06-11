using Godot;
using System;
using System.Collections.Generic;

public partial class MagicProp : Resource
{
	//ATTRIBUTI
	public enum MagicType{
        FIRE,
        GRASS,
        EARTH,
        WATER,
        CAOS
    }
	private MagicType type;
	private float magicPower;
    
	//COSTRUTTORE
    public MagicProp()
    {
        Type = MagicType.CAOS;
        MagicPower = 0;
    }
	public MagicProp(MagicType type, float magicPower)
    {
        Type = type;
        MagicPower = magicPower;
    }
	//FUNZIONI

    //CALCOLO DELL'EFFETTO
        //formula dell'effetto da usare per dividere la difesa nella formula del calcolo
        //restituisce 1 se i valori non si influenzano
        //restituisce il valore della propensione se è superefficace (divide la difesa riducendola)
        //restituisce l'INVERSO DEL VALORE il valore dell'effetto se è poco efficace (divide la difesa aumentandola)
    //CALCOLO DELL'EFFETTO PLAYER -> ENEMY
    public float CalcMagicEffect(PlayerInfoManager pim, EnemyInfoManager eim){
        float effectValue = 1;
        switch (pim.MagicProp.Type){ //SOSTITUIRE CON LA PROPENSIONE DELLA MOSSA
            case MagicType.FIRE:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = eim.MagicProp.MagicPower; //SUPEREFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = (1/eim.MagicProp.MagicPower); //INEFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.GRASS:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = (1/eim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = eim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.EARTH:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = (1/eim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = eim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.WATER:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = eim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = (1/eim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.CAOS:
                effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                break;
            default:
                break;
        }
        return effectValue;
    }
    //CALCOLO DELL'EFFETTO ALLY -> ENEMY
    public float CalcMagicEffect(AllyInfoManager aim ,EnemyInfoManager eim){
        float effectValue = 1;
        switch (aim.MagicProp.Type){ //SOSTITUIRE CON LA PROPENSIONE DELLA MOSSA
            case MagicType.FIRE:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = eim.MagicProp.MagicPower; //SUPEREFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = (1/eim.MagicProp.MagicPower); //INEFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.GRASS:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = (1/eim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = eim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.EARTH:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = (1/eim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = eim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.WATER:
                switch (eim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = eim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = (1/eim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.CAOS:
                effectValue = GetCaosValue(eim.MagicProp.MagicPower);
                break;
            default:
                break;
        }
        return effectValue;
    }
    //CALCOLO DELL'EFFETTO ENEMY -> PLAYER
    public float CalcMagicEffect(EnemyInfoManager eim, PlayerInfoManager pim){
        float effectValue = 1;
        switch (eim.MagicProp.Type){ //SOSTITUIRE CON LA PROPENSIONE DELLA MOSSA
            case MagicType.FIRE:
                switch (pim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = pim.MagicProp.MagicPower; //SUPEREFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = (1/pim.MagicProp.MagicPower); //INEFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(pim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.GRASS:
                switch (pim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = (1/pim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = pim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(pim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.EARTH:
                switch (pim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = (1/pim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = pim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(pim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.WATER:
                switch (pim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = pim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = (1/pim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(pim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.CAOS:
                effectValue = GetCaosValue(pim.MagicProp.MagicPower);
                break;
            default:
                break;
        }
        return effectValue;
    }
    //CALCOLO DELL'EFFETTO ENEMY -> ALLY
    public float CalcMagicEffect(EnemyInfoManager eim, AllyInfoManager aim){
        float effectValue = 1;
        switch (eim.MagicProp.Type){ //SOSTITUIRE CON LA PROPENSIONE DELLA MOSSA
            case MagicType.FIRE:
                switch (aim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = aim.MagicProp.MagicPower; //SUPEREFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = (1/aim.MagicProp.MagicPower); //INEFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(aim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.GRASS:
                switch (aim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = (1/aim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = aim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(aim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.EARTH:
                switch (aim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = 1;
                        break;
                    case MagicType.GRASS:
                        effectValue = (1/aim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.EARTH:
                        effectValue = 1;
                        break;
                    case MagicType.WATER:
                        effectValue = aim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(aim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.WATER:
                switch (aim.MagicProp.Type){
                    case MagicType.FIRE:
                        effectValue = aim.MagicProp.MagicPower;//SUPEREFFICACE
                        break;
                    case MagicType.GRASS:
                        effectValue = 1;
                        break;
                    case MagicType.EARTH:
                        effectValue = (1/aim.MagicProp.MagicPower);//INEFFICACE
                        break;
                    case MagicType.WATER:
                        effectValue = 1;
                        break;
                    case MagicType.CAOS:
                        effectValue = GetCaosValue(aim.MagicProp.MagicPower);
                        break;
                }
                break;
            case MagicType.CAOS:
                effectValue = GetCaosValue(aim.MagicProp.MagicPower);
                break;
            default:
                break;
        }
        return effectValue;
    }

	//GETTER AND SETTER + FUNZIONI DI SUPPORTO
    public float GetCaosValue(float magicPower){ //Serve per generare un effectValue casuale
        Random random = new Random();
        int randomNumber = random.Next(1, 4); // Genera un numero casuale compreso tra 1 e 3
        // Mappa il numero casuale ai tuoi tre valori
        switch (randomNumber){
        case 1:
            return 1.0f;
        case 2:
            return magicPower;
        case 3:
            return (1/magicPower);
        default:
            throw new InvalidOperationException("Invalid random number generated.");
        }
    } 

    public MagicType Type{
		get => type;
		set => type = value;}
    public float MagicPower{
		get => magicPower;
		set => magicPower = value; }

}
