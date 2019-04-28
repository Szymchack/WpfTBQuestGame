using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTBQuestGame.DataLayer;
using WpfTBQuestGame.Models;
using WpfTBQuestGame.PresentationLayer;

namespace WpfTBQuestGame.BusinessLayer
{


    public class GameBusiness
    {

        bool _newPlayer = false;

        GameSessionViewModel _gameSessionViewModel;
        Player _player = new Player();
        Map _gameMap;
        GameMapCoordinates _initialLocationCoordinates;

        PlayerSetupView _playerSetupView = null;

        public GameBusiness()
        {
            SetupPlayer();
            InitializeDataSet();
            InstantiateAndShowView();
        }


        private void SetupPlayer()
        {
            if (_newPlayer)
            {
                _playerSetupView = new PlayerSetupView(_player);
                _playerSetupView.ShowDialog();

               
                _player.ExperiencePoints = 0;
                _player.Health = 100;
                _player.Lives = 3;
            }
            else
            {
                _player = GameData.PlayerData();
            }
        }

        
        private void InitializeDataSet()
        {
            _gameMap = GameData.GameMap();
            _initialLocationCoordinates = GameData.InitialGameMapLocation();
        }

        
        private void InstantiateAndShowView()
        {
           
            _gameSessionViewModel = new GameSessionViewModel(
                _player,
                _gameMap,
                _initialLocationCoordinates
                );
            GameSessionView gameSessionView = new GameSessionView(_gameSessionViewModel);

            gameSessionView.DataContext = _gameSessionViewModel;

            gameSessionView.Show();


            _playerSetupView.Close();
        }
    }
}
    
