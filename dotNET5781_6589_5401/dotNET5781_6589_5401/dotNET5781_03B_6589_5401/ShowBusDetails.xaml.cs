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

namespace dotNET5781_03B_6589_5401
{
    /// <summary>
    /// Interaction logic for ShowBusDetails.xaml
    /// </summary>
    public partial class ShowBusDetails : Window
    {
        public ShowBusDetails()
        {
            
            InitializeComponent();

            Details.DataContext = bus;
        }
        public Bus bus { get; set; }
    }
}
