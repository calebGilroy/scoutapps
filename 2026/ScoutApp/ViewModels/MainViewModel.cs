using System.IO;
using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using Avalonia.Platform;

namespace ScoutApp.ViewModels
{
    public enum AutoStartingPosition
    {
        Left,
        Center,
        Right
    }

    public enum Breakdown2026
    {
        None,
        Tipped,
        MechanicalFailure,
        Incapacitated,
        Disabled,
        Beached
    }

    public enum TowerClimb2026
    {
        Level1,
        Level2,
        Level3,
        Failed,
        DidNotAttempt
    }

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
        Red1,
        Red2,
        Red3,
        Blue1,
        Blue2,
        Blue3
    }

    public partial class MainViewModel : ObservableObject
    {
        [RelayCommand]
        private void NextMatch()
        {
            MatchNumber += 1;
            SPosition2026 = null;
            AutoMove = false;
            // Auto FUEL
            AutoFuelScored = 0;
            AutoFuelMissed = 0;
            AutoTowerClimb = null;
            // Auto Obstacles
            AutoUsedBump = false;
            AutoUsedTrench = false;
            // Auto Intake
            AutoIntakeDepot = false;
            AutoIntakeOutpost = false;
            AutoIntakeNeutralZone = false;
            // TeleOp FUEL
            TeleOpFuelScored = 0;
            TeleOpFuelMissed = 0;
            // TeleOp Obstacles
            TeleOpUsedBump = false;
            TeleOpUsedTrench = false;
            // TeleOp Intake
            TeleOpIntakeDepot = false;
            TeleOpIntakeOutpost = false;
            TeleOpIntakeNeutralZone = false;
            // Endgame
            TowerClimb = null;
            Breakdown = Breakdown2026.None;
            DefenseRating = 0;
            Notes = string.Empty;
            ShowSummary = false;
            SelectedHeadingButton = HeadingButtons.PreMatch;
        }

        private static int? GetTeamNumberFromSchedule(int matchNumber, AlliancePosition? alliancePosition)
        {
            if (alliancePosition == null)
                return null;

            return JsonToCSConverter.TryGetTeamNumber(matchNumber, alliancePosition.Value, out int teamNumber)
                ? teamNumber
                : null;
        }

        [RelayCommand]
        private void UpdateTeamNumberFromSchedule()
        {
            int? scheduledTeamNumber = GetTeamNumberFromSchedule(MatchNumber, SelectedAlliancePosition);
            if (scheduledTeamNumber.HasValue)
                TeamNumber = scheduledTeamNumber.Value;
        }

        partial void OnMatchNumberChanged(int value)
        {
            UpdateTeamNumberFromSchedule();
        }

        partial void OnSelectedAlliancePositionChanged(AlliancePosition? value) => UpdateTeamNumberFromSchedule();

        [ObservableProperty]
        private HeadingButtons _SelectedHeadingButton = HeadingButtons.PreMatch;

        [RelayCommand]
        private void HeadingButton(object button)
        {
            if (button is string heading)
            {
                switch (heading)
                {
                    case "PreMatch":
                        SelectedHeadingButton = HeadingButtons.PreMatch;
                        break;
                    case "Auto":
                        SelectedHeadingButton = HeadingButtons.Auto;
                        break;
                    case "TeleOp":
                        SelectedHeadingButton = HeadingButtons.TeleOp;
                        break;
                    case "Endgame":
                        SelectedHeadingButton = HeadingButtons.Endgame;
                        break;
                    case "PostMatch":
                        SelectedHeadingButton = HeadingButtons.PostMatch;
                        break;
                }
            }
        }

        [ObservableProperty]
        private bool showSummary = false;

        [RelayCommand]
        private void ToggleSummary()
        {
            ShowSummary = !ShowSummary;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private string? _ScoutName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _MatchNumber = 1;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int? _TeamNumber;

        [ObservableProperty]
        private AlliancePosition? _SelectedAlliancePosition;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private AutoStartingPosition? _SPosition2026;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoMove = false;

        // ===== AUTO FUEL =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoFuelScored = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoFuelMissed = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private TowerClimb2026? _AutoTowerClimb;

        // ===== AUTO OBSTACLES =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoUsedBump = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoUsedTrench = false;

        // ===== AUTO INTAKE =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoIntakeDepot = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoIntakeOutpost = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoIntakeNeutralZone = false;

        // ===== TELEOP FUEL =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpFuelScored = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpFuelMissed = 0;

        // ===== TELEOP OBSTACLES =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _TeleOpUsedBump = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _TeleOpUsedTrench = false;

        // ===== TELEOP INTAKE =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _TeleOpIntakeDepot = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _TeleOpIntakeOutpost = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _TeleOpIntakeNeutralZone = false;

        // ===== ENDGAME =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private TowerClimb2026? _TowerClimb;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private Breakdown2026 _Breakdown = Breakdown2026.None;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _DefenseRating = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private string _Notes = string.Empty;

        public string Summary
        {
            get
            {
                return $$"""
Scout Name: {{ScoutName}}
Team Number: {{TeamNumber}}
Match Number: {{MatchNumber}}
Alliance Position: {{SelectedAlliancePosition}}
Auto Starting Position: {{SPosition2026}}
Auto Move: {{AutoMove}}
Auto FUEL Scored: {{AutoFuelScored}}
Auto FUEL Missed: {{AutoFuelMissed}}
Auto Tower Climb: {{AutoTowerClimb}}
Auto Used Bump: {{AutoUsedBump}}
Auto Used Trench: {{AutoUsedTrench}}
Auto Intake Depot: {{AutoIntakeDepot}}
Auto Intake Outpost: {{AutoIntakeOutpost}}
Auto Intake Neutral Zone: {{AutoIntakeNeutralZone}}
TeleOp FUEL Scored: {{TeleOpFuelScored}}
TeleOp FUEL Missed: {{TeleOpFuelMissed}}
TeleOp Used Bump: {{TeleOpUsedBump}}
TeleOp Used Trench: {{TeleOpUsedTrench}}
TeleOp Intake Depot: {{TeleOpIntakeDepot}}
TeleOp Intake Outpost: {{TeleOpIntakeOutpost}}
TeleOp Intake Neutral Zone: {{TeleOpIntakeNeutralZone}}
Endgame Tower Climb: {{TowerClimb}}
Breakdown: {{Breakdown}}
Defense Rating: {{DefenseRating}}
Notes: {{Notes}}
""";
            }
        }

        public Bitmap QRCode1
        {
            get
            {
                string textForQRCode =
                    $$"""
Name-{{ScoutName}}
Team-{{TeamNumber}}
Match-{{MatchNumber}}
APos-{{SelectedAlliancePosition}}
SPos-{{SPosition2026}}
AMove-{{AutoMove}}
AFS-{{AutoFuelScored}}
AFM-{{AutoFuelMissed}}
ATC-{{AutoTowerClimb}}
ABump-{{AutoUsedBump}}
ATrench-{{AutoUsedTrench}}
AIDep-{{AutoIntakeDepot}}
AIOut-{{AutoIntakeOutpost}}
AINZ-{{AutoIntakeNeutralZone}}
TFS-{{TeleOpFuelScored}}
TFM-{{TeleOpFuelMissed}}
TBump-{{TeleOpUsedBump}}
TTrench-{{TeleOpUsedTrench}}
TIDep-{{TeleOpIntakeDepot}}
TIOut-{{TeleOpIntakeOutpost}}
TINZ-{{TeleOpIntakeNeutralZone}}
TC-{{TowerClimb}}
BD-{{Breakdown}}
DEF-{{DefenseRating}}
NOTE-{{Notes}}
""";

                if (SelectedAlliancePosition == null || SPosition2026 == null || TowerClimb == null)
                {
                    var uri = new Uri("avares://ScoutApp/Assets/cartman.png");
                    using var stream = AssetLoader.Open(uri);
                    return new Bitmap(stream);
                }

                using QRCodeGenerator qrGenerator = new();
                using QRCodeData qrCodeData = qrGenerator.CreateQrCode(textForQRCode, QRCodeGenerator.ECCLevel.Q);
                using PngByteQRCode qrCode = new(qrCodeData);
                byte[] data = qrCode.GetGraphic(20);
                return new Bitmap(new MemoryStream(data));
            }
        }
    }
}
