using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using Avalonia.Platform;
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
        [EnumMember(Value = "Left Bump")] LeftBump,
        [EnumMember(Value = "Left Trench")] LeftTrench,
        [EnumMember(Value = "Center")] Center,
        [EnumMember(Value = "Right Trench")] RightTrench,
        [EnumMember(Value = "Right Bump")] RightBump
    }

    public enum Breakdown2026
    {
        [EnumMember(Value = "None")] None,
        [EnumMember(Value = "Tipped")] Tipped,
        [EnumMember(Value = "Mechanical Failure")] MechanicalFailure,
        [EnumMember(Value = "Connection Failure")] ConnectionFailure,
        [EnumMember(Value = "Disabled")] Disabled,
        [EnumMember(Value = "Beached on Fuel")] BeachedOnFuel
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
        [EnumMember(Value = "~0-15 Seconds")] _0to10Seconds,
        [EnumMember(Value = "~10-20 Seconds")] _10to20Seconds,
        [EnumMember(Value = "~20-30 Seconds")] _20to30Seconds,
        [EnumMember(Value = "~30+ Seconds")] _30PlusSeconds
    }

    public enum BreakdownTimes
    {
        [EnumMember(Value = "~0-15 Seconds")] _0to15Seconds,
        [EnumMember(Value = "~15-30 Seconds")] _15to30Seconds,
        [EnumMember(Value = "~30+ Seconds")] _30PlusSeconds
    }

    public partial class MainViewModel : ObservableObject
    {
        private static int? GetTeamNumberFromSchedule(int matchNumber, AlliancePosition? alliance)
        {
            if (alliance == null)
                return null;

            return JsonToCSConverter.TryGetTeamNumber(matchNumber, alliance.Value, out int teamNumber)
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

        partial void OnSelectedAlliancePositionChanged(AlliancePosition? value)
        {
            UpdateTeamNumberFromSchedule();

            if (value == null)
            {
                SelectedAlliance = null;
            }
            else if (value is AlliancePosition.Red1 or AlliancePosition.Red2 or AlliancePosition.Red3)
            {
                SelectedAlliance = Alliance.Red;
            }
            else if (value is AlliancePosition.Blue1 or AlliancePosition.Blue2 or AlliancePosition.Blue3)
            {
                SelectedAlliance = Alliance.Blue;
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        [NotifyPropertyChangedFor(nameof(MissingFields))]
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
        public int _TextSize = 16;

        [ObservableProperty]
        public int _HeadingsTextSize = 18;

        public List<string>? ScoutNames { get; } = new List<string>
        {
            "Mentor",
            "Aarush Manoj",
            "Aditi Boddu",
            "Alan Maldonado",
            "Anders Lilly",
            "Andrew Bogen",
            "Aram Kim",
            "Ateev Singh",
            "Audrey Le",
            "Avani Pullela",
            "Blair Liebman",
            "Bridgette Reinecke",
            "Caleb Gilroy",
            "Damien Phan",
            "Dagmawi Tewodros",
            "Dhananjay Giridharan",
            "Diganth Vijay Kumar",
            "Elise Walters",
            "Elizabeth Huang",
            "Embry Ciaravella",
            "Gabe Cronk",
            "Garrett Munson",
            "Gavin Dalton-Higbee",
            "Gelilla Kifle",
            "Grayden Corkins",
            "Hailey Rosenau",
            "Halak Patel",
            "Holly Hills",
            "Ibrahim Yusufov",
            "Jaxten Hammersmith",
            "Jaxon Neilson",
            "Kate Rhoades",
            "Kendrich Calub",
            "Landon Theaker",
            "Liliana Hills",
            "Lucas Fabela",
            "Manasi Sabarinath",
            "Martina Butlay",
            "Michael Lee",
            "Michael Workneh",
            "Mia Steffen",
            "Parth Dixit",
            "Paul Daly III",
            "Rayan Ahmad",
            "Rithvik Chokkakula",
            "Rithvika Kondeti",
            "Ritvik Rajkumar",
            "Sankalp Arya",
            "Siddharth Singaravadivelu",
            "Sophia Pucek",
            "Sritanvi Gopu",
            "Sripad Kandala",
            "Surriyaa Sudhakar",
            "Tanvi Somayajula",
            "Tejit Kumar",
            "Tvisha Prajapati",
            "Varshini Karthik",
            "Viljami Baker",
            "Vinamn Datta",
            "Yu-Chen (Emily) Lin"
        };

        [RelayCommand]
        private void AutoScoredUp()
        {
            AutoFuelScored += 1;
        }
        [RelayCommand]
        private void AutoScoredDown()
        {
            AutoFuelScored -= 1;
            if (AutoFuelScored < 0)
                AutoFuelScored = 0;
        }
        [RelayCommand]
        private void AutoMissedUp()
        {
            AutoFuelMissed += 1;
        }
        [RelayCommand]
        private void AutoMissedDown()
        {
            AutoFuelMissed -= 1;
            if (AutoFuelMissed < 0)
                AutoFuelMissed = 0;
        }
        [RelayCommand]
        private void TeleOpScoredUp()
        {
            TeleOpFuelScored += 1;
        }
        [RelayCommand]
        private void TeleOpScoredDown()
        {
            TeleOpFuelScored -= 1;
            if (TeleOpFuelScored < 0)
                TeleOpFuelScored = 0;
        }
        [RelayCommand]
        private void TeleOpMissedUp()
        {
            TeleOpFuelMissed += 1;
        }
        [RelayCommand]
        private void TeleOpMissedDown()
        {
            TeleOpFuelMissed -= 1;
            if (TeleOpFuelMissed < 0)
                TeleOpFuelMissed = 0;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        [NotifyPropertyChangedFor(nameof(MissingFields))]
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
        [NotifyPropertyChangedFor(nameof(MissingFields))]
        private AlliancePosition? _SelectedAlliancePosition;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private Alliance? _SelectedAlliance;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        [NotifyPropertyChangedFor(nameof(MissingFields))]
        private AutoStartingPosition? _StartingPosition;

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
        [NotifyPropertyChangedFor(nameof(MissingFields))]
        private Climb2026? _AutoClimb;

        // ===== AUTO OBSTACLES =====
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoBump = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoTrench = false;

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
        private bool _TeleOpBump = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _TeleOpTrench = false;

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
        [NotifyPropertyChangedFor(nameof(TeleOpClimbTime))]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        [NotifyPropertyChangedFor(nameof(MissingFields))]
        private Climb2026? _TeleOpClimb;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private ClimbTimes? _TeleOpClimbTime;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        [NotifyPropertyChangedFor(nameof(MissingFields))]
        private Breakdown2026 _Breakdown = Breakdown2026.None;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        [NotifyPropertyChangedFor(nameof(MissingFields))]
        private BreakdownTimes? _BreakdownTime;

        [ObservableProperty]
        private bool _ShowClimbTimes = false;

        partial void OnTeleOpClimbChanged(Climb2026? value)
        {
            ShowClimbTimes = value is Climb2026.Level1 or Climb2026.Level2 or Climb2026.Level3;

            if (!ShowClimbTimes)
                TeleOpClimbTime = null;
        }

        [ObservableProperty]
        private bool _ShowBreakdownTimes = false;

        partial void OnBreakdownChanged(Breakdown2026 value)
        {
            ShowBreakdownTimes = value != Breakdown2026.None;

            if (!ShowBreakdownTimes)
                BreakdownTime = null;
        }

        public string MissingFields
        {
            get
            {
                var missing = new List<string>();
                if (string.IsNullOrEmpty(ScoutName)) missing.Add("Scout Name");
                if (SelectedAlliancePosition == null) missing.Add("Alliance Position");
                if (StartingPosition == null) missing.Add("Starting Position");
                if (AutoClimb == null) missing.Add("Auto Climb");
                if (TeleOpClimb == null) missing.Add("Endgame Tower Climb");
                if (TeleOpClimbTime == null && TeleOpClimb != null && TeleOpClimb != Climb2026.DidNotAttempt && TeleOpClimb != Climb2026.Failed) missing.Add("Endgame Climb Time");
                if (BreakdownTime == null && Breakdown != Breakdown2026.None) missing.Add("Breakdown Time");
                if (missing.Count == 0) return string.Empty;
                return "Missing required fields:\n" + string.Join("\n", missing.Select(f => "  \u2022 " + f));
            }
        }

        [ObservableProperty]
        private bool showSummary = false;

        [RelayCommand]
        private void ToggleSummary()
        {
            ShowSummary = !ShowSummary;
        }

        public string Summary
        {
            get
            {
                return $$"""
Team Number: {{TeamNumber}}
Match Number: {{MatchNumber}}
Scout Name: {{ScoutName}}
Alliance: {{SelectedAlliance}}
Starting Position: {{StartingPosition}}
Auto Move: {{AutoMove}}
Auto Fuel Scored: {{AutoFuelScored}}
Auto Fuel Missed: {{AutoFuelMissed}}
Auto Bump: {{AutoBump}}
Auto Trench: {{AutoTrench}}
Auto Intake Depot: {{AutoIntakeDepot}}
Auto Intake Outpost: {{AutoIntakeOutpost}}
Auto Intake Neutral Zone: {{AutoIntakeNeutralZone}}
Auto Climb: {{AutoClimb}}
TeleOp Fuel Scored: {{TeleOpFuelScored}}
TeleOp Fuel Missed: {{TeleOpFuelMissed}}
TeleOp Bump: {{TeleOpBump}}
TeleOp Trench: {{TeleOpTrench}}
TeleOp Intake Depot: {{TeleOpIntakeDepot}}
TeleOp Intake Outpost: {{TeleOpIntakeOutpost}}
TeleOp Intake Neutral Zone: {{TeleOpIntakeNeutralZone}}
Endgame Tower Climb: {{TeleOpClimb}}
Endgame Climb Time: {{TeleOpClimbTime}}
Breakdown: {{Breakdown}}
Breakdown Time: {{BreakdownTime}}
""";
            }
        }

        public Bitmap QRCode1
        {
            get
            {
                string textForQRCode =
                    $$"""
Team-{{TeamNumber}}
Match-{{MatchNumber}}
Name-{{ScoutName}}
Alliance-{{SelectedAlliance}}
StartingPosition-{{StartingPosition}}
AutoMove-{{AutoMove}}
AutoFuelScored-{{AutoFuelScored}}
AutoFuelMissed-{{AutoFuelMissed}}
AutoBump-{{AutoBump}}
AutoTrench-{{AutoTrench}}
AutoIntakeDepot-{{AutoIntakeDepot}}
AutoIntakeOutpost-{{AutoIntakeOutpost}}
AutoIntakeNeutralZone-{{AutoIntakeNeutralZone}}
AutoClimb-{{AutoClimb}}
TeleOpFuelScored-{{TeleOpFuelScored}}
TeleOpFuelMissed-{{TeleOpFuelMissed}}
TeleOpBump-{{TeleOpBump}}
TeleOpTrench-{{TeleOpTrench}}
TeleOpIntakeDepot-{{TeleOpIntakeDepot}}
TeleOpIntakeOutpost-{{TeleOpIntakeOutpost}}
TeleOpIntakeNeutralZone-{{TeleOpIntakeNeutralZone}}
TowerClimb-{{TeleOpClimb}}
ClimbTime-{{TeleOpClimbTime}}
Breakdown-{{Breakdown}}
BreakdownTime-{{BreakdownTime}}
""";

                if (string.IsNullOrEmpty(ScoutName) || SelectedAlliancePosition == null || StartingPosition == null || TeleOpClimb == null || AutoClimb == null || SelectedHeadingButton != HeadingButtons.PostMatch)
                {
                    using var stream = AssetLoader.Open(new Uri("avares://ScoutApp/Assets/cartman.jpg"));
                    return new Bitmap(stream);
                }

                using QRCodeGenerator qrGenerator = new();
                using QRCodeData qrCodeData = qrGenerator.CreateQrCode(textForQRCode, QRCodeGenerator.ECCLevel.Q);
                using PngByteQRCode qrCode = new(qrCodeData);
                byte[] data = qrCode.GetGraphic(20);
                return new Bitmap(new MemoryStream(data));
            }
        }

        [RelayCommand]
        private void NextMatch()
        {
            MatchNumber += 1;
            StartingPosition = null;
            AutoMove = false;
            // Auto FUEL
            AutoFuelScored = 0;
            AutoFuelMissed = 0;
            AutoClimb = null;
            // Auto Obstacles
            AutoBump = false;
            AutoTrench = false;
            // Auto Intake
            AutoIntakeDepot = false;
            AutoIntakeOutpost = false;
            AutoIntakeNeutralZone = false;
            // TeleOp FUEL
            TeleOpFuelScored = 0;
            TeleOpFuelMissed = 0;
            // TeleOp Obstacles
            TeleOpBump = false;
            TeleOpTrench = false;
            // TeleOp Intake
            TeleOpIntakeDepot = false;
            TeleOpIntakeOutpost = false;
            TeleOpIntakeNeutralZone = false;
            // Endgame
            TeleOpClimb = null;
            TeleOpClimbTime = null;
            ShowClimbTimes = false;
            Breakdown = Breakdown2026.None;
            BreakdownTime = null;
            ShowBreakdownTimes = false;
            ShowSummary = false;
            SelectedHeadingButton = HeadingButtons.PreMatch;
        }
    }
}
