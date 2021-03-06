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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ikst.KeyboardHook;

namespace Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KeyboardHook kh = new KeyboardHook();

        public MainWindow()
        {
            InitializeComponent();

            kh.KeyDown += (sender, e) =>
            {
                Title = e.Key.ToString();
            };

            kh.Start();
        }
    }
}
