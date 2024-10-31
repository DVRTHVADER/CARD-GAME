namespace Devoir_card_game;
using Devoir_card_game;
using System;

public class ObservateurConcret : IObservateur
{
    public void Notifier(string message)
    {
        Console.WriteLine(message);
    }
}