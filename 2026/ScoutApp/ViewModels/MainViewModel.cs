using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.Serialization;

namespace ScoutApp.ViewModels
{
    public enum HeadingButtons
    {
        PreMatch,
        Auto,
        TeleOp,
        Endgame,
        PostMatch
    }

    public enum AlliancePosition
    {
        [EnumMember(Value = "Red 1")] Red1,
        [EnumMember(Value = "Red 2")] Red2,
        [EnumMember(Value = "Red 3")] Red3,
        [EnumMember(Value = "Blue 1")] Blue1,
        [EnumMember(Value = "Blue 2")] Blue2,
        [EnumMember(Value = "Blue 3")] Blue3
    }

    public enum Alliance
    {
        Red,
        Blue
    }

    public enum AutoStartingPosition
    {
        [EnumMember(Value = "Left Trench")] LeftTrench,
        [EnumMember(Value = "Left Bump")] LeftBump,
        [EnumMember(Value = "Center")] Center,
        [EnumMember(Value = "Right Bump")] RightBump,
        [EnumMember(Value = "Right Trench")] RightTrench
    }

    public enum Breakdown2026
    {
        [EnumMember(Value = "None")] None,
        [EnumMember(Value = "Tipped")] Tipped,
        [EnumMember(Value = "Mechanical Failure")] MechanicalFailure,
        [EnumMember(Value = "Connection Failure")] ConnectionFailure,
        [EnumMember(Value = "Disabled")] Disabled,
        [EnumMember(Value = "Beached on Fuel")] BeachedOnFuel,
        [EnumMember(Value = "Stuck on Bump")] StuckOnBump
    }

    public enum Climb2026
    {
        [EnumMember(Value = "Level 1")] Level1,
        [EnumMember(Value = "Level 2")] Level2,
        [EnumMember(Value = "Level 3")] Level3,
        [EnumMember(Value = "Failed")] Failed,
        [EnumMember(Value = "Did Not Attempt")] DidNotAttempt
    }

    public enum ClimbTimes
    {
        [EnumMember(Value = "0-10 Seconds")] _0to10Seconds,
        [EnumMember(Value = "10-20 Seconds")] _10to20Seconds,
        [EnumMember(Value = "20-30 Seconds")] _20to30Seconds,
        [EnumMember(Value = "30+ Seconds")] _30PlusSeconds
    }

    public enum BreakdownTimes
    {
        [EnumMember(Value = "0-15 Seconds")] _0to15Seconds,
        [EnumMember(Value = "15-30 Seconds")] _15to30Seconds,
        [EnumMember(Value = "30+ Seconds")] _30PlusSeconds
    }

    public partial class MainViewModel : ObservableObject
    {
        public PreMatchViewModel PreMatchVM { get; } = new PreMatchViewModel();

        public AutoViewModel AutoVM { get; } = new AutoViewModel();

        public TeleopViewModel TeleopVM { get; } = new TeleopViewModel();

        public PostMatchViewModel PostMatchVM { get; }

        public MainViewModel()
        {
            PostMatchVM = new(PreMatchVM, AutoVM, TeleopVM);

            PostMatchVM.NextMatchRequested += () => SelectedHeadingButton = HeadingButtons.PreMatch;
        }

        [ObservableProperty]
        private HeadingButtons _SelectedHeadingButton = HeadingButtons.PreMatch;

        [RelayCommand]
        private void HeadingButton(HeadingButtons heading)
        {
            SelectedHeadingButton = heading;
        }

        [ObservableProperty]
        public int _TextSize = 20;

        [ObservableProperty]
        public int _HeadingsTextSize = 24;

    }
}
