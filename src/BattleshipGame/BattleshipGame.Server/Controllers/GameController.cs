using BattleshipGame.Application.Interfaces;
using BattleshipGame.Domain.Enums;
using BattleshipGame.SharedKernel.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("start")]
        public ActionResult StartGame()
        {
            _gameService.RestartGame(); 
            return Ok(GameConstants.GameStartedMessage); 
        }

        [HttpPost("shoot")]
        public ActionResult Shoot([FromBody] string coordinate)
        {
            try
            {
                var result = _gameService.Shoot(coordinate.ToUpper());
                return Ok(result.ToString());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("restart")]
        public ActionResult Restart()
        {
            _gameService.RestartGame();
            return Ok(GameConstants.GameRestartedMessage);
        }

        [HttpGet("status")]
        public ActionResult<CellState[][]> GetStatus()
        {
            var visibleGrid = _gameService.GetVisibleGrid();
            return Ok(visibleGrid);
        }
    }
}