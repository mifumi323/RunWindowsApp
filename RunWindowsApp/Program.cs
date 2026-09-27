using System.Diagnostics;
using System.Text.Json;

// PowerShellでアプリ一覧を出力
var psi = new ProcessStartInfo
{
    FileName = "powershell.exe",
    Arguments = "-NoProfile -Command \"Get-StartApps | ConvertTo-Json\"",
    UseShellExecute = false,
    RedirectStandardOutput = true,
    CreateNoWindow = true,
};
using var process = Process.Start(psi);
if (process == null)
{
    Console.Error.WriteLine("パワーシェル起動できなかったよー");
    return;
}
var output = process.StandardOutput.ReadToEnd();
process.WaitForExit();

// 返ってきたJSONをパース
var entries = JsonSerializer.Deserialize<StartAppEntry[]>(output);
if (entries == null)
{
    Console.Error.WriteLine("JSON読めなかったよー");
    return;
}

// 目的のアプリを探す
var name = "カメラ";
var entry = entries.FirstOrDefault(e => e.Name == name);
if (entry == null)
{
    Console.Error.WriteLine($"{name}見つからなかったよー");
    return;
}

// 起動
var appId = entry.AppID;
Process.Start(new ProcessStartInfo
{
    FileName = "explorer.exe",
    Arguments = $@"shell:AppsFolder\{appId}",
    UseShellExecute = false,
});
