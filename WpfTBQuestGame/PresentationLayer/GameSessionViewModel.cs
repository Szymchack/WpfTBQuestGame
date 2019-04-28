using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfTBQuestGame.Models;

namespace WpfTBQuestGame.PresentationLayer
{
    public class GameSessionViewModel : ObservableObject
    {
        #region ENUMS

        #endregion

        #region FIELDS

        private DateTime _gameStartTime;
        private string _gameTimeDisplay;
        private TimeSpan _gameTime;

        private Player _player;

        private Map _gameMap;
        private Location _currentLocation;
        private Location _northLocation, _eastLocation, _southLocation, _westLocation;
        private string _currentLocationInformation;

        private GameItemQuantity _currentGameItem;
        private Npc _currentNpc;

        private Random random = new Random();

        #endregion

        #region PROPERTIES

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public string MessageDisplay
        {
            get { return _currentLocation.Message; }
        }
        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                _currentLocationInformation = _currentLocation.Description;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }

        
        public Location NorthLocation
        {
            get { return _northLocation; }
            set
            {
                _northLocation = value;
                OnPropertyChanged(nameof(NorthLocation));
                OnPropertyChanged(nameof(HasNorthLocation));
            }
        }

        public Location EastLocation
        {
            get { return _eastLocation; }
            set
            {
                _eastLocation = value;
                OnPropertyChanged(nameof(EastLocation));
                OnPropertyChanged(nameof(HasEastLocation));
            }
        }

        public Location SouthLocation
        {
            get { return _southLocation; }
            set
            {
                _southLocation = value;
                OnPropertyChanged(nameof(SouthLocation));
                OnPropertyChanged(nameof(HasSouthLocation));
            }
        }

        public Location WestLocation
        {
            get { return _westLocation; }
            set
            {
                _westLocation = value;
                OnPropertyChanged(nameof(WestLocation));
                OnPropertyChanged(nameof(HasWestLocation));
            }
        }


        public string CurrentLocationInformation
        {
            get { return _currentLocationInformation; }
            set
            {
                _currentLocationInformation = value;
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }

        public bool HasNorthLocation
        {
            get
            {
                if (NorthLocation != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool HasEastLocation { get { return EastLocation != null; } }
        public bool HasSouthLocation { get { return SouthLocation != null; } }
        public bool HasWestLocation { get { return WestLocation != null; } }

        public string MissionTimeDisplay
        {
            get { return _gameTimeDisplay; }
            set
            {
                _gameTimeDisplay = value;
                OnPropertyChanged(nameof(MissionTimeDisplay));
            }
        }

        public GameItemQuantity CurrentGameItem
        {
            get { return _currentGameItem; }
            set
            {
                _currentGameItem = value;
                OnPropertyChanged(nameof(CurrentGameItem));
                if (_currentGameItem != null && _currentGameItem.GameItem is Weapon)
                {
                    _player.CurrentWeapon = _currentGameItem.GameItem as Weapon;
                }
            }
        }


        public Npc CurrentNpc
        {
            get { return _currentNpc; }
            set
            {
                _currentNpc = value;
                OnPropertyChanged(nameof(CurrentNpc));
            }
        }

        #endregion

        #region CONSTRUCTORS

        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(
            Player player,
            Map gameMap,
            GameMapCoordinates currentLocationCoordinates)
        {
            _player = player;

            _gameMap = gameMap;
            _gameMap.CurrentLocationCoordinates = currentLocationCoordinates;
            _currentLocation = _gameMap.CurrentLocation;
            InitializeView();

            GameTimer();
        }

        #endregion

        #region METHODS

        private void InitializeView()
        {
            _gameStartTime = DateTime.Now;
            UpdateAvailableTravelPoints();
            _currentLocationInformation = CurrentLocation.Description;
            _player.UpdateInventoryCategories();
            _player.CalculateWealth();
        }

        #region MOVEMENT METHODS

        private void UpdateAvailableTravelPoints()
        {
            NorthLocation = null;
            EastLocation = null;
            SouthLocation = null;
            WestLocation = null;

            if (_gameMap.NorthLocation() != null)
            {
                Location nextNorthLocation = _gameMap.NorthLocation();

                if (nextNorthLocation.Accessible == true || PlayerCanAccessLocation(nextNorthLocation))
                {
                    NorthLocation = nextNorthLocation;
                }
            }

            if (_gameMap.EastLocation() != null)
            {
                Location nextEastLocation = _gameMap.EastLocation();

                //
                // location generally accessible or player has required conditions
                //
                if (nextEastLocation.Accessible == true || PlayerCanAccessLocation(nextEastLocation))
                {
                    EastLocation = nextEastLocation;
                }
            }

            if (_gameMap.SouthLocation() != null)
            {
                Location nextSouthLocation = _gameMap.SouthLocation();

                if (nextSouthLocation.Accessible == true || PlayerCanAccessLocation(nextSouthLocation))
                {
                    SouthLocation = nextSouthLocation;
                }
            }

            if (_gameMap.WestLocation() != null)
            {
                Location nextWestLocation = _gameMap.WestLocation();

                if (nextWestLocation.Accessible == true || PlayerCanAccessLocation(nextWestLocation))
                {
                    WestLocation = nextWestLocation;
                }
            }
        }
        private bool PlayerCanAccessLocation(Location nextLocation)
        {

            if (nextLocation.IsAccessibleByExperiencePoints(_player.ExperiencePoints))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnPlayerMove()
        {

            if (!_player.HasVisited(_currentLocation))
            {

                _player.LocationsVisited.Add(_currentLocation);

                _player.ExperiencePoints += _currentLocation.ModifiyExperiencePoints;

                _player.Health += _currentLocation.ModifyHealth;

                _player.Lives += _currentLocation.ModifyLives;

                OnPropertyChanged(nameof(MessageDisplay));
            }
        }

        public void MoveNorth()
        {
            if (HasNorthLocation)
            {
                _gameMap.MoveNorth();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
            }
        }

        public void MoveEast()
        {
            if (HasEastLocation)
            {
                _gameMap.MoveEast();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
            }
        }

        public void MoveSouth()
        {
            if (HasSouthLocation)
            {
                _gameMap.MoveSouth();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
            }
        }

        public void MoveWest()
        {
            if (HasWestLocation)
            {
                _gameMap.MoveWest();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
            }
        }

        #endregion

        #region GAME TIME METHODS

        private TimeSpan GameTime()
        {
            return DateTime.Now - _gameStartTime;
        }

        public void GameTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += OnGameTimerTick;
            timer.Start();
        }

        void OnGameTimerTick(object sender, EventArgs e)
        {
            _gameTime = DateTime.Now - _gameStartTime;
            MissionTimeDisplay = "Mission Time " + _gameTime.ToString(@"hh\:mm\:ss");
        }

        #endregion

        #region ACTION METHODS

        public void AddItemToInventory()
        {

            if (_currentGameItem != null && _currentLocation.GameItems.Contains(_currentGameItem))
            {

                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.RemoveGameItemQuantityFromLocation(selectedGameItemQuantity);
                _player.AddGameItemQuantityToInventory(selectedGameItemQuantity);

                OnPlayerPickUp(selectedGameItemQuantity);
            }
        }
        public void RemoveItemFromInventory()
        {

            if (_currentGameItem != null)
            {

                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.AddGameItemQuantityToLocation(selectedGameItemQuantity);
                _player.RemoveGameItemQuantityFromInventory(selectedGameItemQuantity);

                OnPlayerPutDown(selectedGameItemQuantity);
            }
        }

        private void OnPlayerPickUp(GameItemQuantity gameItemQuantity)
        {
            _player.ExperiencePoints += gameItemQuantity.GameItem.ExperiencePoints;
            _player.Wealth += gameItemQuantity.GameItem.Value;
        }

        private void OnPlayerPutDown(GameItemQuantity gameItemQuantity)
        {
            _player.Wealth -= gameItemQuantity.GameItem.Value;
        }

        public void OnUseGameItem()
        {
            switch (_currentGameItem.GameItem)
            {
                case Potion potion:
                    ProcessPotionUse(potion);
                    break;
                case Relic relic:
                    ProcessRelicUse(relic);
                    break;
                default:
                    break;
            }
        }

        private void ProcessRelicUse(Relic relic)
        {
            string message;

            switch (relic.UseAction)
            {
                case Relic.UseActionType.OPENLOCATION:
                    message = _gameMap.OpenLocationsByRelic(relic.Id);
                    CurrentLocationInformation = relic.UseMessage;
                    break;
                case Relic.UseActionType.KILLPLAYER:
                    OnPlayerDies(relic.UseMessage);
                    break;
                default:
                    break;
            }
        }

        private void ProcessPotionUse(Potion potion)
        {
            _player.Health += potion.HealthChange;
            _player.Lives += potion.LivesChange;
            _player.RemoveGameItemQuantityFromInventory(_currentGameItem);
        }

        private void OnPlayerDies(string message)
        {
            string messagetext = message +
                "\n\nWould you like to play again?";

            string titleText = "Death";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(messagetext, titleText, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetPlayer();
                    break;
                case MessageBoxResult.No:
                    QuiteApplication();
                    break;
            }
        }

        public void OnPlayerTalkTo()
        {
            if (CurrentNpc != null && CurrentNpc is ISpeak)
            {
                ISpeak speakingNpc = CurrentNpc as ISpeak;
                CurrentLocationInformation = speakingNpc.Speak();
            }
        }

        public void OnPlayerAttack()
        {
            _player.BattleMode = BattleModeName.ATTACK;
            Battle();
        }

        public void OnPlayerDefend()
        {
            _player.BattleMode = BattleModeName.DEFEND;
            Battle();
        }

        public void OnPlayerRetreat()
        {
            _player.BattleMode = BattleModeName.RETREAT;
            Battle();
        }

        private void QuiteApplication()
        {
            Environment.Exit(0);
        }

        private void ResetPlayer()
        {
            Environment.Exit(0);
        }

        #region BATTLE METHODS

        private void Battle()
        {
 
            if (_currentNpc is IBattle)
            {
                IBattle battleNpc = _currentNpc as IBattle;
                int playerHitPoints = 0;
                int battleNpcHitPoints = 0;
                string battleInformation = "";

                if (_player.CurrentWeapon != null)
                {
                    playerHitPoints = CalculatePlayerHitPoints();
                }
                else
                {
                    battleInformation = "It appears you are entering into battle without a weapon." + Environment.NewLine;
                }

                if (battleNpc.CurrentWeapon != null)
                {
                    battleNpcHitPoints = CalculateNpcHitPoints(battleNpc);
                }
                else
                {
                    battleInformation = $"It appears you are entering into battle with {_currentNpc.Name} who has no weapon." + Environment.NewLine;
                }

                battleInformation +=
                    $"Player: {_player.BattleMode}     Hit Points: {playerHitPoints}" + Environment.NewLine +
                    $"NPC: {battleNpc.BattleMode}     Hit Points: {battleNpcHitPoints}" + Environment.NewLine;

                if (playerHitPoints >= battleNpcHitPoints)
                {
                    battleInformation += $"You have slain {_currentNpc.Name}.";
                    _currentLocation.Npcs.Remove(_currentNpc);
                }
                else
                {
                    battleInformation += $"You have been slain by {_currentNpc.Name}.";
                    _player.Lives--;
                }

                CurrentLocationInformation = battleInformation;
                if (_player.Lives <= 0) OnPlayerDies("You have been slain and have no lives left.");
            }
            else
            {
                CurrentLocationInformation = "The current NPC will is not battle ready. Seems you are a bit jumpy and your experience suffers.";
                _player.ExperiencePoints -= 10;
            }

        }

        private int CalculatePlayerHitPoints()
        {
            int playerHitPoints = 0;

            switch (_player.BattleMode)
            {
                case BattleModeName.ATTACK:
                    playerHitPoints = _player.Attack();
                    break;
                case BattleModeName.DEFEND:
                    playerHitPoints = _player.Defend();
                    break;
                case BattleModeName.RETREAT:
                    playerHitPoints = _player.Retreat();
                    break;
            }

            return playerHitPoints;
        }

        private int CalculateNpcHitPoints(IBattle battleNpc)
        {
            int battleNpcHitPoints = 0;

            switch (NpcBattleResponse())
            {
                case BattleModeName.ATTACK:
                    battleNpcHitPoints = battleNpc.Attack();
                    break;
                case BattleModeName.DEFEND:
                    battleNpcHitPoints = battleNpc.Defend();
                    break;
                case BattleModeName.RETREAT:
                    battleNpcHitPoints = battleNpc.Retreat();
                    break;
            }

            return battleNpcHitPoints;
        }

        private BattleModeName NpcBattleResponse()
        {
            BattleModeName npcBattleResponse = BattleModeName.RETREAT;

            switch (DieRoll(3))
            {
                case 1:
                    npcBattleResponse = BattleModeName.ATTACK;
                    break;
                case 2:
                    npcBattleResponse = BattleModeName.DEFEND;
                    break;
                case 3:
                    npcBattleResponse = BattleModeName.RETREAT;
                    break;
            }
            return npcBattleResponse;
        }

        #endregion

        #region HELPER METHODS

        private int DieRoll(int sides)
        {
            return random.Next(1, sides + 1);
        }

        #endregion

        #endregion

        #endregion

        #region EVENTS



        #endregion
    }

}
