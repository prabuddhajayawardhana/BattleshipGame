using BattleshipGame.Application.Interfaces;
using BattleshipGame.Application.Services;
using BattleshipGame.Domain.Entities;
using BattleshipGame.Domain.Enums;
using Moq;

namespace BattleshipGame.Tests;
public class GameServiceTests
{
    private readonly GameService _gameService;
    private readonly Mock<IGridService> _gridServiceMock;

    public GameServiceTests()
    {
        _gridServiceMock = new Mock<IGridService>();
        _gameService = new GameService(_gridServiceMock.Object);
    }

    [Fact]
    public void RestartGame_ShouldInitializeGrid()
    {
        // Arrange
        _gridServiceMock.Setup(gs => gs.InitializeGrid(It.IsAny<List<Ship>>()))
            .Returns(new CellState[10][]);

        // Act

        // Assert
        var gridStatus = _gameService.GetVisibleGrid();
        Assert.NotNull(gridStatus);
    }

    [Fact]
    public void Shoot_ValidHit_ShouldReturnHit()
    {
        // Arrange
        string coordinate = "A1";
        _gridServiceMock.Setup(gs => gs.InitializeGrid(It.IsAny<List<Ship>>()))
            .Returns(new CellState[][] {
                    new CellState[] { CellState.Ship, CellState.Empty },
                    new CellState[] { CellState.Empty, CellState.Empty }
            });

        _gameService.RestartGame();

        // Act
        var result = _gameService.Shoot(coordinate);

        // Assert
        Assert.Equal(GameResult.Hit, result);
    }

    [Fact]
    public void Shoot_ValidMiss_ShouldReturnMiss()
    {
        // Arrange
        string coordinate = "B2";
        _gridServiceMock.Setup(gs => gs.InitializeGrid(It.IsAny<List<Ship>>()))
            .Returns(new CellState[][] {
                    new CellState[] { CellState.Ship, CellState.Empty },
                    new CellState[] { CellState.Empty, CellState.Empty }
            });

        _gameService.RestartGame();

        // Act
        var result = _gameService.Shoot(coordinate);

        // Assert
        Assert.Equal(GameResult.Miss, result);
    }

    [Fact]
    public void Shoot_AlreadyTried_ShouldReturnAlreadyTried()
    {
        // Arrange
        string coordinate = "A1";
        _gridServiceMock.Setup(gs => gs.InitializeGrid(It.IsAny<List<Ship>>()))
            .Returns(new CellState[][] {
                    new CellState[] { CellState.Ship, CellState.Empty },
                    new CellState[] { CellState.Empty, CellState.Empty }
            });

        _gameService.RestartGame();
        _gameService.Shoot(coordinate); // First attempt

        // Act
        var result = _gameService.Shoot(coordinate); // Second attempt

        // Assert
        Assert.Equal(GameResult.AlreadyTried, result);
    }
}
