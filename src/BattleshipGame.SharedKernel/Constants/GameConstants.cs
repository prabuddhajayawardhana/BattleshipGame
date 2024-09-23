namespace BattleshipGame.SharedKernel.Constants;

public static class GameConstants
{
    public const int GridSize = 10;
    public static readonly char[] Columns = "ABCDEFGHIJ".ToCharArray();

    public const string InvalidCoordinateMessage = "Invalid coordinate";
    public const string GameStartedMessage = "Game has been started.";
    public const string GameRestartedMessage = "Game has been restarted.";
}