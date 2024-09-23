import { useEffect } from 'react';
import Board from './components/Board';
import { useGame } from './hooks/useGame';
import './App.css';

const App = () => {
  const { grid, message, isGameOver, fetchGridStatus, handleCellClick, handleRestart, handleStart } = useGame();

  useEffect(() => {
    fetchGridStatus();
  }, [fetchGridStatus]);

  return (
    <div className="App">
      <h1>Battleship Game</h1>
      {isGameOver && <h2 className="game-over">Game Over! You won!</h2>}
      <Board grid={grid} onCellClick={handleCellClick} />
      <p className="message">{message}</p>

      {!isGameOver && (
        <button className="start-button" onClick={handleStart}>
          Start Game
        </button>
      )}

      {isGameOver && (
        <button className="restart-button" onClick={handleRestart}>
          Restart Game
        </button>
      )}
    </div>
  );
};

export default App;
