using System;

public class VilleRack : Rack
{
    private const EScrollDestination Destination = EScrollDestination.Ville;

    private void Start()
    {
        GameState().villeRack = this;
    }
}