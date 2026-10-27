using UnityEngine;
using Zenject;

public class Resolver 
{
    [Inject]
    public Resolver(CustomScanner scanner, ViewModel viewModel, ScoreView scoreView, ShipSpeedView shipSpeedView)
    {
        object[] scanning = {
            viewModel,
            scoreView,
            shipSpeedView
        };

        // foreach (var VARIABLE in scanning)
        // {
        //     Debug.Log(VARIABLE);
        // }
        
        scanner.Contact(scanning);
    }
}