# CARD-GAME
CARD GAME Developed in C
Developper par Abdelahid Awessou.
Le Devoir Consiste a cree un projet de jeu de cartes inspiré de jeux de type pêche où plusieurs joueurs jouent à tour de rôle en tentant de minimiser leurs points. Le jeu inclut une logique pour distribuer et jouer des cartes, gérer le score, appliquer des règles spéciales pour certaines cartes et observer les événements de jeu grâce à un modèle d'observateur.
Fonctionnalités
Distribution des cartes : Chaque joueur reçoit un nombre initial de cartes au début de la partie.
Règles de jeu personnalisées : Certaines cartes déclenchent des actions spéciales (As pour passer le tour, 7 pour forcer le joueur suivant à piocher deux cartes, etc.).
Stratégie de minimisation des points : Un joueur est choisi aléatoirement pour minimiser ses points dès le début de la partie.
Système d’observation : Le modèle d'observateur est utilisé pour notifier les événements importants, comme les tours de jeu, les actions spéciales, et la fin de la partie.
Structure du projet
Le projet est organisé en plusieurs classes et interfaces :
Carte : Représente une carte individuelle avec une couleur, une valeur et un nom.
PaireDeCartes : Initialise un paquet de cartes mélangé pour le jeu.
Joueur : Représente un joueur du jeu, avec une stratégie de minimisation optionnelle.
TableDeJeu : Gère les piles de pioche et de défausse, et les règles pour déposer et piocher les cartes.
JeuxDePeche : Gère le déroulement de la partie et l'application des règles.
ObservateurConcret : Implémente IObservateur pour gérer les notifications de jeu.
