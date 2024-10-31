namespace Devoir_card_game;
using Devoir_card_game;

public struct Carte
{
    public Couleur Couleur { get; }
    public Valeur Valeur { get; }
    public string Nom { get; }  // Nouveau champ pour le nom de la carte

    public Carte(Couleur couleur, Valeur valeur, string nom)
    {
        Couleur = couleur;
        Valeur = valeur;
        Nom = nom;
    }

    public override string ToString()
    {
        return $"{Nom} de {Couleur}";
    }
}
