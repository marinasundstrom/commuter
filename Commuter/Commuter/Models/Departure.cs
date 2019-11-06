﻿using System;

namespace Commuter.Models
{
    public class Departure : ObservableObject
    {
        private DateTime time;
        private int line;
        private string? name;
        private string? towards;
        private int id;
        private string? stopPoint;
        private string? lineType;

        public int RunNo { get => id; set => SetProperty(ref id, value); }
        public string? LineType { get => lineType; set => SetProperty(ref lineType, value); }
        public int Line { get => line; set => SetProperty(ref line, value); }
        public string? StopPoint { get => stopPoint; set => SetProperty(ref stopPoint, value); }

        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string? Towards
        {
            get => towards;
            set => SetProperty(ref towards, value);
        }

        public DateTime Time
        {
            get => time; set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
    }
}