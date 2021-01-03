﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLAPI;
using PO;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DistanceToPreviousStation.xaml
    /// </summary>
    public partial class DistanceToPreviousStation : Window
    {
        static IBL bl;

        public DistanceToPreviousStation(int previousStationID, int thisStationID)
        {
            InitializeComponent();
            bl = BlFactory.GetBl();
        }
    }
}
