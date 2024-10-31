namespace Devoir_card_game;
using System.Linq;
using System;
using System.Collections.Generic;

public class PaireDeCartes
{
    private readonly List<Carte> _cartes = new List<Carte>();

    public PaireDeCartes()
    {
        foreach (Couleur couleur in (Couleur[])Enum.GetValues(typeof(Couleur)))
        {
            _cartes.Add(new Carte(couleur, Valeur.As, "As"));
            _cartes.Add(new Carte(couleur, Valeur.Deux, "Deux"));
            _cartes.Add(new Carte(couleur, Valeur.Trois, "Trois"));
            _cartes.Add(new Carte(couleur, Valeur.Quatre, "Quatre"));
            _cartes.Add(new Carte(couleur, Valeur.Cinq, "Cinq"));
            _cartes.Add(new Carte(couleur, Valeur.Six, "Six"));
            _cartes.Add(new Carte(couleur, Valeur.Sept, "Sept"));
            _cartes.Add(new Carte(couleur, Valeur.Huit, "Huit"));
            _cartes.Add(new Carte(couleur, Valeur.Neuf, "Neuf"));
            _cartes.Add(new Carte(couleur, Valeur.Dix, "Dix"));
            _cartes.Add(new Carte(couleur, Valeur.Valet, "Valet"));
            _cartes.Add(new Carte(couleur, Valeur.Dame, "Dame"));
            _cartes.Add(new Carte(couleur, Valeur.Roi, "Roi"));
        }

        // Mélanger les cartes
        var random = new Random();
        _cartes = _cartes.OrderBy(_ => random.Next()).ToList();
    }

    public List<Carte> Cartes => _cartes;
}



