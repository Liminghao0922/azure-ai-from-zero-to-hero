using CsvHelper;
using CsvHelper.Configuration;
using ManufacturingAi.Web.Models;
using System.Globalization;

namespace ManufacturingAi.Web.Services;

public class SensorParser
{
    public async Task<(List<SensorRecord> records, DataSummary summary)> ParseAsync(Stream csvStream)
    {
        var records = new List<SensorRecord>();
        using var reader = new StreamReader(csvStream, leaveOpen: true);
        csvStream.Position = 0;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            DetectDelimiter = true,
        };
        using var csv = new CsvReader(reader, config);
        var headerRead = false;
        while (await csv.ReadAsync())
        {
            if (!headerRead)
            {
                headerRead = true; // skip header mapping
            }
            try
            {
                var row = csv.Parser.Record;
                // Expected order fallback: 時刻,機械ID,温度(℃),圧力(MPa),振動レベル,状態
                DateTime ts = DateTime.Parse(row[0]);
                string mid = row[1];
                double temp = double.Parse(row[2]);
                double pressure = double.Parse(row[3]);
                double vib = double.Parse(row[4]);
                string status = row.Length > 5 ? row[5] : "Unknown";
                records.Add(new SensorRecord(ts, mid, temp, pressure, vib, status));
            }
            catch
            {
                // ignore malformed line
            }
        }
        var summary = BuildSummary(records);
        return (records, summary);
    }

    private DataSummary BuildSummary(List<SensorRecord> records)
    {
        var machineGroups = records.GroupBy(r => r.MachineId);
        var machines = new List<MachineStats>();
        foreach (var g in machineGroups)
        {
            var temps = g.Select(x => x.Temperature).ToList();
            var vibs = g.Select(x => x.Vibration).ToList();
            var statusDist = g.GroupBy(x => x.Status).ToDictionary(x => x.Key, x => x.Count());
            machines.Add(new MachineStats(
                g.Key,
                g.Count(),
                temps.Average(), temps.Min(), temps.Max(),
                vibs.Average(),
                statusDist
            ));
        }
        return new DataSummary(records.Count, machines.Count, machines);
    }
}
