using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;

// Use Alt-Shift-F to format. Do this continually!

namespace ScoutApp.ViewModels
{
    // BEGIN enum (part 1)
    public enum AutoStartingPosition
    {
        Left,
        Middle,
        Right
    }
    // END enum (part 1)

    // BEGIN enum (part 1)
    public enum TBreakdown2025
    {
        None,
        Tipped,
        MechanicalFailure,
        Incapacitated,
        CoralStuck,
        StuckOnAlgae
    }
    // END enum (part 1)

    // BEGIN enum (part 1)
    public enum EClimb2025
    {
        Success,
        Failed,
        DidNotAttempt,
        DeepCage,
        Parked,
        ShallowCage
    }
    // END enum (part 1)

    public partial class MainViewModel : ObservableObject
    {
         [ObservableProperty]
        private bool showText = false;

        [RelayCommand]
        private void ToggleText()
        {
            ShowText = !ShowText;
        }

        public MainViewModel()
        {
        }

        // BEGIN string
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private string? _ScoutName;
        // END string

        // BEGIN bool
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _AutoLeave = true;
        // END bool

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeamNumber32 = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _MatchNumber = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL4Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL3Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL2Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL1Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL4Miss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL3Miss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL2Miss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoCoralL1Miss = 0;
        // END int

        // BEGIN bool
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private bool _GroundPickup = false;
        // END bool

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL4Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL3Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL2Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL1Score = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL4Miss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL3Miss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL2Miss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpCoralL1Miss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoProcessorScore = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoProcessorMiss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoNetScore = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _AutoNetMiss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpProcessorScore = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpProcessorMiss = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpNetScore = 0;
        // END int

        // BEGIN int
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private int _TeleOpNetMiss = 0;
        // END int

        // BEGIN enum (part 2)
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private AutoStartingPosition _SPosition2025;
        // END enum (part 2)

        // BEGIN enum (part 2)
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private TBreakdown2025 _TeleOpBreakdown;
        // END enum (part 2)

        // BEGIN enum (part 2)
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Summary))]
        [NotifyPropertyChangedFor(nameof(QRCode1))]
        private EClimb2025 _TeleOpClimb;
        // END enum (part 2)

        // Summary will change based on all the fields that
        // have [NotifyPropertyChangedFor(nameof(Summary))].
        public string Summary
        {
            get
            {
                {
                    return $$"""
ScoutName:{{ScoutName}}
AutoStartingPosition:{{SPosition2025}}
TeamNumber:{{TeamNumber32}}
MatchNumber:{{MatchNumber}}
AutoLeave:{{AutoLeave}}
AutoL4-Score:{{AutoCoralL4Score}}
AutoL3-Score:{{AutoCoralL3Score}}
AutoL2-Score:{{AutoCoralL2Score}}
AutoL1-Score:{{AutoCoralL1Score}}
AutoL4-Miss:{{AutoCoralL4Miss}}
AutoL3-Miss:{{AutoCoralL3Miss}}
AutoL2-Miss:{{AutoCoralL2Miss}}
AutoL1-Miss:{{AutoCoralL1Miss}}
GroundPickup:{{GroundPickup}}
TeleOpL4-Score:{{TeleOpCoralL4Score}}
TeleOpL3-Score:{{TeleOpCoralL3Score}}
TeleOpL2-Score:{{TeleOpCoralL2Score}}
TeleOpL1-Score:{{TeleOpCoralL1Score}}
TeleOpL4-Miss:{{TeleOpCoralL4Miss}}
TeleOpL3-Miss:{{TeleOpCoralL3Miss}}
TeleOpL2-Miss:{{TeleOpCoralL2Miss}}
TeleOpL1-Miss:{{TeleOpCoralL1Miss}}
AutoProcessor-Score:{{AutoProcessorScore}}
AutoProcessor-Miss:{{AutoProcessorMiss}}
AutoNet-Score:{{AutoNetScore}}
AutoNet-Miss:{{AutoNetMiss}}
TeleOpProcessor-Score:{{TeleOpProcessorScore}}
TeleOpProcessor-Miss:{{TeleOpProcessorMiss}}
TeleOpNet-Score:{{TeleOpNetScore}}
TeleOpNet-Miss:{{TeleOpNetMiss}}
TeleOpBreakdown:{{TeleOpBreakdown}}
Climb:{{TeleOpClimb}}
""";
                }
            }
        }

        // QRCode1 will change based on all the fields that
        // have [NotifyPropertyChangedFor(nameof(QRCode1))].
        public Bitmap QRCode1
        {
            get
            {
                string textForQRCode =
                    $$"""
Name-{{ScoutName}}
SPos-{{SPosition2025}}
Team-{{TeamNumber32}}
Match-{{MatchNumber}}
ALeave-{{AutoLeave}}
AL4-{{AutoCoralL4Score}}
AL3-{{AutoCoralL3Score}}
AL2-{{AutoCoralL2Score}}
AL1-{{AutoCoralL1Score}}
AM4-{{AutoCoralL4Miss}}
AM3-{{AutoCoralL3Miss}}
AM2-{{AutoCoralL2Miss}}
AM1-{{AutoCoralL1Miss}}
GPickup-{{GroundPickup}}
TL4-{{TeleOpCoralL4Score}}
TL3-{{TeleOpCoralL3Score}}
TL2-{{TeleOpCoralL2Score}}
TL1-{{TeleOpCoralL1Score}}
TM4-{{TeleOpCoralL4Miss}}
TM3-{{TeleOpCoralL3Miss}}
TM2-{{TeleOpCoralL2Miss}}
TM1-{{TeleOpCoralL1Miss}}
APS-{{AutoProcessorScore}}
APM-{{AutoProcessorMiss}}
ANS-{{AutoNetScore}}
ANM-{{AutoNetMiss}}
TPS-{{TeleOpProcessorScore}}
TPM-{{TeleOpProcessorMiss}}
TNS-{{TeleOpNetScore}}
TNM-{{TeleOpNetMiss}}
TBK-{{TeleOpBreakdown}}
CLB-{{TeleOpClimb}}
""";
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(textForQRCode, QRCodeGenerator.ECCLevel.Q))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] data = qrCode.GetGraphic(20);
                    return new Bitmap(new MemoryStream(data));
                }
            }
        }
    }
}
