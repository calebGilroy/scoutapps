using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using Avalonia.Platform;

namespace ScoutApp.ViewModels
{
    public enum AutoStartingPosition
    {
        LEFT_BUMP,
        LEFT_TRENCH,
        CENTER,
        RIGHT_TRENCH,
        RIGHT_BUMP
    }

    public enum Breakdown2026
    {
        NONE,
        TIPPED,
        MECHANICAL_FAILURE,
        CONNECTION_FAILURE,
        DISABLED,
        BEACHED_ON_FUEL
    }

    public enum Climb2026
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        FAILED,
        DID_NOT_ATTEMPT
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
        RED_1,
        RED_2,
        RED_3,
        BLUE_1,
        BLUE_2,
        BLUE_3
    }

    public enum Alliance
    {
        Red,
        Blue
    }

    public partial class MainViewModel : ObservableObject
    {
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
            Breakdown = Breakdown2026.NONE;
            ShowSummary = false;
            SelectedHeadingButton = HeadingButtons.PreMatch;
        }

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
            else if (value is AlliancePosition.RED_1 or AlliancePosition.RED_2 or AlliancePosition.RED_3)
            {
                SelectedAlliance = Alliance.Red;
            }
            else if (value is AlliancePosition.BLUE_1 or AlliancePosition.BLUE_2 or AlliancePosition.BLUE_3)
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
        private bool showSummary = false;

        [RelayCommand]
        private void ToggleSummary()
        {
            ShowSummary = !ShowSummary;
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
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        [NotifyPropertyChangedFor(nameof(MissingFields))]
        private Climb2026? _TeleOpClimb;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private Breakdown2026 _Breakdown = Breakdown2026.NONE;

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
Breakdown: {{Breakdown}}
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
ClimbTime-6
Breakdown-{{Breakdown}}
BreakdownTime-5
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
                if (missing.Count == 0) return string.Empty;
                return "Missing required fields:\n" + string.Join("\n", missing.Select(f => "  \u2022 " + f));
            }
        }
    }
}
