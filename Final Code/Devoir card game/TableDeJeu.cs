using System;
using System.Collections.Generic;
using System.Linq;
using Devoir_card_game;

public class TableDeJeu
{
    public Stack<Carte> PileDeDepot { get; private set; } = new Stack<Carte>();
    public Stack<Carte> PileDePioche { get; private set; }
    private Couleur? _couleurActuelle; // Utilisé pour stocker la couleur choisie après le jeu d'un Valet

    public TableDeJeu(PaireDeCartes paireDeCartes)
    {
        PileDePioche = new Stack<Carte>(paireDeCartes.Cartes);

        // Placer la première carte de la pioche dans la pile de dépôt
        if (PileDePioche.Count > 0)
        {
            PileDeDepot.Push(PileDePioche.Pop());
        }
    }

    public Carte DerniereCarteDepot()
    {
        return PileDeDepot.Peek();
    }

    public void DeposerCarte(Carte carte)
    {
        PileDeDepot.Push(carte);

        // Réinitialiser la couleur imposée seulement si la carte jouée est de la couleur imposée par le Valet
        if (_couleurActuelle.HasValue && carte.Couleur == _couleurActuelle)
        {
            _couleurActuelle = null; // Libère la couleur imposée uniquement si elle est respectée
        }
    }

    public Carte? PiocherCarte()
    {
        if (PileDePioche.Count > 0)
        {
            return PileDePioche.Pop();
        }
        return null;
    }

    public bool PileDePiocheVide()
    {
        return PileDePioche.Count == 0;
    }

    public bool ReinitialiserPioche()
    {
        if (PileDeDepot.Count > 1)
        {
            var depotCartes = new List<Carte>(PileDeDepot);
            Carte derniereCarte = depotCartes.Last(); // Garde la dernière carte dans le dépôt
            depotCartes.RemoveAt(depotCartes.Count - 1);
            PileDeDepot.Clear();
            PileDeDepot.Push(derniereCarte);

            var random = new Random();
            depotCartes = depotCartes.OrderBy(_ => random.Next()).ToList();

            foreach (var carte in depotCartes)
            {
                PileDePioche.Push(carte);
            }

            Console.WriteLine("La pile de pioche est vide. Mélange de la pile de défausse pour reformer la pile de pioche.");
            return true;
        }
        return false;
    }

    public void DefinirCouleur(Couleur nouvelleCouleur)
    {
        _couleurActuelle = nouvelleCouleur;
    }

    public bool CarteEstJouable(Carte carte)
    {
        if (_couleurActuelle.HasValue)
        {
            // Permet uniquement les cartes de la couleur imposée ou un autre Valet
            if (carte.Couleur == _couleurActuelle || carte.Nom == "Valet")
            {
                // Si la carte jouée est de la couleur imposée, réinitialise la couleur imposée
                if (carte.Couleur == _couleurActuelle)
                {
                    _couleurActuelle = null; // Libère la couleur imposée pour les prochains tours
                }
                return true; // Permet de jouer la carte de la couleur imposée ou un autre Valet
            }
            return false; // Refuse toute autre carte ne respectant pas la couleur imposée
        }

        // Si aucune couleur n'est imposée, on applique la règle de correspondance couleur/valeur habituelle
        return carte.Couleur == DerniereCarteDepot().Couleur || carte.Valeur == DerniereCarteDepot().Valeur || carte.Nom == "Valet";
    }


    public Couleur? ObtenirCouleurActuelle()
    {
        return _couleurActuelle;
    }
}

