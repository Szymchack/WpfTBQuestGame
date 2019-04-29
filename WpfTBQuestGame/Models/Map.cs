using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTBQuestGame.Models
{
    public class Map
    {
        #region FIELDS

        private Location[,] _mapLocations;
        private int _maxRows, _maxColumns;
        private GameMapCoordinates _currentLocationCoordinates;
        private List<GameItem> _standardGameItems;

        #endregion

        #region PROPERTIES

        public Location[,] MapLocations
        {
            get { return _mapLocations; }
            set { _mapLocations = value; }
        }

        public GameMapCoordinates CurrentLocationCoordinates
        {
            get { return _currentLocationCoordinates; }
            set { _currentLocationCoordinates = value; }
        }

        public Location CurrentLocation
        {
            get { return _mapLocations[_currentLocationCoordinates.Row, _currentLocationCoordinates.Column]; }
        }

        public List<GameItem> StandardGameItems
        {
            get { return _standardGameItems; }
            set { _standardGameItems = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public Map(int rows, int columns)
        {
            _maxRows = rows;
            _maxColumns = columns;
            _mapLocations = new Location[rows, columns];
        }

        #endregion

        #region METHODS

        #region MOVEMENT METHODS

        public void MoveNorth()
        {

            if (_currentLocationCoordinates.Row > 0)
            {
                _currentLocationCoordinates.Row -= 1;
            }
        }

        public void MoveEast()
        {
  
            if (_currentLocationCoordinates.Column < _maxColumns - 1)
            {
                _currentLocationCoordinates.Column += 1;
            }
        }

        public void MoveSouth()
        {
            if (_currentLocationCoordinates.Row < _maxRows - 1)
            {
                _currentLocationCoordinates.Row += 1;
            }
        }

        public void MoveWest()
        {

            if (_currentLocationCoordinates.Column > 0)
            {
                _currentLocationCoordinates.Column -= 1;
            }
        }

        public Location NorthLocation()
        {
            Location northLocation = null;

            if (_currentLocationCoordinates.Row > 0)
            {
                Location nextNorthLocation = _mapLocations[_currentLocationCoordinates.Row - 1, _currentLocationCoordinates.Column];


                if (nextNorthLocation != null)
                {
                    northLocation = nextNorthLocation;
                }
            }

            return northLocation;
        }

        public Location EastLocation()
        {
            Location eastLocation = null;


            if (_currentLocationCoordinates.Column < _maxColumns - 1)
            {
                Location nextEastLocation = _mapLocations[_currentLocationCoordinates.Row, _currentLocationCoordinates.Column + 1];


                if (nextEastLocation != null)
                {
                    eastLocation = nextEastLocation;
                }
            }

            return eastLocation;
        }

        public Location SouthLocation()
        {
            Location southLocation = null;


            if (_currentLocationCoordinates.Row < _maxRows - 1)
            {
                Location nextSouthLocation = _mapLocations[_currentLocationCoordinates.Row + 1, _currentLocationCoordinates.Column];

                if (nextSouthLocation != null)
                {
                    southLocation = nextSouthLocation;
                }
            }

            return southLocation;
        }

        public Location WestLocation()
        {
            Location westLocation = null;

   
            if (_currentLocationCoordinates.Column > 0)
            {
                Location nextWestLocation = _mapLocations[_currentLocationCoordinates.Row, _currentLocationCoordinates.Column - 1];

                if (nextWestLocation != null)
                {
                    westLocation = nextWestLocation;
                }
            }

            return westLocation;
        }

        #endregion

        public GameItem GameItemById(int gameItemId)
        {
            return StandardGameItems.FirstOrDefault(i => i.Id == gameItemId);
        }

        public string OpenLocationsByRelic(int relicId)
        {
            string message = "The relic did nothing.";
            Location mapLocation = new Location();

            for (int row = 0; row < _maxRows; row++)
            {
                for (int column = 0; column < _maxColumns; column++)
                {
                    mapLocation = _mapLocations[row, column];

                    if (mapLocation != null && mapLocation.RequiredRelicId == relicId)
                    {
                        mapLocation.Accessible = true;
                        message = $"{mapLocation.Name} is now accessible.";
                    }
                }
            }

            return message;
        }

        #endregion
    }
}
