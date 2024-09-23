import { useState, useCallback } from 'react';
import { fetchGridStatus, handleCellClick, handleRestart, handleStart } from '../services/GameService';

export const useGame = () => {
    const [grid, setGrid] = useState<string[][]>([]);
    const [message, setMessage] = useState<string>('');
    const [isGameOver, setIsGameOver] = useState<boolean>(false);

    const fetchGameStatus = useCallback(async () => {
        const data = await fetchGridStatus();
        setGrid(data);
    }, []);

    const onCellClick = useCallback(async (x: number, y: number) => {
        if (isGameOver) return;

        const result = await handleCellClick(x, y);
        if (result.includes("GameOver")) {
            setIsGameOver(true);
        }
        setMessage(result);
        fetchGameStatus();
    }, [isGameOver, fetchGameStatus]);

    const restartGame = useCallback(async () => {
        const response = await handleRestart();
        if (response.ok) {
            setMessage("Game has been restarted.");
            setIsGameOver(false);
            fetchGameStatus();
        }
    }, [fetchGameStatus]);

    const startGame = useCallback(async () => {
        const response = await handleStart();
        if (response.ok) {
            setMessage("Game has been started.");
            setIsGameOver(false);
            fetchGameStatus();
        }
    }, [fetchGameStatus]);

    return {
        grid,
        message,
        isGameOver,
        fetchGridStatus: fetchGameStatus,
        handleCellClick: onCellClick,
        handleRestart: restartGame,
        handleStart: startGame,
    };
};
