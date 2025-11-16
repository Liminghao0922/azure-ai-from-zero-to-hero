namespace ManufacturingAi.Web.Models;


public record SensorRecord(
    DateTime Timestamp,
    string MachineId,
    double Temperature,
    double Pressure,
    double Vibration,
    string Status
);

public record MachineStats(
    string MachineId,
    int Count,
    double TempAvg,
    double TempMin,
    double TempMax,
    double VibAvg,
    Dictionary<string, int> StatusDistribution
);

public record DataSummary(
    int TotalRecords,
    int MachineCount,
    List<MachineStats> Machines
);

