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

namespace E.A.T
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StyleWindow : Window
    {
        private EyeTrack parent;
        public StyleWindow(EyeTrack parent)
        {
            this.parent = parent;
            InitializeComponent();
        }
    }
}
