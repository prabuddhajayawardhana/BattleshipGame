using BattleshipGame.Domain.Enums;

namespace BattleshipGame.Application.Interfaces;

public interface IGameService
{
    void RestartGame();
    GameResult Shoot(string coordinate);
    CellState[][] GetVisibleGrid();
}