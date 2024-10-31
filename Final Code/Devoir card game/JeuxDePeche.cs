using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Devoir_card_game;

public class JeuxDePeche
{
    private readonly TableDeJeu _tableDeJeu;
    private readonly List<Joueur> _joueurs;
    private readonly List<IObservateur> _observateurs;
    private int _indexJoueur;
    private bool _sensHoraire;
    private readonly Random _random = new Random();
    private readonly Joueur _joueurAvecStrategie; // Joueur sélectionné pour la stratégie de minimisation

    public JeuxDePeche(List<Joueur> joueurs, PaireDeCartes paireDeCartes)
    {
        _tableDeJeu = new TableDeJeu(paireDeCartes);
        _joueurs = joueurs;
        _observateurs = new List<IObservateur>();
        _indexJoueur = 0;
        _sensHoraire = true;

        // Choix aléatoire d'un joueur pour la stratégie de minimisation des points
        _joueurAvecStrategie = _joueurs[_random.Next(_joueurs.Count)];
        _joueurAvecStrategie.AdopterStrategieMinimisationPoints = true;
    }

    public void AjouterObservateur(IObservateur observateur)
    {
        if (!_observateurs.Contains(observateur))
            _observateurs.Add(observateur);
    }

    private void NotifierObservateurs(string message)
    {
        foreach (var observateur in _observateurs)
        {
            observateur.Notifier(message);
        }
    }

    private void DistribuerCartesInitiales(int nombreDeCartes = 5)
    {
        foreach (var joueur in _joueurs)
        {
            for (int i = 0; i < nombreDeCartes; i++)
            {
                Carte? carte = _tableDeJeu.PiocherCarte();
                if (carte.HasValue)
                {
                    joueur.AjouterCarte(carte.Value);
                }
            }
        }
    }

    public async Task LancerPartie()
{
    NotifierObservateurs("Début de la partie de pêche !");

    // Annonce du joueur choisi pour la stratégie de minimisation
    NotifierObservateurs($"{_joueurAvecStrategie.Nom} {_joueurAvecStrategie.Prenom} a été choisi pour adopter une stratégie de minimisation des points.");

    DistribuerCartesInitiales();

    Carte premiereCarte = _tableDeJeu.DerniereCarteDepot();
    NotifierObservateurs($"La première carte de la pile de jeu est : {premiereCarte}");

    while (true)
    {
        if (_joueurs.Any(j => !j.AEncoreDesCartes()))
        {
            var gagnant = _joueurs.First(j => !j.AEncoreDesCartes());
            NotifierObservateurs($"Partie terminée ! Le vainqueur est {gagnant.Nom} {gagnant.Prenom}");
            CalculerBilanPoints();
            break;
        }

        Joueur joueurActuel = _joueurs[_indexJoueur];

        if (joueurActuel.Main.Count == 1)
        {
            NotifierObservateurs($"{joueurActuel.Nom} {joueurActuel.Prenom} n’a plus qu’une carte !");
            foreach (var joueur in _joueurs)
            {
                joueur.AdopterStrategieMinimisationPoints = true;
            }
        }

        string mainJoueur = string.Join(", ", joueurActuel.Main.Select(c => c.ToString()));
        NotifierObservateurs($"Main de {joueurActuel.Nom} {joueurActuel.Prenom} : {mainJoueur}");
        NotifierObservateurs($"Tour de {joueurActuel.Nom} {joueurActuel.Prenom}");

        Carte carteDepot = _tableDeJeu.DerniereCarteDepot();

        // Application de la stratégie de minimisation pour jouer les cartes ayant les plus hauts points
        List<Carte> cartesJouables = joueurActuel.AdopterStrategieMinimisationPoints
            ? joueurActuel.Main.OrderByDescending(c => (int)c.Valeur).ToList()
            : joueurActuel.Main;

        Carte? carteAJouer = cartesJouables.FirstOrDefault(c => _tableDeJeu.CarteEstJouable(c));

        if (carteAJouer.HasValue && !string.IsNullOrEmpty(carteAJouer.Value.Nom))
        {
            Carte carte = carteAJouer.Value;
            joueurActuel.RetirerCarte(carte);
            _tableDeJeu.DeposerCarte(carte);
            NotifierObservateurs($"{joueurActuel.Nom} a joué : {carte}");

            switch (carte.Valeur)
            {
                case Valeur.As:
                    NotifierObservateurs("L'As a été joué, le tour du prochain joueur est sauté.");
                    _indexJoueur = (_indexJoueur + (_sensHoraire ? 2 : -2) + _joueurs.Count) % _joueurs.Count;
                    continue;
                case Valeur.Dix:
                    _sensHoraire = !_sensHoraire;
                    NotifierObservateurs("Le sens du jeu est inversé !");
                    _indexJoueur = (_indexJoueur + (_sensHoraire ? 1 : -1) + _joueurs.Count) % _joueurs.Count;
                    continue;
                case Valeur.Sept:
                    int prochainJoueurIndex = (_indexJoueur + (_sensHoraire ? 1 : -1) + _joueurs.Count) % _joueurs.Count;
                    Joueur prochainJoueur = _joueurs[prochainJoueurIndex];
                    bool possedeSept = prochainJoueur.Main.Any(c => c.Valeur == Valeur.Sept);

                    if (possedeSept)
                    {
                        string mainProchainJoueur = string.Join(", ", prochainJoueur.Main.Select(c => c.ToString()));
                        NotifierObservateurs($"Main de {prochainJoueur.Nom} {prochainJoueur.Prenom} avant de contrer : {mainProchainJoueur}");

                        Carte septPourContrer = prochainJoueur.Main.First(c => c.Valeur == Valeur.Sept);
                        NotifierObservateurs($"{prochainJoueur.Nom} {prochainJoueur.Prenom} contre avec un autre {septPourContrer}");

                        prochainJoueur.RetirerCarte(septPourContrer);
                        _tableDeJeu.DeposerCarte(septPourContrer);
                        _indexJoueur = (_indexJoueur + (_sensHoraire ? 2 : -2) + _joueurs.Count) % _joueurs.Count;
                        continue;
                    }
                    else
                    {
                        Carte? carte1 = _tableDeJeu.PiocherCarte();
                        Carte? carte2 = _tableDeJeu.PiocherCarte();
                        if (carte1.HasValue) prochainJoueur.AjouterCarte(carte1.Value);
                        if (carte2.HasValue) prochainJoueur.AjouterCarte(carte2.Value);
                        NotifierObservateurs($"{prochainJoueur.Nom} {prochainJoueur.Prenom} pioche deux cartes à cause du 7.");
                        _indexJoueur = (_indexJoueur + (_sensHoraire ? 2 : -2) + _joueurs.Count) % _joueurs.Count;
                        continue;
                    }
                case Valeur.Valet:
                    if (carte.Nom == "Valet")
                    {
                        // Vérification : Si la main est vide ou ne contient aucune couleur dominante
                        var couleurDominante = joueurActuel.Main
                            .GroupBy(c => c.Couleur)
                            .OrderByDescending(g => g.Count())
                            .FirstOrDefault()?.Key ?? Couleur.Trèfle; // Par défaut : Trèfle si aucune couleur dominante

                        NotifierObservateurs($"{joueurActuel.Nom} peut changer la couleur. La couleur choisie est {couleurDominante}.");
                        _tableDeJeu.DefinirCouleur(couleurDominante);
                    }
                    break;
            }

            _indexJoueur = (_indexJoueur + (_sensHoraire ? 1 : -1) + _joueurs.Count) % _joueurs.Count;
        }
        else
        {
            NotifierObservateurs($"{joueurActuel.Nom} pioche une carte.");
            Carte? cartePiochee = _tableDeJeu.PiocherCarte();
            if (cartePiochee.HasValue)
                joueurActuel.AjouterCarte(cartePiochee.Value);
            else
            {
                if (_tableDeJeu.ReinitialiserPioche())
                {
                    NotifierObservateurs("La pile de pioche est vide. Mélange de la pile de défausse pour reformer la pile de pioche.");
                }
            }
            _indexJoueur = (_indexJoueur + (_sensHoraire ? 1 : -1) + _joueurs.Count) % _joueurs.Count;
        }

        await Task.Delay(1000);
    }
}


    private void CalculerBilanPoints()
    {
        NotifierObservateurs("Calcul du bilan de points de chaque joueur :");

        foreach (var joueur in _joueurs)
        {
            int totalPoints = joueur.Main.Sum(carte => (int)carte.Valeur);
            NotifierObservateurs($"{joueur.Nom} {joueur.Prenom} a un total de {totalPoints} points.");
        }
    }
}
