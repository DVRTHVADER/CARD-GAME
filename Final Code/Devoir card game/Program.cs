using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Devoir_card_game;
namespace Devoir_card_game;

public class Program
{
    public static async Task Main()
    {
        var joueurs = new List<Joueur>
        {
            new Joueur("Bennani", "Mohamed"),
            new Joueur("Abdelahid", "Awessou"),
            new Joueur("Graciela", "Livane")
        };

        var paireDeCartes = new PaireDeCartes();
        var jeuxDePeche = new JeuxDePeche(joueurs, paireDeCartes);
        var observateur = new ObservateurConcret();
        jeuxDePeche.AjouterObservateur(observateur);
        await jeuxDePeche.LancerPartie();
    }
}