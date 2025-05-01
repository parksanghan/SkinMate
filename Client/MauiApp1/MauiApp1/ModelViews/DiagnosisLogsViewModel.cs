using MauiApp1.Data;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ModelViews
{
    public class DiagnosisLogsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> DiagnosisDates { get; set; } = new();// 날짜 변경 시 마다 변경
    

        public async Task LoadDiagnosisLogAsync()
        {
            var logs =  HttpService.Instance.GetDiaLogEntries().ToList();
            foreach (var log in logs) {
                DiagnosisDates.Add(log.timestamp.ToString("yy-MM-dd HH:mm:ss"));
            }
        }
        public LogEntry GetDiagnosisLogIdx(int index)
        {
            IReadOnlyList<LogEntry> logs = HttpService.Instance.GetDiaLogEntries();
            return logs[index]; 
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
