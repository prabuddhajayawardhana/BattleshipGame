using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private static readonly char[] Columns = "ABCDEFGHIJ".ToCharArray();
        private static readonly int GridSize = 10;
        private static string[][] grid;
        private static List<Ship> ships;
        private static readonly Random random = new Random();

        public GameController()
        {
            if (grid == null)
            {
                InitializeGame();
            }
        }

        [HttpPost("shoot")]
        public ActionResult Shoot([FromBody] string coordinate)
        {
            coordinate = coordinate.ToUpper();
            var column = coordinate[0];
            var row = int.Parse(coordinate.Substring(1)) - 1;

            int x = Array.IndexOf(Columns, column);
            int y = row;

            if (x < 0 || x >= GridSize || y < 0 || y >= GridSize)
            {
                return BadRequest("Invalid coordinate");
            }

            if (grid[y][x] == "S")
            {
                grid[y][x] = "X"; 
                if (CheckIfSunk(x, y))
                {
                    if (CheckGameOver())
                    {
                        return Ok("Sunk - Game Over! Please restart the game.");
                    }
                    return Ok("Sunk");
                }
                return Ok("Hit");
            }
            else if (grid[y][x] == "X" || grid[y][x] == "M")
            {
                return Ok("Already tried");
            }
            else
            {
                grid[y][x] = "M";
                return Ok("Miss");
            }
        }

        [HttpPost("restart")]
        public ActionResult Restart()
        {
            InitializeGame(); 
            return Ok("Game has been restarted.");
        }

      
        [HttpGet("status")]
        public ActionResult<string[][]> GetStatus()
        {
            return Ok(grid);
        }

        private void InitializeGame()
        {
            grid = new string[GridSize][];
            for (int i = 0; i < GridSize; i++)
            {
                grid[i] = new string[GridSize];
                for (int j = 0; j < GridSize; j++)
                {
                    grid[i][j] = " "; 
                }
            }

            ships = new List<Ship>
            {
                new Ship("Battleship", 5),
                new Ship("Destroyer 1", 4),
                new Ship("Destroyer 2", 4)
            };

            foreach (var ship in ships)
            {
                PlaceShip(ship);
            }
        }

        private void PlaceShip(Ship ship)
        {
            bool placed = false;

            while (!placed)
            {
                int direction = random.Next(2);
                int startX = random.Next(GridSize);
                int startY = random.Next(GridSize);

                if (direction == 0)
                {
                    if (startX + ship.Size <= GridSize && CanPlaceShip(startX, startY, ship.Size, 0))
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            grid[startY][startX + i] = "S";
                            ship.Coordinates.Add((startX + i, startY)); 
                        }
                        placed = true;
                    }
                }
                else 
                {
                    if (startY + ship.Size <= GridSize && CanPlaceShip(startX, startY, ship.Size, 1))
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            grid[startY + i][startX] = "S";
                            ship.Coordinates.Add((startX, startY + i)); 
                        }
                        placed = true;
                    }
                }
            }
        }

        private bool CanPlaceShip(int x, int y, int size, int direction)
        {
            for (int i = 0; i < size; i++)
            {
                if (direction == 0 && grid[y][x + i] == "S") return false; 
                if (direction == 1 && grid[y + i][x] == "S") return false;
            }
            return true;
        }

        private bool CheckIfSunk(int x, int y)
        {
            var ship = ships.FirstOrDefault(s => s.Coordinates.Any(c => c.x == x && c.y == y));
            return ship?.IsSunk(grid) == true;
        }

        private class Ship
        {
            public string Name { get; }
            public int Size { get; }
            public List<(int x, int y)> Coordinates { get; }

            public Ship(string name, int size)
            {
                Name = name;
                Size = size;
                Coordinates = new List<(int x, int y)>();
            }

            public bool IsSunk(string[][] grid)
            {
                return Coordinates.All(coord => grid[coord.y][coord.x] == "X");
            }
        }

        private bool CheckGameOver()
        {
            return ships.All(ship => ship.IsSunk(grid));
        }
    }
}
