using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GameExample;
using MonoGame.Framework;

namespace Platformer2D
{
    public partial class GamePage : Page
    {
        #region Class fields
        private PlatformerGame game;

        private readonly SolidColorBrush yellow = new SolidColorBrush(Colors.Yellow);
        private readonly SolidColorBrush red = new SolidColorBrush(Colors.Red);
        private GameState gameState;
        #endregion

        #region Constructor
        // Constructor
        public GamePage()
        {
            InitializeComponent();

            Window.Current.Activated += OnActivated;
            StartGameAsync();
        }
        #endregion

        #region Events
        private void OnActivated(object sender, WindowActivatedEventArgs e)
        {
            if (gameState != null && e.WindowActivationState == CoreWindowActivationState.Deactivated)
                XmlLoadManager<GameState>.Save(gameState, "Save.xml");
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateUiSize(e.NewSize.Width, e.PreviousSize.Width > 0 ? e.PreviousSize.Width : PlatformerGame.baseScreenSize.X);

            if (game != null)
                game.UpdateGlobalTransformation();
        }
        
        private void OnUpdateUi(Level level)
        {
            Timer.Text = string.Format("TIME: {0:mm\\:ss}", level.TimeRemaining);
            Timer.Foreground = level.TimeRemaining > PlatformerGame.WarningTime ||
                               level.ReachedExit ||
                               (int)level.TimeRemaining.TotalSeconds % 2 == 0
                ? yellow
                : red;

            Score.Text = string.Format("SCORE: {0}", level.Score);
        }
        #endregion

        #region Private methdos
        private async void StartGameAsync()
        {
            gameState = (await XmlLoadManager<GameState>.Load("Save.xml")) ?? new GameState();

            game = XamlGame<PlatformerGame>.Create("", Window.Current.CoreWindow, GamePanel);
           
            game.UpdateUI += OnUpdateUi;
            SizeChanged += OnSizeChanged;
            
            game.Start(gameState);
        }
        
        private void UpdateUiSize(double newWidth, double prevWidth)
        {
            Timer.FontSize *= newWidth/prevWidth;
            Score.FontSize *= newWidth/prevWidth;
        }
        #endregion
    }
}