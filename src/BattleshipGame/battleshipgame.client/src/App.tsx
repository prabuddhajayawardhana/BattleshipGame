import { useState, useEffect } from 'react';
import Board from './components/Board';
import './App.css';

const App = () => {
  const [grid, setGrid] = useState<string[][]>([]);
  const [message, setMessage] = useState<string>('');
  const [isGameOver, setIsGameOver] = useState<boolean>(false);

  useEffect(() => {
    fetchGridStatus();
  }, []);

  const fetchGridStatus = async () => {
    const response = await fetch('api/game/status');
    const data = await response.json();
    setGrid(data);
  };

  const handleCellClick = async (x: number, y: number) => {
    if (isGameOver) return; 

    const coordinate = String.fromCharCode(65 + x) + (y + 1);
    const response = await fetch('api/game/shoot', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(coordinate),
    });
    const result = await response.text();
    
    if (result.includes("Game Over")) {
      setIsGameOver(true); 
    }

    setMessage(result);
    fetchGridStatus();
  };

  const handleRestart = async () => {
    const response = await fetch('api/game/restart', {
      method: 'POST',
    });

    if (response.ok) {
      setMessage("Game has been restarted.");
      setIsGameOver(false); 
      fetchGridStatus(); 
    }
  };

  return (
    <div className="App">
      <h1>Battleship Game</h1>
      {isGameOver && <h2 className="game-over">Game Over! You won!</h2>} 
      <Board grid={grid} onCellClick={handleCellClick} />
      <p className="message">{message}</p>

      {isGameOver && (
        <button className="restart-button" onClick={handleRestart}>
          Restart Game
        </button>
      )}
    </div>
  );
};

export default App;
