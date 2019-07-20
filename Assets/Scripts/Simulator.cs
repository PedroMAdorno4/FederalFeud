
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Depart {
    public string initials;
    public string owner;
    public int strength;
    public List<string> owns;

    public Depart(string init, int str) {
        initials = init;
        owner = null;
        strength = str;
        owns = new List<string>();
    }
}
public struct Faction {
    public string name;
    public Dictionary<string, Depart> dpts;
    public Color color;
}


public class Simulator : MonoBehaviour {
    public Faction[] factions;
    public int factionQt;
    private Transform mapFederal;
    public GameObject textPrefab;
    void Start()
    {
        mapFederal = GameObject.Find("FederalMap").transform;

        InitializeFactions();
        InstantiateLabels();
        
    }

    private void InitializeFactions() {
        factions = new Faction[3];

        factions[0].name = "Centro de Ciencias Biologicas e da Saude";
        factions[1].name = "Centro de Ciencias Exatas e de Tecnologia";
        factions[2].name = "Centro de Educacao e Ciencias Humanas";
        factionQt = factions.Length;

        factions[0].color = new Color(0, 255, 0);
        factions[1].color = new Color(255, 0, 0);
        factions[2].color = new Color(255, 255, 0);

        factions[0].dpts = new Dictionary<string, Depart>();
        factions[1].dpts = new Dictionary<string, Depart>();
        factions[2].dpts = new Dictionary<string, Depart>();
        //Bio
        factions[0].dpts.Add("Departamento de Botanica", new Depart("DB", 1));
        factions[0].dpts.Add("Departamento de Ciencias Fisiologicas", new Depart("DCF", 2));
        factions[0].dpts.Add("Departamento de Ecologia e Biologia Evolutiva", new Depart("DEBE", 3));
        factions[0].dpts.Add("Departamente de Educacao Fisica e Motricidade Humana", new Depart("DEFMH", 4));
        factions[0].dpts.Add("Departamento de Enfermagem", new Depart("DEnf", 5));
        factions[0].dpts.Add("Departamento de Fisioterapia", new Depart("DFisio", 6));
        factions[0].dpts.Add("Departamento de Genetica e Evolucao", new Depart("DGE", 7));
        factions[0].dpts.Add("Departamento de Gerontologia", new Depart("GERo", 8));
        factions[0].dpts.Add("Departamento de Hidrobiologia", new Depart("DHb", 44));
        factions[0].dpts.Add("Departamento de Medicina", new Depart("DMed", 58));
        factions[0].dpts.Add("Departamento de Morfologia e Patologia", new Depart("DMP", 14));
        factions[0].dpts.Add("Departamento de Terapia Ocupacional", new Depart("DTO", 32));
        factions[0].dpts.Add("Departamento de Ciencias Ambientais", new Depart("DCAm", 2));
        //Exatas
        factions[1].dpts.Add("Departamento de Computacao", new Depart("DC", 99));
        factions[1].dpts.Add("Departamento de Estatistica", new Depart("DEs", 24));
        factions[1].dpts.Add("Departamento de Engenharia Civil", new Depart("DECiv", 35));
        factions[1].dpts.Add("Departamento de Engenharia de Materiais", new Depart("DEMa", 44));
        factions[1].dpts.Add("Departamento de Engenharia Mecanica-Eletrica", new Depart("DEMec-DEE", 20));
        factions[1].dpts.Add("Departamento de Engenharia de Producao", new Depart("DEP", 50));
        factions[1].dpts.Add("Departamento de Engenharia Quimica", new Depart("DEQ", 19));
        factions[1].dpts.Add("Departamento de Matematica", new Depart("DM", 72));
        factions[1].dpts.Add("Departamento de Quimica", new Depart("DQ", 43));
        //Humanas
        factions[2].dpts.Add("Departamento de Artes e Comunicacao", new Depart("DAC", 44));
        factions[2].dpts.Add("Departamento de Ciencias Sociais", new Depart("DCSo", 11));
        factions[2].dpts.Add("Departamento de Educacao Especial", new Depart("DEEs", 25));
        factions[2].dpts.Add("Departamento de Filosofia e Metodologia das Ciencias", new Depart("DFMC", 64));
        factions[2].dpts.Add("Departamento de Letras", new Depart("DL", 75));
        factions[2].dpts.Add("Departamento de Metodologia de Ensino", new Depart("DME", 64));
        factions[2].dpts.Add("Departamento de Psicologia", new Depart("DPsi", 64));
        factions[2].dpts.Add("Departamento de Sociologia", new Depart("DS", 53));

        for(int i = 0; i < factionQt; i++) {
            int dpts = factions[i].dpts.Count;
            for(int j = 0; j < dpts; j++) {
                factions[i].dpts.ElementAt(j).Value.owner = factions[i].dpts.ElementAt(j).Key;
            }
        }
    }

    private void InstantiateLabels() {
        int childCount = mapFederal.childCount;
        for(int i = 0; i < childCount; i++) {
            Transform dpt = mapFederal.GetChild(i);
            GameObject go = Instantiate(textPrefab, dpt.position, Quaternion.identity, dpt);
            go.transform.GetChild(0).GetComponent<Text>().text = dpt.name;
        }

        for(int i = 0; i < factionQt; i++) {
            int dpts = factions[i].dpts.Count;
            for(int j = 0; j < dpts; j++) {
                mapFederal.Find(factions[i].dpts.ElementAt(j).Key).GetComponentInChildren<Text>().text = factions[i].dpts.ElementAt(j).Value.initials;
            }
        }
    }

    public int GetDefFaction(int attacker) {
        return (attacker + Random.Range(1, factionQt)) % factionQt;
    }

    public string GetDefDpt(int defFact, int atkFact, string attacker) {
        int ind;
        if(defFact == atkFact) {
            do {
                ind = GetRandomIndex(factions[defFact].dpts.Count);
            } while(factions[defFact].dpts.ElementAt(ind).Key == factions[defFact].dpts[attacker].owner ||
            factions[defFact].dpts[factions[defFact].dpts[attacker].owner].owns.Contains(factions[defFact].dpts.ElementAt(ind).Key));
        } else {
            ind = GetRandomIndex(factions[defFact].dpts.Count);
        }
        return factions[defFact].dpts.ElementAt(ind).Key;
    }

    public void CheckFactions() {
        for(int i = 0; i < factionQt; i++) {
            if(factions[i].dpts.Count == 0) {
                ExcludeFaction(i);
            }
        }
    }

    private void ExcludeFaction(int a) {
        Faction aux;
        aux = factions[factionQt-1];
        factions[--factionQt] = factions[a];
        factions[a] = aux;
    }

    private bool CheckWinner() {
        if(factionQt == 1) {
            int dptCount = factions[0].dpts.Count;
            for(int i = 0; i < dptCount; i++) {

            }
            return true;
        } else {
            return false;
        }
    }


    public void SimulateNextTurn() {

        if(CheckWinner()) {
            print(factions[0].dpts.ElementAt(0).Key + " agora impera a Federal!");
            return;
        }

        int atkFact = GetRandomIndex(factionQt);
        string atkDpt = factions[atkFact].dpts.ElementAt(GetRandomIndex(factions[atkFact].dpts.Count)).Key;
        int defFact = GetDefFaction(atkFact);
        string defDpt = GetDefDpt(defFact, atkFact, atkDpt);

        int winnerFact = -1;
        string winnerDpt = null;
        int loserFact = -1;
        string loserDpt = null;

        if(factions[atkFact].dpts[atkDpt].strength > factions[defFact].dpts[defDpt].strength) {
            winnerFact = atkFact;
            winnerDpt = atkDpt;
            loserFact = defFact;
            loserDpt = defDpt;
            print(factions[winnerFact].dpts[winnerDpt].owner + " venceu e agora domina o " + factions[loserFact].dpts[loserDpt].owner);
        } else if(factions[atkFact].dpts[atkDpt].strength < factions[defFact].dpts[defDpt].strength) {
            winnerFact = defFact;
            winnerDpt = defDpt;
            loserFact = atkFact;
            loserDpt = atkDpt;
            print(factions[winnerFact].dpts[winnerDpt].owner + " defendeu seu territorio e conquistou o " + factions[loserFact].dpts[loserDpt].owner);
        } else {
            print(factions[atkFact].dpts[atkDpt].owner + " e " + factions[defFact].dpts[defDpt].owner + " desistiram da batalha");
        }

        if(winnerFact > -1 && winnerDpt != null && loserFact > -1 && loserDpt != null) {
            factions[winnerFact].dpts[winnerDpt].strength += factions[loserFact].dpts[loserDpt].strength;
            factions[loserFact].dpts[loserDpt].strength = factions[winnerFact].dpts[winnerDpt].strength;


            string owner = factions[loserFact].dpts[loserDpt].owner;
            foreach(string ownedDpt in factions[loserFact].dpts[owner].owns) {
                mapFederal.Find(ownedDpt).GetComponent<SpriteRenderer>().color = factions[winnerFact].color;
                mapFederal.Find(ownedDpt).GetComponentInChildren<Text>().text = factions[winnerFact].dpts[factions[winnerFact].dpts[winnerDpt].owner].initials;
                factions[loserFact].dpts[ownedDpt].owner = factions[winnerFact].dpts[winnerDpt].owner;
                factions[winnerFact].dpts[factions[winnerFact].dpts[winnerDpt].owner].owns.Add(ownedDpt);

                if(winnerFact != loserFact) {
                    factions[winnerFact].dpts.Add(ownedDpt, factions[loserFact].dpts[ownedDpt]);
                    factions[loserFact].dpts.Remove(ownedDpt);
                }
            }

            mapFederal.Find(owner).GetComponent<SpriteRenderer>().color = factions[winnerFact].color;
            mapFederal.Find(owner).GetComponentInChildren<Text>().text = factions[winnerFact].dpts[factions[winnerFact].dpts[winnerDpt].owner].initials;
            factions[winnerFact].dpts[factions[winnerFact].dpts[winnerDpt].owner].owns.Add(owner);
            factions[loserFact].dpts[owner].owner = factions[winnerFact].dpts[winnerDpt].owner;

            if(winnerFact != loserFact) {
                factions[winnerFact].dpts.Add(owner, factions[loserFact].dpts[owner]);
                factions[loserFact].dpts.Remove(owner);
            }
        }

        CheckFactions();
    }

    private int GetRandomIndex(int max) {
        return Random.Range(0, max);
    }
}
