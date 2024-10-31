using System.Collections.Generic;
using Devoir_card_game;

namespace Devoir_card_game;

public class Joueur : Personne
{
    public List<Carte> Main { get; } = new List<Carte>();
    public bool AdopterStrategieMinimisationPoints { get; set; } = false; // Stratégie de minimisation

    public Joueur(string nom, string prenom) : base(nom, prenom) { }

    public void AjouterCarte(Carte carte)
    {
        if (Main.Count < 8)
        {
            Main.Add(carte);
            if (AdopterStrategieMinimisationPoints)
            {
                TrierMainParPoints();
            }
        }
    }

    public void RetirerCarte(Carte carte) => Main.Remove(carte);

    public bool AEncoreDesCartes() => Main.Count > 0;

    private void TrierMainParPoints()
    {
        // Trie la main pour jouer en premier les cartes ayant les valeurs les plus élevées
        Main.Sort((c1, c2) => c2.Valeur.CompareTo(c1.Valeur));
    }
}
