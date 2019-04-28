using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTBQuestGame.Models
{
    public class Player : Character, IBattle
    {
        #region ENUMS

        public enum JobTitleName { Explorer, MissionLeader, Supervisor }

        #endregion

        private const int DEFENDER_DAMAGE_ADJUSTMENT = 10;
        private const int MAXIMUM_RETREAT_DAMAGE = 10;

        #region FIELDS

        private int _lives;
        private int _health;
        private int _experiencePoints;
        private int _wealth;
        private int _skillLevel;
        private Weapon _currentWeapon;
        private BattleModeName _battleMode;
        private JobTitleName _jobTitle;
        private List<Location> _locationsVisited;
        private ObservableCollection<GameItemQuantity> _inventory;
        private ObservableCollection<GameItemQuantity> _potions;
        private ObservableCollection<GameItemQuantity> _treasure;
        private ObservableCollection<GameItemQuantity> _weapons;
        private ObservableCollection<GameItemQuantity> _relics;

        #endregion

        #region PROPERTIES

        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                OnPropertyChanged(nameof(Lives));
            }
        }

        public JobTitleName JobTitle
        {
            get { return _jobTitle; }
            set
            {
                _jobTitle = value;
                OnPropertyChanged(nameof(JobTitle));
            }
        }

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;

                if (_health > 100)
                {
                    _health = 100;
                }
                else if (_health <= 0)
                {
                    _health = 100;
                    _lives--;
                }

                OnPropertyChanged(nameof(Health));
            }
        }

        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            set
            {
                _experiencePoints = value;
                OnPropertyChanged(nameof(ExperiencePoints));
            }
        }

        public int Wealth
        {
            get { return _wealth; }
            set
            {
                _wealth = value;
                OnPropertyChanged(nameof(Wealth));
            }
        }

        public int SkillLevel
        {
            get { return _skillLevel; }
            set { _skillLevel = value; }
        }

        public Weapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set { _currentWeapon = value; }
        }

        public BattleModeName BattleMode
        {
            get { return _battleMode; }
            set { _battleMode = value; }
        }

        public List<Location> LocationsVisited
        {
            get { return _locationsVisited; }
            set { _locationsVisited = value; }
        }

        public ObservableCollection<GameItemQuantity> Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public ObservableCollection<GameItemQuantity> Weapons
        {
            get { return _weapons; }
            set { _weapons = value; }
        }

        public ObservableCollection<GameItemQuantity> Potions
        {
            get { return _potions; }
            set { _potions = value; }
        }

        public ObservableCollection<GameItemQuantity> Treasure
        {
            get { return _treasure; }
            set { _treasure = value; }
        }

        public ObservableCollection<GameItemQuantity> Relics
        {
            get { return _relics; }
            set { _relics = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public Player()
        {
            _locationsVisited = new List<Location>();
            _weapons = new ObservableCollection<GameItemQuantity>();
            _treasure = new ObservableCollection<GameItemQuantity>();
            _potions = new ObservableCollection<GameItemQuantity>();
            _relics = new ObservableCollection<GameItemQuantity>();
        }

        #endregion

        #region METHODS

        public void CalculateWealth()
        {
            Wealth = _inventory.Sum(i => i.GameItem.Value * i.Quantity);
        }

        /// <summary>
        /// update the game item category lists
        /// </summary>
        public void UpdateInventoryCategories()
        {
            Potions.Clear();
            Weapons.Clear();
            Treasure.Clear();
            Relics.Clear();

            foreach (var gameItemQuantity in _inventory)
            {
                if (gameItemQuantity.GameItem is Potion) Potions.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is Weapon) Weapons.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is Treasure) Treasure.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is Relic) Relics.Add(gameItemQuantity);
            }
        }

        /// <summary>
        /// add selected item to inventory or update quantity if already in inventory
        /// </summary>
        /// <param name="selectedGameItemQuantity">selected item</param>
        public void AddGameItemQuantityToInventory(GameItemQuantity selectedGameItemQuantity)
        {
            //
            // locate selected item in inventory
            //
            GameItemQuantity gameItemQuantity = _inventory.FirstOrDefault(i => i.GameItem.Id == selectedGameItemQuantity.GameItem.Id);

            if (gameItemQuantity == null)
            {
                GameItemQuantity newGameItemQuantity = new GameItemQuantity();
                newGameItemQuantity.GameItem = selectedGameItemQuantity.GameItem;
                newGameItemQuantity.Quantity = 1;

                _inventory.Add(newGameItemQuantity);
            }
            else
            {
                gameItemQuantity.Quantity++;
            }

            UpdateInventoryCategories();
        }

        /// <summary>
        /// remove selected item from inventory
        /// </summary>
        /// <param name="selectedGameItemQuantity">selected item</param>
        public void RemoveGameItemQuantityFromInventory(GameItemQuantity selectedGameItemQuantity)
        {
            //
            // locate selected item in inventory
            //
            GameItemQuantity gameItemQuantity = _inventory.FirstOrDefault(i => i.GameItem.Id == selectedGameItemQuantity.GameItem.Id);

            if (gameItemQuantity != null)
            {
                if (selectedGameItemQuantity.Quantity == 1)
                {
                    _inventory.Remove(gameItemQuantity);
                }
                else
                {
                    gameItemQuantity.Quantity--;
                }
            }

            UpdateInventoryCategories();
        }
        #region BATTLE METHODS

        /// <summary>
        /// return hit points [0 - 100] based on the player's weapon and skill level
        /// </summary>
        /// <returns>hit points 0-100</returns>
        public int Attack()
        {
            int hitPoints = random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * _skillLevel;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// return hit points [0 - 100] based on the player's weapon and skill level
        /// adjusted to deliver more damage when first attacked
        /// </summary>
        /// <returns>hit points 0-100</returns>
        public int Defend()
        {
            int hitPoints = (random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * _skillLevel) - DEFENDER_DAMAGE_ADJUSTMENT;

            if (hitPoints >= 0 && hitPoints <= 100)
            {
                return hitPoints;
            }
            else if (hitPoints > 100)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// return hit points [0 - 100] based on the player's skill level
        /// </summary>
        /// <returns>hit points 0-100</returns>
        public int Retreat()
        {
            int hitPoints = _skillLevel * MAXIMUM_RETREAT_DAMAGE;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }

        #endregion

        /// <summary>
        /// determine if this is a old location
        /// </summary>
        /// <param name="location">old location</param>
        /// <returns></returns>
        public bool HasVisited(Location location)
        {
            return _locationsVisited.Contains(location);
        }

        /// <summary>
        /// override the default greeting in the Character class to include the job title
        /// set the proper article based on the job title
        /// </summary>
        /// <returns>default greeting</returns>
        public override string DefaultGreeting()
        {
            string article = "a";

            List<string> vowels = new List<string>() { "A", "E", "I", "O", "U" };

            if (vowels.Contains(_jobTitle.ToString().Substring(0, 1))) ;
            {
                article = "an";
            }

            return $"Hello, my name is {_name} and I am {article} {_jobTitle} for the Aion Project.";
        }

        #endregion

        #region EVENTS



        #endregion

    }
}
